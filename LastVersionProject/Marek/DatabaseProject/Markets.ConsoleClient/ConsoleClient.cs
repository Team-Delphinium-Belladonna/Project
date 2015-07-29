using System;
using System.Data.OracleClient;
using System.Xml.Serialization;
using DataManagement.Exporters;
using DataManagement.Importers;
using Markets.Model;

namespace Markets.ConsoleClient
{
    using System.Linq;
    using Markets.Data;

    class ConsoleClient
    {
        static void Main()
        {
            var db = new ChainOfSupermarketsContext();
            //var MeasureCount = db.Measures.Count();

            //ExportDbToSqlServer();

            //var import = new XlsImporter(db);
            //import.ImportSales();

            //var export = new PdfExporter();
            //export.ExportSales("01-Jan-2015", "25-Feb-2015");
            //var importXML = new LoadXML();
            //importXML.ImportXML();

            var exportXML = new ExportReportAsXML();
            exportXML.ExportSales("20-Jul-2014","22-Jul-2014");




        }

        public static void ExportDbToSqlServer()
        {
            var connection = new OracleConnection(Settings.Default.OracleConnectionString);
            connection.Open();
            using (connection)
            {
                using (var sqlServerDb = new ChainOfSupermarketsContext())
                {
                    ExportMeasures(connection, sqlServerDb);
                    sqlServerDb.SaveChanges();

                    ExportVendors(connection, sqlServerDb);
                    sqlServerDb.SaveChanges();

                    ExportProducts(connection, sqlServerDb);
                    sqlServerDb.SaveChanges();
                }
            }
        }

        private static void ExportMeasures(OracleConnection connection, ChainOfSupermarketsContext db)
        {
            using (var command = new OracleCommand("SELECT MEASURE_NAME FROM MEASURES", connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        db.Measures.Add(new Measure
                        {
                            MeasureName = (string)reader["MEASURE_NAME"]
                        });
                    }
                }
            }
        }

        private static void ExportVendors(OracleConnection connection, ChainOfSupermarketsContext db)
        {
            using (var command = new OracleCommand("SELECT VENDOR_NAME FROM VENDORS", connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        db.Vendors.Add(new Vendor()
                        {
                            VendorName = (string)reader["VENDOR_NAME"]
                        });
                    }
                }
            }
        }

        private static void ExportProducts(OracleConnection connection, ChainOfSupermarketsContext db)
        {
            using (var command = new OracleCommand("SELECT PRODUCT_NAME, PRICE, VENDOR_ID, MEASURE_ID FROM PRODUCTS", connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        db.Products.Add(new Product()
                        {
                            ProductName = (string)reader["PRODUCT_NAME"],
                            Price = (decimal)reader["PRICE"],
                            MeasureId = Convert.ToInt32(reader["MEASURE_ID"]),
                            VendorId = Convert.ToInt32(reader["VENDOR_ID"]),
                        });
                    }
                }
            }
        }
    }
}
