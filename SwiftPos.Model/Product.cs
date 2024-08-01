using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwiftPos.Model
{
    public class Product
    {
        [Key]
        public string ProductId { get; set; } = Guid.NewGuid().ToString(); 

        [Required]
        [StringLength(100, ErrorMessage = "Name length can't be more than 100.")]
        public string Name { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Type length can't be more than 50.")]
        public string Type { get; set; }

        [Required]
        public string CategoryId { get; set; }

        public Category Category { get; set; }

        public ICollection<ProductItem> ProductItems { get; set; } = new List<ProductItem>();

        public Product() { }

        public Product(string name, decimal price, int quantity, string type, string categoryId)
        {
            ProductId = Guid.NewGuid().ToString(); 
            Name = name;
            Price = price;
            Quantity = quantity;
            Type = type;
            CategoryId = categoryId;
        }

        public Product(string productId, string name, decimal price, int quantity, string type, string categoryId)
        {
            ProductId = productId;
            Name = name;
            Price = price;
            Quantity = quantity;
            Type = type;
            CategoryId = categoryId;
        }

        public Product(string name, decimal price, int quantity, string type)
        {
            ProductId = Guid.NewGuid().ToString();  
            Name = name;
            Price = price;
            Quantity = quantity;
            Type = type;
        }
    }
}
