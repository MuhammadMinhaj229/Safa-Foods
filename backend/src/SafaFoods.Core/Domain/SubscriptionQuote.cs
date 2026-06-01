namespace SafaFoods.Core.Domain;

public sealed record SubscriptionQuote(
    bool Success,
    string? ErrorMessage,
    string? PlanCode,
    string? PlanName,
    string? BillingCycle,
    int DeliveriesInCycle,
    decimal DiscountPercent,
    decimal CycleSubtotal,
    decimal DiscountAmount,
    decimal Total);
