using AutoMapper;
using Blog.Entity.Entity;
using Blog.Entity.ViewModels.Articles;
using Blog.Services.Extensions;
using Blog.Services.Helpers.Images;
using Blog.Services.Services.Abstractions;
using Blog.Web.ResultMessages;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace Blog.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ArticleController : Controller
    {
        private readonly IArticleServices articleServices;
        private readonly ICategoryService categoryService;
        private readonly IMapper mapper;
        private readonly IValidator<Article> validator;
        private readonly IToastNotification toast;

        public ArticleController(IArticleServices articleServices, ICategoryService categoryService, IMapper mapper, IValidator<Article> validator, IToastNotification toast, IImageHelper imageHelper)
        {
            this.articleServices = articleServices;
            this.categoryService = categoryService;
            this.mapper = mapper;
            this.validator = validator;
            this.toast = toast;
        }
        [HttpGet]
        [Authorize(Roles = "SuperAdmin, Admin, User")]
        public async Task<IActionResult> Index()
        {
            var articles = await articleServices.GetAllArticlesWithCategoryNonDeletedAsync();
            return View(articles);
        }
        [HttpGet]
        public async Task<IActionResult> DeletedArticles()
        {
            var articles = await articleServices.GetAllDeletedArticlesWithCategoryAsync();
            return View(articles);
        }
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var categories = await categoryService.GetAllCategoriesNonDeletedAsync();
            return View(new ArticleAddVM { Categories = categories });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(ArticleAddVM articleAddVM)
        {
            var map = mapper.Map<Article>(articleAddVM);
            var result = await validator.ValidateAsync(map);
            if (result.IsValid)
            {
                await articleServices.CreateArticleAsync(articleAddVM);
                toast.AddSuccessToastMessage(Messages.Article.Add(articleAddVM.Title), new ToastrOptions { Title = "Başarılı" });
                return RedirectToAction("Index", "Article", new { Area = "Admin" });
            }
            else
            {
                result.AddToModelState(this.ModelState);
                var categories = await categoryService.GetAllCategoriesNonDeletedAsync();
                return View(new ArticleAddVM { Categories = categories });
            }

        }
        [HttpGet]
        public async Task<IActionResult> Update(Guid articleId)
        {
            var article = await articleServices.GetArticleWithCategoryNonDeletedAsync(articleId);
            var categories = await categoryService.GetAllCategoriesNonDeletedAsync();

            var articleUpdateVM = mapper.Map<ArticleUpdateVM>(article);
            articleUpdateVM.Categories = categories;
            return View(articleUpdateVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(ArticleUpdateVM articleUpdateVM)
        {
            var map = mapper.Map<Article>(articleUpdateVM);
            var result = await validator.ValidateAsync(map);
            var categories = await categoryService.GetAllCategoriesNonDeletedAsync();
            articleUpdateVM.Categories = categories;
            if (result.IsValid)
            {
                var title = await articleServices.UpdateArticleAsync(articleUpdateVM);
                toast.AddSuccessToastMessage(Messages.Article.Update(title), new ToastrOptions { Title = "Başarılı" });
                return RedirectToAction("Index", "Article", new { Area = "Admin" });
            }
            else
            {
                result.AddToModelState(this.ModelState);
            }
            return View(articleUpdateVM);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteSafely(Guid articleId)
        {
            var title = await articleServices.DeleteArticleSafeAsync(articleId);

            toast.AddSuccessToastMessage(Messages.Article.Update(title), new ToastrOptions { Title = "Başarılı" });
            return RedirectToAction("Index", "Article", new { Area = "Admin" });
        }
        [HttpGet]
        public async Task<IActionResult> UndoDelete(Guid articleId)
        {
            var title = await articleServices.UndoDeleteArticleWithCategoryAsync(articleId);

            toast.AddSuccessToastMessage(Messages.Article.UndoDelete(title), new ToastrOptions { Title = "Başarılı" });
            return RedirectToAction("DeletedArticles", "Article", new { Area = "Admin" });
        }
    }
}
