using Microsoft.EntityFrameworkCore;

using SafaFoods.Core.Enums;
using SafaFoods.Core.Services;
using SafaFoods.Core.Services.Models;
using SafaFoods.Infrastructure.Persistence;

namespace SafaFoods.Infrastructure.Services;

public sealed class AdminQueryService(SafaFoodsDbContext dbContext) : IAdminQueryService
{
    public async Task<AdminDashboardResult> GetDashboardAsync(CancellationToken cancellationToken = default)
    {
        var todayStart = DateTimeOffset.UtcNow.Date;
        var todayEnd = todayStart.AddDays(1);

        var totalCustomers = await dbContext.Customers.CountAsync(cancellationToken);
        var activeSubscriptions = await dbContext.Subscriptions.CountAsync(
            x => x.Status == SubscriptionStatus.Active,
            cancellationToken);
        var pendingSubscriptions = await dbContext.Subscriptions.CountAsync(
            x => x.Status == SubscriptionStatus.PendingPayment,
            cancellationToken);
        var openOrders = await dbContext.Orders.CountAsync(
            x => x.Status == OrderStatus.Placed ||
                 x.Status == OrderStatus.Confirmed ||
                 x.Status == OrderStatus.Preparing,
            cancellationToken);
        var outForDeliveryOrders = await dbContext.Orders.CountAsync(
            x => x.Status == OrderStatus.OutForDelivery,
            cancellationToken);
        var pendingPaymentOrders = await dbContext.Orders.CountAsync(
            x => x.PaymentMethod != PaymentMethod.Cod && x.PaymentStatus == PaymentStatus.Pending,
            cancellationToken);

        var todayRevenue = await dbContext.Orders
            .Where(x => x.PaymentStatus == PaymentStatus.Paid && x.PaidAt >= todayStart && x.PaidAt < todayEnd)
            .SumAsync(x => (decimal?)x.GrandTotal, cancellationToken) ?? 0m;

        var outstandingPrepaidRevenue = await dbContext.Orders
            .Where(x => x.PaymentMethod != PaymentMethod.Cod && x.PaymentStatus == PaymentStatus.Pending)
            .SumAsync(x => (decimal?)x.GrandTotal, cancellationToken) ?? 0m;

        return new AdminDashboardResult(
            totalCustomers,
            activeSubscriptions,
            pendingSubscriptions,
            openOrders,
            outForDeliveryOrders,
            pendingPaymentOrders,
            todayRevenue,
            outstandingPrepaidRevenue);
    }

