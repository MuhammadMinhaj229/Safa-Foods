using Microsoft.AspNetCore.Http.HttpResults;

using SafaFoods.Api.Configuration;
using SafaFoods.Api.Contracts.Admin;
using SafaFoods.Api.Contracts.Orders;
using SafaFoods.Api.Contracts.Payments;
using SafaFoods.Api.Contracts.Subscriptions;
using SafaFoods.Core.Services;
using SafaFoods.Core.Services.Models;

namespace SafaFoods.Api.Endpoints;

public static class AdminEndpoints
{
    public static RouteGroupBuilder MapAdminEndpoints(this RouteGroupBuilder api)
    {
        var group = api.MapGroup("/admin")
            .WithTags("Admin")
            .AddEndpointFilter<AdminAccessFilter>();

        group.MapGet("/dashboard", GetDashboardAsync)
            .WithName("GetAdminDashboard")
            .WithSummary("Returns dashboard metrics for owner and staff operations.");

        group.MapGet("/orders", GetAdminOrdersAsync)
            .WithName("GetAdminOrders")
            .WithSummary("Returns operational order list for admin users.");

        group.MapGet("/subscriptions", GetAdminSubscriptionsAsync)
            .WithName("GetAdminSubscriptions")
            .WithSummary("Returns operational subscription list for admin users.");

        group.MapGet("/notifications", GetAdminNotificationsAsync)
            .WithName("GetAdminNotifications")
            .WithSummary("Returns recent notification logs.");

        group.MapGet("/notifications/health", GetAdminNotificationHealthAsync)
            .WithName("GetAdminNotificationHealth")
            .WithSummary("Returns notification and outbox pipeline health metrics.");

        group.MapGet("/outbox", GetAdminOutboxEventsAsync)
            .WithName("GetAdminOutboxEvents")
            .WithSummary("Returns recent outbox events for automation diagnostics.");

        group.MapGet("/audit-logs", GetAdminAuditLogsAsync)
            .WithName("GetAdminAuditLogs")
            .WithSummary("Returns recent admin audit activity.");

        group.MapPost("/notifications/dispatch", DispatchNotificationsAsync)
            .WithName("DispatchNotifications")
            .WithSummary("Dispatches queued notifications through the current provider layer.");

        group.MapPost("/orders/{orderId:guid}/payments/confirm", ConfirmOrderPaymentAsync)
            .WithName("ConfirmOrderPayment")
            .WithSummary("Confirms a prepaid order payment and stores the payment reference.");

        group.MapPost("/orders/{orderId:guid}/status", UpdateOrderStatusAsync)
            .WithName("AdminUpdateOrderStatus")
            .WithSummary("Updates order status from the admin operations dashboard.");

        group.MapPost("/subscriptions/{subscriptionId:guid}/payments/confirm", ConfirmSubscriptionPaymentAsync)
            .WithName("ConfirmSubscriptionPayment")
            .WithSummary("Confirms a subscription payment and activates the subscription.");

        group.MapPost("/subscriptions/{subscriptionId:guid}/status", UpdateSubscriptionStatusAsync)
            .WithName("AdminUpdateSubscriptionStatus")
            .WithSummary("Updates subscription lifecycle status from admin operations.");

        return group;
    }

    private static async Task<Ok<AdminDashboardResponse>> GetDashboardAsync(
        IAdminQueryService adminQueryService,
        CancellationToken cancellationToken)
    {
        var dashboard = await adminQueryService.GetDashboardAsync(cancellationToken);

        return TypedResults.Ok(new AdminDashboardResponse(
            dashboard.TotalCustomers,
            dashboard.ActiveSubscriptions,
            dashboard.PendingSubscriptions,
            dashboard.OpenOrders,
            dashboard.OutForDeliveryOrders,
            dashboard.PendingPaymentOrders,
            dashboard.TodayRevenue,
            dashboard.OutstandingPrepaidRevenue));
    }

