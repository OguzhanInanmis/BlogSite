using Blog.Entity.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.DAL.Mappings
{
    public class ArticleMap : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {
            builder.Property(x => x.Title).HasMaxLength(150);
            builder.Property(x => x.Title).IsRequired();
            builder.HasData(new Article
            {
                Id = Guid.NewGuid(),
                Title = "ASP.NET Core Deneme Makalesi",
                Content = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                ViewCount = 15,
                CategoryId = Guid.Parse("4BD9F410-8DFF-438D-BE27-D190EB756AC4"),
                ImageId = Guid.Parse("4D949128-E62F-4DD5-A8DD-425D9BBC758F"),
                CreatedBy = "Admin Test",
                CreatedDate = DateTime.Now,
                UserId = Guid.Parse("5459CEC5-9FC7-4120-A179-79BE3D482712")
            },
            new Article
            {
                Id = Guid.NewGuid(),
                Title = "Visual Studio Deneme Makalesi 1",
                Content = "Visual Studio Deneme Makalesi 1 Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                ViewCount = 15,
                CategoryId = Guid.Parse("A15A905F-DC3C-457C-8EB9-34CA43A06521"),
                ImageId = Guid.Parse("AEE078AF-0433-475C-A67E-7196400CCA57"),
                CreatedBy = "Admin Test",
                CreatedDate = DateTime.Now,
                UserId = Guid.Parse("331A8EF9-552F-4F3B-9168-9682763FD6EB"),
            }
            );
        }
    }
}
