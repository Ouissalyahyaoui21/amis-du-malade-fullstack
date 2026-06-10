using CommunityToolkit.Mvvm.Input;
using AmisDuMaladeApp.Constants;
using AmisDuMaladeApp.Services;
using AmisDuMaladeApp.Views;

namespace AmisDuMaladeApp.ViewModels;

public partial class HomeViewModel : BaseViewModel
{
    public string TitleLabel    => Loc.Get("home_project_title");
    public string SubtitleLabel => Loc.Get("home_subtitle");
    public string CareLabel     => Loc.Get("home_care_btn");
    public string CareSub       => Loc.Get("care_subtitle");
    public string VolLabel      => Loc.Get("home_volunteer_btn");
    public string VolSub        => Loc.Get("volunteer_subtitle");
    public string ContribLabel  => Loc.Get("home_contribute_btn");
    public string ContribSub    => Loc.Get("contribute_subtitle");
    public string NewBadge      => Loc.Get("new_badge");
    public string AccountLabel  => Loc.Get("my_account");
    public string AboutLabel    => Loc.Get("about_association");
    public string ContactLabel  => Loc.Get("contact_us");
    public string WhatsAppLabel => Loc.Get("whatsapp");
    public string FacebookLabel => Loc.Get("facebook");
    public string OfflineLabel  => Loc.Get("offline_support");
    public string AdminLabel    => Loc.Get("admin_login");

    public HomeViewModel(LocalizationService loc) : base(loc)
    {
        Loc.LanguageChanged += (_, _) => RefreshLabels();
    }

    [RelayCommand]
    private async Task OpenCareRequest() =>
        await Shell.Current.GoToAsync(nameof(Views.CareRequestPage));

    [RelayCommand]
    private async Task OpenVolunteer() =>
        await Shell.Current.GoToAsync(nameof(Views.VolunteerRegisterPage));

    [RelayCommand]
    private async Task OpenContribute() =>
        await Shell.Current.GoToAsync(nameof(Views.ContributePage));

    [RelayCommand]
    private async Task OpenWhatsApp()
    {
        var url = $"https://wa.me/{AppConstants.WhatsAppNumber}";
        await Launcher.OpenAsync(url);
    }

    [RelayCommand]
    private async Task OpenFacebook() =>
        await Launcher.OpenAsync(AppConstants.FacebookUrl);

    [RelayCommand]
    private void SetLanguage(string lang)
    {
        Loc.SetLanguage(lang);
    }

    [RelayCommand]
    private async Task OpenAbout() =>
        await Shell.Current.GoToAsync(nameof(Views.AboutPage));

    [RelayCommand]
    private async Task OpenAdmin() =>
        await Shell.Current.GoToAsync(nameof(Views.AdminLoginPage));

    private void RefreshLabels()
    {
        OnPropertyChanged(nameof(TitleLabel));    OnPropertyChanged(nameof(SubtitleLabel));
        OnPropertyChanged(nameof(CareLabel));     OnPropertyChanged(nameof(CareSub));
        OnPropertyChanged(nameof(VolLabel));      OnPropertyChanged(nameof(VolSub));
        OnPropertyChanged(nameof(ContribLabel));  OnPropertyChanged(nameof(ContribSub));
        OnPropertyChanged(nameof(NewBadge));      OnPropertyChanged(nameof(AccountLabel));
        OnPropertyChanged(nameof(AboutLabel));    OnPropertyChanged(nameof(ContactLabel));
        OnPropertyChanged(nameof(WhatsAppLabel)); OnPropertyChanged(nameof(FacebookLabel));
        OnPropertyChanged(nameof(OfflineLabel));  OnPropertyChanged(nameof(AdminLabel));
        OnPropertyChanged(nameof(FlowDirection));
    }
}
