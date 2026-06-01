namespace SafaFoods.Core.Services.Models;

public sealed record OrderDetailItemResult(
    Guid ProductId,
    Guid VariantId,
    string ProductName,
    string VariantLabel,
    int Quantity,
    decimal UnitPrice,
    decimal TotalPrice);
