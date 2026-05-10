namespace AmisDuMaladeApp.Models;

// Matches backend DashboardVM exactly
public class DashboardData
{
    public int TotalVolunteers { get; set; }
    public int ActiveVolunteers { get; set; }
    public int PendingVolunteers { get; set; }
    public int TotalPatients { get; set; }
    public int TotalCareRequests { get; set; }
    public int PendingRequests { get; set; }
    public int ActiveAssignments { get; set; }
    public int OpenAlerts { get; set; }
    public int NewVolunteersThisMonth { get; set; }
    public int NewRequestsThisMonth { get; set; }
    public Dictionary<string, int> VolunteersByStatus { get; set; } = new();
    public Dictionary<string, int> RequestsByPriority { get; set; } = new();
    public Dictionary<string, int> VolunteersByMunicipality { get; set; } = new();
    public List<ActivityItem> RecentActivities { get; set; } = new();
}

public class ActivityItem
{
    public string Description { get; set; } = "";
    public string TimeAgo { get; set; } = "";
    public string Type { get; set; } = "";
    // Types: volunteer / request / assignment / alert
}
