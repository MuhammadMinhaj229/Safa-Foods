using SafaFoods.Core.Enums;

namespace SafaFoods.Core.Services.Models;

public sealed record AdminSubscriptionListItemResult(
    Guid SubscriptionId,
    string CustomerName,
    string CustomerPhone,
    string ProductName,
    string VariantLabel,
    SubscriptionStatus Status,
    string PlanCode,
    decimal PlanPrice,
    string? PaymentReference,
    DateTimeOffset NextDeliveryDate);
