namespace Markets.ConsoleClient
{
    using System;
    using Markets.Data;
    using Markets.Data.Migrations;
    using Markets.DataManipulation;
    using System.Data.Entity;

    public class Program
    {
        public static void Main()
        {

            var db = new ChainOfSupermarketsContext();

            // JSON Reports
            string reportsDirectoryPath = @"..\..\Json-Reports";
            var json = new JsonReport(db, reportsDirectoryPath);
            DateTime start = new DateTime(27/7/2015);
            DateTime end = new DateTime(28/7/2015);
            json.ProductSalesForPeriod(start, end);
            Console.WriteLine("JSON reports generate.");

            Console.ReadLine();
        }
    }
}
