using Blog.Entity.Enums;
using Blog.Entity.ViewModels.Images;
using Microsoft.AspNetCore.Http;

namespace Blog.Services.Helpers.Images
{
    public interface IImageHelper
    {
        Task<ImageUploadedVM> Upload(string name, IFormFile imageFile,ImageType imageType,string folderName = null);
        void Delete(string imageName);
    }
}
