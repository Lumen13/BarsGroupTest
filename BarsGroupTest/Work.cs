using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;

using System;
using System.Collections.Generic;
using System.IO;
using BarsGroupTest.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace BarsGroupTest
{
    public class Work
    {
        private static readonly string[] _scopes = { SheetsService.Scope.Spreadsheets };
        private const string applicationName = "BarsGroupTest";
        private const string spreadsheetId = "1emVkyWkxLp9BudpS7qsYhgBGJn8In8ZwDZGhTJ3xAf0";
        private const string sheet = "Awesome sheet";
        private static SheetsService service;

        internal void GetGoogleApi()
        {
            GoogleCredential credential;
            using (var stream =
                new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(_scopes);
            }

            service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = applicationName,
            });
        }

        internal DbContextOptions<MyContext> DbGetConnection()
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();
            // setup current catalog
            builder.SetBasePath(Directory.GetCurrentDirectory());
            // take info from appsettings.json
            builder.AddJsonFile("appsettings.json");
            // make configuration
            var config = builder.Build();
            // get connection string enabled
            string connectionString = config.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<MyContext>();
            var options = optionsBuilder
                .UseNpgsql(connectionString)
                .Options;
            // using options from the DesignTimeDbContextFactory
            // to get all connection options succesfull

            return options;
        }

        internal void DbWrite(DbContextOptions<MyContext> options)
        {
            //To get Database Size we need to use this SQL command:
            //
            //"insert into info ("DbSize") values((select distinct pg_size_pretty(pg_database_size('barsgrouptest')) from pg_class))"

            using (MyContext db = new MyContext(options))
            {
                var infoList = db.info.ToList();
                infoList.Last().ServerName = "Postgre SQL 12";
                infoList.Last().DbName = "barsgrouptest";
                infoList.Last().CurrentDateTime = DateTime.Now;
                db.SaveChanges();
            }
        }

        internal void DbRead(DbContextOptions<MyContext> options)
        {
            using (MyContext db = new MyContext(options))
            {
                var info = db.info.ToList();
                foreach (var inf in info)
                {
                    Console.WriteLine($"{inf.Id}.{inf.ServerName}.{inf.DbName} - {inf.DbSize}.({inf.CurrentDateTime})");
                }
            }
        }

        internal void SheetRead()
        {
            var range = $"{sheet}!A1:E1";
            SpreadsheetsResource.ValuesResource.GetRequest request =
                    service.Spreadsheets.Values.Get(spreadsheetId, range);

            ValueRange response = request.Execute();
            IList<IList<object>> values = response.Values;
            if (values != null && values.Count > 0)
            {
                foreach (var row in values)
                {
                    Console.WriteLine("{0}, {1}, {2}, {3}",
                        row[0], row[1], row[2], row[3]);
                }
            }
            else
            {
                Console.WriteLine("No data found.");
            }
        }

        internal void SheetWrite(DbContextOptions<MyContext> options)
        {
            var writeRange = $"{sheet}!A1:E1";
            var oblist = new List<object>();

            using (MyContext db = new MyContext(options))
            {
                var valueRange = new ValueRange();

                foreach (var info in db.info)
                {
                    oblist.Add(info.Id);
                    oblist.Add(info.ServerName);
                    oblist.Add(info.DbName);
                    oblist.Add(info.DbSize);
                    oblist.Add(info.CurrentDateTime);
                }

                valueRange.Values = new List<IList<object>> { oblist };

                var appendRequest = service.Spreadsheets.Values.Append(valueRange, spreadsheetId, writeRange);
                appendRequest.ValueInputOption =
                    SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
                appendRequest.Execute();
            }
        }

        internal void SheetUpdate()
        {
            var range = $"{sheet}!B:F";
            var valueRange = new ValueRange();

            var oblist = new List<object>() { "updated" };
            valueRange.Values = new List<IList<object>> { oblist };

            var updateRequest = service.Spreadsheets.Values.Update(valueRange, spreadsheetId, range);
            updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
            updateRequest.Execute();
        }

        internal void SheetDelete()
        {
            var range = $"{sheet}!A:F";
            var requestBody = new ClearValuesRequest();

            var deleteRequest = service.Spreadsheets.Values.Clear(requestBody, spreadsheetId, range);
            deleteRequest.Execute();
        }

        internal void SheetNewSheet()
        {
            var addSheetRequest = new AddSheetRequest();
            addSheetRequest.Properties = new SheetProperties();
            addSheetRequest.Properties.Title = "Awesome Sheet";
            BatchUpdateSpreadsheetRequest batchUpdateSpreadsheetRequest = new BatchUpdateSpreadsheetRequest();
            batchUpdateSpreadsheetRequest.Requests = new List<Request>();
            batchUpdateSpreadsheetRequest.Requests.Add(new Request { AddSheet = addSheetRequest });

            var batchUpdateRequest = service.Spreadsheets.BatchUpdate(batchUpdateSpreadsheetRequest, spreadsheetId);
            batchUpdateRequest.Execute();
        }
    }
}
