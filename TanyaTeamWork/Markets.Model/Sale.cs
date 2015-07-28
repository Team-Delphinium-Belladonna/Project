namespace Markets.Model
{
    //OKI
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Sale : IEntity
    {
        [Key]
        public int Id { get; set; }
        public decimal Quantity { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        public DateTime DateOfSale { get; set; }

        //Tanya
        public int VendorId { get; set; }
        public virtual Vendor Vendor { get; set; }
       
        //Tanya
        public bool Equals(Sale other)
        {
            if ((this.ProductId == other.ProductId) &&
                (this.DateOfSale.Date == other.DateOfSale.Date) &&
                (this.VendorId == other.VendorId))
            {
                return true;
            }

            return false;
        }
    }
}
            //public int Id { get; set; }

            //[Required]
            //public decimal Quantity { get; set; }

            //[Required]
            //public int ProductId { get; set; }
            //public virtual Product Product { get; set; }

            //public decimal PricePerUnit { get; set; }

            //[Required]
            //[Column(TypeName = "Date")]
            //public DateTime DateOfSale { get; set; }

            //[Required]
            //public int LocationId { get; set; }
            //public virtual Location Location { get; set; }
        