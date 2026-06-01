namespace SafaFoods.Api.Contracts.Subscriptions;

public sealed record SubscriptionQuoteResponse(
    string PlanCode,
    string PlanName,
    string BillingCycle,
    int DeliveriesInCycle,
    decimal DiscountPercent,
    decimal CycleSubtotal,
    decimal DiscountAmount,
    decimal Total);
