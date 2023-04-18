using AutoMapper;
using Blog.Entity.Entity;
using Blog.Entity.ViewModels.Users;

namespace Blog.Services.AutoMapper.Users
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<AppUser, UsersVM>().ReverseMap();
            CreateMap<AppUser, UserAddVM>().ReverseMap();
            CreateMap<AppUser, UserUpdateVM>().ReverseMap();
        }
    }
}
