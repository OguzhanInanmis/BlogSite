using Blog.Entity.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.DAL.Mappings
{
    public class ImageMap : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.HasData(new Image
            {
                Id = Guid.Parse("4D949128-E62F-4DD5-A8DD-425D9BBC758F"),
                FileName = "Images/testimage",
                FileType = "jpg",
                CreatedBy = "Admin Test",
                CreatedDate = DateTime.Now,
                IsDeleted = false
            },
            new Image
            {
                Id = Guid.Parse("AEE078AF-0433-475C-A67E-7196400CCA57"),
                FileName = "Images/imgtest",
                FileType = "png",
                CreatedBy = "Admin Test",
                CreatedDate = DateTime.Now,
                IsDeleted = false
            }
                );

        }

    }
}
