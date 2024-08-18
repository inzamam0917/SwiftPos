using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        // Method to remove a quantity from the ProductItem
        public void RemoveQuantity(int quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity to remove must be greater than zero.");
            }

            if (quantity > Quantity)
            {
                throw new InvalidOperationException("Cannot remove more quantity than is present in the item.");
            }

            Quantity -= quantity;
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
