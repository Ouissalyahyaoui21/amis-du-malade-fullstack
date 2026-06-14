using AmisDuMaladeApp.Constants;

namespace AmisDuMaladeApp.Services;

public class AuthTokenService
{
    public string? Token => Preferences.Get(AppConstants.TokenKey, null);

    public void SaveToken(string token) => Preferences.Set(AppConstants.TokenKey, token);

    public void ClearToken() => Preferences.Remove(AppConstants.TokenKey);

    public bool IsLoggedIn => !string.IsNullOrEmpty(Token);
}
