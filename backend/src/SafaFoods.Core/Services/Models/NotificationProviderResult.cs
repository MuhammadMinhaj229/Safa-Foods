namespace SafaFoods.Core.Services.Models;

public sealed record NotificationProviderResult(
    bool Success,
    string MessageStatus,
    string? ProviderReference,
    string? ErrorMessage)
{
    public static NotificationProviderResult Sent(string status, string? providerReference) =>
        new(true, status, providerReference, null);

    public static NotificationProviderResult Failed(string status, string? errorMessage) =>
        new(false, status, null, errorMessage);
}
