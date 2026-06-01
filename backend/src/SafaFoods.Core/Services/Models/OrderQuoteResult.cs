namespace SafaFoods.Core.Services.Models;

public sealed record OrderQuoteResult(
    decimal Subtotal,
    decimal DiscountTotal,
    decimal DeliveryFee,
    decimal GrandTotal,
    bool CodAllowed,
    string? ZoneLabel);
