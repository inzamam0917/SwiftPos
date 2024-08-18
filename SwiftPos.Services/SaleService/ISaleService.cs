using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwiftPos.Services.SaleService
{
    public interface ISaleService
    {
        Task<bool> StartSaleAsync(string cashierId);
        Task<bool> AddProductToSaleAsync(string saleId, string productId, int quantity);
        Task<bool> RemoveProductFromSaleAsync(string saleId, string productId, int quantity);
        Task<bool> CompleteSaleAsync(string saleId);
        Task<decimal> GetTotalAmountAsync(string saleId);
    }
}
