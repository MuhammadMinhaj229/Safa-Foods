using SafaFoods.Core.Entities;
using SafaFoods.Core.Services;
using SafaFoods.Core.Services.Models;

namespace SafaFoods.Infrastructure.Services;

public sealed class SimulatedWhatsAppNotificationProvider : IWhatsAppNotificationProvider
{
    public Task<NotificationProviderResult> SendAsync(
        NotificationLog notification,
        string message,
        CancellationToken cancellationToken = default)
    {
        var providerReference = $"wa-sim-{notification.Id:N}";
        return Task.FromResult(NotificationProviderResult.Sent("sent_simulated", providerReference));
    }
}
