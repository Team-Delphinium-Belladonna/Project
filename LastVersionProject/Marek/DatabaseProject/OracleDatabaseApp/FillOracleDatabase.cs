using System.IO;
using System.Linq;

namespace OracleDatabaseApp
{
    public static class FillOracleDatabase
    {
        public static void FillVendorTable()
        {
            var context = new ChainOfSupermarketsEntities();

            using (var vendorReader = new StreamReader(@"..\..\..\Vendors.txt"))
            {
                var vendorLine = vendorReader.ReadLine();
                vendorLine = vendorReader.ReadLine();
                while (vendorLine != null)
                {
                    var vendorData = vendorLine.Split(' ');
                    var vendor = new VENDOR() {ID = int.Parse(vendorData[0]), VENDOR_NAME = vendorData[1]};
                    context.VENDORS.Add(vendor);

                    vendorLine = vendorReader.ReadLine();
                }
            }

            context.SaveChanges();
        }

        public static void FillMeasureTable()
        {
            var context = new ChainOfSupermarketsEntities();

            using (var measureReader = new StreamReader(@"..\..\..\Measures.txt"))
            {
                var measureLine = measureReader.ReadLine();
                measureLine = measureReader.ReadLine();
                while (measureLine != null)
                {
                    var measureData = measureLine.Split(' ');
                    var measure = new MEASURE() {ID = int.Parse(measureData[0]), MEASURE_NAME = measureData[1]};
                    context.MEASURES.Add(measure);

                    measureLine = measureReader.ReadLine();
                }
            }

            context.SaveChanges();
        }


        public static void FillProductsTable()
        {
            var context = new ChainOfSupermarketsEntities();

            var vendors = context.VENDORS.ToList();
            var measures = context.MEASURES.ToList();
            using (var productReader = new StreamReader(@"..\..\..\Products.txt"))
            {
                var line = productReader.ReadLine();
                line = productReader.ReadLine();
                while (!string.IsNullOrEmpty(line))
                {
                    var data = line.Split('|');
                    int productId = int.Parse(data[0]);
                    int vendorId = int.Parse(data[1]);
                    var productName = data[2];
                    int measureId = int.Parse(data[3]);
                    decimal price = decimal.Parse(data[4]);

                    context.PRODUCTS.Add(new PRODUCT()
                    {
                        ID = productId,
                        VENDOR = vendors.Find(v => v.ID == vendorId),
                        PRODUCT_NAME = productName,
                        MEASURE = measures.Find(m => m.ID == measureId),
                        PRICE = price
                    });

                    line = productReader.ReadLine();
                }

                context.SaveChanges();
            }
        }
    }
}
