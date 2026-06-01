using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using SafaFoods.Core.Enums;
using SafaFoods.Core.Options;
using SafaFoods.Core.Services;
using SafaFoods.Core.Services.Models;
using SafaFoods.Infrastructure.Persistence;

namespace SafaFoods.Infrastructure.Services;

public sealed class NotificationDispatchService(
    SafaFoodsDbContext dbContext,
    INotificationMessageRenderer notificationMessageRenderer,
    SimulatedWhatsAppNotificationProvider simulatedWhatsAppNotificationProvider,
    MetaCloudWhatsAppNotificationProvider metaCloudWhatsAppNotificationProvider,
    IOptions<WhatsAppOptions> whatsAppOptions) : INotificationDispatchService
{
    public async Task<IReadOnlyList<NotificationDispatchResult>> DispatchPendingAsync(
        int take,
        CancellationToken cancellationToken = default)
    {
        var notifications = await dbContext.NotificationLogs
            .Where(x => x.MessageStatus == "queued")
            .OrderBy(x => x.CreatedAt)
            .Take(Math.Clamp(take <= 0 ? 20 : take, 1, 100))
            .ToListAsync(cancellationToken);

        foreach (var notification in notifications)
        {
            if (notification.Channel != NotificationChannel.WhatsApp)
            {
                notification.MessageStatus = "unsupported_channel";
                continue;
            }

            var message = notificationMessageRenderer.Render(notification);
            var effectiveRecipient = ResolveRecipient(notification.Recipient);
            var provider = ResolveWhatsAppProvider();
            var providerNotification = CloneForDispatch(notification, effectiveRecipient);
            var result = await provider.SendAsync(providerNotification, message, cancellationToken);

            notification.MessageStatus = result.MessageStatus;
            notification.ProviderReference = result.ProviderReference ?? notification.ProviderReference;
            notification.PayloadJson ??= $$"""{"message":"{{EscapeJson(message)}}","effectiveRecipient":"{{EscapeJson(effectiveRecipient)}}"}""";
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return notifications
            .Select(x => new NotificationDispatchResult(
                x.Id,
                x.EventType,
                x.Recipient,
                x.MessageStatus,
                x.ProviderReference))
            .ToList();
    }

    private IWhatsAppNotificationProvider ResolveWhatsAppProvider() =>
        whatsAppOptions.Value.Provider.Trim().ToLowerInvariant() switch
        {
            "meta" or "meta-cloud" or "meta_cloud" => metaCloudWhatsAppNotificationProvider,
            _ => simulatedWhatsAppNotificationProvider
        };

    private string ResolveRecipient(string originalRecipient)
    {
        var options = whatsAppOptions.Value;
        if (options.UseDevelopmentRecipientOverride && !string.IsNullOrWhiteSpace(options.DevelopmentRecipientOverride))
        {
            return options.DevelopmentRecipientOverride.Trim();
        }

        return originalRecipient;
    }

    private static Core.Entities.NotificationLog CloneForDispatch(Core.Entities.NotificationLog notification, string recipient) =>
        new()
        {
            Id = notification.Id,
            CustomerId = notification.CustomerId,
            Channel = notification.Channel,
            EventType = notification.EventType,
            Recipient = recipient,
            MessageStatus = notification.MessageStatus,
            ProviderReference = notification.ProviderReference,
            PayloadJson = notification.PayloadJson,
            CreatedAt = notification.CreatedAt
        };

    private static string EscapeJson(string value) =>
        value.Replace("\\", "\\\\").Replace("\"", "\\\"");

    public async Task SendOtpAsync(string phone, string otpCode, CancellationToken cancellationToken = default)
    {
        var effectiveRecipient = ResolveRecipient(phone);
        var provider = ResolveWhatsAppProvider();
        var message = $"Your Safa Foods verification code is: *{otpCode}*. It is valid for 10 minutes. Do not share this with anyone.";

        var fakeNotification = new Core.Entities.NotificationLog
        {
            CustomerId = null,
            Channel = Core.Enums.NotificationChannel.WhatsApp,
            EventType = "customer.otp_requested",
            Recipient = effectiveRecipient,
            MessageStatus = "sending"
        };

        await provider.SendAsync(fakeNotification, message, cancellationToken);
    }
}
