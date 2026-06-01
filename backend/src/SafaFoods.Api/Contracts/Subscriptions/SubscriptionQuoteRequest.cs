namespace SafaFoods.Api.Contracts.Subscriptions;

public sealed record SubscriptionQuoteRequest(
    decimal UnitPrice,
    int QuantityPerDelivery,
    string PlanCode);
