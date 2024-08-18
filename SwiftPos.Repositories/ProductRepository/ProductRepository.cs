using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using SwiftPos.Model;  

namespace SwiftPos.Repositories.ProductRepository
{
    public class ProductRepository : IProductRepository
    {
        private readonly Container _productContainer;
        private readonly Container _categoryContainer;

        public ProductRepository(CosmosClient cosmosClient, IConfiguration configuration)
        {
            var databaseName = configuration["DatabaseName"]!.ToString();
            _productContainer = cosmosClient.GetContainer(databaseName, "Product");
            _categoryContainer = cosmosClient.GetContainer(databaseName, "Category");
        }

        public async Task AddProductAsync(Product product)
        {
            try
            {
                if (string.IsNullOrEmpty(product.ProductId))
                {
                    product.ProductId = Guid.NewGuid().ToString();  
                }
                await _productContainer.CreateItemAsync(product, new PartitionKey(product.ProductId));
            }
            catch (CosmosException ex)
            {
                Console.WriteLine($"CosmosException occurred: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                throw;
            }
        }

        public async Task<Product> GetProductAsync(string productId)
        {
            try
            {
                ItemResponse<Product> response = await _productContainer.ReadItemAsync<Product>(productId, new PartitionKey(productId));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null; // Handle not found exception
            }
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            var query = new QueryDefinition("SELECT * FROM c");
            var iterator = _productContainer.GetItemQueryIterator<Product>(query);
            var products = new List<Product>();
            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                products.AddRange(response);
            }
            return products;
        }

        public async Task UpdateProductAsync(Product product)
        {
            try
            {
                if (string.IsNullOrEmpty(product.ProductId))
                {
                    product.ProductId = Guid.NewGuid().ToString();
                }
                await _productContainer.UpsertItemAsync(product, new PartitionKey(product.ProductId));
            }
            catch (CosmosException ex)
            {
                Console.WriteLine($"CosmosException occurred: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                throw;
            }
        }

        public async Task RemoveProductAsync(Product product)
        {
            try
            {
                await _productContainer.DeleteItemAsync<Product>(product.ProductId, new PartitionKey(product.ProductId));
            }
            catch (CosmosException ex)
            {
                Console.WriteLine($"CosmosException occurred: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> CategoryExistsAsync(string categoryId)
        {
            var query = new QueryDefinition("SELECT VALUE COUNT(1) FROM c WHERE c.id = @id")
                .WithParameter("@id", categoryId);
            var iterator = _categoryContainer.GetItemQueryIterator<int>(query);
            var response = await iterator.ReadNextAsync();
            return response.FirstOrDefault() > 0;
        }
    }
}
