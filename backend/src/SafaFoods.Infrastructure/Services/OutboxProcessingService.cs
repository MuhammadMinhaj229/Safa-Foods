using System.Text.Json;

using Microsoft.EntityFrameworkCore;

using SafaFoods.Core.Entities;
using SafaFoods.Core.Enums;
using SafaFoods.Core.Services;
using SafaFoods.Infrastructure.Persistence;

namespace SafaFoods.Infrastructure.Services;

public sealed class OutboxProcessingService(
    SafaFoodsDbContext dbContext,
    INotificationTemplateRegistry notificationTemplateRegistry) : IOutboxProcessingService
{
    public async Task<int> ProcessPendingAsync(int take, CancellationToken cancellationToken = default)
    {
        var events = await dbContext.OutboxEvents
            .Where(x => x.Status == "pending")
            .OrderBy(x => x.OccurredAt)
            .Take(Math.Clamp(take <= 0 ? 20 : take, 1, 100))
            .ToListAsync(cancellationToken);

        var created = 0;

        foreach (var outboxEvent in events)
        {
            try
            {
                if (!ShouldCreateNotification(outboxEvent.EventType))
                {
                    outboxEvent.Status = "processed";
                    outboxEvent.ProcessedAt = DateTimeOffset.UtcNow;
                    continue;
                }

                var payload = ReadPayload(outboxEvent.PayloadJson);
                if (!payload.TryGetValue("recipient", out var recipient) || string.IsNullOrWhiteSpace(recipient))
                {
                    outboxEvent.Status = "failed";
                    outboxEvent.LastError = "recipient_missing";
                    outboxEvent.RetryCount += 1;
                    continue;
                }

                dbContext.NotificationLogs.Add(new NotificationLog
                {
                    CustomerId = TryReadGuid(payload, "customerId"),
                    Channel = NotificationChannel.WhatsApp,
                    EventType = outboxEvent.EventType,
                    Recipient = recipient,
                    MessageStatus = "queued",
                    ProviderReference = outboxEvent.Id.ToString("N"),
                    PayloadJson = outboxEvent.PayloadJson
                });

                outboxEvent.Status = "processed";
                outboxEvent.ProcessedAt = DateTimeOffset.UtcNow;
                outboxEvent.LastError = null;
                created += 1;
            }
            catch (Exception ex)
            {
                outboxEvent.Status = "failed";
                outboxEvent.LastError = ex.Message[..Math.Min(ex.Message.Length, 1000)];
                outboxEvent.RetryCount += 1;
            }
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        return created;
    }

    private bool ShouldCreateNotification(string eventType) =>
        !string.IsNullOrWhiteSpace(notificationTemplateRegistry.GetTemplate(eventType));

    private static Dictionary<string, string> ReadPayload(string payloadJson) =>
        JsonSerializer.Deserialize<Dictionary<string, string>>(payloadJson) ?? new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

    private static Guid? TryReadGuid(Dictionary<string, string> payload, string key) =>
        payload.TryGetValue(key, out var raw) && Guid.TryParse(raw, out var value) ? value : null;
}
