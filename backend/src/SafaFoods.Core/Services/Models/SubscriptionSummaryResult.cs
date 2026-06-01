using SafaFoods.Core.Enums;

namespace SafaFoods.Core.Services.Models;

public sealed record SubscriptionSummaryResult(
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
