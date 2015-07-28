namespace Markets.Model
{
    //OKI
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Expense
    {
        private DateTime month;

        public int Id { get; set; }

        [Required]
        public decimal Value { get; set; }

        [Required]
        public int VendorId { get; set; }
        public virtual Vendor Vendor { get; set; }

        [Required]
        [Column(TypeName = "Date")]
        public DateTime Month
        {
            get
            {
                return this.month;
            }
            set
            {
                this.month = new DateTime(value.Year, value.Month, 1);
            }
        }
    }
}