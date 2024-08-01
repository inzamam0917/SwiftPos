using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SwiftPos.Model;

namespace SwiftPos.Services.AuthService
{
    public interface IAuthService
    {
        Task<User> GetUserFromTokenAsync(string token);
        string GenerateJwtToken(User user);
    }
}
