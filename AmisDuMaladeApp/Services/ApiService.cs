using System.Net.Http.Json;
using AmisDuMaladeApp.Constants;
using AmisDuMaladeApp.Models;

namespace AmisDuMaladeApp.Services;

public class ApiService
{
    private readonly HttpClient _http;

    public ApiService()
    {
        _http = new HttpClient { BaseAddress = new Uri(AppConstants.BaseUrl) };
    }

    public async Task<(bool Success, string? Error)> RegisterVolunteerAsync(VolunteerRegisterRequest request)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("/api/volunteers/register", request);
            return response.IsSuccessStatusCode
                ? (true, null)
                : (false, await response.Content.ReadAsStringAsync());
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }

    public async Task<(bool Success, string? ReferenceNumber, string? Error)> SubmitCareRequestAsync(CareRequestPayload payload)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("/api/care-requests", payload);
            if (!response.IsSuccessStatusCode)
                return (false, null, await response.Content.ReadAsStringAsync());

            var body = await response.Content.ReadFromJsonAsync<CareRequestResponse>();
            return (true, body?.ReferenceNumber, null);
        }
        catch (Exception ex)
        {
            return (false, null, ex.Message);
        }
    }

    private record CareRequestResponse(string ReferenceNumber);
}
