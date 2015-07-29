
namespace DataManagement.Importers
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Markets.Data;
    using Markets.Model;
    using Excel;
    using Ionic.Zip;
    
    

    public class XlsImporter
    {
        public void ImportSales()
        {
            using (var zip = ZipFile.Read(DataManagement.Default.SalesReportsZipFile))
            {
                foreach (var entry in zip.Entries.Where(e => Path.GetExtension(e.FileName) == ".xls"))
                {
                    using (var stream = new MemoryStream())
                    {
                        entry.Extract(stream);
                        using (var excelReader = ExcelReaderFactory.CreateBinaryReader(stream))
                        {
                            var currentDate = DateTime.Parse(entry.FileName.Split('/').First());
                            AddSalesToDb(excelReader, currentDate);
                        }
                    }
                }
            }
        }

        private void AddSalesToDb(IExcelDataReader excelReader, DateTime currentDate)
        {
            using (var db = new ChainOfSupermarketsContext())
            {
                var salesTable = excelReader.AsDataSet().Tables["Sales"];

                var locationName = (string)salesTable.Rows[1].ItemArray[1];
                var currentLocation = GetOrCreateLocation(locationName, db);

                for (var i = 3; i < salesTable.Rows.Count; i++)
                {
                    if (((string)salesTable.Rows[i].ItemArray[1]).Contains("Total sum"))
                    {
                        break;
                    }

                    var productName = (string)salesTable.Rows[i].ItemArray[1];
                    productName = Regex.Replace(productName, @"[^\w'\. ]", string.Empty);
                    var currentProduct = GetOrCreateProduct(productName, db);

                    var quantity = (double)salesTable.Rows[i].ItemArray[2];
                    var pricePerUnit = (double)salesTable.Rows[i].ItemArray[3];

                    db.Sales.Add(new Sale
                    {
                        Location = currentLocation,
                        DateOfSale = currentDate,
                        Product = currentProduct,
                        Quantity = (decimal)quantity,
                        PricePerUnit = (decimal)pricePerUnit
                    });
                }

                db.SaveChanges();
            }
        }

        private Product GetOrCreateProduct(string productName, ChainOfSupermarketsContext db)
        {
            var product = db.Products.FirstOrDefault(p => p.ProductName == productName);
            if (product == null)
            {
                product = new Product()
                {
                    ProductName = productName,
                    Vendors = new Vendor { VendorName = productName.Split(' ').Last() + " Corp." }
                };

                db.Products.Add(product);
                db.SaveChanges();
            }

            return product;
        }

        private Location GetOrCreateLocation(string locationName, ChainOfSupermarketsContext db)
        {
            var location = db.Locations.FirstOrDefault(l => l.Name == locationName);
            if (location == null)
            {
                location = new Location { Name = locationName };
                db.Locations.Add(location);
                db.SaveChanges();
            }

            return location;
        }
    }
}
