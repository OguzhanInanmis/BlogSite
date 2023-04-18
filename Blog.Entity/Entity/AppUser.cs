using Microsoft.AspNetCore.Identity;

namespace Blog.Entity.Entity
{
    public class AppUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid ImageId { get; set; } = Guid.Parse("4D949128-E62F-4DD5-A8DD-425D9BBC758F");
        public Image Image { get; set; }
        public ICollection<Article> Articles { get; set; }
    }
}
