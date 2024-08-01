using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwiftPos.Model
{
    public class ProductItem
    {
        [Key]
        public string ProductItemId { get; set; } = Guid.NewGuid().ToString();  

        [Required]
        public string SaleId { get; set; }

        public Sale Sale { get; set; }

        [Required]
        public string ProductId { get; set; }

        public Product Product { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }

        public ProductItem() { }

        public ProductItem(string productId, int quantity)
        {
            ProductItemId = Guid.NewGuid().ToString();  
            ProductId = productId;
            Quantity = quantity;
        }

        public ProductItem(Product product, int quantity)
        {
            ProductItemId = Guid.NewGuid().ToString();  
            Product = product;
            ProductId = product.ProductId;
            Quantity = quantity;
        }

        public void AddQuantity(int quantity)
        {
            Quantity += quantity;
        }

        public decimal TotalAmount
        {
            get
            {
                return Product.Price * Quantity;
            }
        }
    }
}
