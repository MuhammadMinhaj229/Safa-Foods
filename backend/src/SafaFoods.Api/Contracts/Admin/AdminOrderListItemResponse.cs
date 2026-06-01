using SafaFoods.Core.Enums;

namespace SafaFoods.Api.Contracts.Admin;

public sealed record AdminOrderListItemResponse(
    Guid OrderId,
    string CustomerName,
    string CustomerPhone,
    OrderStatus Status,
    PaymentMethod PaymentMethod,
    PaymentStatus PaymentStatus,
    decimal GrandTotal,
    string? ZoneLabel,
    string? PaymentReference,
    DateTimeOffset PlacedAt);
