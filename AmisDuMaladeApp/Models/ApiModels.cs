namespace AmisDuMaladeApp.Models;

// ── Volunteer ────────────────────────────────────────────────────────────────

public class VolunteerRegisterRequest
{
    public string FullName          { get; set; } = "";
    public string Phone             { get; set; } = "";
    public string Municipality      { get; set; } = "";
    public string VolunteerCategory { get; set; } = "";
    public List<VolunteerSkillRequest>        Skills         { get; set; } = new();
    public List<VolunteerAvailabilityRequest> Availabilities { get; set; } = new();
}

public class VolunteerSkillRequest
{
    public string Level { get; set; } = "";
}

public class VolunteerAvailabilityRequest
{
    public string DayOfWeek { get; set; } = "";
    public string StartTime { get; set; } = "";
    public string EndTime   { get; set; } = "";
}

public class VolunteerResponse
{
    public Guid   Id                { get; set; }
    public string FullName          { get; set; } = "";
    public string Phone             { get; set; } = "";
    public string? Email            { get; set; }
    public string? Municipality     { get; set; }
    public string? VolunteerCategory { get; set; }
    public string Status            { get; set; } = "Pending";
    public bool   CanHomeVisit      { get; set; }
    public bool   CanHospitalVisit  { get; set; }
    public bool   CanNightPresence  { get; set; }
    public bool   HasTransportation { get; set; }
    public DateTime CreatedAt       { get; set; }
}

public class VolunteerSuggestion
{
    public Guid   VolunteerId  { get; set; }
    public string FullName     { get; set; } = "";
    public string? Municipality { get; set; }
    public double MatchScore   { get; set; }
}

public class SuggestionsResponse
{
    public List<VolunteerSuggestion> Suggestions { get; set; } = new();
}

// ── Contribution (public submit) ─────────────────────────────────────────────

public class ContributionPayload
{
    public string  ContributorName { get; set; } = "";
    public string  Phone           { get; set; } = "";
    public string  Type            { get; set; } = "Money"; // Money | Goods | Time
    public decimal? Amount         { get; set; }
    public string? Description     { get; set; }
    public string? Message         { get; set; }
}

// ── Care Request ─────────────────────────────────────────────────────────────

public class CareRequestPayload
{
    public string RequesterName    { get; set; } = "";
    public string RequesterPhone   { get; set; } = "";
    public string CityDistrict     { get; set; } = "";
    public string RelationKey      { get; set; } = "";
    public string PatientName      { get; set; } = "";
    public string PatientAge       { get; set; } = "";
    public string PatientGender    { get; set; } = "";
    public string DetailedAddress  { get; set; } = "";
    public string Description      { get; set; } = "";
    public string InsuranceKey     { get; set; } = "";
    public List<string> HealthConditionKeys { get; set; } = new();
    public DateTime StartDate      { get; set; }
    public bool NeedsNightPresence { get; set; }
    public string LocationKey      { get; set; } = "";
    public string DurationKey      { get; set; } = "";
    public List<string> QualificationKeys { get; set; } = new();
    public string CompanionNotes   { get; set; } = "";
}

public class CareRequestApiResponse
{
    public Guid   Id              { get; set; }
    public string? ReferenceNumber { get; set; }
    public string Status          { get; set; } = "";
}

public class CareRequestListItem
{
    public Guid   Id            { get; set; }
    public string RequesterName { get; set; } = "";
    public string PatientName   { get; set; } = "";
    public string Status        { get; set; } = "";
    public string? Priority     { get; set; }
    public DateTime CreatedAt   { get; set; }
}

// ── Patient ──────────────────────────────────────────────────────────────────

public class PatientResponse
{
    public Guid   Id          { get; set; }
    public string FullName    { get; set; } = "";
    public string? Phone      { get; set; }
    public int?   Age         { get; set; }
    public string? Municipality { get; set; }
    public DateTime CreatedAt { get; set; }
}

// ── Alert ────────────────────────────────────────────────────────────────────

public class AlertResponse
{
    public Guid   Id          { get; set; }
    public string Title       { get; set; } = "";
    public string Description { get; set; } = "";
    public string Status      { get; set; } = "Open";
    public string? Type       { get; set; }
    public DateTime CreatedAt { get; set; }
}

// ── Generic ──────────────────────────────────────────────────────────────────

public class ApiIdResponse
{
    public Guid Id { get; set; }
}

// ── Contribution ─────────────────────────────────────────────────────────────

public class ContributionItem
{
    public Guid   Id              { get; set; }
    public string ContributorName { get; set; } = "";
    public string Phone           { get; set; } = "";
    public string Type            { get; set; } = "Money"; // Money | Goods | Time
    public string? Amount         { get; set; }
    public string? Description    { get; set; }
    public string? Message        { get; set; }
    public string Status          { get; set; } = "Pending"; // Pending | Confirmed | Distributed
    public DateTime CreatedAt     { get; set; }

    public string TypeIcon => Type switch
    {
        "Goods" => "📦",
        "Time"  => "⏰",
        _       => "💵",
    };
    public string TypeLabel => Type switch
    {
        "Goods" => "عيني",
        "Time"  => "وقت",
        _       => "نقدي",
    };
    public string StatusLabel => Status switch
    {
        "Confirmed"   => "✅ مؤكد",
        "Distributed" => "📤 موزَّع",
        _             => "⏳ معلق",
    };
    public Color StatusColor => Status switch
    {
        "Confirmed"   => Color.FromArgb("#16a34a"),
        "Distributed" => Color.FromArgb("#64748b"),
        _             => Color.FromArgb("#b45309"),
    };
    public Color TypeColor => Type switch
    {
        "Goods" => Color.FromArgb("#1d4ed8"),
        "Time"  => Color.FromArgb("#7c3aed"),
        _       => Color.FromArgb("#15803d"),
    };
}
