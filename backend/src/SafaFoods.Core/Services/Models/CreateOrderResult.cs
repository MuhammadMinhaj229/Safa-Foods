using SafaFoods.Core.Enums;

namespace SafaFoods.Core.Services.Models;

public sealed record CreateOrderResult(
    Guid OrderId,
    OrderStatus Status,
    decimal Subtotal,
    decimal DiscountTotal,
    decimal DeliveryFee,
    decimal GrandTotal,
    string? ZoneLabel,
    bool CodAllowed);
