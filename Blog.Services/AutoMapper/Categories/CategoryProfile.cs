using AutoMapper;
using Blog.Entity.Entity;
using Blog.Entity.ViewModels.Categories;

namespace Blog.Services.AutoMapper.Categories
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<CategoryVM, Category>().ReverseMap();
            CreateMap<CategoryAddVM, Category>().ReverseMap();
            CreateMap<CategoryUpdateVM, Category>().ReverseMap();
        }
    }
}
