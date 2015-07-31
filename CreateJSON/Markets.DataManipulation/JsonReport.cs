namespace Markets.DataManipulation
{
    using System;
    using System.Linq;
    using System.IO;

    using Newtonsoft.Json;

    using Markets.Data;
    using Markets.Model;

    public class JsonReport
    {
        private ChainOfSupermarketsContext dbContext;
        private string folderPath;

        public JsonReport(ChainOfSupermarketsContext context, string path)
        {
            this.dbContext = context;
            this.folderPath = path;
        }

        public void ProductSalesForPeriod(DateTime startDate, DateTime endDate)
        {
            var salesForPeriod = dbContext.Sales
                    //.Where(s => s.DateOfSale >= startDate && s.DateOfSale <= endDate)
                    .Select(s => new ProductTotalSale
                    {
                        ProductId = s.ProductId,
                        ProductName = s.Product.ProductName,
                        VendorName = s.Product.Vendors.VendorName,
                        QuantitySold = s.Quantity,
                        TotalIncomes = s.Quantity * s.PricePerUnit
                    })
                    //.GroupBy(s => s.ProductId)
                    .ToList();



            Directory.CreateDirectory(this.folderPath);

            foreach (var saleProduct in salesForPeriod)
            {
                string fileName = saleProduct.ProductId.ToString();
                string result = JsonConvert.SerializeObject(saleProduct);

                SaveToFile(fileName, result);
            }
        }

        private void SaveToFile(string name, string content)
        {
            if (name.Length < 2)
            {
                name = '0' + name;
            }
            string path = Path.Combine(this.folderPath, name + ".json");

            File.WriteAllText(path, content);
        }
    }
}
