using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SwiftPos.Model;

namespace SwiftPos.Repositories.UserRepository
{
    public interface IUserRepository
    {
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByEmailOrUsernameAsync(string emailOrUsername);
        Task<User> GetUserByUsernameAsync(string username);
        Task<User> GetUserByIdAsync(string userId);
        Task<List<User>> GetAllUsersAsync();
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
    }
}
