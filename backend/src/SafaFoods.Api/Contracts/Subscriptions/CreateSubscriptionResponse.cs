using SafaFoods.Core.Enums;

namespace SafaFoods.Api.Contracts.Subscriptions;

public sealed record CreateSubscriptionResponse(
    Guid SubscriptionId,
    SubscriptionStatus Status,
    string PlanCode,
    string BillingCycle,
    int DeliveriesInCycle,
    int QuantityPerDelivery,
    decimal PlanPrice,
    decimal DiscountPercent,
    DateTimeOffset StartDate,
    DateTimeOffset NextDeliveryDate);
