using Markets.Data;
using Markets.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ServiceStack.Text;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBandJSON
{
    class JsonReporter
    {
        private string outputPath;

        public JsonReporter(string outputPath)
        {
            this.outputPath = outputPath;
        }

        public void GenerateReport(DateTime startDate, DateTime endDate)
        {
            IEnumerable<ProductTotalSale> periodReports = GetDataFromSqlServer(startDate, endDate);
            foreach (var report in periodReports)
            {
                CreateJson(report);
                //AddToMongoDb(CreateJson(report));
            }
        }

        public IEnumerable<ProductTotalSale> GetDataFromSqlServer(DateTime startDate, DateTime endDate)
        {
            var result = new List<ProductTotalSale>();
            var context = new ChainOfSupermarketsContext();

            var allSales = context.Sales
                .Where(s => s.DateOfSale >= startDate && s.DateOfSale <= endDate)
                .ToList();
            foreach (var pr in context.Products)
            {
                var currentProducts = allSales.Where(x => x.Product.Id == pr.Id);

                double totalQuantityForProduct = 0.0;
                double totalMoneyForProduct = 0.0;
                int prodId = 0;
                
                foreach (var currentProduct in currentProducts)
                {
                    totalQuantityForProduct += (double)currentProduct.Quantity;
                    totalMoneyForProduct += 
                        (double)currentProduct.Quantity * (double)currentProduct.PricePerUnit;
                    prodId = currentProduct.ProductId;
                }

                var reportForCurrentProduct = new ProductTotalSale()
                {
                    ProductId = prodId,
                    ProductName = pr.ProductName,
                    VendorName = pr.Vendors.VendorName,
                    QuantitySold = totalQuantityForProduct,
                    TotalIncomes = totalMoneyForProduct
                };

                result.Add(reportForCurrentProduct);
            }        

            return result;
        }

        private void CreateJson(ProductTotalSale periodReportsetData)
        {
            var actualPath = outputPath;// + "Json-Reports";
            if (!Directory.Exists(actualPath))
            {
                Directory.CreateDirectory(actualPath);
            }

            //var writer = new StreamWriter(string.Format("{0}\\{1}.json", actualPath, periodReportsedData.ID));
            //using (writer)
            //{
            //    JsonSerializer.SerializeToWriter<ProductTotalSale>(periodReportsedData, writer);
            //}
        }

        private void AddToMongoDb(IEnumerable<ProductTotalSale> periodReportsedData)
        {
            //MySqlDatabase.MySqlCreator.CreateDatabase();
            //using (var ctx = new MySqlEntities())
            //{
            //    ctx.Add(periodReportsedData);
            //    ctx.SaveChanges();
            //}
        }
    }
}
