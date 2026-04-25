namespace AmisduMalade.ViewModels
{
    public class LoginVM
    {
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
    }

    public class RegisterUserVM
    {
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public string Role { get; set; } = "Admin";
        public Guid? BranchId { get; set; }
    }
}