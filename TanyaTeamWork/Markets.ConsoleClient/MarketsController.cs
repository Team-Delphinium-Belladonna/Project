namespace Markets.ConsoleClient
{
    using System;
    using Markets.Data;
    using Markets.DataManipulation;
    using Markets.Model;

    public static class MarketsController
    {
        //private const string DataFolderPath = @"..\..\..\..\..\DataFiles\";
        private const string ReportsFolderPath = @"..\..\Reports\";
        //public static void OracleToMsSql()
        //{
        //    var oracleTransmitter = new OracleTransmiter();
        //    var data = oracleTransmitter.GetData();
        //    DbManager.SaveData(data);
        //}

        //public static void ZipExcelToMsSql(string filename)
        //{
        //    var zipTransmiter = new Extractor(DataFolderPath + filename);
        //    var data = zipTransmiter.GetData();
        //    DbManager.SaveData(data);
        //}

        //public static void XmlToMsSql(string filename)
        //{
        //    var xmlTransmitter = new XmlVendorExpensesImport(DataFolderPath + filename);
        //    var data = xmlTransmitter.GetData();
        //    DbManager.SaveData(data);
        //}

        //public static void MsSqlToPdf(string fileName, DateTime startDate, DateTime endDate)
        //{
        //    var data = DbManager.GetSalesForPeriod(startDate, endDate);
        //    PdfAggregatedSalesReport.PdfSaleReportForPeriod(fileName, data);
        //}

        //public static void MsSqlToXml(string fileName, DateTime startDate, DateTime endDate)
        //{
        //    var data = DbManager.GetSalesGroupByVendorAndDate(startDate, endDate);
        //    var xmlGenerator = new XmlSalesReportByVendor(data);
        //    xmlGenerator.Save(fileName);
        //}

        public static void JsonFilesToMongoDb(string directoryName)
        {
            var data = Json.GetProductReportsFromDirectory(directoryName);
            MongoDbManager.AddSalesProductReportsFromFilesToDatabase(data);
        }

        public static void MsSqlToJson(string directoryName, DateTime startDate, DateTime endDate)
        {
            var data = DbManager.GetSalesOfEachProductForPeriod(startDate, endDate);
            Json.SaveRaportToJsonFiles(directoryName, data);
        }

        /*
        public static void MsSqlToMySql()
        {
            var data = DbManager.LoadData();
            MySQLDbManager.SaveData(data);
        }

        public static void SqliteMySqlToXlsx(string filename)
        {
            XlsxManager.FinancialReportByVendor(DataFolderPath + filename);
        }
        */
    }
}
