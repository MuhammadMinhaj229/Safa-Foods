using SafaFoods.Core.Services.Models;

namespace SafaFoods.Core.Services;

public interface INotificationDispatchService
{
    Task<IReadOnlyList<NotificationDispatchResult>> DispatchPendingAsync(
        int take,
        CancellationToken cancellationToken = default);

    /// <summary>Sends a one-time OTP code directly to the given phone number via WhatsApp.</summary>
    Task SendOtpAsync(string phone, string otpCode, CancellationToken cancellationToken = default);
}
