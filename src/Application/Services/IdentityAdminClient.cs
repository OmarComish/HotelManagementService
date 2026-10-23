using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using HotelManagementService.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace HotelManagementService.Application.Services;

public class IdentityAdminClient : IIdentityAdminClient
{
    private static readonly SemaphoreSlim TokenLock = new(1, 1);
    private static string? _token;
    private static DateTime _tokenExpiresUtc = DateTime.MinValue;

    private readonly HttpClient _http;
    private readonly IConfiguration _config;

    public IdentityAdminClient(HttpClient http, IConfiguration config)
    {
        _http = http;
        _config = config;
    }

    public Task<IdentityOperationResult> CreateAccountAsync(string username, string email, string password, CancellationToken ct = default) =>
        SendAsync(HttpMethod.Post, "api/admin/accounts", new { userName = username, email, password }, ct);

    public Task<IdentityOperationResult> ResetPasswordAsync(string username, string newPassword, CancellationToken ct = default) =>
        SendAsync(HttpMethod.Put, $"api/admin/accounts/{Uri.EscapeDataString(username)}/password", new { newPassword }, ct);

    public Task<IdentityOperationResult> DeleteAccountAsync(string username, CancellationToken ct = default) =>
        SendAsync(HttpMethod.Delete, $"api/admin/accounts/{Uri.EscapeDataString(username)}", null, ct);

    private async Task<IdentityOperationResult> SendAsync(HttpMethod method, string path, object? body, CancellationToken ct)
    {
        try
        {
            var baseUrl = _config["IdentityService:identityServerUrl"]?.TrimEnd('/')
                ?? throw new InvalidOperationException("IdentityService:identityServerUrl is not configured");

            using var request = new HttpRequestMessage(method, $"{baseUrl}/{path}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await GetTokenAsync(baseUrl, ct));
            if (body is not null)
                request.Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

            using var response = await _http.SendAsync(request, ct);
            return response.IsSuccessStatusCode
                ? new IdentityOperationResult(true, null)
                : new IdentityOperationResult(false, ReadError(await response.Content.ReadAsStringAsync(ct), response));
        }
        catch (HttpRequestException ex)
        {
            //return new IdentityOperationResult(false, "The identity service is unavailable.");
              return new IdentityOperationResult(false, $"The identity service is unavailable: {ex.Message}");
        }
    }

    private async Task<string> GetTokenAsync(string baseUrl, CancellationToken ct)
    {
        if (_token is not null && DateTime.UtcNow < _tokenExpiresUtc) return _token;

        await TokenLock.WaitAsync(ct);
        try
        {
            if (_token is not null && DateTime.UtcNow < _tokenExpiresUtc) return _token;

            using var response = await _http.PostAsync($"{baseUrl}/connect/token",
                new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    ["grant_type"] = "client_credentials",
                    ["client_id"] = _config["IdentityService:admin_client_id"] ?? "",
                    ["client_secret"] = _config["IdentityService:admin_client_secret"] ?? "",
                    ["scope"] = "IdentityServerApi"
                }), ct);
            //response.EnsureSuccessStatusCode();
            if(!response.IsSuccessStatusCode)
            {
                 var error = await response.Content.ReadAsStringAsync(ct);
                 throw new HttpRequestException($"token request failed ({(int)response.StatusCode}): {error}");
            }

            using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync(ct));
            _token = doc.RootElement.GetProperty("access_token").GetString();
            var seconds = doc.RootElement.TryGetProperty("expires_in", out var e) ? e.GetInt32() : 300;
            _tokenExpiresUtc = DateTime.UtcNow.AddSeconds(Math.Max(seconds - 60, 30));
            return _token!;
        }
        finally
        {
            TokenLock.Release();
        }
    }

    private static string ReadError(string body, HttpResponseMessage response)
    {
        try
        {
            using var doc = JsonDocument.Parse(body);
            var root = doc.RootElement;

            if (root.TryGetProperty("errors", out var errors) && errors.ValueKind == JsonValueKind.Array)
            {
                var joined = string.Join(",", errors.EnumerateArray()
                    .Select(e => e.TryGetProperty("description", out var d) ? d.GetString() : null)
                    .Where(m => !string.IsNullOrWhiteSpace(m)));
                if (joined.Length > 0) return joined;
            }
            if (root.TryGetProperty("message", out var message) && message.GetString() is { Length: > 0 } text)
                return text;
        }
        catch (JsonException) { }

        return $"The identity service returned {(int)response.StatusCode}.";
    }
}