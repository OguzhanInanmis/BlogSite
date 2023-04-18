using Blog.Entity.Entity;
using Blog.Entity.ViewModels.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Services.Services.Abstractions
{
    public interface ICategoryService
    {
        Task<List<CategoryVM>> GetAllCategoriesNonDeletedAsync();
        Task<List<CategoryVM>> GetAllDeletedCategoriesAsync();
        Task<Category> GetCategoryByGuid(Guid categoryId);
        Task CreateCategoryAsync(CategoryAddVM categoryAddVM);
        Task<string> UpdateCategoryAsync(CategoryUpdateVM categoryUpdateVM);
        Task<string> DeleteCategorySafeAsync(Guid categoryId);
        Task<string> UndoDeleteCategoryAsync(Guid categoryId);
    }
}
