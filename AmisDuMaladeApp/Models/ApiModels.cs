namespace AmisDuMaladeApp.Models;

// ── Volunteer registration ────────────────────────────────────────────────────

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

// ── Care request ─────────────────────────────────────────────────────────────

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
    public List<string> HealthConditionKeys   { get; set; } = new();
    public DateTime StartDate      { get; set; }
    public bool NeedsNightPresence { get; set; }
    public string LocationKey      { get; set; } = "";
    public string DurationKey      { get; set; } = "";
    public List<string> QualificationKeys     { get; set; } = new();
    public string CompanionNotes   { get; set; } = "";
}
