using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AmisDuMaladeApp.Models;
using AmisDuMaladeApp.Services;
using AmisDuMaladeApp.Views;

namespace AmisDuMaladeApp.ViewModels;

public partial class DashboardViewModel : BaseViewModel
{
    private readonly ApiService _api;
    private readonly AuthTokenService _authToken;

    // ── KPI numbers ──────────────────────────────────────────────────────────
    [ObservableProperty] private int totalVolunteers;
    [ObservableProperty] private int activeVolunteers;
    [ObservableProperty] private int pendingVolunteers;
    [ObservableProperty] private int totalPatients;
    [ObservableProperty] private int totalRequests;
    [ObservableProperty] private int pendingRequests;
    [ObservableProperty] private int openAlerts;
    [ObservableProperty] private int activeAssignments;
    [ObservableProperty] private int newVolunteersThisMonth;
    [ObservableProperty] private int newRequestsThisMonth;

    [ObservableProperty] private List<ActivityItem> recentActivities = new();

    // ── Labels ───────────────────────────────────────────────────────────────
    public string TitleLabel        => Loc.Get("dashboard_title");
    public string VolunteersLabel   => Loc.Get("dashboard_volunteers");
    public string RequestsLabel     => Loc.Get("dashboard_requests");
    public string PatientsLabel     => Loc.Get("dashboard_patients");
    public string AssignmentsLabel  => Loc.Get("dashboard_assignments");
    public string AlertsLabel       => Loc.Get("dashboard_alerts");
    public string RecentLabel       => Loc.Get("recent_activity");
    public string LogoutLabel       => Loc.Get("logout");
    public string RefreshLabel      => Loc.Get("refresh");

    public DashboardViewModel(ApiService api, AuthTokenService authToken, LocalizationService loc)
        : base(loc)
    {
        _api = api;
        _authToken = authToken;
        Loc.LanguageChanged += (_, _) => RefreshLabels();
    }

    [RelayCommand]
    private async Task LoadDashboard()
    {
        IsBusy = true;
        try
        {
            var data = await _api.GetDashboardAsync();
            if (data is null) return;

            TotalVolunteers    = data.TotalVolunteers;
            ActiveVolunteers   = data.ActiveVolunteers;
            PendingVolunteers  = data.PendingVolunteers;
            TotalPatients      = data.TotalPatients;
            TotalRequests      = data.TotalCareRequests;
            PendingRequests    = data.PendingRequests;
            OpenAlerts         = data.OpenAlerts;
            ActiveAssignments  = data.ActiveAssignments;
            NewVolunteersThisMonth = data.NewVolunteersThisMonth;
            NewRequestsThisMonth   = data.NewRequestsThisMonth;
            RecentActivities   = data.RecentActivities;
        }
        finally { IsBusy = false; }
    }

    [RelayCommand]
    private async Task Logout()
    {
        _authToken.ClearToken();
        await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
    }

    private void RefreshLabels()
    {
        OnPropertyChanged(nameof(TitleLabel));
        OnPropertyChanged(nameof(VolunteersLabel));
        OnPropertyChanged(nameof(RequestsLabel));
        OnPropertyChanged(nameof(PatientsLabel));
        OnPropertyChanged(nameof(AssignmentsLabel));
        OnPropertyChanged(nameof(AlertsLabel));
        OnPropertyChanged(nameof(RecentLabel));
        OnPropertyChanged(nameof(LogoutLabel));
        OnPropertyChanged(nameof(FlowDirection));
    }
}
