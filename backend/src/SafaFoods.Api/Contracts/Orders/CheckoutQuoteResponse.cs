namespace SafaFoods.Api.Contracts.Orders;

public sealed record CheckoutQuoteResponse(
    decimal Subtotal,
    decimal DiscountTotal,
    decimal DeliveryFee,
    decimal GrandTotal,
    bool CodAllowed,
    string? ZoneLabel);
