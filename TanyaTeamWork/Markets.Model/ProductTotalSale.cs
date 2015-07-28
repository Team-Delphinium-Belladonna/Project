namespace Markets.Model
{
    public class ProductTotalSale
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string VendorName { get; set; }

        public decimal QuantitySold { get; set; }

        public decimal TotalIncomes { get; set; }
    }
}