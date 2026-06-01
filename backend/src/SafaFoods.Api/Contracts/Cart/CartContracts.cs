namespace SafaFoods.Api.Contracts.Cart;

public sealed record AddCartItemRequest(Guid ProductId, Guid VariantId, int Quantity);

public sealed record UpdateCartItemRequest(int Quantity);

public sealed record CartItemResponse(
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

public sealed record ApplyCouponRequest(string CouponCode);

public sealed record CartResponse(
    Guid CartId,
    Guid CustomerId,
    IReadOnlyList<CartItemResponse> Items,
    decimal Subtotal,
    int TotalItems,
    string? AppliedCouponCode,
    decimal DiscountTotal,
    decimal GrandTotal);
