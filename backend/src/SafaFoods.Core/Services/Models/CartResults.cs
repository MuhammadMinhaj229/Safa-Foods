namespace SafaFoods.Core.Services.Models;

public sealed record CartItemResult(
    Guid ItemId,
    Guid ProductId,
    Guid VariantId,
    string ProductName,
    string VariantLabel,
    string? ImageUrl,
    int Quantity,
    decimal UnitPrice,
    decimal LineTotal,
    bool InStock);

public sealed record CartResult(
    Guid CartId,
    Guid CustomerId,
    IReadOnlyList<CartItemResult> Items,
    decimal Subtotal,
    int TotalItems,
    string? AppliedCouponCode = null,
    decimal DiscountTotal = 0,
    decimal GrandTotal = 0);
