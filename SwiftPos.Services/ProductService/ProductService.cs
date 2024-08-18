using SwiftPos.Model;
using SwiftPos.Repositories.ProductRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwiftPos.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task AddProductAsync(Product product)
        {
            if (!await _productRepository.CategoryExistsAsync(product.CategoryId))
            {
                throw new ArgumentException("The specified category does not exist.");
            }

            await _productRepository.AddProductAsync(product);
        }

        public async Task<Product> GetProductAsync(string productId)
        {
            return await _productRepository.GetProductAsync(productId);
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _productRepository.GetAllProductsAsync();
        }

        public async Task UpdateProductAsync(Product product)
        {
            if (string.IsNullOrEmpty(product.ProductId))
            {
                throw new ArgumentException("ProductId cannot be null or empty.");
            }

            if (!await _productRepository.CategoryExistsAsync(product.CategoryId))
            {
                throw new ArgumentException("The specified category does not exist.");
            }

            await _productRepository.UpdateProductAsync(product);
        }

        public async Task RemoveProductAsync(string productId)
        {
            var product = await _productRepository.GetProductAsync(productId);
            if (product == null)
            {
                throw new ArgumentException("Product not found.");
            }

            await _productRepository.RemoveProductAsync(product);
        }
    }
}
