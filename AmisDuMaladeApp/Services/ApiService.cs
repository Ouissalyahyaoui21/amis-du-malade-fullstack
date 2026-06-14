using System.Net.Http.Headers;
using System.Net.Http.Json;
using AmisDuMaladeApp.Constants;
using AmisDuMaladeApp.Models;

namespace AmisDuMaladeApp.Services;

public class ApiService
{
    private readonly HttpClient _http;
    private readonly AuthTokenService _auth;

    public ApiService(AuthTokenService auth)
    {
        _auth = auth;
        _http = new HttpClient
        {
            BaseAddress = new Uri(AppConstants.BaseUrl),
            Timeout     = TimeSpan.FromSeconds(15)
        };
    }

    private void SetAuthHeader()
    {
        _http.DefaultRequestHeaders.Authorization = _auth.IsLoggedIn
            ? new AuthenticationHeaderValue("Bearer", _auth.Token)
            : null;
    }

    // ── Auth ─────────────────────────────────────────────────────────────────

    public async Task<LoginResponse?> LoginAsync(LoginRequest request)
    {
        try
        {
            var response = await _http.PostAsJsonAsync(ApiEndpoints.Login, request);
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsync<LoginResponse>();
        }
        catch { return null; }
    }

    // ── Volunteer ─────────────────────────────────────────────────────────────

    public async Task<(bool Success, string? Error)> RegisterVolunteerAsync(VolunteerRegisterRequest request)
    {
        try
        {
            var response = await _http.PostAsJsonAsync(ApiEndpoints.VolunteerRegister, request);
            return response.IsSuccessStatusCode
                ? (true, null)
                : (false, await response.Content.ReadAsStringAsync());
        }
        catch (Exception ex) { return (false, ex.Message); }
    }

    public async Task<List<VolunteerResponse>> GetVolunteersAsync()
    {
        SetAuthHeader();
        try { return await _http.GetFromJsonAsync<List<VolunteerResponse>>(ApiEndpoints.Volunteers) ?? new(); }
        catch { return new(); }
    }

