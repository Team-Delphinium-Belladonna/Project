namespace Markets.Model
{
    //OKI-Tanya-dopulnenie
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
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalSum { get; set; }
        public DateTime? Date { get; set; }
    }
}
