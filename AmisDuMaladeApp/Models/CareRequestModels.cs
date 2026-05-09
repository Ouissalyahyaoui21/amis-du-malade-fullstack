namespace AmisDuMaladeApp.Models;

// Matches backend CreateCareRequestVM exactly
public class CreateCareRequestModel
{
    public Guid PatientId { get; set; }
    public Guid? RequestedByContactId { get; set; }
    public Guid? BranchId { get; set; }
    public string? CareLocationType { get; set; }
    public Guid? HospitalId { get; set; }
    public string? LocationDetails { get; set; }
    public string? Municipality { get; set; }
    public DateTime RequestedStartDate { get; set; }
    public DateTime? RequestedEndDate { get; set; }
    public string? DurationUnit { get; set; }
    public string PriorityLevel { get; set; } = "Normal";
    public bool NeedsNightPresence { get; set; } = false;
    public bool NeedsTransportSupport { get; set; } = false;
    public string? MedicalSummary { get; set; }
    public string? SupportSummary { get; set; }
    public List<RequiredSkillModel> RequiredSkills { get; set; } = new();
}

public class RequiredSkillModel
{
    public Guid SkillId { get; set; }
    public string? RequiredLevel { get; set; }
    public bool Mandatory { get; set; } = true;
}

public class CareRequestResponse
{
    public Guid Id { get; set; }
    public string Status { get; set; } = "";
    public string PriorityLevel { get; set; } = "";
    public DateTime RequestedStartDate { get; set; }
    public string? Municipality { get; set; }
    public string? MedicalSummary { get; set; }
}

public class SuggestionsResponse
{
    public Guid RequestId { get; set; }
    public List<VolunteerSuggestion> Suggestions { get; set; } = new();
}
