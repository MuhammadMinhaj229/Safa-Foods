using SafaFoods.Core.Enums;

namespace SafaFoods.Core.Services.Models;

public sealed record OrderDetailResult(
    Guid OrderId,
    Guid CustomerId,
    Guid AddressId,
    OrderStatus Status,
    PaymentMethod PaymentMethod,
    PaymentStatus PaymentStatus,
    decimal Subtotal,
    decimal DiscountTotal,
    decimal DeliveryFee,
    decimal GrandTotal,
    string? ZoneLabel,
    DateTimeOffset PlacedAt,
    IReadOnlyList<OrderDetailItemResult> Items);
