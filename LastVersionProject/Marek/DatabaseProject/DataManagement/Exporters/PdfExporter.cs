

namespace DataManagement.Exporters
{
    using System;
    using System.IO;
    using Markets.Data;
    using System.Linq;
    using iTextSharp.text;
    using iTextSharp.text.pdf;

    public class PdfExporter
    {
        private  readonly BaseFont BaseFont =
            BaseFont.CreateFont(DataManagement.Default.VerdanaFontLocation, BaseFont.CP1252, false);

        public  void ExportSales(string startDate, string endDate)
        {
            var start = DateTime.Parse(startDate);
            var end = DateTime.Parse(endDate);
            ExportSales(start, end);
        }

        public void ExportSales(DateTime startDate, DateTime endDate)
        {
            using (var pdfDocument = new Document())
            {
                var file = File.Create(DataManagement.Default.PdfSalesReportLocation);
                PdfWriter.GetInstance(pdfDocument, file);
                pdfDocument.Open();

                var table = new PdfPTable(5)
                {
                    TotalWidth = 550f,
                    LockedWidth = true
                };

                table.SetWidths(new[] { 150f, 70f, 70f, 230f, 70f });

                table.AddCell(new PdfPCell(
                    new Phrase("Aggregated Sales Report", new Font(BaseFont, 14, Font.BOLD)))
                {
                    Colspan = 5,
                    HorizontalAlignment = 1,
                    BackgroundColor = new BaseColor(255, 255, 255),
                    PaddingTop = 10f,
                    PaddingBottom = 10f
                });

                var grandTotal = 0m;

                using (var db = new ChainOfSupermarketsContext())
                {
                    var dates = db.Sales
                        .Select(sale => sale.DateOfSale)
                        .Where(date => date >= startDate && date <= endDate)
                        .Distinct()
                        .ToList();

                    foreach (var date in dates)
                    {
                        AddHeaderToTable(table, date);

                        var sales = db.Sales
                            .Where(sale => sale.DateOfSale == date)
                            .Select(sale => new
                            {
                                sale.Product.ProductName,
                                sale.Quantity,
                                sale.PricePerUnit,
                                sale.DateOfSale,
                                Location = sale.Location.Name,
                                TotalValue = sale.Quantity * sale.PricePerUnit
                            })
                            .ToList();

                        foreach (var sale in sales)
                        {
                            AddNormalCellToTable(table, sale.ProductName);
                            AddNormalCellToTable(table, sale.Quantity.ToString("F2"));
                            AddNormalCellToTable(table, sale.PricePerUnit.ToString("F2"));
                            AddNormalCellToTable(table, sale.Location);
                            AddNormalCellToTable(table, sale.TotalValue.ToString("F2"));
                        }

                        var totalSum = sales.Sum(sale => sale.TotalValue);
                        AddSaleFooterToTable(table, date, totalSum);
                        grandTotal += totalSum;
                    }
                }

                AddGrandTotalToTable(table, grandTotal);

                pdfDocument.Add(table);
                pdfDocument.Close();
            }
        }

        private  void AddHeaderToTable(PdfPTable table, DateTime date)
        {
            table.AddCell(new PdfPCell(
                new Phrase("Date: " + date.ToString("d-MMM-yyyy"), new Font(BaseFont, 12, Font.BOLD)))
            {
                Colspan = 5,
                HorizontalAlignment = Element.ALIGN_LEFT,
                BackgroundColor = new BaseColor(242, 242, 242),
                PaddingTop = 10f,
                PaddingBottom = 10f
            });

            AddHeaderCellToTable(table, "Product");
            AddHeaderCellToTable(table, "Quantity");
            AddHeaderCellToTable(table, "Unit Price");
            AddHeaderCellToTable(table, "Location");
            AddHeaderCellToTable(table, "Sum");
        }

        private  void AddSaleFooterToTable(PdfPTable table, DateTime date, decimal totalSum)
        {
            var boldFont = new Font(BaseFont, 12, Font.BOLD);
            table.AddCell(new PdfPCell(new Phrase("Total sum for " + date.ToString("d-MMM-yyyy") + ":", boldFont))
            {
                Colspan = 4,
                HorizontalAlignment = Element.ALIGN_RIGHT,
                BackgroundColor = new BaseColor(255, 255, 255),
                PaddingTop = 5f,
                PaddingBottom = 5f
            });

            table.AddCell(new PdfPCell(new Phrase(totalSum.ToString("F2"), boldFont))
            {
                Colspan = 1,
                HorizontalAlignment = Element.ALIGN_RIGHT,
                BackgroundColor = new BaseColor(255, 255, 255),
                PaddingTop = 5f,
                PaddingBottom = 5f
            });
        }

        private  void AddGrandTotalToTable(PdfPTable table, decimal grandTotal)
        {
            var extraBoldFont = new Font(BaseFont, 14, Font.BOLD);
            table.AddCell(new PdfPCell(new Phrase("Grand Total:", extraBoldFont))
            {
                Colspan = 4,
                HorizontalAlignment = Element.ALIGN_RIGHT,
                BackgroundColor = new BaseColor(180, 190, 231),
                PaddingTop = 5f,
                PaddingBottom = 5f
            });

            table.AddCell(new PdfPCell(new Phrase(grandTotal.ToString("F2"), extraBoldFont))
            {
                Colspan = 1,
                HorizontalAlignment = Element.ALIGN_RIGHT,
                BackgroundColor = new BaseColor(180, 190, 231),
                PaddingTop = 5f,
                PaddingBottom = 5f
            });
        }

        private void AddNormalCellToTable(PdfPTable table, string text)
        {
            table.AddCell(new PdfPCell(new Phrase(text, new Font(BaseFont, 10)))
            {
                Colspan = 1,
                HorizontalAlignment = Element.ALIGN_LEFT,
                BackgroundColor = new BaseColor(255, 255, 255),
                PaddingTop = 5f,
                PaddingBottom = 5f
            });
        }

        private void AddHeaderCellToTable(PdfPTable table, string text)
        {
            table.AddCell(new PdfPCell(new Phrase(text, new Font(BaseFont, 12, Font.BOLD)))
            {
                Colspan = 1,
                HorizontalAlignment = Element.ALIGN_LEFT,
                BackgroundColor = new BaseColor(217, 217, 217),
                PaddingTop = 5f,
                PaddingBottom = 5f
            });
        }
    }
}
