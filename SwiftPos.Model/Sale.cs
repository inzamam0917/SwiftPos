using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwiftPos.Model
{
    public enum SaleStatus
    {
        Start,
        Pending,
        Complete
    }

    public class Sale
    {
        [Key]
        public string SaleId { get; set; } = Guid.NewGuid().ToString(); 

        [Required]
        public string CashierId { get; set; }

        public User Cashier { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public SaleStatus Status { get; set; }

        [Required]
        public List<ProductItem> Products { get; set; } = new List<ProductItem>();

        public Sale() { }

        public Sale(User cashier, SaleStatus status)
        {
            SaleId = Guid.NewGuid().ToString();  
            CashierId = cashier.UserID; 
            Cashier = cashier;
            Date = DateTime.Now;
            Status = status;
        }

        public void AddProduct(Product product, int quantity)
        {
            var productItem = Products.Find(p => p.ProductId == product.ProductId);
            if (productItem == null)
            {
                Products.Add(new ProductItem(product, quantity));
            }
            else
            {
                productItem.AddQuantity(quantity);
            }
        }

        public decimal TotalAmount
        {
            get
            {
                return Products.Sum(pi => pi.TotalAmount);
            }
        }
    }

}
