using CommunityToolkit.Mvvm.ComponentModel;
using AmisDuMaladeApp.Services;

namespace AmisDuMaladeApp.ViewModels;

public partial class BaseViewModel : ObservableObject
{
    protected readonly LocalizationService Loc;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string title = "";

    public FlowDirection FlowDirection => Loc.FlowDirection;

    public BaseViewModel(LocalizationService loc)
    {
        Loc = loc;
        Loc.LanguageChanged += (_, _) => OnPropertyChanged(nameof(FlowDirection));
    }

    protected async Task ShowErrorAsync(string message) =>
        await Shell.Current.DisplayAlert(Loc.Get("error"), message, "OK");

    protected async Task ShowSuccessAsync(string message) =>
        await Shell.Current.DisplayAlert(Loc.Get("success"), message, "OK");
}
