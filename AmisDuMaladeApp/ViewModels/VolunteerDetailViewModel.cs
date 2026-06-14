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
        "Interview" => "📋 في مرحلة المقابلة",
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

    public bool IsPending   => Volunteer?.Status == "Pending";
    public bool IsInterview => Volunteer?.Status == "Interview";
    public bool IsApproved  => Volunteer?.Status == "Approved";

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
                NotifyStatusChanged();
            }
        }
        finally { IsBusy = false; }
    }

    private void NotifyStatusChanged()
    {
        OnPropertyChanged(nameof(StatusLabel));
        OnPropertyChanged(nameof(StatusColor));
        OnPropertyChanged(nameof(StatusBg));
        OnPropertyChanged(nameof(IsPending));
        OnPropertyChanged(nameof(IsInterview));
        OnPropertyChanged(nameof(IsApproved));
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

        var location = await Shell.Current.DisplayPromptAsync(
            "جدولة مقابلة",
            $"المتطوع: {Volunteer.FullName}\nأدخل مكان المقابلة:",
            "تأكيد", "إلغاء",
            "مقر الجمعية - سكيكدة");

        if (location == null) return;

        var ok = await _api.ScheduleInterviewAsync(new ScheduleInterviewRequest
        {
            VolunteerId = Volunteer.Id,
            ScheduledAt = DateTime.UtcNow.AddDays(7),
            Location    = string.IsNullOrWhiteSpace(location) ? "مقر الجمعية" : location
        });

        if (ok)
        {
            Volunteer.Status = "Interview";
            NotifyStatusChanged();
            await Shell.Current.DisplayAlert("تمت الجدولة", "تمت جدولة المقابلة بنجاح", "حسناً");
        }
    }

    [RelayCommand]
    private async Task RejectAsync()
    {
        if (Volunteer == null) return;

        bool confirmed = await Shell.Current.DisplayAlert(
            "رفض المتطوع",
            $"هل أنت متأكد من رفض طلب {Volunteer.FullName}؟",
            "نعم، ارفض", "إلغاء");

        if (!confirmed) return;

        if (await _api.UpdateVolunteerStatusAsync(Volunteer.Id, "Rejected"))
        {
            Volunteer.Status = "Rejected";
            NotifyStatusChanged();
        }
    }

    [RelayCommand]
    private async Task GoBackAsync() => await Shell.Current.GoToAsync("..");
}
