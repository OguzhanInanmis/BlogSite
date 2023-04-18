using Blog.Entity.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.DAL.Mappings
{
    public class UserRoleMap : IEntityTypeConfiguration<AppUserRole>
    {
        public void Configure(EntityTypeBuilder<AppUserRole> builder)
        {
            // Primary key
            builder.HasKey(r => new { r.UserId, r.RoleId });

            // Maps to the AspNetUserRoles table
            builder.ToTable("AspNetUserRoles");

            builder.HasData(new AppUserRole
            {
                UserId = Guid.Parse("5459CEC5-9FC7-4120-A179-79BE3D482712"),
                RoleId = Guid.Parse("49EE7DC2-C5CD-46B9-BF41-63F1D72A1520"),
            },
            new AppUserRole
            {
                UserId = Guid.Parse("331A8EF9-552F-4F3B-9168-9682763FD6EB"),
                RoleId = Guid.Parse("B2C252AD-73F9-4517-8120-DD2D6EA3350C"),

            });
        }
    }
}
