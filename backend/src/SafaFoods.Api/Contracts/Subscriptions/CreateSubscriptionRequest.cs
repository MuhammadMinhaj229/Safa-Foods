namespace SafaFoods.Api.Contracts.Subscriptions;

public sealed record CreateSubscriptionRequest(
    Guid AddressId,
    Guid VariantId,
    string PlanCode,
    int QuantityPerDelivery,
    DateTimeOffset StartDate);
