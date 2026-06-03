using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AmisDuMaladeApp.Models;
using AmisDuMaladeApp.Services;
using AmisDuMaladeApp.Views;

namespace AmisDuMaladeApp.ViewModels;

public partial class AdminLoginViewModel : BaseViewModel
{
    private readonly ApiService _api;
    private readonly AuthTokenService _auth;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsUsernameTab))]
    private bool isEmailTab = true;
    public bool IsUsernameTab => !IsEmailTab;

    [ObservableProperty] private string email    = "";
    [ObservableProperty] private string username = "";
    [ObservableProperty] private string password = "";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsPasswordHidden))]
    [NotifyPropertyChangedFor(nameof(PasswordToggleIcon))]
    private bool isPasswordVisible;

    public bool   IsPasswordHidden   => !IsPasswordVisible;
    public string PasswordToggleIcon => IsPasswordVisible ? "🙈" : "👁️";

    [ObservableProperty] private bool rememberMe = true;

    [ObservableProperty] private bool   hasLoginError;
    [ObservableProperty] private string loginErrorText      = "";
    [ObservableProperty] private bool   showAttemptsWarning;
    [ObservableProperty] private string attemptsWarningText = "";
    [ObservableProperty] private bool   isLocked;
    [ObservableProperty] private bool   showForgotDialog;

    private int _failedAttempts;

    public AdminLoginViewModel(ApiService api, AuthTokenService auth, LocalizationService loc)
        : base(loc)
    {
        _api  = api;
        _auth = auth;
    }

    [RelayCommand]
    private void SetEmailTab()    { IsEmailTab = true;  ClearLoginErrors(); }

    [RelayCommand]
    private void SetUsernameTab() { IsEmailTab = false; ClearLoginErrors(); }

    [RelayCommand]
    private void TogglePassword() => IsPasswordVisible = !IsPasswordVisible;

    [RelayCommand]
    private void ToggleRememberMe() => RememberMe = !RememberMe;

    [RelayCommand]
    private void ShowForgotPassword() => ShowForgotDialog = true;

    [RelayCommand]
    private void CloseForgotDialog() => ShowForgotDialog = false;

    [RelayCommand]
    private async Task LoginAsync()
    {
        if (IsLocked || IsBusy) return;

        var credential = IsEmailTab ? Email.Trim() : Username.Trim();
        if (string.IsNullOrWhiteSpace(credential) || string.IsNullOrWhiteSpace(Password))
        {
            SetLoginError("يرجى تعبئة جميع الحقول المطلوبة.");
            return;
        }

        IsBusy = true;
        ClearLoginErrors();
        try
        {
            var request = new LoginRequest
            {
                Email    = credential,
                Password = Password
            };

            var result = await _api.LoginAsync(request);

            if (result != null && !string.IsNullOrEmpty(result.Token))
            {
                _auth.SaveToken(result.Token);
                _failedAttempts = 0;
                await Shell.Current.GoToAsync(nameof(AdminDashboardPage));
            }
            else
            {
                _failedAttempts++;
                if (_failedAttempts >= 5)
                {
                    SetLoginError("تم تأمين الحساب مؤقتاً. أعد المحاولة بعد قليل.");
                    ShowAttemptsWarning = false;
                    IsLocked = true;
                    _ = Task.Delay(TimeSpan.FromMinutes(5)).ContinueWith(_ =>
                    {
                        IsLocked = false; _failedAttempts = 0;
                    });
                }
                else
                {
                    SetLoginError("بيانات الدخول غير صحيحة. تحقق وأعد المحاولة.");
                    var left = 5 - _failedAttempts;
                    if (left <= 2)
                    {
                        AttemptsWarningText = left == 1
                            ? "تبقّت لك محاولة واحدة قبل تأمين الحساب مؤقتاً."
                            : $"تبقّت لك {left} محاولتان قبل تأمين الحساب مؤقتاً.";
                        ShowAttemptsWarning = true;
                    }
                }
            }
        }
        finally { IsBusy = false; }
    }

    private void SetLoginError(string msg) { LoginErrorText = msg; HasLoginError = true; }

    private void ClearLoginErrors()
    {
        HasLoginError = false; LoginErrorText = ""; ShowAttemptsWarning = false;
    }
}
