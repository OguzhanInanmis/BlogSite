using Blog.Services.Services.Abstractions;
using BlogSitesi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BlogSitesi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IArticleServices articleServices;

        public HomeController(ILogger<HomeController> logger,IArticleServices articleService)
        {
            _logger = logger;
            this.articleServices = articleService;
        }

        public async Task<IActionResult> Index()
        {
            var articles = await articleServices.GetAllArticlesWithCategoryNonDeletedAsync();
            return View(articles);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}