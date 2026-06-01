namespace SafaFoods.Core.Domain;

public sealed record SubscriptionPlan(
    string Code,
    string Name,
    string BillingCycle,
    int DeliveriesInCycle,
    decimal DiscountPercent);
