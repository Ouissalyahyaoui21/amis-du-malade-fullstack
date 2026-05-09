using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AmisDuMaladeApp.Models;
using AmisDuMaladeApp.Services;
using AmisDuMaladeApp.Views;

namespace AmisDuMaladeApp.ViewModels;

public partial class LoginViewModel : BaseViewModel
{
    private readonly ApiService _api;
    private readonly AuthTokenService _authToken;

    [ObservableProperty] private string email = "";
    [ObservableProperty] private string password = "";

    public string TitleLabel    => Loc.Get("login_title");
    public string EmailLabel    => Loc.Get("login_email");
    public string PasswordLabel => Loc.Get("login_password");
    public string LoginBtnLabel => Loc.Get("login_btn");

    public LoginViewModel(ApiService api, AuthTokenService authToken, LocalizationService loc)
        : base(loc)
    {
        _api = api;
        _authToken = authToken;
        Loc.LanguageChanged += (_, _) =>
        {
            OnPropertyChanged(nameof(TitleLabel));
            OnPropertyChanged(nameof(EmailLabel));
            OnPropertyChanged(nameof(PasswordLabel));
            OnPropertyChanged(nameof(LoginBtnLabel));
            OnPropertyChanged(nameof(FlowDirection));
        };
    }

    [RelayCommand]
    private async Task LoginAsync()
    {
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            await ShowErrorAsync(Loc.Get("required_field"));
            return;
        }

        IsBusy = true;
        try
        {
            var result = await _api.LoginAsync(new LoginRequest { Email = Email, Password = Password });
            if (result != null && !string.IsNullOrEmpty(result.Token))
            {
                _authToken.SaveToken(result.Token);
                await Shell.Current.GoToAsync($"//{nameof(DashboardPage)}");
            }
            else
                await ShowErrorAsync(Loc.Get("error"));
        }
        finally { IsBusy = false; }
    }
}
