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
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasContent))]
    private PatientDetailResponse? patient;
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasContent))]
    private bool hasLoadError;
    [ObservableProperty] private string loadErrorDetail = "";

    public bool HasContent        => !HasLoadError && Patient != null;
    public bool HasNoCareRequests => Patient?.CareRequests?.Count == 0;
    public bool HasNoContacts     => Patient?.Contacts?.Count == 0;
    public bool HasNoConditions   => Patient?.Conditions?.Count == 0;

    public string LocationDisplay =>
        (string.IsNullOrEmpty(Patient?.Municipality), string.IsNullOrEmpty(Patient?.Address)) switch
        {
            (true,  true)  => "",
            (false, true)  => Patient!.Municipality!,
            (true,  false) => Patient!.Address!,
            _              => $"{Patient!.Municipality} — {Patient!.Address}"
        };

    public string MobilityDisplay =>
        (string.IsNullOrEmpty(Patient?.MobilityStatus), string.IsNullOrEmpty(Patient?.DependencyLevel)) switch
        {
            (true,  true)  => "",
            (false, true)  => $"الحركة: {Patient!.MobilityStatus}",
            (true,  false) => $"الاعتمادية: {Patient!.DependencyLevel}",
            _              => $"الحركة: {Patient!.MobilityStatus}   الاعتمادية: {Patient!.DependencyLevel}"
        };

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
        IsBusy          = true;
        HasLoadError    = false;
        LoadErrorDetail = "";
        try
        {
            if (!Guid.TryParse(PatientId, out var guid)) return;
            var (data, errorCode) = await _api.GetPatientByIdAsync(guid);
            if (data == null)
            {
                LoadErrorDetail = errorCode ?? "unknown";
                HasLoadError    = true;
                return;
            }
            Patient = data;
            NotifyComputedChanged();
        }
        catch (Exception ex)
        {
            LoadErrorDetail = ex.Message;
            HasLoadError    = true;
        }
        finally { IsBusy = false; }
    }

    private void NotifyComputedChanged()
    {
        OnPropertyChanged(nameof(HasNoCareRequests));
        OnPropertyChanged(nameof(HasNoContacts));
        OnPropertyChanged(nameof(HasNoConditions));
        OnPropertyChanged(nameof(LocationDisplay));
        OnPropertyChanged(nameof(MobilityDisplay));
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
