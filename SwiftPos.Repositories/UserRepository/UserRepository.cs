using SwiftPos.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Container = Microsoft.Azure.Cosmos.Container;
using Microsoft.Extensions.Configuration;

namespace SwiftPos.Repositories.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly Container _container;

        public UserRepository(CosmosClient cosmosClient, IConfiguration configuration)
        {
            var databaseName = configuration["CosmosDb:DatabaseName"];
            _container = cosmosClient.GetContainer(databaseName, "User");
        }

        public async Task<SwiftPos.Model.User> GetUserByEmailAsync(string email)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.Email = @Email")
                .WithParameter("@Email", email);
            var queryResultSetIterator = _container.GetItemQueryIterator<SwiftPos.Model.User>(query);
            var response = await queryResultSetIterator.ReadNextAsync();
            return response.FirstOrDefault();
        }

        public async Task<SwiftPos.Model.User> GetUserByEmailOrUsernameAsync(string emailOrUsername)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.Email = @EmailOrUsername OR c.Username = @EmailOrUsername")
                .WithParameter("@EmailOrUsername", emailOrUsername);
            var queryResultSetIterator = _container.GetItemQueryIterator<SwiftPos.Model.User>(query);
            var response = await queryResultSetIterator.ReadNextAsync();
            return response.FirstOrDefault();
        }

        public async Task<SwiftPos.Model.User> GetUserByUsernameAsync(string username)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.Username = @Username")
                .WithParameter("@Username", username);
            var queryResultSetIterator = _container.GetItemQueryIterator<SwiftPos.Model.User>(query);
            var response = await queryResultSetIterator.ReadNextAsync();
            return response.FirstOrDefault();
        }

        public async Task<SwiftPos.Model.User> GetUserByIdAsync(string userId)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.UserID = @UserId")
                .WithParameter("@UserId", userId);
            var queryResultSetIterator = _container.GetItemQueryIterator<SwiftPos.Model.User>(query);
            var response = await queryResultSetIterator.ReadNextAsync();
            return response.FirstOrDefault();
        }

        public async Task<List<SwiftPos.Model.User>> GetAllUsersAsync()
        {
            var query = new QueryDefinition("SELECT * FROM c");
            var queryResultSetIterator = _container.GetItemQueryIterator<SwiftPos.Model.User>(query);
            var users = new List<SwiftPos.Model.User>();
            while (queryResultSetIterator.HasMoreResults)
            {
                var response = await queryResultSetIterator.ReadNextAsync();
                users.AddRange(response);
            }
            return users;
        }

        public async Task AddUserAsync(SwiftPos.Model.User user)
        {
            try
            {
                if (string.IsNullOrEmpty(user.id))
                {
                    user.id = Guid.NewGuid().ToString(); 
                }

                var partitionKeyValue = user.id;

                await _container.CreateItemAsync(user, new PartitionKey(partitionKeyValue));
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

        public async Task UpdateUserAsync(SwiftPos.Model.User user)
        {
            try
            {
                if (string.IsNullOrEmpty(user.UserID))
                {
                    user.UserID = Guid.NewGuid().ToString();
                }

                var partitionKeyValue = user.UserID;
                await _container.UpsertItemAsync(user, new PartitionKey(partitionKeyValue));
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
