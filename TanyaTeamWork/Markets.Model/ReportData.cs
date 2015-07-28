namespace Markets.Model
{
    using System;
    using System.Collections.Generic;

    public class ReportData
    {
        public ReportData()
        {
            this.Date = null;
        }

        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Measure { get; set; }
        public string VendorName { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
        public double TotalSum { get; set; }
        public DateTime? Date { get; set; }
    }
}
