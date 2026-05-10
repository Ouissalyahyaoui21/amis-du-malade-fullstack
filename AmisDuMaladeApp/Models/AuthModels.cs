namespace AmisDuMaladeApp.Models;

public class LoginRequest
{
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
}

public class LoginResponse
{
    public string Message { get; set; } = "";
    public string Token { get; set; } = "";
}

public class RegisterUserRequest
{
    public string FullName { get; set; } = "";
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
    public string Role { get; set; } = "Admin";
    public Guid? BranchId { get; set; }
}
