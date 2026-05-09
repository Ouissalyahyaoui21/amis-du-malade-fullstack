using System.Net.Http.Headers;
using System.Net.Http.Json;
using AmisDuMaladeApp.Constants;
using AmisDuMaladeApp.Models;

namespace AmisDuMaladeApp.Services;

public class ApiService
{
    private readonly HttpClient _http;
    private readonly AuthTokenService _auth;

    public ApiService(HttpClient http, AuthTokenService auth)
    {
        _http = http;
        _auth = auth;
    }

    private void SetAuthHeader()
    {
        if (_auth.IsLoggedIn)
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _auth.Token);
    }

    // ─── Auth ───────────────────────────────────────────────────────────────

    public async Task<LoginResponse?> LoginAsync(LoginRequest request)
    {
        var response = await _http.PostAsJsonAsync(ApiEndpoints.Login, request);
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<LoginResponse>();
    }

    // ─── Volunteer ──────────────────────────────────────────────────────────

    public async Task<(bool Success, Guid Id)> RegisterVolunteerAsync(VolunteerRegisterRequest request)
    {
        var response = await _http.PostAsJsonAsync(ApiEndpoints.VolunteerRegister, request);
        if (!response.IsSuccessStatusCode) return (false, Guid.Empty);
        var result = await response.Content.ReadFromJsonAsync<ApiIdResponse>();
        return (true, result?.Id ?? Guid.Empty);
    }

    public async Task<List<VolunteerResponse>> GetVolunteersAsync()
    {
        SetAuthHeader();
        var result = await _http.GetFromJsonAsync<List<VolunteerResponse>>(ApiEndpoints.Volunteers);
        return result ?? new();
    }

    public async Task<bool> UpdateVolunteerStatusAsync(Guid id, string status)
    {
        SetAuthHeader();
        var response = await _http.PutAsJsonAsync(ApiEndpoints.VolunteerStatus(id), new { Status = status });
        return response.IsSuccessStatusCode;
    }

    // ─── CareRequest ────────────────────────────────────────────────────────

    public async Task<(bool Success, Guid Id)> CreateCareRequestAsync(CreateCareRequestModel request)
    {
        var response = await _http.PostAsJsonAsync(ApiEndpoints.CareRequests, request);
        if (!response.IsSuccessStatusCode) return (false, Guid.Empty);
        var result = await response.Content.ReadFromJsonAsync<ApiIdResponse>();
        return (true, result?.Id ?? Guid.Empty);
    }

    public async Task<List<CareRequestResponse>> GetCareRequestsAsync()
    {
        SetAuthHeader();
        var result = await _http.GetFromJsonAsync<List<CareRequestResponse>>(ApiEndpoints.CareRequests);
        return result ?? new();
    }

    public async Task<List<VolunteerSuggestion>> GetSuggestionsAsync(Guid requestId)
    {
        SetAuthHeader();
        var response = await _http.GetFromJsonAsync<SuggestionsResponse>(
            ApiEndpoints.CareRequestSuggestions(requestId));
        return response?.Suggestions ?? new();
    }

    // ─── Patient ────────────────────────────────────────────────────────────

    public async Task<(bool Success, Guid Id)> CreatePatientAsync(CreatePatientModel request)
    {
        SetAuthHeader();
        var response = await _http.PostAsJsonAsync(ApiEndpoints.Patients, request);
        if (!response.IsSuccessStatusCode) return (false, Guid.Empty);
        var result = await response.Content.ReadFromJsonAsync<ApiIdResponse>();
        return (true, result?.Id ?? Guid.Empty);
    }

    public async Task<List<PatientResponse>> GetPatientsAsync()
    {
        SetAuthHeader();
        var result = await _http.GetFromJsonAsync<List<PatientResponse>>(ApiEndpoints.Patients);
        return result ?? new();
    }

    // ─── Dashboard ──────────────────────────────────────────────────────────

    public async Task<DashboardData?> GetDashboardAsync()
    {
        SetAuthHeader();
        return await _http.GetFromJsonAsync<DashboardData>(ApiEndpoints.Dashboard);
    }

    // ─── Alerts ─────────────────────────────────────────────────────────────

    public async Task<List<AlertResponse>> GetOpenAlertsAsync()
    {
        SetAuthHeader();
        var result = await _http.GetFromJsonAsync<List<AlertResponse>>(ApiEndpoints.OpenAlerts);
        return result ?? new();
    }
}
