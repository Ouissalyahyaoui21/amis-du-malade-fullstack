using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AmisDuMaladeApp.Models;
using AmisDuMaladeApp.Services;

namespace AmisDuMaladeApp.ViewModels;

public partial class AdminDashboardViewModel : BaseViewModel
{
    private readonly ApiService      _api;
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
    [ObservableProperty] private int statContributions;
    [ObservableProperty] private int statPendingContributions;

    // ── Collections ────────────────────────────────────────────────────────────
    public ObservableCollection<AlertResponse>       OpenAlertsList    { get; } = new();
    public ObservableCollection<ActivityItem>        RecentActivities  { get; } = new();
    public ObservableCollection<VolunteerResponse>   AllVolunteers     { get; } = new();
    public ObservableCollection<VolunteerResponse>   PendingVolunteers { get; } = new();
    public ObservableCollection<CareRequestListItem> NewRequests       { get; } = new();
    public ObservableCollection<CareRequestListItem> AllRequests       { get; } = new();
    public ObservableCollection<PatientResponse>     PatientsList      { get; } = new();
    public ObservableCollection<ContributionItem>    ContributionsList { get; } = new();

    // ── Volunteer filter ───────────────────────────────────────────────────────
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(FilteredVolunteers))]
    private string volunteerFilter = "All";

    public IEnumerable<VolunteerResponse> FilteredVolunteers =>
        VolunteerFilter == "All"
            ? AllVolunteers
            : AllVolunteers.Where(v => v.Status == VolunteerFilter);

    // ── Active tab ─────────────────────────────────────────────────────────────
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsOverviewTab))]
    [NotifyPropertyChangedFor(nameof(IsVolunteersTab))]
    [NotifyPropertyChangedFor(nameof(IsInterviewsTab))]
    [NotifyPropertyChangedFor(nameof(IsTrainingTab))]
    [NotifyPropertyChangedFor(nameof(IsEvaluationsTab))]
    [NotifyPropertyChangedFor(nameof(IsNewRequestsTab))]
    [NotifyPropertyChangedFor(nameof(IsRequestsTab))]
    [NotifyPropertyChangedFor(nameof(IsPatientsTab))]
    [NotifyPropertyChangedFor(nameof(IsAlertsTab))]
    [NotifyPropertyChangedFor(nameof(IsReportsTab))]
    [NotifyPropertyChangedFor(nameof(IsContributionsTab))]
    private string activeTab = "overview";

    public bool IsOverviewTab      => ActiveTab == "overview";
    public bool IsVolunteersTab    => ActiveTab == "volunteers";
    public bool IsInterviewsTab    => ActiveTab == "interviews";
    public bool IsTrainingTab      => ActiveTab == "training";
    public bool IsEvaluationsTab   => ActiveTab == "evaluations";
    public bool IsNewRequestsTab   => ActiveTab == "newrequests";
    public bool IsRequestsTab      => ActiveTab == "requests";
    public bool IsPatientsTab      => ActiveTab == "patients";
    public bool IsAlertsTab        => ActiveTab == "alerts";
    public bool IsReportsTab       => ActiveTab == "reports";
    public bool IsContributionsTab => ActiveTab == "contributions";

    public AdminDashboardViewModel(ApiService api, AuthTokenService auth, LocalizationService loc)
        : base(loc)
    {
        _api  = api;
        _auth = auth;
    }

    // ── Initial load (overview data) ──────────────────────────────────────────
    [RelayCommand]
    public async Task LoadDataAsync()
    {
        IsBusy = true;
        try
        {
            await Task.WhenAll(
                LoadOverviewAsync(),
                LoadVolunteersAsync(),
                LoadRequestsAsync()
            );
        }
        finally { IsBusy = false; }
    }

    private async Task LoadOverviewAsync()
    {
        try
        {
            var dash = await _api.GetDashboardAsync();
            if (dash != null)
            {
                StatVolunteers         = dash.TotalVolunteers;
                StatActiveVolunteers   = dash.ActiveVolunteers;
                StatPendingVolunteers  = dash.PendingVolunteers;
                StatPatients           = dash.TotalPatients;
                StatRequests           = dash.TotalCareRequests;
                StatPendingRequests    = dash.PendingRequests;
                StatAssignments        = dash.ActiveAssignments;
                StatAlerts             = dash.OpenAlerts;
                StatNewVolunteersMonth = dash.NewVolunteersThisMonth;
                StatNewRequestsMonth   = dash.NewRequestsThisMonth;

                RecentActivities.Clear();
                foreach (var a in dash.RecentActivities) RecentActivities.Add(a);
            }

            var alerts = await _api.GetOpenAlertsAsync();
            OpenAlertsList.Clear();
            foreach (var a in alerts) OpenAlertsList.Add(a);
            StatAlerts = OpenAlertsList.Count;
        }
        catch { /* graceful offline */ }
    }

    private async Task LoadVolunteersAsync()
    {
        try
        {
            var list = await _api.GetVolunteersAsync();
            AllVolunteers.Clear();
            PendingVolunteers.Clear();
            foreach (var v in list)
            {
                AllVolunteers.Add(v);
                if (v.Status == "Pending") PendingVolunteers.Add(v);
            }
            OnPropertyChanged(nameof(FilteredVolunteers));
        }
        catch { }
    }

    private async Task LoadRequestsAsync()
    {
        try
        {
            var list = await _api.GetCareRequestsAsync();
            AllRequests.Clear();
            NewRequests.Clear();
            foreach (var r in list)
            {
                AllRequests.Add(r);
                if (r.Status == "Pending") NewRequests.Add(r);
            }
        }
        catch { }
    }

    private async Task LoadPatientsAsync()
    {
        try
        {
            var list = await _api.GetPatientsAsync();
            PatientsList.Clear();
            foreach (var p in list) PatientsList.Add(p);
        }
        catch { }
    }

    private async Task LoadContributionsAsync()
    {
        try
        {
            var list = await _api.GetContributionsAsync();
            ContributionsList.Clear();
            foreach (var c in list) ContributionsList.Add(c);
            StatContributions        = ContributionsList.Count;
            StatPendingContributions = ContributionsList.Count(c => c.Status == "Pending");
        }
        catch { }
    }

    // ── Tab switching with lazy load ───────────────────────────────────────────
    [RelayCommand]
    private void SetTab(string tab)
    {
        ActiveTab = tab;
        _ = tab switch
        {
            "patients"      when PatientsList.Count     == 0 => LoadPatientsAsync(),
            "contributions" when ContributionsList.Count == 0 => LoadContributionsAsync(),
            _ => Task.CompletedTask
        };
    }

    // ── Volunteer filter ───────────────────────────────────────────────────────
    [RelayCommand]
    private void FilterVolunteers(string filter)
    {
        VolunteerFilter = filter;
    }

    // ── WhatsApp ───────────────────────────────────────────────────────────────
    [RelayCommand]
    private async Task ContactWhatsApp(string phone)
    {
        var digits = new string(phone.Where(char.IsDigit).ToArray()).TrimStart('0');
        await Launcher.OpenAsync($"https://wa.me/213{digits}");
    }

    // ── Alert actions ──────────────────────────────────────────────────────────
    [RelayCommand]
    private async Task ResolveAlertAsync(AlertResponse alert)
    {
        if (await _api.ResolveAlertAsync(alert.Id))
        {
            OpenAlertsList.Remove(alert);
            StatAlerts = Math.Max(0, StatAlerts - 1);
        }
    }

    // ── Volunteer actions ──────────────────────────────────────────────────────
    [RelayCommand]
    private async Task ApproveVolunteerAsync(VolunteerResponse v)
    {
        if (await _api.UpdateVolunteerStatusAsync(v.Id, "Active"))
        {
            v.Status = "Active";
            PendingVolunteers.Remove(v);
            StatPendingVolunteers = Math.Max(0, StatPendingVolunteers - 1);
            StatActiveVolunteers++;
            OnPropertyChanged(nameof(FilteredVolunteers));
        }
    }

    [RelayCommand]
    private async Task RejectVolunteerAsync(VolunteerResponse v)
    {
        if (await _api.UpdateVolunteerStatusAsync(v.Id, "Rejected"))
        {
            v.Status = "Rejected";
            PendingVolunteers.Remove(v);
            OnPropertyChanged(nameof(FilteredVolunteers));
        }
    }

    // ── Contribution actions ───────────────────────────────────────────────────
    [RelayCommand]
    private async Task ConfirmContributionAsync(ContributionItem c)
    {
        if (await _api.UpdateContributionStatusAsync(c.Id, "Confirmed"))
        {
            c.Status = "Confirmed";
            StatPendingContributions = Math.Max(0, StatPendingContributions - 1);
            // Force CollectionView to re-render the item
            var idx = ContributionsList.IndexOf(c);
            if (idx >= 0) { ContributionsList.RemoveAt(idx); ContributionsList.Insert(idx, c); }
        }
    }

    [RelayCommand]
    private async Task DistributeContributionAsync(ContributionItem c)
    {
        if (await _api.UpdateContributionStatusAsync(c.Id, "Distributed"))
        {
            c.Status = "Distributed";
            var idx = ContributionsList.IndexOf(c);
            if (idx >= 0) { ContributionsList.RemoveAt(idx); ContributionsList.Insert(idx, c); }
        }
    }

    // ── Logout ────────────────────────────────────────────────────────────────
    [RelayCommand]
    private async Task LogoutAsync()
    {
        _auth.ClearToken();
        await Shell.Current.GoToAsync("//HomePage");
    }
}
