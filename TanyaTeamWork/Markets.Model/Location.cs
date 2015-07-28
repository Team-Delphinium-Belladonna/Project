namespace Markets.Model
{
    //OKI
    using System.ComponentModel.DataAnnotations;

    public class Location
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}