using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AmisDuMaladeApp.Models;
using AmisDuMaladeApp.Services;

namespace AmisDuMaladeApp.ViewModels;

[QueryProperty(nameof(VolunteerId), "volunteerId")]
public partial class VolunteerDetailViewModel : BaseViewModel
{
    private readonly ApiService _api;

    [ObservableProperty] private string volunteerId = "";
    [ObservableProperty] private VolunteerResponse? volunteer;

    public string StatusLabel => Volunteer?.Status switch
    {
        "Approved"  => "✓ نشط ومعتمد",
        "Pending"   => "⏳ معلق",
        "Rejected"  => "✗ مرفوض",
        "Interview" => "📋 مقابلة",
        "Suspended" => "⛔ موقوف",
        _           => Volunteer?.Status ?? ""
    };
    public Color StatusColor => Volunteer?.Status switch
    {
        "Approved"  => Color.FromArgb("#16a34a"),
        "Rejected"  => Color.FromArgb("#dc2626"),
        "Suspended" => Color.FromArgb("#dc2626"),
        _           => Color.FromArgb("#b45309")
    };
    public Color StatusBg => Volunteer?.Status switch
    {
        "Approved"  => Color.FromArgb("#dcfce7"),
        "Rejected"  => Color.FromArgb("#fee2e2"),
        "Suspended" => Color.FromArgb("#fee2e2"),
        _           => Color.FromArgb("#fef3c7")
    };

    public VolunteerDetailViewModel(ApiService api, LocalizationService loc) : base(loc)
    {
        _api = api;
    }

    partial void OnVolunteerIdChanged(string value)
    {
        if (!string.IsNullOrEmpty(value))
            _ = LoadAsync();
    }

    private async Task LoadAsync()
    {
        IsBusy = true;
        try
        {
            var list = await _api.GetVolunteersAsync();
            if (Guid.TryParse(VolunteerId, out var guid))
            {
                Volunteer = list.FirstOrDefault(v => v.Id == guid);
                OnPropertyChanged(nameof(StatusLabel));
                OnPropertyChanged(nameof(StatusColor));
                OnPropertyChanged(nameof(StatusBg));
            }
        }
        finally { IsBusy = false; }
    }

    [RelayCommand]
    private async Task ContactWhatsApp()
    {
        if (Volunteer?.Phone is null) return;
        var digits = new string(Volunteer.Phone.Where(char.IsDigit).ToArray()).TrimStart('0');
        await Launcher.OpenAsync($"https://wa.me/213{digits}");
    }

    [RelayCommand]
    private async Task ApproveAsync()
    {
        if (Volunteer == null) return;
        if (await _api.UpdateVolunteerStatusAsync(Volunteer.Id, "Approved"))
        {
            Volunteer.Status = "Approved";
            OnPropertyChanged(nameof(Volunteer));
            OnPropertyChanged(nameof(StatusLabel));
            OnPropertyChanged(nameof(StatusColor));
            OnPropertyChanged(nameof(StatusBg));
        }
    }

    [RelayCommand]
    private async Task RejectAsync()
    {
        if (Volunteer == null) return;
        if (await _api.UpdateVolunteerStatusAsync(Volunteer.Id, "Rejected"))
        {
            Volunteer.Status = "Rejected";
            OnPropertyChanged(nameof(Volunteer));
            OnPropertyChanged(nameof(StatusLabel));
            OnPropertyChanged(nameof(StatusColor));
            OnPropertyChanged(nameof(StatusBg));
        }
    }

    [RelayCommand]
    private async Task GoBackAsync() => await Shell.Current.GoToAsync("..");
}
