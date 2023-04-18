using AutoMapper;
using Azure.Core;
using Blog.DAL.Repositories.IRepositories;
using Blog.Entity.Entity;
using Blog.Entity.Enums;
using Blog.Entity.ViewModels.Articles;
using Blog.Entity.ViewModels.Categories;
using Blog.Services.Extensions;
using Blog.Services.Helpers.Images;
using Blog.Services.Services.Abstractions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Blog.Services.Services.Concrete
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor contextAccessor;
        private readonly ClaimsPrincipal _user;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor contextAccessor)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.contextAccessor = contextAccessor;
            _user = contextAccessor.HttpContext.User;
        }
        public async Task<List<CategoryVM>> GetAllCategoriesNonDeletedAsync()
        {
            var categories = await unitOfWork.GetRepository<Category>().GetAllAsync(x => !x.IsDeleted);
            var map = mapper.Map<List<CategoryVM>>(categories);
            return map;
        }

        public async Task<Category> GetCategoryByGuid(Guid categoryId)
        {
            var category = await unitOfWork.GetRepository<Category>().GetByGuidAsync(categoryId);
            return category;
        }
        public async Task CreateCategoryAsync(CategoryAddVM categoryAddVM)
        {
            //var userId = Guid.Parse("5459CEC5-9FC7-4120-A179-79BE3D482712");
            var userId = _user.GetLoggedInUserId();
            var email = _user.GetLoggedInEmail();

            var category = new Category()
            {
                CreatedBy = email,
                Name = categoryAddVM.Name,
            };

            await unitOfWork.GetRepository<Category>().AddAsync(category);
            await unitOfWork.SaveAsync();
        }

        public async Task<string> UpdateCategoryAsync(CategoryUpdateVM categoryUpdateVM)
        {
            var email = _user.GetLoggedInEmail();
            var category = await unitOfWork.GetRepository<Category>().GetByGuidAsync(categoryUpdateVM.ID);
            category.Name = categoryUpdateVM.Name;
            category.ModifiedBy = email;
            category.UpdatedDate = DateTime.Now;
            await unitOfWork.GetRepository<Category>().UpdateAsync(category);
            await unitOfWork.SaveAsync();
            return category.Name;
        }

        public async Task<string> DeleteCategorySafeAsync(Guid categoryId)
        {
            var email = _user.GetLoggedInEmail();
            var category = await unitOfWork.GetRepository<Category>().GetByGuidAsync(categoryId);
            category.IsDeleted = true;
            category.DeletedDate = DateTime.Now;
            category.DeletedBy = email;

            await unitOfWork.GetRepository<Category>().UpdateAsync(category);
            await unitOfWork.SaveAsync();
            return category.Name;
        }

        public async Task<List<CategoryVM>> GetAllDeletedCategoriesAsync()
        {
            var categories = await unitOfWork.GetRepository<Category>().GetAllAsync(x => x.IsDeleted);
            var map = mapper.Map<List<CategoryVM>>(categories);
            return map;
        }

        public async Task<string> UndoDeleteCategoryAsync(Guid categoryId)
        {
            var category = await unitOfWork.GetRepository<Category>().GetByGuidAsync(categoryId);
            category.IsDeleted = false;
            category.DeletedDate = null;
            category.DeletedBy = null;

            await unitOfWork.GetRepository<Category>().UpdateAsync(category);
            await unitOfWork.SaveAsync();
            return category.Name;
        }
    }
}
