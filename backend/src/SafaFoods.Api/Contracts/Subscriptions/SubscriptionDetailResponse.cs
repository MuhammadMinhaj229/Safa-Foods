using SafaFoods.Core.Enums;

namespace SafaFoods.Api.Contracts.Subscriptions;

public sealed record SubscriptionDetailResponse(
    Guid SubscriptionId,
    Guid CustomerId,
    Guid AddressId,
    Guid ProductId,
    Guid VariantId,
    string ProductName,
    string VariantLabel,
    SubscriptionStatus Status,
    string PlanCode,
    string BillingCycle,
    int DeliveriesInCycle,
    int QuantityPerDelivery,
    decimal PlanPrice,
    decimal DiscountPercent,
    DateTimeOffset StartDate,
    DateTimeOffset NextDeliveryDate,
    DateTimeOffset? NextBillingDate,
    DateTimeOffset? EndDate,
    string? ZoneLabel,
    IReadOnlyList<SubscriptionDeliveryResponse> Deliveries);
