using SwiftPos.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwiftPos.Repositories.SaleRepository
{
    public interface ISaleRepository
    {
        Task<User> GetUserByIdAsync(string userId);
        Task<Product> GetProductByIdAsync(string productId);
        Task<Sale> GetSaleByIdAsync(string saleId);
        Task AddSaleAsync(Sale sale);
        Task UpdateSaleAsync(Sale sale);
        Task UpdateProductAsync(Product product);
    }
}
