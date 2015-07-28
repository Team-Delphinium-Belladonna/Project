namespace Markets.Model
{
    //OKI-Tanya-dobaveno
    using System.Collections.Generic;

    public class ReportContainer
    {
        public List<ReportData> SaleReport { get; set; }
        public string SupermarkeName { get; set; }
        public string PrductName { get; set; }
    }
}