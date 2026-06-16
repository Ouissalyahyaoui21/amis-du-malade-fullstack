namespace AmisDuMaladeApp.Models;

// ── Volunteer ────────────────────────────────────────────────────────────────

public class VolunteerRegisterRequest
{
    public string FullName           { get; set; } = "";
    public string Phone              { get; set; } = "";
    public string Municipality       { get; set; } = "";
    public string VolunteerCategory  { get; set; } = "";
    public bool   CanHomeVisit       { get; set; } = true;
    public bool   CanHospitalVisit   { get; set; } = false;
    public bool   CanNightPresence   { get; set; } = false;
    public bool   HasTransportation  { get; set; } = false;
    public List<VolunteerSkillRequest>        Skills         { get; set; } = new();
    public List<VolunteerAvailabilityRequest> Availabilities { get; set; } = new();
    public List<string>                       CoverageAreas  { get; set; } = new();
}

public class VolunteerSkillRequest
{
    public string SkillName { get; set; } = "";  // e.g. "patient_care", "transport"
    public string? Level    { get; set; }
}

public class VolunteerAvailabilityRequest
{
    public string DayOfWeek { get; set; } = "";
    public string StartTime { get; set; } = "";
    public string EndTime   { get; set; } = "";
}

public class VolunteerResponse
{
    public Guid   Id                 { get; set; }
    public string FullName           { get; set; } = "";
    public string Phone              { get; set; } = "";
    public string? Email             { get; set; }
    public string? Municipality      { get; set; }
    public string? VolunteerCategory { get; set; }
    public string Status             { get; set; } = "Pending";
    public bool   CanHomeVisit       { get; set; }
    public bool   CanHospitalVisit   { get; set; }
    public bool   CanNightPresence   { get; set; }
    public bool   HasTransportation  { get; set; }
    public DateTime CreatedAt        { get; set; }
}

public class VolunteerSuggestion
{
    public Guid   VolunteerId   { get; set; }
    public string Name          { get; set; } = "";
    public string? Phone        { get; set; }
    public string? Municipality { get; set; }
    public double MatchScore    { get; set; }
    public List<string> Reasons { get; set; } = new();
    public string ReasonsText   => string.Join(" · ", Reasons);
}

public class SuggestionsApiResponse
{
    public Guid RequestId { get; set; }
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

// ── Care Request — نموذج جديد متوافق مع Backend ──────────────────────────────

public class CareRequestPublicPayload
{
    // معلومات المريض
    public string PatientName        { get; set; } = "";
    public int?   PatientAge         { get; set; }
    public string? PatientGender     { get; set; }
    public string? PatientMunicipality { get; set; }

    // معلومات الطالب
    public string RequesterName      { get; set; } = "";
    public string RequesterPhone     { get; set; } = "";
    public string? RequesterRelation { get; set; }

    // تفاصيل الطلب
    public string? CareLocationType  { get; set; }  // home/hospital/clinic/elderly_home/other
    public string? Municipality      { get; set; }
    public DateTime RequestedStartDate { get; set; } = DateTime.Now;
    public bool NeedsNightPresence   { get; set; } = false;
    public bool NeedsTransportSupport { get; set; } = false;
    public string PriorityLevel      { get; set; } = "Normal";
    public string? MedicalSummary    { get; set; }
    public string? SupportSummary    { get; set; }
    public List<string> RequiredSkillNames { get; set; } = new();
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

    public string StatusLabel => Status switch
    {
        "New"       => "جديد",
        "Reviewing" => "قيد المراجعة",
        "Assigned"  => "تم التعيين",
        "Active"    => "نشط",
        "Completed" => "مكتمل",
        "Cancelled" => "ملغى",
        _           => Status
    };

    public string PriorityLabel => Priority switch
    {
        "Low"    => "أولوية: منخفضة",
        "Normal" => "أولوية: عادية",
        "High"   => "أولوية: مرتفعة",
        "Urgent" => "أولوية: عاجلة",
        _        => $"أولوية: {Priority}"
    };

    public Color StatusColor => Status switch
    {
        "New"       => Color.FromArgb("#2563eb"),
        "Reviewing" => Color.FromArgb("#d97706"),
        "Assigned"  => Color.FromArgb("#7c3aed"),
        "Active"    => Color.FromArgb("#16a34a"),
        "Completed" => Color.FromArgb("#64748b"),
        "Cancelled" => Color.FromArgb("#dc2626"),
        _           => Color.FromArgb("#6b7280"),
    };
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

public class PatientDetailResponse
{
    public Guid     Id              { get; set; }
    public string    FullName        { get; set; } = "";
    public string?   Phone           { get; set; }
    public int?      Age             { get; set; }
    public string?   Gender          { get; set; }
    public string?   Address         { get; set; }
    public string?   Municipality    { get; set; }
    public string?   ResidenceType   { get; set; }
    public string?   HospitalName    { get; set; }
    public string?   MobilityStatus  { get; set; }
    public string?   DependencyLevel { get; set; }
    public string?   Notes           { get; set; }
    public DateTime  CreatedAt       { get; set; }

