using SafaFoods.Core.Enums;

namespace SafaFoods.Api.Contracts.Subscriptions;

public sealed record SubscriptionSummaryResponse(
    Guid SubscriptionId,
    Guid ProductId,
    Guid VariantId,
    string ProductName,
    string VariantLabel,
    SubscriptionStatus Status,
    string PlanCode,
    decimal PlanPrice,
    DateTimeOffset StartDate,
    DateTimeOffset NextDeliveryDate);
