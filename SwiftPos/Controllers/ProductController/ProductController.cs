using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SwiftPos.Model;
using SwiftPos.Services.ProductService;
using SwiftPos.AutoMapper;
using SwiftPos.Services.AuthService;
using System;
using SwiftPos.Dto;

namespace SwiftPos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IAuthService _authService;

        public ProductController(IProductService productService, IAuthService authService)
        {
            _productService = productService;
            _authService = authService;
        }

        [HttpPost("add")]
        [Authorize]
        public async Task<IActionResult> AddProduct([FromBody] ProductDTO productmodel)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var admin = await _authService.GetUserFromTokenAsync(token);

            if (admin == null || admin.UserRole != UserRole.Admin)
            {
                return Unauthorized(new { message = "You are not authorized to perform this action." });
            }

            try
            {
                var product = new Product
                {
                    Name = productmodel.Name,
                    Price = productmodel.Price,
                    Quantity = productmodel.Quantity,
                    Type = productmodel.Type,
                    CategoryId = productmodel.CategoryId
                };

                await _productService.AddProductAsync(product);
                return Ok(new { message = "Product added successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("update")]
        [Authorize]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductDTO productmodel)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var admin = await _authService.GetUserFromTokenAsync(token);

            if (admin == null || admin.UserRole != UserRole.Admin)
            {
                return Unauthorized(new { message = "You are not authorized to perform this action." });
            }

            try
            {
                var product = new Product
                {
                    ProductId = productmodel.ProductId,
                    Name = productmodel.Name,
                    Price = productmodel.Price,
                    Quantity = productmodel.Quantity,
                    Type = productmodel.Type,
                    CategoryId = productmodel.CategoryId
                };

                await _productService.UpdateProductAsync(product);
                return Ok(new { message = "Product updated successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("delete/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var admin = await _authService.GetUserFromTokenAsync(token);

            if (admin == null || admin.UserRole != UserRole.Admin)
            {
                return Unauthorized(new { message = "You are not authorized to perform this action." });
            }

            try
            {
                await _productService.RemoveProductAsync(id);
                return Ok(new { message = "Product deleted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("get/{id}")]
        [Authorize]
        public async Task<IActionResult> GetProduct(string id)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var admin = await _authService.GetUserFromTokenAsync(token);

            if (admin == null || admin.UserRole != UserRole.Admin)
            {
                return Unauthorized(new { message = "You are not authorized to perform this action." });
            }

            var product = await _productService.GetProductAsync(id);
            if (product == null)
            {
                return NotFound(new { message = "Product not found" });
            }

            return Ok(product);
        }

        [HttpGet("getall")]
        [Authorize]
        public async Task<IActionResult> GetAllProducts()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var admin = await _authService.GetUserFromTokenAsync(token);

            if (admin == null || admin.UserRole != UserRole.Admin)
            {
                return Unauthorized(new { message = "You are not authorized to perform this action." });
            }

            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpPut("updatequantity")]
        [Authorize]
        public async Task<IActionResult> UpdateQuantity([FromBody] UpdateQuantityDTO model)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var user = await _authService.GetUserFromTokenAsync(token);

            if (user == null || (user.UserRole != UserRole.Admin && user.UserRole != UserRole.Cashier))
            {
                return Unauthorized(new { message = "You are not authorized to perform this action." });
            }

            try
            {
                var product = await _productService.GetProductAsync(model.ProductId);

                if (product == null)
                {
                    return NotFound(new { message = "Product not found" });
                }

                product.Quantity = model.Quantity;
                await _productService.UpdateProductAsync(product);

                return Ok(new { message = "Product quantity updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
