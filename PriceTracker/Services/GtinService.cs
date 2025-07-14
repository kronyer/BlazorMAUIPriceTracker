using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

public class GtinService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;
    private string? _token;
    private DateTime _tokenExpiresAt = DateTime.MinValue;

    public GtinService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;
    }

    private async Task<string> GetTokenAsync()
    {
        try
        {

            if (!string.IsNullOrEmpty(_token) && DateTime.UtcNow < _tokenExpiresAt)
                return _token;

            var username = "user";
            var password = "123";
            var basicAuth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));

            var request = new HttpRequestMessage(HttpMethod.Post, "https://gtin.rscsistemas.com.br/oauth/token");
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", basicAuth);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            _token = doc.RootElement.GetProperty("token").GetString();
            _tokenExpiresAt = GetJwtExpiration(_token) ?? DateTime.UtcNow.AddHours(1); // fallback 1h

            return _token!;
        }
        catch (Exception ex)
        {
            // Log the exception (consider using a logging framework)
            Console.WriteLine($"Error getting token: {ex.Message}");
            throw;

        }
    }

    public async Task<GtinInfo?> GetGtinInfoAsync(string ean)
    {
        await EnsureTokenAsync();

        var request = new HttpRequestMessage(HttpMethod.Get, $"https://gtin.rscsistemas.com.br/api/gtin/infor/{ean}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

        var response = await _httpClient.SendAsync(request);

        if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
        {
            // Token expirou, tenta renovar e refazer a requisição
            _token = null;
            await EnsureTokenAsync();
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            response = await _httpClient.SendAsync(request);
        }

        if (!response.IsSuccessStatusCode)
            return null;

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<GtinInfo>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    private async Task EnsureTokenAsync()
    {
        if (string.IsNullOrEmpty(_token) || DateTime.UtcNow >= _tokenExpiresAt)
            await GetTokenAsync();
    }

    private static DateTime? GetJwtExpiration(string jwt)
    {
        if (string.IsNullOrEmpty(jwt)) return null;

        var parts = jwt.Split('.');
        if (parts.Length != 3) return null;

        var payload = parts[1];
        // Corrige padding do base64
        switch (payload.Length % 4)
        {
            case 2: payload += "=="; break;
            case 3: payload += "="; break;
        }

        var bytes = Convert.FromBase64String(payload.Replace('-', '+').Replace('_', '/'));
        var json = System.Text.Encoding.UTF8.GetString(bytes);

        using var doc = JsonDocument.Parse(json);
        if (doc.RootElement.TryGetProperty("exp", out var expElement))
        {
            var exp = expElement.GetInt64();
            // exp é em segundos desde 1970-01-01T00:00:00Z (Unix epoch)
            var date = DateTimeOffset.FromUnixTimeSeconds(exp).UtcDateTime;
            return date;
        }
        return null;
    }

}