    private static async Task<Ok<IReadOnlyList<AdminOrderListItemResponse>>> GetAdminOrdersAsync(
        SafaFoods.Core.Enums.OrderStatus? status,
        SafaFoods.Core.Enums.PaymentStatus? paymentStatus,
        int take,
        IAdminQueryService adminQueryService,
        CancellationToken cancellationToken)
    {
        var orders = await adminQueryService.GetOrdersAsync(status, paymentStatus, take, cancellationToken);

        return TypedResults.Ok<IReadOnlyList<AdminOrderListItemResponse>>(orders
            .Select(x => new AdminOrderListItemResponse(
                x.OrderId,
                x.CustomerName,
                x.CustomerPhone,
                x.Status,
                x.PaymentMethod,
                x.PaymentStatus,
                x.GrandTotal,
                x.ZoneLabel,
                x.PaymentReference,
                x.PlacedAt))
            .ToList());
    }

    private static async Task<Ok<IReadOnlyList<AdminSubscriptionListItemResponse>>> GetAdminSubscriptionsAsync(
        SafaFoods.Core.Enums.SubscriptionStatus? status,
        int take,
        IAdminQueryService adminQueryService,
        CancellationToken cancellationToken)
    {
        var subscriptions = await adminQueryService.GetSubscriptionsAsync(status, take, cancellationToken);

        return TypedResults.Ok<IReadOnlyList<AdminSubscriptionListItemResponse>>(subscriptions
            .Select(x => new AdminSubscriptionListItemResponse(
                x.SubscriptionId,
                x.CustomerName,
                x.CustomerPhone,
                x.ProductName,
                x.VariantLabel,
                x.Status,
                x.PlanCode,
                x.PlanPrice,
                x.PaymentReference,
                x.NextDeliveryDate))
            .ToList());
    }

    private static async Task<Ok<IReadOnlyList<AdminNotificationLogItemResponse>>> GetAdminNotificationsAsync(
        int take,
        IAdminQueryService adminQueryService,
        CancellationToken cancellationToken)
    {
        var notifications = await adminQueryService.GetNotificationsAsync(take, cancellationToken);

        return TypedResults.Ok<IReadOnlyList<AdminNotificationLogItemResponse>>(notifications
            .Select(x => new AdminNotificationLogItemResponse(
                x.NotificationId,
                x.CustomerId,
                x.EventType,
                x.Recipient,
                x.MessageStatus,
                x.ProviderReference,
                x.CreatedAt))
            .ToList());
    }

    private static async Task<Ok<AdminNotificationHealthResponse>> GetAdminNotificationHealthAsync(
        IAdminQueryService adminQueryService,
        CancellationToken cancellationToken)
    {
        var health = await adminQueryService.GetNotificationHealthAsync(cancellationToken);

        return TypedResults.Ok(new AdminNotificationHealthResponse(
            health.QueuedNotifications,
            health.SentNotifications,
            health.FailedNotifications,
            health.PendingOutboxEvents,
            health.FailedOutboxEvents,
            health.LatestNotificationAt,
            health.LatestOutboxEventAt));
    }

    private static async Task<Ok<IReadOnlyList<AdminOutboxEventItemResponse>>> GetAdminOutboxEventsAsync(
        string? status,
        int take,
        IAdminQueryService adminQueryService,
        CancellationToken cancellationToken)
    {
        var outboxEvents = await adminQueryService.GetOutboxEventsAsync(status, take, cancellationToken);

        return TypedResults.Ok<IReadOnlyList<AdminOutboxEventItemResponse>>(outboxEvents
            .Select(x => new AdminOutboxEventItemResponse(
                x.OutboxEventId,
                x.EventType,
                x.AggregateType,
                x.AggregateId,
                x.Status,
                x.RetryCount,
                x.LastError,
                x.OccurredAt,
                x.ProcessedAt))
            .ToList());
    }

    private static async Task<Ok<IReadOnlyList<AdminAuditLogItemResponse>>> GetAdminAuditLogsAsync(
        int take,
        IAdminQueryService adminQueryService,
        CancellationToken cancellationToken)
    {
        var auditLogs = await adminQueryService.GetAuditLogsAsync(take, cancellationToken);

        return TypedResults.Ok<IReadOnlyList<AdminAuditLogItemResponse>>(auditLogs
            .Select(x => new AdminAuditLogItemResponse(
                x.AuditLogId,
                x.AdminUserId,
                x.AdminName,
                x.AdminEmail,
                x.Role,
                x.Action,
                x.EntityType,
                x.EntityId,
                x.MetadataJson,
                x.CreatedAt))
            .ToList());
    }

