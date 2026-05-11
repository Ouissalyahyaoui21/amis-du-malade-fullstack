using CommunityToolkit.Mvvm.Input;
using AmisDuMaladeApp.Services;

namespace AmisDuMaladeApp.ViewModels;

public partial class AboutViewModel : BaseViewModel
{
    public string PageTitle => Loc.Get("about_association");

    public AboutViewModel(LocalizationService loc) : base(loc) { }

    [RelayCommand]
    private async Task GoBack() => await Shell.Current.GoToAsync("..");

    [RelayCommand]
    private async Task OpenWebsite() =>
        await Launcher.OpenAsync("https://www.amisdumalade.org");

    [RelayCommand]
    private async Task OpenFacebook() =>
        await Launcher.OpenAsync("https://www.facebook.com/");
}
