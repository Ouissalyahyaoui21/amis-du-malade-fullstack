using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AmisDuMaladeApp.Models;
using AmisDuMaladeApp.Services;

namespace AmisDuMaladeApp.ViewModels;

[QueryProperty(nameof(PatientId), "patientId")]
public partial class PatientDetailViewModel : BaseViewModel
{
    private readonly ApiService _api;

    [ObservableProperty] private string patientId = "";
    [ObservableProperty] private PatientDetailResponse? patient;
    [ObservableProperty] private bool hasLoadError;

    public bool HasNoCareRequests => Patient != null && Patient.CareRequests.Count == 0;
    public bool HasNoContacts     => Patient != null && Patient.Contacts.Count == 0;
    public bool HasNoConditions   => Patient != null && Patient.Conditions.Count == 0;

    public PatientDetailViewModel(ApiService api, LocalizationService loc) : base(loc)
    {
        _api = api;
    }

    partial void OnPatientIdChanged(string value)
    {
        if (!string.IsNullOrEmpty(value))
            _ = LoadAsync();
    }

    private async Task LoadAsync()
    {
        IsBusy       = true;
        HasLoadError = false;
        try
        {
            if (!Guid.TryParse(PatientId, out var guid)) return;
            Patient = await _api.GetPatientByIdAsync(guid);
            if (Patient == null) HasLoadError = true;
            NotifyComputedChanged();
        }
        catch { HasLoadError = true; }
        finally { IsBusy = false; }
    }

    private void NotifyComputedChanged()
    {
        OnPropertyChanged(nameof(HasNoCareRequests));
        OnPropertyChanged(nameof(HasNoContacts));
        OnPropertyChanged(nameof(HasNoConditions));
    }

    [RelayCommand]
    private async Task ContactWhatsApp(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone)) return;
        var digits = new string(phone.Where(char.IsDigit).ToArray()).TrimStart('0');
        await Launcher.OpenAsync($"https://wa.me/213{digits}");
    }

    [RelayCommand]
    private async Task GoBackAsync() => await Shell.Current.GoToAsync("..");
}
