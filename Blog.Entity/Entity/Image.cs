using Blog.Core.Entities;
using Blog.Entity.Enums;

namespace Blog.Entity.Entity
{
    public class Image : EntityBase
    {
        public Image() { }
        public Image(string fileName, string fileType,string createdby)
        {
            FileName = fileName;
            FileType = fileType;
            CreatedBy = createdby;
        }
        public string FileName { get; set; }
        public string FileType { get; set; }
        //public ImageType ImageType { get; set; }
        public ICollection<Article> Articles { get; set; }
        public ICollection<AppUser> Users { get; set; }
    }
}