    public async Task<IReadOnlyList<AdminOrderListItemResult>> GetOrdersAsync(
        OrderStatus? status,
        PaymentStatus? paymentStatus,
        int take,
        CancellationToken cancellationToken = default)
    {
        var query = dbContext.Orders
            .AsNoTracking()
            .Include(x => x.Customer)
            .Include(x => x.DeliveryZone)
            .AsQueryable();

        if (status is not null)
        {
            query = query.Where(x => x.Status == status);
        }

        if (paymentStatus is not null)
        {
            query = query.Where(x => x.PaymentStatus == paymentStatus);
        }

        return await query
            .OrderByDescending(x => x.PlacedAt)
            .Take(NormalizeTake(take))
            .Select(x => new AdminOrderListItemResult(
                x.Id,
                x.Customer.FullName,
                x.Customer.Phone,
                x.Status,
                x.PaymentMethod,
                x.PaymentStatus,
                x.GrandTotal,
                x.DeliveryZone != null ? x.DeliveryZone.Name : null,
                x.PaymentReference,
                x.PlacedAt))
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<AdminSubscriptionListItemResult>> GetSubscriptionsAsync(
        SubscriptionStatus? status,
        int take,
        CancellationToken cancellationToken = default)
    {
        var query = dbContext.Subscriptions
            .AsNoTracking()
            .Include(x => x.Customer)
            .Include(x => x.Product)
            .Include(x => x.Variant)
            .AsQueryable();

        if (status is not null)
        {
            query = query.Where(x => x.Status == status);
        }

        return await query
            .OrderByDescending(x => x.CreatedAt)
            .Take(NormalizeTake(take))
            .Select(x => new AdminSubscriptionListItemResult(
                x.Id,
                x.Customer.FullName,
                x.Customer.Phone,
                x.Product.Name,
                x.Variant.Label,
                x.Status,
                x.PlanCode,
                x.PlanPrice,
                x.PaymentReference,
                x.NextDeliveryDate))
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<NotificationLogItemResult>> GetNotificationsAsync(
        int take,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.NotificationLogs
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .Take(NormalizeTake(take))
            .Select(x => new NotificationLogItemResult(
                x.Id,
                x.CustomerId,
                x.EventType,
                x.Recipient,
                x.MessageStatus,
                x.ProviderReference,
                x.CreatedAt))
            .ToListAsync(cancellationToken);
    }

    public async Task<AdminNotificationHealthResult> GetNotificationHealthAsync(
        CancellationToken cancellationToken = default)
    {
        var queuedNotifications = await dbContext.NotificationLogs.CountAsync(
            x => x.MessageStatus == "queued",
            cancellationToken);
        var sentNotifications = await dbContext.NotificationLogs.CountAsync(
            x => x.MessageStatus == "sent",
            cancellationToken);
        var failedNotifications = await dbContext.NotificationLogs.CountAsync(
            x => x.MessageStatus != "queued" && x.MessageStatus != "sent",
            cancellationToken);
        var pendingOutboxEvents = await dbContext.OutboxEvents.CountAsync(
            x => x.Status == "pending",
            cancellationToken);
        var failedOutboxEvents = await dbContext.OutboxEvents.CountAsync(
            x => x.Status == "failed",
            cancellationToken);
        var latestNotificationAt = await dbContext.NotificationLogs
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => (DateTimeOffset?)x.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);
        var latestOutboxEventAt = await dbContext.OutboxEvents
            .OrderByDescending(x => x.OccurredAt)
            .Select(x => (DateTimeOffset?)x.OccurredAt)
            .FirstOrDefaultAsync(cancellationToken);

        return new AdminNotificationHealthResult(
            queuedNotifications,
            sentNotifications,
            failedNotifications,
            pendingOutboxEvents,
            failedOutboxEvents,
            latestNotificationAt,
            latestOutboxEventAt);
    }

    public async Task<IReadOnlyList<AdminOutboxEventItemResult>> GetOutboxEventsAsync(
        string? status,
        int take,
        CancellationToken cancellationToken = default)
    {
        var normalizedStatus = string.IsNullOrWhiteSpace(status) ? null : status.Trim().ToLowerInvariant();

        var query = dbContext.OutboxEvents
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(normalizedStatus))
        {
            query = query.Where(x => x.Status == normalizedStatus);
        }

        return await query
            .OrderByDescending(x => x.OccurredAt)
            .Take(NormalizeTake(take))
            .Select(x => new AdminOutboxEventItemResult(
                x.Id,
                x.EventType,
                x.AggregateType,
                x.AggregateId,
                x.Status,
                x.RetryCount,
                x.LastError,
                x.OccurredAt,
                x.ProcessedAt))
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<AdminAuditLogItemResult>> GetAuditLogsAsync(
        int take,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.AdminAuditLogs
            .AsNoTracking()
            .Include(x => x.AdminUser)
            .OrderByDescending(x => x.CreatedAt)
            .Take(NormalizeTake(take))
            .Select(x => new AdminAuditLogItemResult(
                x.Id,
                x.AdminUserId,
                x.AdminUser != null ? x.AdminUser.Name : null,
                x.AdminUser != null ? x.AdminUser.Email : null,
                x.Role,
                x.Action,
                x.EntityType,
                x.EntityId,
                x.MetadataJson,
                x.CreatedAt))
            .ToListAsync(cancellationToken);
    }

    private static int NormalizeTake(int take) => Math.Clamp(take <= 0 ? 20 : take, 1, 100);
}
