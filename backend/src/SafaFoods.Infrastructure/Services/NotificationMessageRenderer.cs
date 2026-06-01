using System.Text.Json;
using System.Text.RegularExpressions;

using SafaFoods.Core.Entities;
using SafaFoods.Core.Services;

namespace SafaFoods.Infrastructure.Services;

public sealed partial class NotificationMessageRenderer(
    INotificationTemplateRegistry notificationTemplateRegistry) : INotificationMessageRenderer
{
    public string Render(NotificationLog notification)
    {
        var payload = ReadPayload(notification.PayloadJson);
        var template = notificationTemplateRegistry.GetTemplate(notification.EventType);
        template ??= GetLegacyFallbackTemplate(notification.EventType);

        if (!string.IsNullOrWhiteSpace(template))
        {
            return ApplyTemplate(template, payload);
        }

        return payload is not null && payload.TryGetValue("message", out var message) && !string.IsNullOrWhiteSpace(message)
            ? message
            : $"Safa Foods update: {notification.EventType.Replace('_', ' ')}.";
    }

    private static Dictionary<string, string>? ReadPayload(string? payloadJson)
    {
        if (string.IsNullOrWhiteSpace(payloadJson))
        {
            return null;
        }

        try
        {
            return JsonSerializer.Deserialize<Dictionary<string, string>>(payloadJson);
        }
        catch
        {
            return null;
        }
    }

    private static string ApplyTemplate(string template, Dictionary<string, string>? payload)
    {
        if (payload is null || payload.Count == 0)
        {
            return PlaceholderRegex().Replace(template, string.Empty).Replace("  ", " ").Trim();
        }

        return PlaceholderRegex().Replace(template, match =>
        {
            var key = match.Groups[1].Value;
            return payload.TryGetValue(key, out var value) ? value : string.Empty;
        }).Replace("  ", " ").Trim();
    }

    private static string? GetLegacyFallbackTemplate(string eventType) => eventType switch
    {
        "order_payment_submitted" =>
            "Safa Foods payment received for review. Reference: {{paymentReference}}. We will verify your payment shortly.",
        "order_payment_confirmed" =>
            "Safa Foods payment confirmed. Reference: {{paymentReference}}. Your order is now cleared for processing.",
        "subscription_payment_submitted" =>
            "Safa Foods subscription payment received for review. Reference: {{paymentReference}}. We will verify it shortly.",
        "subscription_payment_confirmed" =>
            "Safa Foods subscription payment confirmed. Reference: {{paymentReference}}. Your Monday delivery schedule is now active.",
        _ => null
    };

    [GeneratedRegex("\\{\\{([a-zA-Z0-9_]+)\\}\\}")]
    private static partial Regex PlaceholderRegex();
}
