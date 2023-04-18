using AutoMapper;
using Blog.DAL.Repositories.IRepositories;
using Blog.Entity.Entity;
using Blog.Entity.Enums;
using Blog.Entity.ViewModels.Articles;
using Blog.Services.Extensions;
using Blog.Services.Helpers.Images;
using Blog.Services.Services.Abstractions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Blog.Services.Services.Concrete
{
    public class ArticleServices : IArticleServices
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor contextAccessor;
        private readonly IImageHelper imageHelper;
        private readonly ClaimsPrincipal _user;

        public ArticleServices(IUnitOfWork unitOfWork,IMapper mapper,IHttpContextAccessor contextAccessor,IImageHelper imageHelper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.contextAccessor = contextAccessor;
            this.imageHelper = imageHelper;
            _user = contextAccessor.HttpContext.User;
        }

        public async Task CreateArticleAsync(ArticleAddVM articleAddVM)
        {
            //var userId = Guid.Parse("5459CEC5-9FC7-4120-A179-79BE3D482712");
            var userId = _user.GetLoggedInUserId();
            var email= _user.GetLoggedInEmail();

            var imageUpload = await imageHelper.Upload(articleAddVM.Title, articleAddVM.Photo, ImageType.Post);
            Image image = new(imageUpload.FullName,articleAddVM.Photo.ContentType,email);
            await unitOfWork.GetRepository<Image>().AddAsync(image);
            var article = new Article
            {
                Title = articleAddVM.Title,
                Content = articleAddVM.Content,
                CategoryId = articleAddVM.CategoryId,
                UserId = userId,
                CreatedBy = email,
                ImageId = image.Id                
            };
            await unitOfWork.GetRepository<Article>().AddAsync(article);
            await unitOfWork.SaveAsync();           

        }

        public async Task<List<ArticleVM>> GetAllArticlesWithCategoryNonDeletedAsync()
        {
            var articles = await unitOfWork.GetRepository<Article>().GetAllAsync(x=>!x.IsDeleted,x=>x.Category);
            var map = mapper.Map<List<ArticleVM>>(articles);
            return map;
        }

        public async Task <ArticleVM> GetArticleWithCategoryNonDeletedAsync(Guid articleId)
        {
            var articles = await unitOfWork.GetRepository<Article>().GetAsync(x=>!x.IsDeleted && x.Id == articleId,x=>x.Category,i=>i.Image);
            var map = mapper.Map<ArticleVM>(articles);
            return map;
        }
        public async Task<string> UpdateArticleAsync(ArticleUpdateVM articleUpdateVM)
        {
            var email = _user.GetLoggedInEmail();
            var article = await unitOfWork.GetRepository<Article>().GetAsync(x=>!x.IsDeleted && x.Id == articleUpdateVM.ID,x=>x.Category,i=>i.Image);

            if(articleUpdateVM.Photo != null)
            {
                imageHelper.Delete(article.Image.FileName);
                var imageUpload = await imageHelper.Upload(articleUpdateVM.Title, articleUpdateVM.Photo,ImageType.Post);
                Image image = new(imageUpload.FullName, articleUpdateVM.Photo.ContentType, email);
                await unitOfWork.GetRepository<Image>().AddAsync(image);
                article.ImageId = image.Id;
            }

            article.Title = articleUpdateVM.Title;
            article.Content = articleUpdateVM.Content;
            article.CategoryId = articleUpdateVM.CategoryId;
            article.ModifiedBy = email;
            article.UpdatedDate = DateTime.Now;
            await unitOfWork.GetRepository<Article>().UpdateAsync(article);
            await unitOfWork.SaveAsync();
            return article.Title;
        }

        public async Task<string> DeleteArticleSafeAsync(Guid articleId)
        {
            var email = _user.GetLoggedInEmail();
            var article = await unitOfWork.GetRepository<Article>().GetByGuidAsync(articleId);
            article.IsDeleted = true;
            article.DeletedDate = DateTime.Now;
            article.DeletedBy = email;

            await unitOfWork.GetRepository<Article>().UpdateAsync(article); 
            await unitOfWork.SaveAsync();
            return article.Title;
        }

        public async Task<List<ArticleVM>> GetAllDeletedArticlesWithCategoryAsync()
        {
            var articles = await unitOfWork.GetRepository<Article>().GetAllAsync(x => x.IsDeleted, x => x.Category);
            var map = mapper.Map<List<ArticleVM>>(articles);
            return map;
        }

        public async Task<string> UndoDeleteArticleWithCategoryAsync(Guid articleId)
        {
            var article = await unitOfWork.GetRepository<Article>().GetByGuidAsync(articleId);
            article.IsDeleted = false;
            article.DeletedDate = null;
            article.DeletedBy = null;

            await unitOfWork.GetRepository<Article>().UpdateAsync(article);
            await unitOfWork.SaveAsync();
            return article.Title;
        }
    }
}
