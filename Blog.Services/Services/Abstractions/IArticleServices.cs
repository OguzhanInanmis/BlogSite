using Blog.Entity.ViewModels.Articles;

namespace Blog.Services.Services.Abstractions
{
    public interface IArticleServices
    {
        Task<List<ArticleVM>> GetAllArticlesWithCategoryNonDeletedAsync();
        Task<List<ArticleVM>> GetAllDeletedArticlesWithCategoryAsync();
        Task CreateArticleAsync(ArticleAddVM articleAddVM);
        Task<ArticleVM> GetArticleWithCategoryNonDeletedAsync(Guid articleId);
        Task<string> UndoDeleteArticleWithCategoryAsync(Guid articleId);
        Task<string> UpdateArticleAsync(ArticleUpdateVM articleUpdateVM);
        Task<string> DeleteArticleSafeAsync(Guid articleId);
    }
}
