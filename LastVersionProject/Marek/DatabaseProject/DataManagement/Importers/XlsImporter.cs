
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
        private ChainOfSupermarketsContext db;

        public XlsImporter(ChainOfSupermarketsContext context)
        {
            this.db = context;
        }

        public void ImportSales()
        {
            //string zipDir = @"C:\Users\Marek\Desktop\LastVersionProject\Marek\DatabaseProject\AdditionalFiles\Sales-Reports.zip";
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
                            this.AddSalesToDB(excelReader, currentDate);
                        }
                    }
                }
            }
        }

        private void AddSalesToDB(IExcelDataReader excelReader, DateTime currentDate)
        {
                var salesTable = excelReader.AsDataSet().Tables["Sales"];

                var locationName = (string)salesTable.Rows[1].ItemArray[1];
                var currentLocation = GetOrCreateLocation(locationName);

                for (var i = 3; i < salesTable.Rows.Count; i++)
                {
                    if (((string)salesTable.Rows[i].ItemArray[1]).Contains("Total sum"))
                    {
                        break;
                    }

                    var productName = (string)salesTable.Rows[i].ItemArray[1];


                    var quantity = (double)salesTable.Rows[i].ItemArray[2];
                    var pricePerUnit = (double)salesTable.Rows[i].ItemArray[3];

                    var currentProduct = GetOrCreateProduct(productName,pricePerUnit);

                    Sale currentSale = new Sale
                    {
                        LocationId = currentLocation.Id,
                        DateOfSale = currentDate,
                        ProductId = currentProduct.Id,
                        Quantity = (decimal) quantity,
                        PricePerUnit = (decimal) pricePerUnit
                    };

                    this.db.Sales.Add(currentSale);
                }

                this.db.SaveChanges();
        }

        private Product GetOrCreateProduct(string productName , double price)
        {
            var product = this.db.Products.FirstOrDefault(p => p.ProductName == productName);
            if (product == null)
            {
                product = new Product()
                {
                    ProductName = productName,
                    Price = (decimal)price,
                    MeasureId = 2,
                    Vendors = new Vendor { VendorName = productName.Split(' ').Last() + " Corp." }
                };

                this.db.Products.Add(product);
                this.db.SaveChanges();
            }

            return product;
        }

        private Location GetOrCreateLocation(string locationName)
        {
            var location = this.db.Locations.FirstOrDefault(l => l.Name == locationName);
            if (location == null)
            {
                location = new Location { Name = locationName };
                this.db.Locations.Add(location);
                this.db.SaveChanges();
            }

            return location;
        }
    }
}
