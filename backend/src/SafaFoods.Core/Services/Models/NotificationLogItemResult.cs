namespace SafaFoods.Core.Services.Models;

public sealed record NotificationLogItemResult(
    Guid NotificationId,
    Guid? CustomerId,
    string EventType,
    string Recipient,
    string MessageStatus,
    string? ProviderReference,
    DateTimeOffset CreatedAt);
