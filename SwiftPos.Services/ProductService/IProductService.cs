using SwiftPos.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwiftPos.Services.ProductService
{
    public interface IProductService
    {
        Task AddProductAsync(Product product);
        Task<Product> GetProductAsync(string productId);
        Task<List<Product>> GetAllProductsAsync();
        Task UpdateProductAsync(Product product);
        Task RemoveProductAsync(string productId);
    }
}
