using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SwiftPos.Dto;
using SwiftPos.Services.CategoryService;
using System.Threading.Tasks;
using SwiftPos.Model;
using SwiftPos.Services.AuthService;
using SwiftPos.Services.UserService;

namespace SwiftPos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IAuthService _authService;

        public CategoryController(ICategoryService categoryService, IAuthService authService)
        {
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService)); ;
            _authService = authService ?? throw new ArgumentNullException(nameof(authService)); ;
        }

        [HttpPost("add")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddCategoryAsync([FromBody] CategoryDTO categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var category = new Category
            {
                categoryid = categoryDto.categoryid,
                Name = categoryDto.Name
            };

            await _categoryService.AddCategoryAsync(category);
            return Ok(new { message = "Category added successfully" });
        }

        [HttpPut("update/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCategoryAsync(string id, [FromBody] CategoryDTO categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = await _categoryService.UpdateCategoryAsync(id, categoryDto.Name);
            if (!success)
            {
                return NotFound(new { message = "Category not found" });
            }
            return Ok(new { message = "Category updated successfully" });
        }

        [HttpDelete("remove/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveCategoryAsync(string id)
        {
            var success = await _categoryService.RemoveCategoryAsync(id);
            if (!success)
            {
                return NotFound(new { message = "Category not found" });
            }
            return Ok(new { message = "Category removed successfully" });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryAsync(string id)
        {
            var category = await _categoryService.GetCategoryAsync(id);
            if (category == null)
            {
                return NotFound(new { message = "Category not found" });
            }
            return Ok(category);
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAllCategoriesAsync()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            Console.WriteLine("In Controller");
            return Ok(categories);
        }
    }
}
