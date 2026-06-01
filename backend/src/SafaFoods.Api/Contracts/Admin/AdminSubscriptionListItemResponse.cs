using SafaFoods.Core.Enums;

namespace SafaFoods.Api.Contracts.Admin;

public sealed record AdminSubscriptionListItemResponse(
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
