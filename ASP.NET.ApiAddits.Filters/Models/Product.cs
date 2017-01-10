using System.ComponentModel.DataAnnotations;

namespace ASP.NET.ApiAddits.Filters.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public decimal Price { get; set; }
        [Range(0, 10)]
        public double Weight { get; set; }
    }
}