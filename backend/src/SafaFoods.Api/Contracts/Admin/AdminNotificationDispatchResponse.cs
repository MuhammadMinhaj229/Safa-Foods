namespace SafaFoods.Api.Contracts.Admin;

public sealed record AdminNotificationDispatchResponse(
    Guid NotificationId,
    string EventType,
    string Recipient,
    string MessageStatus,
    string? ProviderReference);
