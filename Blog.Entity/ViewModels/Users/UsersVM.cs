namespace Blog.Entity.ViewModels.Users
{
    public class UsersVM
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public int AccessFailedCount { get; set; }
        public string Role { get; set; }
    }
}
