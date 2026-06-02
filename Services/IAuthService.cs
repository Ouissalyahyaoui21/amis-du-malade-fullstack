using AmisduMalade.ViewModels;

namespace AmisduMalade.Services
{
    public interface IAuthService
    {
        Task<(string? Token, string? Error)> LoginAsync(LoginVM vm);
        Task<bool> RegisterAsync(RegisterUserVM vm);
    }
}