using SafaFoods.Core.Enums;
using SafaFoods.Core.Services.Models;

namespace SafaFoods.Core.Services;

public interface IAdminQueryService
{
    Task<AdminDashboardResult> GetDashboardAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AdminOrderListItemResult>> GetOrdersAsync(
        OrderStatus? status,
        PaymentStatus? paymentStatus,
        int take,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AdminSubscriptionListItemResult>> GetSubscriptionsAsync(
        SubscriptionStatus? status,
        int take,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<NotificationLogItemResult>> GetNotificationsAsync(
        int take,
        CancellationToken cancellationToken = default);

    Task<AdminNotificationHealthResult> GetNotificationHealthAsync(
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AdminOutboxEventItemResult>> GetOutboxEventsAsync(
        string? status,
        int take,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AdminAuditLogItemResult>> GetAuditLogsAsync(
        int take,
        CancellationToken cancellationToken = default);
}
