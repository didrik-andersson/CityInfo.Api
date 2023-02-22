using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityInfo.Api.Entities
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public decimal Price { get; set; }
        public int InventoryCount { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public Product(string name, decimal price) 
        {
            Name = name;
            Price = price;
        }
    }
}