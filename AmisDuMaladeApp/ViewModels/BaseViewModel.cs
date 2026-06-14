using CommunityToolkit.Mvvm.ComponentModel;
using AmisDuMaladeApp.Services;

namespace AmisDuMaladeApp.ViewModels;

public partial class BaseViewModel : ObservableObject
{
    protected readonly LocalizationService Loc;

    [ObservableProperty] private bool   isBusy;
    [ObservableProperty] private string errorMessage = "";
    [ObservableProperty] private bool   hasError;

    public FlowDirection FlowDirection => Loc.FlowDirection;

    protected BaseViewModel(LocalizationService loc)
    {
        Loc = loc;
        Loc.LanguageChanged += (_, _) => OnPropertyChanged(nameof(FlowDirection));
    }

    protected async Task ShowErrorAsync(string message)
    {
        ErrorMessage = message;
        HasError = true;
        await Task.Delay(3000);
        HasError = false;
        ErrorMessage = "";
    }
}
