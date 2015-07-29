
namespace Markets.Model
{
    public class ProductTotalSale
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string VendorName { get; set; }

        public double QuantitySold { get; set; }

        public double TotalIncomes { get; set; }
    }
}
