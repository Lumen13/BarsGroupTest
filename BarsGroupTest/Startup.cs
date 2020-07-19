namespace BarsGroupTest
{
    public class Startup
    {
        Work w = new Work();

        public void Run()
        {
            w.GetGoogleApi();

            var options = w.DbGetConnection();
            w.DbWrite(options);
            //w.DbRead(options);      
            
            //w.SheetNewSheet();
            //w.SheetUpdate();
            w.SheetWrite(options);
            w.SheetRead();
            //w.SheetDelete();
        }     
    }
}
