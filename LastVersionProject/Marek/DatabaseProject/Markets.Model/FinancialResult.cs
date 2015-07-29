
namespace Markets.Model
{
    public class FinancialResult
    {
        public string Vendor { get; set; }

        public decimal Incomes { get; set; }

        public decimal Expenses { get; set; }

        public decimal Taxes { get; set; }

        public decimal Total
        {
            get
            {
                return this.Incomes - this.Expenses - this.Taxes;
            }
        }
    }
}
