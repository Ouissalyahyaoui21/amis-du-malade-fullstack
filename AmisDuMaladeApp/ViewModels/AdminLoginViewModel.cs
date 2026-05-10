using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AmisDuMaladeApp.Services;

namespace AmisDuMaladeApp.ViewModels;

public partial class AdminLoginViewModel : BaseViewModel
{
    // ── Tab ──────────────────────────────────────────────────────────────────
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsUsernameTab))]
    private bool isEmailTab = true;
    public bool IsUsernameTab => !IsEmailTab;

    // ── Fields ───────────────────────────────────────────────────────────────
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

    // ── Error / lock state ───────────────────────────────────────────────────
    [ObservableProperty] private bool   hasLoginError;
    [ObservableProperty] private string loginErrorText      = "";
    [ObservableProperty] private bool   showAttemptsWarning;
    [ObservableProperty] private string attemptsWarningText = "";
    [ObservableProperty] private bool   isLocked;

    // ── Dialog ───────────────────────────────────────────────────────────────
    [ObservableProperty] private bool showForgotDialog;

    private int _failedAttempts;

    public AdminLoginViewModel(LocalizationService loc) : base(loc) { }

    // ── Tab commands ─────────────────────────────────────────────────────────
    [RelayCommand]
    private void SetEmailTab()    { IsEmailTab = true;  ClearLoginErrors(); }

    [RelayCommand]
    private void SetUsernameTab() { IsEmailTab = false; ClearLoginErrors(); }

    // ── Password toggle ──────────────────────────────────────────────────────
    [RelayCommand]
    private void TogglePassword() => IsPasswordVisible = !IsPasswordVisible;

    // ── Remember me ──────────────────────────────────────────────────────────
    [RelayCommand]
    private void ToggleRememberMe() => RememberMe = !RememberMe;

    // ── Forgot password dialog ────────────────────────────────────────────────
    [RelayCommand]
    private void ShowForgotPassword() => ShowForgotDialog = true;

    [RelayCommand]
    private void CloseForgotDialog() => ShowForgotDialog = false;

    // ── Login ────────────────────────────────────────────────────────────────
    [RelayCommand]
    private async Task LoginAsync()
    {
        if (IsLocked || IsBusy) return;

        var credential = IsEmailTab ? Email : Username;
        if (string.IsNullOrWhiteSpace(credential) || string.IsNullOrWhiteSpace(Password))
        {
            SetLoginError("يرجى تعبئة جميع الحقول المطلوبة.");
            return;
        }

        IsBusy = true;
        try
        {
            await Task.Delay(1500);
            _failedAttempts++;

            if (_failedAttempts >= 3)
            {
                SetLoginError("تم تأمين الحساب مؤقتاً لمدة 15 دقيقة بسبب المحاولات المتكررة.");
                ShowAttemptsWarning = false;
                IsLocked = true;
            }
            else
            {
                SetLoginError("بيانات الدخول غير صحيحة. تحقق من بريدك وكلمة المرور.");
                var left = 3 - _failedAttempts;
                if (left <= 2)
                {
                    AttemptsWarningText = left == 1
                        ? "تبقّت لك محاولة واحدة قبل تأمين الحساب مؤقتاً."
                        : $"تبقّت لك {left} محاولتان قبل تأمين الحساب مؤقتاً.";
                    ShowAttemptsWarning = true;
                }
            }
        }
        finally { IsBusy = false; }
    }

    private void SetLoginError(string msg) { LoginErrorText = msg; HasLoginError = true; }
    private void ClearLoginErrors()
    {
        HasLoginError = false;
        LoginErrorText = "";
        ShowAttemptsWarning = false;
    }
}
