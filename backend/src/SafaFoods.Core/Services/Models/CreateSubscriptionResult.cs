using SafaFoods.Core.Enums;

namespace SafaFoods.Core.Services.Models;

public sealed record CreateSubscriptionResult(
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
