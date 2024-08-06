using SwiftPos.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwiftPos.Services.CategoryService
{
    public interface ICategoryService
    {
        Task AddCategoryAsync(Category category);
        Task<bool> UpdateCategoryAsync(string id, string name);
        Task<bool> RemoveCategoryAsync(string id);
        Task<Category> GetCategoryAsync(string id);
        Task<List<Category>> GetAllCategoriesAsync();
    }
}
