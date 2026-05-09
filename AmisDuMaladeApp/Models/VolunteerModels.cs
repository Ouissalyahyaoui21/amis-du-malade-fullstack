namespace AmisDuMaladeApp.Models;

// Matches backend VolunteerRegisterVM exactly
public class VolunteerRegisterRequest
{
    public string FullName { get; set; } = "";
    public string Phone { get; set; } = "";
    public string? Email { get; set; }
    public string? Municipality { get; set; }
    public string? VolunteerCategory { get; set; }
    public bool CanHomeVisit { get; set; } = true;
    public bool CanHospitalVisit { get; set; } = false;
    public bool CanNightPresence { get; set; } = false;
    public bool HasTransportation { get; set; } = false;
    public Guid? BranchId { get; set; }
    public List<VolunteerSkillRequest> Skills { get; set; } = new();
    public List<VolunteerAvailabilityRequest> Availabilities { get; set; } = new();
    public List<string> CoverageAreas { get; set; } = new();
}

public class VolunteerSkillRequest
{
    public Guid SkillId { get; set; }
    public string? Level { get; set; }
}

public class VolunteerAvailabilityRequest
{
    public string DayOfWeek { get; set; } = "";
    public string StartTime { get; set; } = "";
    public string EndTime { get; set; } = "";
}

public class VolunteerResponse
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = "";
    public string Phone { get; set; } = "";
    public string? Email { get; set; }
    public string? Municipality { get; set; }
    public string? VolunteerCategory { get; set; }
    public string Status { get; set; } = "Pending";
    public bool CanHomeVisit { get; set; }
    public bool CanHospitalVisit { get; set; }
    public bool CanNightPresence { get; set; }
    public bool HasTransportation { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class VolunteerSuggestion
{
    public Guid VolunteerId { get; set; }
    public string FullName { get; set; } = "";
    public string? Municipality { get; set; }
    public double MatchScore { get; set; }
}
