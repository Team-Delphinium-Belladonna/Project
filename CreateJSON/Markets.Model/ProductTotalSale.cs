using Newtonsoft.Json;

namespace Markets.Model
{
    public class ProductTotalSale
    {
        [JsonProperty("product-id")]
        public int ProductId { get; set; }

        [JsonProperty("product-name")]
        public string ProductName { get; set; }

        [JsonProperty("vendor-name")]
        public string VendorName { get; set; }

        [JsonProperty("total-quantity-sold")]
        public decimal QuantitySold { get; set; }

        [JsonProperty("total-incomes")]
        public decimal TotalIncomes { get; set; }
    }
}