    private static async Task<Ok<IReadOnlyList<AdminNotificationDispatchResponse>>> DispatchNotificationsAsync(
        HttpContext httpContext,
        int take,
        INotificationDispatchService notificationDispatchService,
        IAdminAuditService adminAuditService,
        CancellationToken cancellationToken)
    {
        var dispatched = await notificationDispatchService.DispatchPendingAsync(take, cancellationToken);

        await adminAuditService.LogAsync(
            GetAdminUserId(httpContext),
            GetAdminRole(httpContext),
            "notifications_dispatch_requested",
            "notification_batch",
            null,
            $$"""{"dispatchedCount":{{dispatched.Count}},"take":{{take}}}""",
            cancellationToken);

        return TypedResults.Ok<IReadOnlyList<AdminNotificationDispatchResponse>>(dispatched
            .Select(x => new AdminNotificationDispatchResponse(
                x.NotificationId,
                x.EventType,
                x.Recipient,
                x.MessageStatus,
                x.ProviderReference))
            .ToList());
    }

    private static async Task<Results<Ok<OrderDetailResponse>, ValidationProblem, NotFound<string>>> ConfirmOrderPaymentAsync(
        HttpContext httpContext,
        Guid orderId,
        ConfirmPaymentRequest request,
        IPaymentManagementService paymentManagementService,
        IAdminAuditService adminAuditService,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.PaymentReference))
        {
            return TypedResults.ValidationProblem(new Dictionary<string, string[]>
            {
                ["paymentReference"] = ["Payment reference is required."]
            });
        }

        var result = await paymentManagementService.ConfirmOrderPaymentAsync(
            orderId,
            request.PaymentReference,
            cancellationToken);

        if (!result.Success || result.Data is null)
        {
            return result.ErrorCode switch
            {
                "order_not_found" => TypedResults.NotFound(result.ErrorMessage ?? "Order not found."),
                _ => TypedResults.ValidationProblem(new Dictionary<string, string[]>
                {
                    ["payment"] = [result.ErrorMessage ?? "Unable to confirm order payment."]
                })
            };
        }

        await adminAuditService.LogAsync(
            GetAdminUserId(httpContext),
            GetAdminRole(httpContext),
            "order_payment_confirmed",
            "order",
            orderId,
            $$"""{"paymentReference":"{{request.PaymentReference.Trim()}}"}""",
            cancellationToken);

        return TypedResults.Ok(MapOrderDetail(result.Data));
    }

    private static async Task<Results<Ok<OrderDetailResponse>, ValidationProblem, NotFound<string>>> UpdateOrderStatusAsync(
        HttpContext httpContext,
        Guid orderId,
        UpdateOrderStatusRequest request,
        IOrderManagementService orderManagementService,
        IAdminAuditService adminAuditService,
        CancellationToken cancellationToken)
    {
        var result = await orderManagementService.UpdateStatusAsync(orderId, request.Status, cancellationToken);
        if (!result.Success || result.Data is null)
        {
            return result.ErrorCode switch
            {
                "order_not_found" => TypedResults.NotFound(result.ErrorMessage ?? "Order not found."),
                _ => TypedResults.ValidationProblem(new Dictionary<string, string[]>
                {
                    ["status"] = [result.ErrorMessage ?? "Unable to update order status."]
                })
            };
        }

        await adminAuditService.LogAsync(
            GetAdminUserId(httpContext),
            GetAdminRole(httpContext),
            "order_status_updated",
            "order",
            orderId,
            $$"""{"status":"{{request.Status}}"}""",
            cancellationToken);

        return TypedResults.Ok(MapOrderDetail(result.Data));
    }

    private static async Task<Results<Ok<SubscriptionDetailResponse>, ValidationProblem, NotFound<string>>> ConfirmSubscriptionPaymentAsync(
        HttpContext httpContext,
        Guid subscriptionId,
        ConfirmPaymentRequest request,
        IPaymentManagementService paymentManagementService,
        IAdminAuditService adminAuditService,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.PaymentReference))
        {
            return TypedResults.ValidationProblem(new Dictionary<string, string[]>
            {
                ["paymentReference"] = ["Payment reference is required."]
            });
        }

        var result = await paymentManagementService.ConfirmSubscriptionPaymentAsync(
            subscriptionId,
            request.PaymentReference,
            cancellationToken);

        if (!result.Success || result.Data is null)
        {
            return result.ErrorCode switch
            {
                "subscription_not_found" => TypedResults.NotFound(result.ErrorMessage ?? "Subscription not found."),
                _ => TypedResults.ValidationProblem(new Dictionary<string, string[]>
                {
                    ["payment"] = [result.ErrorMessage ?? "Unable to confirm subscription payment."]
                })
            };
        }

        await adminAuditService.LogAsync(
            GetAdminUserId(httpContext),
            GetAdminRole(httpContext),
            "subscription_payment_confirmed",
            "subscription",
            subscriptionId,
            $$"""{"paymentReference":"{{request.PaymentReference.Trim()}}"}""",
            cancellationToken);

        return TypedResults.Ok(MapSubscriptionDetail(result.Data));
    }

    private static async Task<Results<Ok<SubscriptionDetailResponse>, ValidationProblem, NotFound<string>>> UpdateSubscriptionStatusAsync(
        HttpContext httpContext,
        Guid subscriptionId,
        UpdateSubscriptionStatusRequest request,
        ISubscriptionManagementService subscriptionManagementService,
        IAdminAuditService adminAuditService,
        CancellationToken cancellationToken)
    {
        var result = await subscriptionManagementService.UpdateStatusAsync(subscriptionId, request.Status, cancellationToken);
        if (!result.Success || result.Data is null)
        {
            return result.ErrorCode switch
            {
                "subscription_not_found" => TypedResults.NotFound(result.ErrorMessage ?? "Subscription not found."),
                _ => TypedResults.ValidationProblem(new Dictionary<string, string[]>
                {
                    ["status"] = [result.ErrorMessage ?? "Unable to update subscription status."]
                })
            };
        }

        await adminAuditService.LogAsync(
            GetAdminUserId(httpContext),
            GetAdminRole(httpContext),
            "subscription_status_updated",
            "subscription",
            subscriptionId,
            $$"""{"status":"{{request.Status}}"}""",
            cancellationToken);

        return TypedResults.Ok(MapSubscriptionDetail(result.Data));
    }

    private static OrderDetailResponse MapOrderDetail(OrderDetailResult order) =>
        new(
            order.OrderId,
            order.CustomerId,
            order.AddressId,
            order.Status,
            order.PaymentMethod,
            order.PaymentStatus,
            order.Subtotal,
            order.DiscountTotal,
            order.DeliveryFee,
            order.GrandTotal,
            order.ZoneLabel,
            order.PlacedAt,
            order.Items.Select(item => new OrderDetailItemResponse(
                item.ProductId,
                item.VariantId,
                item.ProductName,
                item.VariantLabel,
                item.Quantity,
                item.UnitPrice,
                item.TotalPrice)).ToList());

    private static SubscriptionDetailResponse MapSubscriptionDetail(SubscriptionDetailResult subscription) =>
        new(
            subscription.SubscriptionId,
            subscription.CustomerId,
            subscription.AddressId,
            subscription.ProductId,
            subscription.VariantId,
            subscription.ProductName,
            subscription.VariantLabel,
            subscription.Status,
            subscription.PlanCode,
            subscription.BillingCycle,
            subscription.DeliveriesInCycle,
            subscription.QuantityPerDelivery,
            subscription.PlanPrice,
            subscription.DiscountPercent,
            subscription.StartDate,
            subscription.NextDeliveryDate,
            subscription.NextBillingDate,
            subscription.EndDate,
            subscription.ZoneLabel,
            subscription.Deliveries.Select(x => new SubscriptionDeliveryResponse(
                x.DeliveryId,
                x.ScheduledDate,
                x.Quantity,
                x.DeliveryFee,
                x.Status,
                x.FulfilledOrderId)).ToList());

    private static Guid? GetAdminUserId(HttpContext httpContext) =>
        httpContext.Items.TryGetValue("AdminUserId", out var value) && value is Guid id
            ? id
            : null;

    private static SafaFoods.Core.Enums.AdminRole GetAdminRole(HttpContext httpContext) =>
        httpContext.Items.TryGetValue("AdminRole", out var value) && value is SafaFoods.Core.Enums.AdminRole role
            ? role
            : SafaFoods.Core.Enums.AdminRole.StoreOperator;
}
