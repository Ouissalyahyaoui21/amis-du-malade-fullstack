using CommunityToolkit.Mvvm.Input;
using AmisDuMaladeApp.Services;
using AmisDuMaladeApp.Views;

namespace AmisDuMaladeApp.ViewModels;

public partial class HomeViewModel : BaseViewModel
{
    public string WelcomeText      => Loc.Get("home_welcome");
    public string SubtitleText     => Loc.Get("home_subtitle");
    public string VolunteerBtnText => Loc.Get("home_volunteer_btn");
    public string CareBtnText      => Loc.Get("home_care_btn");
    public string ContributeBtnText=> Loc.Get("home_contribute_btn");
    public string AboutText        => Loc.Get("home_about");
    public string AboutBodyText    => Loc.Get("home_about_text");
    public string AdminLoginText   => Loc.Get("admin_login");

    public HomeViewModel(LocalizationService loc) : base(loc)
    {
        Loc.LanguageChanged += OnLanguageChanged;
    }

    private void OnLanguageChanged(object? sender, string lang)
    {
        OnPropertyChanged(nameof(WelcomeText));
        OnPropertyChanged(nameof(SubtitleText));
        OnPropertyChanged(nameof(VolunteerBtnText));
        OnPropertyChanged(nameof(CareBtnText));
        OnPropertyChanged(nameof(ContributeBtnText));
        OnPropertyChanged(nameof(AboutText));
        OnPropertyChanged(nameof(AboutBodyText));
        OnPropertyChanged(nameof(AdminLoginText));
        OnPropertyChanged(nameof(FlowDirection));
    }

    [RelayCommand]
    private async Task GoToVolunteerRegister() =>
        await Shell.Current.GoToAsync(nameof(VolunteerRegisterPage));

    [RelayCommand]
    private async Task GoToCareRequest() =>
        await Shell.Current.GoToAsync(nameof(CareRequestPage));

    [RelayCommand]
    private async Task GoToContribute() =>
        // Contribution page — to be implemented in next phase
        await Shell.Current.DisplayAlert(Loc.Get("home_contribute_btn"), "", "OK");

    [RelayCommand]
    private async Task GoToAdminLogin() =>
        await Shell.Current.GoToAsync(nameof(LoginPage));

    [RelayCommand]
    private void SetLanguage(string lang) => Loc.SetLanguage(lang);
}
