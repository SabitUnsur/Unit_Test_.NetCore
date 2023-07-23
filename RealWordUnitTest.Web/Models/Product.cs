using Microsoft.Build.Framework;

namespace RealWordUnitTest.Web.Models
{
    public class Product
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public decimal? Price { get; set; }

        [Required]
        public int Stock { get; set; }

        [Required]
        public string Color { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
