using SwiftPos.Model;
using SwiftPos.Repositories.SaleRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwiftPos.Services.SaleService
{
    public class SaleService : ISaleService
    {
        private readonly ISaleRepository _saleRepository;

        public SaleService(ISaleRepository saleRepository)
        {
            _saleRepository = saleRepository;
        }

        public async Task<bool> StartSaleAsync(string cashierId)
        {
            var cashier = await _saleRepository.GetUserByIdAsync(cashierId);
            if (cashier == null)
            {
                return false; // Cashier not found
            }

            var sale = new Sale(cashier, SaleStatus.Start);
            await _saleRepository.AddSaleAsync(sale);
            return true;
        }

        public async Task<bool> AddProductToSaleAsync(string saleId, string productId, int quantity)
        {
            var sale = await _saleRepository.GetSaleByIdAsync(saleId);
            if (sale == null || sale.Status != SaleStatus.Start)
            {
                return false; // Sale not found or sale is not in "Start" status
            }

            var product = await _saleRepository.GetProductByIdAsync(productId);
            if (product == null || product.Quantity < quantity)
            {
                return false; // Product not found or insufficient stock
            }

            sale.AddProduct(product, quantity);
            await _saleRepository.UpdateSaleAsync(sale);

            // Update product stock
            product.Quantity -= quantity;
            await _saleRepository.UpdateProductAsync(product);

            return true;
        }

        public async Task<bool> RemoveProductFromSaleAsync(string saleId, string productId, int quantity)
        {
            var sale = await _saleRepository.GetSaleByIdAsync(saleId);
            if (sale == null || sale.Status != SaleStatus.Start)
            {
                return false; // Sale not found or sale is not in "Start" status
            }

            var product = await _saleRepository.GetProductByIdAsync(productId);
            if (product == null)
            {
                return false; // Product not found
            }

            var productItem = sale.Products.Find(p => p.ProductId == productId);
            if (productItem == null || productItem.Quantity < quantity)
            {
                return false; // Product not found in sale or insufficient quantity to remove
            }

            productItem.RemoveQuantity(quantity);

            if (productItem.Quantity == 0)
            {
                sale.Products.Remove(productItem);
            }

            await _saleRepository.UpdateSaleAsync(sale);

            // Update product stock
            product.Quantity += quantity;
            await _saleRepository.UpdateProductAsync(product);

            return true;
        }

        public async Task<bool> CompleteSaleAsync(string saleId)
        {
            var sale = await _saleRepository.GetSaleByIdAsync(saleId);
            if (sale == null || sale.Status != SaleStatus.Start)
            {
                return false;
            }

            sale.Status = SaleStatus.Complete;
            await _saleRepository.UpdateSaleAsync(sale);
            return true;
        }

        public async Task<decimal> GetTotalAmountAsync(string saleId)
        {
            var sale = await _saleRepository.GetSaleByIdAsync(saleId);
            if (sale == null)
            {
                throw new ArgumentException("Sale not found");
            }

            return sale.TotalAmount;
        }
    }
}
