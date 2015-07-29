using System;
using System.Linq;
using System.Text;
using System.Xml;
using System.Data;
using DB_Mapping;


namespace Export_XML
{
    class ExportReportAsXML
    {
        static void Main()
        {
            ExportSales("20-Jul-2014", "22-Jul-2014");
        }

        public static void ExportSales(string startDate, string endDate)
        {
            var start = DateTime.Parse(startDate);
            var end = DateTime.Parse(endDate);
            ExportSales(start, end);
        }

        public static void ExportSales(DateTime startDate, DateTime endDate)
        {
            var encoding = Encoding.GetEncoding("utf-8");
            string documentPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\report.xml";
            using (var writer = new XmlTextWriter(documentPath,encoding))
            {
                writer.Formatting = Formatting.Indented;
                writer.IndentChar = '\t';
                writer.Indentation = 1;

                writer.WriteStartDocument();
                writer.WriteStartElement("sales");
                WriteSalesToWriter(writer, startDate, endDate);
                writer.WriteEndDocument();
            }
        }

        private static void WriteSalesToWriter(XmlWriter writer, DateTime startDate, DateTime endDate)
        {
            using (var db = new SupermarketChainEntities())
            {
                foreach (var vendor in db.Vendors.ToList())
                {
                    var totalSalesSumByDate = db.Sales
                        .Where(sale =>
                            sale.Product.VendorId == vendor.Id &&
                            sale.DateofSale >= startDate &&
                            sale.DateofSale <= endDate)
                        .OrderBy(sale => sale.DateofSale)
                        .GroupBy(sale => sale.DateofSale)
                        .Select(group => new
                        {
                            Date = group.Key,
                            TotalSum = group.Sum(g => g.PricePerUnit * g.Quantity)
                        })
                        .ToList();

                    if (totalSalesSumByDate.Any())
                    {
                        writer.WriteStartElement("sale");
                        writer.WriteAttributeString("vendor", vendor.VendorName);
                        foreach (var sale in totalSalesSumByDate)
                        {
                            AddSaleToVendor(writer, sale.Date, sale.TotalSum);
                        }

                        writer.WriteEndElement();
                    }
                }
            }
        }

        private static void AddSaleToVendor(XmlWriter writer, DateTime date, decimal totalSum)
        {
            writer.WriteStartElement("summary");
            writer.WriteAttributeString("date", date.ToString("d-MMM-yyyy"));
            writer.WriteAttributeString("total-sum", totalSum.ToString("F2"));
            writer.WriteEndElement();
        }
    }
}
