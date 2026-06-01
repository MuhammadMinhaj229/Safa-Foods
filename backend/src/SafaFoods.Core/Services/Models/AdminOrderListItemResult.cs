using SafaFoods.Core.Enums;

namespace SafaFoods.Core.Services.Models;

public sealed record AdminOrderListItemResult(
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