    public List<PatientContactInfo>      Contacts     { get; set; } = new();
    public List<PatientConditionInfo>    Conditions   { get; set; } = new();
    public List<PatientCareRequestInfo>  CareRequests { get; set; } = new();
}

public class PatientContactInfo
{
    public string  FullName          { get; set; } = "";
    public string  Phone             { get; set; } = "";
    public string? RelationToPatient { get; set; }
    public bool    IsPrimaryContact  { get; set; }
}

public class PatientConditionInfo
{
    public string  Name     { get; set; } = "";
    public string? Severity { get; set; }
}

public class PatientCareRequestInfo
{
    public Guid     Id        { get; set; }
    public string   Status    { get; set; } = "";
    public DateTime CreatedAt { get; set; }

    public string StatusLabel => Status switch
    {
        "New"       => "جديد",
        "Reviewing" => "قيد المراجعة",
        "Assigned"  => "تم التعيين",
        "Active"    => "نشط",
        "Completed" => "مكتمل",
        "Cancelled" => "ملغى",
        _           => Status
    };
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

public class ContributionApiResponse
{
    public Guid   Id      { get; set; }
    public string? Message { get; set; }
}

// ── Contribution ─────────────────────────────────────────────────────────────

public class ContributionItem
{
    public Guid   Id              { get; set; }
    public string ContributorName { get; set; } = "";
    public string Phone           { get; set; } = "";
    public string Type            { get; set; } = "Money";
    public string? Amount         { get; set; }
    public string? Description    { get; set; }
    public string? Message        { get; set; }
    public string Status          { get; set; } = "Pending";
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

// ── Interview ─────────────────────────────────────────────────────────────────

public class ScheduleInterviewRequest
{
    public Guid VolunteerId { get; set; }
    public DateTime ScheduledAt { get; set; }
    public string Location { get; set; } = "";
}

public class RecordInterviewResultRequest
{
    public string Result { get; set; } = ""; // Accepted / Rejected
    public int? Score { get; set; }
    public string? Notes { get; set; }
}

public class VolunteerInterviewItem
{
    public Guid Id { get; set; }
    public Guid VolunteerId { get; set; }
    public string VolunteerName { get; set; } = "";
    public string VolunteerPhone { get; set; } = "";
    public DateTime ScheduledAt { get; set; }
    public string Location { get; set; } = "";
    public string Status { get; set; } = "Scheduled";
    public int? Score { get; set; }
    public string? Result { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }

    public string ScheduledAtLabel => ScheduledAt.ToString("dd/MM/yyyy HH:mm");

    public string StatusLabel => (Status, Result) switch
    {
        ("Scheduled", _)             => "⏳ مجدولة",
        ("Completed", "Accepted")    => "✓ مقبول",
        ("Completed", _)             => "✗ مرفوض",
        ("Cancelled", _)             => "❌ ملغاة",
        _                            => Status
    };

    public bool IsScheduled => Status == "Scheduled";
    public bool IsCompleted => Status == "Completed";

    public Color StatusBgColor => (Status, Result) switch
    {
        ("Scheduled", _)          => Color.FromArgb("#fef3c7"),
        ("Completed", "Accepted") => Color.FromArgb("#dcfce7"),
        ("Completed", _)          => Color.FromArgb("#fee2e2"),
        _                         => Color.FromArgb("#f1f5f9")
    };

    public Color StatusTextColor => (Status, Result) switch
    {
        ("Scheduled", _)          => Color.FromArgb("#92400e"),
        ("Completed", "Accepted") => Color.FromArgb("#15803d"),
        ("Completed", _)          => Color.FromArgb("#dc2626"),
        _                         => Color.FromArgb("#64748b")
    };

    public string ResultLabel => Result == "Accepted" ? "✓ تم قبول المتطوع" : "✗ تم رفض المتطوع";

    public Color ResultBgColor => Result == "Accepted"
        ? Color.FromArgb("#dcfce7")
        : Color.FromArgb("#fee2e2");

    public Color ResultTextColor => Result == "Accepted"
        ? Color.FromArgb("#15803d")
        : Color.FromArgb("#dc2626");
}

// ── Training ─────────────────────────────────────────────────────────────────

public class CreateTrainingRequest
{
    public string Title { get; set; } = "";
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public string? Location { get; set; }
    public int Capacity { get; set; } = 20;
}

public class TrainingItem
{
    public Guid Id { get; set; }
    public string Title { get; set; } = "";
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public string? Location { get; set; }
    public int Capacity { get; set; }
    public int EnrolledCount { get; set; }
    public string Status { get; set; } = "Active";
    public DateTime CreatedAt { get; set; }

    public string StartDateLabel => StartDate.ToString("dd/MM/yyyy");
    public string CapacityLabel => $"{EnrolledCount}/{Capacity} مقعد";
}
