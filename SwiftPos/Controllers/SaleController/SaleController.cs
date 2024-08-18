using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SwiftPos.Services.SaleService;
using SwiftPos.Model;
using SwiftPos.Dto;
using System.Threading.Tasks;
using SwiftPos.Services.AuthService;

namespace SwiftPos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SaleController : ControllerBase
    {
        private readonly ISaleService _saleService;
        private readonly IAuthService _authService;

        public SaleController(ISaleService saleService, IAuthService authService)
        {
            _saleService = saleService;
            _authService = authService;
        }

        [HttpPost("start")]
        [Authorize]
        public async Task<IActionResult> StartSale()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var cashier = await _authService.GetUserFromTokenAsync(token);

            if (cashier == null || cashier.UserRole != UserRole.Cashier)
            {
                return Unauthorized(new { message = "You are not authorized to perform this action." });
            }

            var success = await _saleService.StartSaleAsync(cashier.UserID);
            if (!success)
            {
                return BadRequest(new { message = "Failed to start sale." });
            }

            return Ok(new { message = "Sale started successfully." });
        }

        [HttpPost("addproduct")]
        [Authorize]
        public async Task<IActionResult> AddProductToSale([FromBody] SaleProductDTO saleProduct)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var cashier = await _authService.GetUserFromTokenAsync(token);

            if (cashier == null || cashier.UserRole != UserRole.Cashier)
            {
                return Unauthorized(new { message = "You are not authorized to perform this action." });
            }

            var sale = await _saleService.AddProductToSaleAsync(cashier.UserID, saleProduct.ProductId, saleProduct.Quantity);
            if (!sale)
            {
                return BadRequest(new { message = "Failed to add product to sale." });
            }

            return Ok(new { message = "Product added to sale successfully." });
        }

        [HttpPost("removeproduct")]
        [Authorize]
        public async Task<IActionResult> RemoveProductFromSale([FromBody] SaleProductDTO saleProduct)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var cashier = await _authService.GetUserFromTokenAsync(token);

            if (cashier == null || cashier.UserRole != UserRole.Cashier)
            {
                return Unauthorized(new { message = "You are not authorized to perform this action." });
            }

            var success = await _saleService.RemoveProductFromSaleAsync(cashier.UserID, saleProduct.ProductId, saleProduct.Quantity);
            if (!success)
            {
                return BadRequest(new { message = "Failed to remove product from sale." });
            }

            return Ok(new { message = "Product removed from sale successfully." });
        }

        [HttpPost("complete")]
        [Authorize]
        public async Task<IActionResult> CompleteSale([FromQuery] string saleId)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var cashier = await _authService.GetUserFromTokenAsync(token);

            if (cashier == null || cashier.UserRole != UserRole.Cashier)
            {
                return Unauthorized(new { message = "You are not authorized to perform this action." });
            }

            var success = await _saleService.CompleteSaleAsync(saleId);
            if (!success)
            {
                return BadRequest(new { message = "Failed to complete sale." });
            }

            return Ok(new { message = "Sale completed successfully." });
        }

        [HttpGet("totalamount")]
        [Authorize]
        public async Task<IActionResult> GetTotalAmount([FromQuery] string saleId)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var cashier = await _authService.GetUserFromTokenAsync(token);

            if (cashier == null || cashier.UserRole != UserRole.Cashier)
            {
                return Unauthorized(new { message = "You are not authorized to perform this action." });
            }

            var totalAmount = await _saleService.GetTotalAmountAsync(saleId);
            return Ok(new { totalAmount });
        }
    }
}