    public async Task<bool> UpdateVolunteerStatusAsync(Guid id, string status)
    {
        SetAuthHeader();
        try
        {
            var response = await _http.PutAsJsonAsync(ApiEndpoints.VolunteerStatus(id), new { Status = status });
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }

    // ── Interview ─────────────────────────────────────────────────────────────

    public async Task<List<VolunteerInterviewItem>> GetInterviewsAsync()
    {
        SetAuthHeader();
        try { return await _http.GetFromJsonAsync<List<VolunteerInterviewItem>>(ApiEndpoints.Interviews) ?? new(); }
        catch { return new(); }
    }

    public async Task<bool> ScheduleInterviewAsync(ScheduleInterviewRequest request)
    {
        SetAuthHeader();
        try
        {
            var response = await _http.PostAsJsonAsync(ApiEndpoints.Interviews, request);
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }

    public async Task<bool> RecordInterviewResultAsync(Guid id, RecordInterviewResultRequest request)
    {
        SetAuthHeader();
        try
        {
            var response = await _http.PutAsJsonAsync(ApiEndpoints.InterviewResult(id), request);
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }

    public async Task<bool> CancelInterviewAsync(Guid id)
    {
        SetAuthHeader();
        try
        {
            var response = await _http.PutAsJsonAsync(ApiEndpoints.InterviewCancel(id), new { });
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }

    // ── Training ──────────────────────────────────────────────────────────────

    public async Task<List<TrainingItem>> GetTrainingsAsync()
    {
        SetAuthHeader();
        try { return await _http.GetFromJsonAsync<List<TrainingItem>>(ApiEndpoints.Trainings) ?? new(); }
        catch { return new(); }
    }

    public async Task<bool> CreateTrainingAsync(CreateTrainingRequest request)
    {
        SetAuthHeader();
        try
        {
            var response = await _http.PostAsJsonAsync(ApiEndpoints.Trainings, request);
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }

    // ── Care Request ──────────────────────────────────────────────────────────

    public async Task<(bool Success, string? ReferenceNumber, string? Error)> SubmitCareRequestAsync(CareRequestPublicPayload payload)
    {
        try
        {
            var response = await _http.PostAsJsonAsync(ApiEndpoints.CareRequests, payload);
            if (!response.IsSuccessStatusCode)
                return (false, null, await response.Content.ReadAsStringAsync());
            var body = await response.Content.ReadFromJsonAsync<CareRequestApiResponse>();
            return (true, body?.ReferenceNumber ?? body?.Id.ToString(), null);
        }
        catch (Exception ex) { return (false, null, ex.Message); }
    }

    public async Task<List<CareRequestListItem>> GetCareRequestsAsync()
    {
        SetAuthHeader();
        try { return await _http.GetFromJsonAsync<List<CareRequestListItem>>(ApiEndpoints.CareRequests) ?? new(); }
        catch { return new(); }
    }

    public async Task<bool> UpdateCareRequestStatusAsync(Guid id, string status)
    {
        SetAuthHeader();
        try
        {
            var response = await _http.PutAsJsonAsync(ApiEndpoints.CareRequestStatus(id), new { Status = status });
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }

    public async Task<List<VolunteerSuggestion>> GetSuggestionsAsync(Guid requestId)
    {
        SetAuthHeader();
        try
        {
            var response = await _http.GetFromJsonAsync<SuggestionsApiResponse>(
                ApiEndpoints.CareRequestSuggestions(requestId));
            return response?.Suggestions ?? new();
        }
        catch { return new(); }
    }

    public async Task<bool> AssignVolunteerAsync(Guid requestId, Guid volunteerId)
    {
        SetAuthHeader();
        try
        {
            var response = await _http.PostAsJsonAsync(
                ApiEndpoints.Assignments,
                new { CareRequestId = requestId, VolunteerId = volunteerId,
                      AssignmentType = "Primary", StartDate = DateTime.UtcNow });
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }

    // ── Patient ───────────────────────────────────────────────────────────────

    public async Task<List<PatientResponse>> GetPatientsAsync()
    {
        SetAuthHeader();
        try { return await _http.GetFromJsonAsync<List<PatientResponse>>(ApiEndpoints.Patients) ?? new(); }
        catch { return new(); }
    }

    // ── Contribution ─────────────────────────────────────────────────────────

    public async Task<(bool Success, string? Id, string? Error)> SubmitContributionAsync(ContributionPayload payload)
    {
        try
        {
            var response = await _http.PostAsJsonAsync(ApiEndpoints.Contributions, payload);
            if (!response.IsSuccessStatusCode)
                return (false, null, await response.Content.ReadAsStringAsync());
            var body = await response.Content.ReadFromJsonAsync<ContributionApiResponse>();
            return (true, body?.Id.ToString(), null);
        }
        catch (Exception ex) { return (false, null, ex.Message); }
    }

    public async Task<List<ContributionItem>> GetContributionsAsync()
    {
        SetAuthHeader();
        try { return await _http.GetFromJsonAsync<List<ContributionItem>>(ApiEndpoints.Contributions) ?? new(); }
        catch { return new(); }
    }

    public async Task<bool> UpdateContributionStatusAsync(Guid id, string status)
    {
        SetAuthHeader();
        try
        {
            var response = await _http.PutAsJsonAsync(ApiEndpoints.ContributionStatus(id), new { Status = status });
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }

    // ── Dashboard ─────────────────────────────────────────────────────────────

    public async Task<DashboardData?> GetDashboardAsync()
    {
        SetAuthHeader();
        try { return await _http.GetFromJsonAsync<DashboardData>(ApiEndpoints.Dashboard); }
        catch { return null; }
    }

    // ── Alerts ────────────────────────────────────────────────────────────────

    public async Task<List<AlertResponse>> GetOpenAlertsAsync()
    {
        SetAuthHeader();
        try { return await _http.GetFromJsonAsync<List<AlertResponse>>(ApiEndpoints.OpenAlerts) ?? new(); }
        catch { return new(); }
    }

    public async Task<bool> ResolveAlertAsync(Guid id)
    {
        SetAuthHeader();
        try
        {
            var response = await _http.PutAsJsonAsync(ApiEndpoints.ResolveAlert(id), new { });
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }
}
