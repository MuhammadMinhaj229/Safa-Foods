using SafaFoods.Core.Entities;
using SafaFoods.Core.Services.Models;

namespace SafaFoods.Core.Services;

public interface IWhatsAppNotificationProvider
{
    Task<NotificationProviderResult> SendAsync(
        NotificationLog notification,
        string message,
        CancellationToken cancellationToken = default);
}
