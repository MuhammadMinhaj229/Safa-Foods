using SafaFoods.Core.Enums;

namespace SafaFoods.Core.Services.Models;

public sealed record OrderSummaryResult(
    Guid OrderId,
    OrderStatus Status,
    PaymentMethod PaymentMethod,
    PaymentStatus PaymentStatus,
    decimal GrandTotal,
    string? ZoneLabel,
    DateTimeOffset PlacedAt,
    int ItemCount);
