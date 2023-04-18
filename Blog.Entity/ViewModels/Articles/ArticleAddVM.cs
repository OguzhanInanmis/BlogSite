using Blog.Entity.ViewModels.Categories;
using Microsoft.AspNetCore.Http;

namespace Blog.Entity.ViewModels.Articles
{
    public class ArticleAddVM
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public Guid CategoryId { get; set; }
        public IFormFile Photo { get; set; }
        public IList<CategoryVM> Categories { get; set; }
    }
}
