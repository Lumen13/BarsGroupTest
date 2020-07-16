using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace BarsGroupTest
{
    class Program
    {
        private static readonly string[] scopes = { SheetsService.Scope.Spreadsheets };
        private const string applicationName = "BarsGroupTest";
        private const string spreadsheetId = "1emVkyWkxLp9BudpS7qsYhgBGJn8In8ZwDZGhTJ3xAf0";
        private const string sheet = "List0";
        private static SheetsService service;

        static void Main(string[] args)
        {
            GoogleCredential credential;
            using (var stream =
                new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(scopes);
            }

            // Create Google Sheets API service.
            service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = applicationName,
            });

            Write(5);
            Read();
        }

        private static void Read()
        {
            var range = $"{sheet}!B2:H";
            SpreadsheetsResource.ValuesResource.GetRequest request =
                    service.Spreadsheets.Values.Get(spreadsheetId, range);

            ValueRange response = request.Execute();
            IList<IList<Object>> values = response.Values;
            if (values != null && values.Count > 0)
            {
                foreach (var row in values)
                {
                    Console.WriteLine("{0}, {1}, {2}, {3}, {4}",
                        row[0], row[1], row[2], row[3], row[4]);
                }
            }
            else
            {
                Console.WriteLine("No data found.");
            }
            Console.Read();
        }

        private static void Write(int count = 1)
        {
            var writeRange = $"{sheet}!B:F";

            for (var i = 0; i < count; i++)
            {
                var valueRange = new ValueRange();

                var oblist = new List<object>() { i, "My", "cofee", "is", "black" };
                valueRange.Values = new List<IList<object>> { oblist };

                var appendRequest = service.Spreadsheets.Values.Append(valueRange, spreadsheetId, writeRange);
                appendRequest.ValueInputOption =
                    SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
                appendRequest.Execute();
            }
        }
    }
}
