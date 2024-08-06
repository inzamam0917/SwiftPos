using SwiftPos.Model;
using SwiftPos.Repositories.CategoryRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwiftPos.Services.CategoryService
{
    public class CategoryService: ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task AddCategoryAsync(Category category)
        {
            await _categoryRepository.AddCategoryAsync(category);
        }

        public async Task<bool> UpdateCategoryAsync(string id, string name)
        {
            var category = await _categoryRepository.GetCategoryAsync(id);
            if (category == null)
                return false;

            category.Name = name;
            await _categoryRepository.UpdateCategoryAsync(category);
            return true;
        }

        public async Task<bool> RemoveCategoryAsync(string id)
        {
            var category = await _categoryRepository.GetCategoryAsync(id);
            if (category == null)
                return false;

            await _categoryRepository.RemoveCategoryAsync(category);
            return true;
        }

        public async Task<Category> GetCategoryAsync(string id)
        {
            return await _categoryRepository.GetCategoryAsync(id);
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await _categoryRepository.GetAllCategoriesAsync();
        }
    }
}
