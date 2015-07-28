namespace Markets.Model
{
    //OKI
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;


    public class Measure
    {
        private ICollection<Product> products;

        public Measure()
        {
            this.products = new HashSet<Product>();
        }

        public int Id { get; set; }

        [MaxLength(50)]
        public string MeasureName { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}