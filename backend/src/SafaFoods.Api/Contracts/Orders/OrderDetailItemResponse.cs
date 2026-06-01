namespace SafaFoods.Api.Contracts.Orders;

public sealed record OrderDetailItemResponse(
    Guid ProductId,
    Guid VariantId,
    string ProductName,
    string VariantLabel,
    int Quantity,
    decimal UnitPrice,
    decimal TotalPrice);
