using SafaFoods.Core.Enums;

namespace SafaFoods.Api.Contracts.Orders;

public sealed record OrderDetailResponse(
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
    IReadOnlyList<OrderDetailItemResponse> Items);
