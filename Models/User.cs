namespace AmisduMalade.Models
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid? BranchId { get; set; }
        public AssociationBranch? Branch { get; set; }
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public string PasswordHash { get; set; } = "";
        public string Role { get; set; } = "Admin";
        // Admin / Coordinator
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}