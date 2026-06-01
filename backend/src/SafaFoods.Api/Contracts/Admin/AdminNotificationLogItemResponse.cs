namespace SafaFoods.Api.Contracts.Admin;

public sealed record AdminNotificationLogItemResponse(
    Guid NotificationId,
    Guid? CustomerId,
    string EventType,
    string Recipient,
    string MessageStatus,
    string? ProviderReference,
    DateTimeOffset CreatedAt);
