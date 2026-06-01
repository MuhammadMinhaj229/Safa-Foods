using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

using Microsoft.Extensions.Options;

using SafaFoods.Core.Entities;
using SafaFoods.Core.Options;
using SafaFoods.Core.Services;
using SafaFoods.Core.Services.Models;

namespace SafaFoods.Infrastructure.Services;

public sealed class MetaCloudWhatsAppNotificationProvider(
    HttpClient httpClient,
    IOptions<WhatsAppOptions> options) : IWhatsAppNotificationProvider
{
    public async Task<NotificationProviderResult> SendAsync(
        NotificationLog notification,
        string message,
        CancellationToken cancellationToken = default)
    {
        var config = options.Value;
        if (string.IsNullOrWhiteSpace(config.MetaAccessToken) || string.IsNullOrWhiteSpace(config.MetaPhoneNumberId))
        {
            return NotificationProviderResult.Failed(
                "provider_config_missing",
                "Meta WhatsApp Cloud API credentials are not configured.");
        }

        var apiVersion = string.IsNullOrWhiteSpace(config.MetaApiVersion) ? "v23.0" : config.MetaApiVersion.Trim();
        var baseUrl = string.IsNullOrWhiteSpace(config.MetaGraphBaseUrl) ? "https://graph.facebook.com" : config.MetaGraphBaseUrl.Trim().TrimEnd('/');
        var requestUrl = $"{baseUrl}/{apiVersion}/{config.MetaPhoneNumberId.Trim()}/messages";

        using var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", config.MetaAccessToken.Trim());

        var payload = new
        {
            messaging_product = "whatsapp",
            recipient_type = "individual",
            to = notification.Recipient,
            type = "text",
            text = new
            {
                preview_url = false,
                body = message
            }
        };

        request.Content = new StringContent(
            JsonSerializer.Serialize(payload),
            Encoding.UTF8,
            "application/json");

        try
        {
            using var response = await httpClient.SendAsync(request, cancellationToken);
            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                return NotificationProviderResult.Failed(
                    "provider_http_error",
                    $"Meta WhatsApp API returned {(int)response.StatusCode}: {responseBody}");
            }

            var providerReference = TryExtractMessageId(responseBody);
            return NotificationProviderResult.Sent("sent_meta", providerReference);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            return NotificationProviderResult.Failed(
                "provider_transport_error",
                $"Meta WhatsApp API request failed: {ex.Message}");
        }
    }

    private static string? TryExtractMessageId(string responseBody)
    {
        if (string.IsNullOrWhiteSpace(responseBody))
        {
            return null;
        }

        try
        {
            using var document = JsonDocument.Parse(responseBody);
            if (!document.RootElement.TryGetProperty("messages", out var messages) ||
                messages.ValueKind != JsonValueKind.Array ||
                messages.GetArrayLength() == 0)
            {
                return null;
            }

            var first = messages[0];
            return first.TryGetProperty("id", out var id) && id.ValueKind == JsonValueKind.String
                ? id.GetString()
                : null;
        }
        catch
        {
            return null;
        }
    }
}
