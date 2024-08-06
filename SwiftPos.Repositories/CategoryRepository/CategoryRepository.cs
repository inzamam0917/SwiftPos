using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using SwiftPos.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwiftPos.Repositories.CategoryRepository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly Container _container;

        public CategoryRepository(CosmosClient cosmosClient, IConfiguration configuration)
        {
            var databaseName = configuration["DatabaseName"]!.ToString();
            _container = cosmosClient.GetContainer(databaseName, "Category");
        }

        public async Task AddCategoryAsync(Category category)
        {
            try
            {
                if (string.IsNullOrEmpty(category.Id))
                {
                    category.Id = Guid.NewGuid().ToString();   
                }
                await _container.CreateItemAsync(category, new PartitionKey(category.Id));
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

        public async Task<Category> GetCategoryAsync(string id)
        {
            try
            {
                ItemResponse<Category> response = await _container.ReadItemAsync<Category>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;  
            }
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            var query = new QueryDefinition("SELECT * FROM c");
            var iterator = _container.GetItemQueryIterator<Category>(query);
            var categories = new List<Category>();
            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                categories.AddRange(response);
            }
            return categories;
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            try
            {
                if (string.IsNullOrEmpty(category.Id))
                {
                    category.Id = Guid.NewGuid().ToString(); 
                }
                await _container.UpsertItemAsync(category, new PartitionKey(category.Id));
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

        public async Task RemoveCategoryAsync(Category category)
        {
            try
            {
                await _container.DeleteItemAsync<Category>(category.Id, new PartitionKey(category.Id));
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
    }
}
