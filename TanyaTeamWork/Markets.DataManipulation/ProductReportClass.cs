using System.Runtime.Serialization;

namespace Markets.DataManipulation
{
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
        public double totalQuantiySold;
        [DataMember(Name = "total-incomes")]
        public double totalIncomes;
    }
}