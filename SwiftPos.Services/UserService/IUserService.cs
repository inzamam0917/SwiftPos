using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SwiftPos.Model;

namespace SwiftPos.Services.UserService
{
    public interface IUserService
    {
        Task RegisterUserAsync(User user);
        Task<User> AuthenticateUserAsync(string emailOrUsername, string password);
        Task<bool> UpdateUserRoleAsync(string username, UserRole role);
        Task<List<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(string userId);
    }
}
