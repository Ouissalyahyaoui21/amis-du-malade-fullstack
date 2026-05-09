using CommunityToolkit.Mvvm.Input;
using AmisDuMaladeApp.Services;
using AmisDuMaladeApp.Views;

namespace AmisDuMaladeApp.ViewModels;

public partial class HomeViewModel : BaseViewModel
{
    // ── Header labels
    public string ProjectTitleText  => Loc.Get("home_project_title");
    public string SubtitleText      => Loc.Get("home_subtitle");

    // ── Action card labels
    public string CareBtnText       => Loc.Get("home_care_btn");
    public string CareSubtitleText  => Loc.Get("care_subtitle");
    public string VolunteerBtnText  => Loc.Get("home_volunteer_btn");
    public string VolunteerSubText  => Loc.Get("volunteer_subtitle");
    public string ContributeBtnText => Loc.Get("home_contribute_btn");
    public string ContributeSubText => Loc.Get("contribute_subtitle");
    public string NewBadgeText      => Loc.Get("new_badge");

    // ── Secondary cards
    public string MyAccountText     => Loc.Get("my_account");
    public string AboutText         => Loc.Get("about_association");

    // ── Contact + footer
    public string ContactUsText     => Loc.Get("contact_us");
    public string WhatsAppText      => Loc.Get("whatsapp");
    public string FacebookText      => Loc.Get("facebook");
    public string OfflineText       => Loc.Get("offline_support");
    public string AdminLoginText    => Loc.Get("admin_login");

    public HomeViewModel(LocalizationService loc) : base(loc)
    {
        Loc.LanguageChanged += OnLanguageChanged;
    }

    private void OnLanguageChanged(object? sender, string lang)
    {
        OnPropertyChanged(nameof(ProjectTitleText));
        OnPropertyChanged(nameof(SubtitleText));
        OnPropertyChanged(nameof(CareBtnText));
        OnPropertyChanged(nameof(CareSubtitleText));
        OnPropertyChanged(nameof(VolunteerBtnText));
        OnPropertyChanged(nameof(VolunteerSubText));
        OnPropertyChanged(nameof(ContributeBtnText));
        OnPropertyChanged(nameof(ContributeSubText));
        OnPropertyChanged(nameof(NewBadgeText));
        OnPropertyChanged(nameof(MyAccountText));
        OnPropertyChanged(nameof(AboutText));
        OnPropertyChanged(nameof(ContactUsText));
        OnPropertyChanged(nameof(WhatsAppText));
        OnPropertyChanged(nameof(FacebookText));
        OnPropertyChanged(nameof(OfflineText));
        OnPropertyChanged(nameof(AdminLoginText));
        OnPropertyChanged(nameof(FlowDirection));
    }

    [RelayCommand]
    private async Task GoToCareRequest() =>
        await Shell.Current.GoToAsync(nameof(CareRequestPage));

    [RelayCommand]
    private async Task GoToVolunteerRegister() =>
        await Shell.Current.GoToAsync(nameof(VolunteerRegisterPage));

    [RelayCommand]
    private async Task GoToContribute() =>
        await Shell.Current.DisplayAlert("", Loc.Get("home_contribute_btn"), "OK");

    [RelayCommand]
    private async Task GoToMyAccount() =>
        await Shell.Current.DisplayAlert("", Loc.Get("my_account"), "OK");

    [RelayCommand]
    private async Task GoToAbout() =>
        await Shell.Current.DisplayAlert("", Loc.Get("about_association"), "OK");

    [RelayCommand]
    private async Task OpenWhatsApp()
    {
        // Replace with the association's real WhatsApp number
        var uri = new Uri("https://wa.me/213XXXXXXXXX");
        if (await Launcher.CanOpenAsync(uri))
            await Launcher.OpenAsync(uri);
    }

    [RelayCommand]
    private async Task OpenFacebook()
    {
        // Replace with the association's real Facebook page URL
        var uri = new Uri("https://www.facebook.com/AmisduMaladeSkikda");
        if (await Launcher.CanOpenAsync(uri))
            await Launcher.OpenAsync(uri);
    }

    [RelayCommand]
    private async Task GoToAdminLogin() =>
        await Shell.Current.GoToAsync(nameof(LoginPage));

    [RelayCommand]
    private void SetLanguage(string lang) => Loc.SetLanguage(lang);
}
