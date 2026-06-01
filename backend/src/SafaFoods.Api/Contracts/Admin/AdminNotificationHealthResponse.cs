namespace SafaFoods.Api.Contracts.Admin;

public sealed record AdminNotificationHealthResponse(
    int QueuedNotifications,
    int SentNotifications,
    int FailedNotifications,
    int PendingOutboxEvents,
    int FailedOutboxEvents,
    DateTimeOffset? LatestNotificationAt,
    DateTimeOffset? LatestOutboxEventAt);
