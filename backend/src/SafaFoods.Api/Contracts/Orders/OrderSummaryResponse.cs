using SafaFoods.Core.Enums;

namespace SafaFoods.Api.Contracts.Orders;

public sealed record OrderSummaryResponse(
    Guid OrderId,
    OrderStatus Status,
    PaymentMethod PaymentMethod,
    PaymentStatus PaymentStatus,
    decimal GrandTotal,
    string? ZoneLabel,
    DateTimeOffset PlacedAt,
    int ItemCount);
