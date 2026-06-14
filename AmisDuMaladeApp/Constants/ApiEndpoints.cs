namespace AmisDuMaladeApp.Constants;

public static class ApiEndpoints
{
    // Auth (public)
    public const string Login = "api/auth/login";
    public const string AuthRegister = "api/auth/register";

    // Volunteer
    public const string VolunteerRegister = "api/volunteer/register";  // public
    public const string Volunteers = "api/volunteer";                   // [Auth]
    public static string VolunteerById(Guid id) => $"api/volunteer/{id}";
    public static string VolunteerStatus(Guid id) => $"api/volunteer/{id}/status";

    // Interview [Auth]
    public const string Interviews = "api/interview";
    public static string InterviewResult(Guid id) => $"api/interview/{id}/result";
    public static string InterviewCancel(Guid id) => $"api/interview/{id}/cancel";

    // Training [Auth]
    public const string Trainings = "api/training";
    public static string TrainingById(Guid id) => $"api/training/{id}";
    public static string TrainingEnroll(Guid id) => $"api/training/{id}/enroll";
    public static string TrainingEnrollments(Guid id) => $"api/training/{id}/enrollments";

    // CareRequest
    public const string CareRequests = "api/carerequest";              // POST public, GET [Auth]
    public static string CareRequestById(Guid id) => $"api/carerequest/{id}";
    public static string CareRequestStatus(Guid id) => $"api/carerequest/{id}/status";
    public static string CareRequestSuggestions(Guid id) => $"api/carerequest/{id}/suggestions";

    // Patient [Auth]
    public const string Patients = "api/patient";
    public static string PatientById(Guid id) => $"api/patient/{id}";

    // Assignment [Auth]
    public const string Assignments = "api/assignment";
    public static string AssignmentStatus(Guid id) => $"api/assignment/{id}/status";

    // Visit [Auth]
    public const string VisitSession = "api/visit/session";
    public static string VisitsByAssignment(Guid id) => $"api/visit/assignment/{id}";
    public static string VisitSessionById(Guid id) => $"api/visit/session/{id}";
    public static string VisitNote(Guid id) => $"api/visit/session/{id}/note";
    public static string VisitRating(Guid id) => $"api/visit/session/{id}/rating";

    // Alert [Auth]
    public const string Alerts = "api/alert";
    public const string OpenAlerts = "api/alert/open";
    public static string ResolveAlert(Guid id) => $"api/alert/{id}/resolve";

    // Dashboard [Auth]
    public const string Dashboard = "api/dashboard";

    // Contribution [Auth for list, public for submit]
    public const string Contributions = "api/contribution";
    public static string ContributionById(Guid id)      => $"api/contribution/{id}";
    public static string ContributionStatus(Guid id)    => $"api/contribution/{id}/status";

    // Sync [Auth]
    public const string SyncPull = "api/sync/pull";
    public const string SyncPush = "api/sync/push";
}
