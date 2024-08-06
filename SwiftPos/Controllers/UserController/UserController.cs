using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SwiftPos.Dto;
using SwiftPos.Model;
using SwiftPos.Services.AuthService;
using SwiftPos.Services.UserService;

namespace SwiftPos.Controllers.UserController
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, IAuthService authService, ILogger<UserController> logger)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDTO userDto)
        {
            try
            {
                var user = new User
                {
                    Username = userDto.Username,
                    Email = userDto.Email,
                    Password = userDto.Password,
                    UserRole = (UserRole)userDto.UserRole,
                    Name = userDto.Name,
                };

                await _userService.RegisterUserAsync(user);
                return Ok(new { message = "User registered successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registering user");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthDTO authModel)
        {
            var user = await _userService.AuthenticateUserAsync(authModel.EmailOrUsername, authModel.Password);
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid email/username or password" });
            }

            var token = _authService.GenerateJwtToken(user);
            _logger.LogInformation($"User Details: Id={user.UserID}, Name={user.Username}, Role={user.UserRole}");

            return Ok(new { token });
        }

        [Authorize]
        [HttpPost("setrole")]
        public async Task<IActionResult> SetRole([FromBody] SetRoleDTO model)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var userFromToken = await _authService.GetUserFromTokenAsync(token);

            if (userFromToken == null || userFromToken.UserRole != UserRole.Admin)
            {
                return Unauthorized(new { message = "Only admins can update roles" });
            }

            var success = await _userService.UpdateUserRoleAsync(model.Username, model.Role);
            if (!success)
            {
                return NotFound(new { message = "User not found" });
            }

            return Ok(new
            {
                message = "User role updated successfully",
            });
        }

        [HttpGet("allusers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            var result = users.Select(user => new UserDTO
            {
                Username = user.Username,
                Email = user.Email,
                UserRole = (int)user.UserRole,
                Name = user.Name,
            }).ToList();

            return Ok(result);
        }

    }
}
