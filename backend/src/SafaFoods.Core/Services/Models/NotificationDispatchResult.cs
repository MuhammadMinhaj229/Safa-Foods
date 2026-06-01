namespace SafaFoods.Core.Services.Models;

public sealed record NotificationDispatchResult(
    Guid NotificationId,
    string EventType,
    string Recipient,
    string MessageStatus,
    string? ProviderReference);
