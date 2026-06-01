using SafaFoods.Core.Domain;
using SafaFoods.Core.Services;

namespace SafaFoods.Infrastructure.Services;

public sealed class NotificationTemplateRegistry : INotificationTemplateRegistry
{
    private static readonly IReadOnlyDictionary<string, string> Templates = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        [DomainEvents.OrderCreated] =
            "Hi {{customerName}}, your order {{orderCode}} has been received. Total: Rs. {{grandTotal}}. We will notify you when it is confirmed.",
        [DomainEvents.OrderCancelled] =
            "Hi {{customerName}}, your order {{orderCode}} has been cancelled. Reason: {{cancellationReason}}.",
        [DomainEvents.OrderStatusChanged] =
            "Hi {{customerName}}, your order {{orderCode}} is now {{statusLabel}}.",
        [DomainEvents.ReviewRequested] =
            "Hi {{customerName}}, your order {{orderCode}} was delivered. Please review your Safa Foods experience here: {{reviewUrl}}",
        [DomainEvents.PaymentSubmitted] =
            "Hi {{customerName}}, we received your payment reference {{paymentReference}} for {{entityLabel}}. We will verify it shortly.",
        [DomainEvents.PaymentConfirmed] =
            "Hi {{customerName}}, your payment reference {{paymentReference}} for {{entityLabel}} has been confirmed.",
        [NotificationEvents.OrderPaymentSubmitted] =
            "Safa Foods payment received for review. Reference: {{paymentReference}}. We will verify your payment shortly.",
        [NotificationEvents.OrderPaymentConfirmed] =
            "Safa Foods payment confirmed. Reference: {{paymentReference}}. Your order is now cleared for processing.",
        [NotificationEvents.SubscriptionPaymentSubmitted] =
            "Safa Foods subscription payment received for review. Reference: {{paymentReference}}. We will verify it shortly.",
        [NotificationEvents.SubscriptionPaymentConfirmed] =
            "Safa Foods subscription payment confirmed. Reference: {{paymentReference}}. Your Monday delivery schedule is now active.",
        [DomainEvents.SubscriptionCreated] =
            "Hi {{customerName}}, your subscription {{subscriptionCode}} has been created and is awaiting payment confirmation.",
        [DomainEvents.SubscriptionActivated] =
            "Hi {{customerName}}, your subscription {{subscriptionCode}} is now active. Next delivery: {{nextDeliveryDate}}.",
        [DomainEvents.SubscriptionUpcomingDelivery] =
            "Hi {{customerName}}, your Safa Foods subscription {{subscriptionCode}} is scheduled for {{nextDeliveryDate}}. Product: {{productName}} {{variantLabel}} x {{quantityPerDelivery}}.",
        [DomainEvents.SubscriptionPaused] =
            "Hi {{customerName}}, your subscription {{subscriptionCode}} is now paused.",
        [DomainEvents.SubscriptionResumed] =
            "Hi {{customerName}}, your subscription {{subscriptionCode}} has resumed. Next delivery: {{nextDeliveryDate}}.",
        [DomainEvents.SubscriptionCancelled] =
            "Hi {{customerName}}, your subscription {{subscriptionCode}} has been cancelled."
    };

    public string? GetTemplate(string eventType) =>
        Templates.TryGetValue(eventType, out var template) ? template : null;
}
