using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using SwiftPos.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwiftPos.Repositories.SaleRepository
{
    public class SaleRepository : ISaleRepository
    {
        private readonly Container _userContainer;
        private readonly Container _productContainer;
        private readonly Container _saleContainer;

        public SaleRepository(CosmosClient cosmosClient, IConfiguration configuration)
        {
            var databaseName = configuration["DatabaseName"]!.ToString();
            _userContainer = cosmosClient.GetContainer(databaseName, "User");
            _productContainer = cosmosClient.GetContainer(databaseName, "Product");
            _saleContainer = cosmosClient.GetContainer(databaseName, "Sale");
        }

        public async Task<SwiftPos.Model.User> GetUserByIdAsync(string userId)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.id = @userId")
                .WithParameter("@userId", userId);
            var iterator = _userContainer.GetItemQueryIterator<SwiftPos.Model.User>(query);
            var response = await iterator.ReadNextAsync();
            return response.FirstOrDefault();
        }

        public async Task<Product> GetProductByIdAsync(string productId)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.id = @productId")
                .WithParameter("@productId", productId);
            var iterator = _productContainer.GetItemQueryIterator<Product>(query);
            var response = await iterator.ReadNextAsync();
            return response.FirstOrDefault();
        }

        public async Task AddSaleAsync(Sale sale)
        {
            await _saleContainer.CreateItemAsync(sale, new PartitionKey(sale.SaleId));
        }

        public async Task UpdateSaleAsync(Sale sale)
        {
            await _saleContainer.UpsertItemAsync(sale, new PartitionKey(sale.SaleId));
        }

        public async Task UpdateProductAsync(Product product)
        {
            await _productContainer.UpsertItemAsync(product, new PartitionKey(product.ProductId));
        }
    }
}