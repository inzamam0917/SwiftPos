using System.Collections.Generic;
using System.Threading.Tasks;
using SwiftPos.Model;  

namespace SwiftPos.Repositories.ProductRepository
{
    public interface IProductRepository
    {
        Task AddProductAsync(Product product);
        Task<Product> GetProductAsync(string productId); 
        Task<List<Product>> GetAllProductsAsync();
        Task UpdateProductAsync(Product product);
        Task RemoveProductAsync(Product product);
        Task<bool> CategoryExistsAsync(string categoryId);  
    }
}
