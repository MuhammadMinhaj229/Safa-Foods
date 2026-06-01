namespace SafaFoods.Core.Services.Models;

public sealed record AdminNotificationHealthResult(
    int QueuedNotifications,
    int SentNotifications,
    int FailedNotifications,
    int PendingOutboxEvents,
    int FailedOutboxEvents,
    DateTimeOffset? LatestNotificationAt,
    DateTimeOffset? LatestOutboxEventAt);
