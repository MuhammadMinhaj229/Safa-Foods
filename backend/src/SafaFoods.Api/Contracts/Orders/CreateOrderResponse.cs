using SafaFoods.Core.Enums;

namespace SafaFoods.Api.Contracts.Orders;

public sealed record CreateOrderResponse(
    Guid OrderId,
    OrderStatus Status,
    decimal Subtotal,
    decimal DiscountTotal,
    decimal DeliveryFee,
    decimal GrandTotal,
    string? ZoneLabel,
    bool CodAllowed);
