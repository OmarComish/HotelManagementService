using System.Net.Http.Headers;
using System.Net.Http.Json;
using HotelManagementService.Application.Interfaces.Notifications;
using Microsoft.Extensions.Options;

namespace HotelManagementService.Infrastructure.Notifications;

public class WhatsAppSender : IWhatsAppSender
{
    private readonly HttpClient _httpClient;
    private readonly WhatsAppSettings _settings;
    public bool isEnabled => _settings.Enabled;

    public WhatsAppSender(
        HttpClient httpClient,
        IOptions<WhatsAppSettings> options)
    {
        _httpClient = httpClient;
        _settings = options.Value;
    }

    public async Task SendMessageAsync(
        string recipient,
        IReadOnlyList<string> parameters,
        CancellationToken cancellationToken = default)
    {
        var url =
            $"https://graph.facebook.com/{_settings.ApiVersion}/{_settings.PhoneNumberId}/messages";

        var requestBody = new
        {
            messaging_product = "whatsapp",
            recipient_type = "individual",
            to = recipient,
            type = "template",
            template = new
            {
                name = _settings.TemplateName,
                language = new
                {
                    code = _settings.LanguageCode
                },
                components = new[]
                {
                    new
                    {
                        type = "body",
                        parameters = parameters.Select(value => new
                        {
                            type = "text",
                            text = value
                        }).ToArray()
                    }
                }
            }
        };

        using var request = new HttpRequestMessage(HttpMethod.Post, url);

        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", _settings.AccessToken);

        request.Content = JsonContent.Create(requestBody);

        using var response = await _httpClient.SendAsync(
            request,
            cancellationToken);

        response.EnsureSuccessStatusCode();
    }
}