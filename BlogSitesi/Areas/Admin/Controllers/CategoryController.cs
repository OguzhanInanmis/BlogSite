using AutoMapper;
using Blog.Entity.Entity;
using Blog.Entity.ViewModels.Categories;
using Blog.Services.Extensions;
using Blog.Services.Services.Abstractions;
using Blog.Web.ResultMessages;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace Blog.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService categoryService;
        private readonly IMapper mapper;
        private readonly IValidator<Category> validator;
        private readonly IToastNotification toast;

        public CategoryController(ICategoryService categoryService, IMapper mapper, IValidator<Category> validator, IToastNotification toast)
        {
            this.categoryService = categoryService;
            this.mapper = mapper;
            this.validator = validator;
            this.toast = toast;
        }
        public async Task<IActionResult> Index()
        {
            var categories = await categoryService.GetAllCategoriesNonDeletedAsync();
            return View(categories);
        }
        public async Task<IActionResult> DeletedCategories()
        {
            var categories = await categoryService.GetAllDeletedCategoriesAsync();
            return View(categories);
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(CategoryAddVM categoryAddVM)
        {
            var map = mapper.Map<Category>(categoryAddVM);
            var result = await validator.ValidateAsync(map);
            if (result.IsValid)
            {
                toast.AddSuccessToastMessage(Messages.Category.Add(categoryAddVM.Name), new ToastrOptions { Title = "Başarılı" });
                await categoryService.CreateCategoryAsync(categoryAddVM);
                return RedirectToAction("Index", "Category", new { Area = "Admin" });

            }
            result.AddToModelState(this.ModelState);
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> AddWithAjax([FromBody] CategoryAddVM categoryAddVM)
        {
            var map = mapper.Map<Category>(categoryAddVM);
            var result = await validator.ValidateAsync(map);
            if (result.IsValid)
            {
                toast.AddSuccessToastMessage(Messages.Category.Add(categoryAddVM.Name), new ToastrOptions { Title = "İşlem Başarılı" });
                await categoryService.CreateCategoryAsync(categoryAddVM);
                return Json(Messages.Category.Add(categoryAddVM.Name));

            }
            else
            {
                toast.AddErrorToastMessage(result.Errors.FirstOrDefault().ErrorMessage, new ToastrOptions { Title = "İşlem Başarısız!" });
                return Json(result.Errors.First().ErrorMessage);
            }

        }



        [HttpGet]
        public async Task<IActionResult> Update(Guid categoryId)
        {
            var category = await categoryService.GetCategoryByGuid(categoryId);
            var map = mapper.Map<Category, CategoryUpdateVM>(category);
            return View(map);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(CategoryUpdateVM categoryUpdateVM)
        {
            var map = mapper.Map<Category>(categoryUpdateVM);
            var result = await validator.ValidateAsync(map);
            var categories = await categoryService.GetAllCategoriesNonDeletedAsync();
            if (result.IsValid)
            {
                var categoryName = await categoryService.UpdateCategoryAsync(categoryUpdateVM);
                toast.AddSuccessToastMessage(Messages.Category.Update(categoryName), new ToastrOptions { Title = "Başarılı" });
                return RedirectToAction("Index", "Category", new { Area = "Admin" });
            }
            else
            {
                result.AddToModelState(this.ModelState);
            }
            return View(categoryUpdateVM);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid categoryId)
        {
            var name = await categoryService.DeleteCategorySafeAsync(categoryId);
            toast.AddSuccessToastMessage(Messages.Category.Update(name), new ToastrOptions { Title = "Başarılı" });
            return RedirectToAction("Index", "Category", new { Area = "Admin" });
        }
        [HttpGet]
        public async Task<IActionResult> UndoDelete(Guid categoryId)
        {
            var name = await categoryService.UndoDeleteCategoryAsync(categoryId);
            toast.AddSuccessToastMessage(Messages.Category.UndoDelete(name), new ToastrOptions { Title = "Başarılı" });
            return RedirectToAction("DeletedCategories", "Category", new { Area = "Admin" });
        }
    }
}