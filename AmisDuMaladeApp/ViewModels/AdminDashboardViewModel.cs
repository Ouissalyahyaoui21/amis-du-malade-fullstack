using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AmisDuMaladeApp.Models;
using AmisDuMaladeApp.Services;

namespace AmisDuMaladeApp.ViewModels;

public partial class AdminDashboardViewModel : BaseViewModel
{
    private readonly ApiService _api;
    private readonly AuthTokenService _auth;

    // ── Stats ─────────────────────────────────────────────────────────────────
    [ObservableProperty] private int statVolunteers;
    [ObservableProperty] private int statActiveVolunteers;
    [ObservableProperty] private int statPendingVolunteers;
    [ObservableProperty] private int statPatients;
    [ObservableProperty] private int statRequests;
    [ObservableProperty] private int statPendingRequests;
    [ObservableProperty] private int statAssignments;
    [ObservableProperty] private int statAlerts;
    [ObservableProperty] private int statNewVolunteersMonth;
    [ObservableProperty] private int statNewRequestsMonth;

    // ── Lists ─────────────────────────────────────────────────────────────────
    public ObservableCollection<AlertResponse>     OpenAlertsList      { get; } = new();
    public ObservableCollection<ActivityItem>      RecentActivities    { get; } = new();
    public ObservableCollection<VolunteerResponse> PendingVolunteers   { get; } = new();

    // ── Active section tab ────────────────────────────────────────────────────
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsAlertsTab))]
    [NotifyPropertyChangedFor(nameof(IsActivitiesTab))]
    [NotifyPropertyChangedFor(nameof(IsVolunteersTab))]
    private string activeTab = "alerts";

    public bool IsAlertsTab     => ActiveTab == "alerts";
    public bool IsActivitiesTab => ActiveTab == "activities";
    public bool IsVolunteersTab => ActiveTab == "volunteers";

    public AdminDashboardViewModel(ApiService api, AuthTokenService auth, LocalizationService loc)
        : base(loc)
    {
        _api  = api;
        _auth = auth;
    }

    [RelayCommand]
    public async Task LoadDataAsync()
    {
        IsBusy = true;
        try
        {
            var dashboard = await _api.GetDashboardAsync();
            if (dashboard != null)
            {
                StatVolunteers         = dashboard.TotalVolunteers;
                StatActiveVolunteers   = dashboard.ActiveVolunteers;
                StatPendingVolunteers  = dashboard.PendingVolunteers;
                StatPatients           = dashboard.TotalPatients;
                StatRequests           = dashboard.TotalCareRequests;
                StatPendingRequests    = dashboard.PendingRequests;
                StatAssignments        = dashboard.ActiveAssignments;
                StatAlerts             = dashboard.OpenAlerts;
                StatNewVolunteersMonth = dashboard.NewVolunteersThisMonth;
                StatNewRequestsMonth   = dashboard.NewRequestsThisMonth;

                RecentActivities.Clear();
                foreach (var a in dashboard.RecentActivities)
                    RecentActivities.Add(a);
            }

            var alerts = await _api.GetOpenAlertsAsync();
            OpenAlertsList.Clear();
            foreach (var a in alerts) OpenAlertsList.Add(a);

            var volunteers = await _api.GetVolunteersAsync();
            PendingVolunteers.Clear();
            foreach (var v in volunteers.Where(v => v.Status == "Pending"))
                PendingVolunteers.Add(v);
        }
        finally { IsBusy = false; }
    }

    [RelayCommand]
    private void SetTab(string tab) => ActiveTab = tab;

    [RelayCommand]
    private async Task ResolveAlertAsync(AlertResponse alert)
    {
        if (await _api.ResolveAlertAsync(alert.Id))
        {
            OpenAlertsList.Remove(alert);
            StatAlerts = Math.Max(0, StatAlerts - 1);
        }
    }

    [RelayCommand]
    private async Task ApproveVolunteerAsync(VolunteerResponse volunteer)
    {
        if (await _api.UpdateVolunteerStatusAsync(volunteer.Id, "Active"))
        {
            PendingVolunteers.Remove(volunteer);
            StatPendingVolunteers = Math.Max(0, StatPendingVolunteers - 1);
            StatActiveVolunteers++;
        }
    }

    [RelayCommand]
    private async Task RejectVolunteerAsync(VolunteerResponse volunteer)
    {
        if (await _api.UpdateVolunteerStatusAsync(volunteer.Id, "Rejected"))
            PendingVolunteers.Remove(volunteer);
    }

    [RelayCommand]
    private async Task LogoutAsync()
    {
        _auth.ClearToken();
        await Shell.Current.GoToAsync("//HomePage");
    }
}
