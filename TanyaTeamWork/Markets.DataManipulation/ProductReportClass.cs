namespace Markets.DataManipulation
{
    using System.Runtime.Serialization;

    [DataContract]
    public class ProductReportClass
    {
        [DataMember]
        public int id;
        [DataMember(Name = "product-name")]
        public string productName;
        [DataMember(Name = "vendor-name")]
        public string vendorName;
        [DataMember(Name = "total-quantity-sold")]
        public decimal totalQuantiySold;
        [DataMember(Name = "total-incomes")]
        public decimal totalIncomes;
    }
}