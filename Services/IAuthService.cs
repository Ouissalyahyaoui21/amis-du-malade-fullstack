using AmisduMalade.ViewModels;

namespace AmisduMalade.Services
{
    public interface IAuthService
    {
        Task<string?> LoginAsync(LoginVM vm);
        Task<bool> RegisterAdminAsync(RegisterAdminVM vm);
    }
}