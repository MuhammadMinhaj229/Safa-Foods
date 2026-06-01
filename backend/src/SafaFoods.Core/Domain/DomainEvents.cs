namespace SafaFoods.Core.Domain;

public static class DomainEvents
{
    public const string OrderCreated = "order.created";
    public const string OrderCancelled = "order.cancelled";
    public const string OrderStatusChanged = "order.status_changed";
    public const string ReviewRequested = "review.requested";
    public const string PaymentSubmitted = "payment.submitted";
    public const string PaymentConfirmed = "payment.confirmed";
    public const string SubscriptionCreated = "subscription.created";
    public const string SubscriptionActivated = "subscription.activated";
    public const string SubscriptionUpcomingDelivery = "subscription.upcoming_delivery";
    public const string SubscriptionStatusChanged = "subscription.status_changed";
    public const string SubscriptionPaused = "subscription.paused";
    public const string SubscriptionResumed = "subscription.resumed";
    public const string SubscriptionCancelled = "subscription.cancelled";
}
