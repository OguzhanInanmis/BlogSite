using AutoMapper;
using Blog.Entity.Entity;
using Blog.Entity.ViewModels.Articles;

namespace Blog.Services.AutoMapper.Articles
{
    public class ArticleProfile : Profile
    {
        public ArticleProfile()
        {
            CreateMap<ArticleVM, Article>().ReverseMap();
            CreateMap<ArticleAddVM, Article>().ReverseMap();
            CreateMap<ArticleUpdateVM, Article>().ReverseMap();
            CreateMap<ArticleUpdateVM, ArticleVM>().ReverseMap();
        }
    }
}
