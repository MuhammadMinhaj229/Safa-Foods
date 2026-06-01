namespace SafaFoods.Core.Services.Models;

public sealed record InvoiceDetailItemResult(
    string ProductName,
    string VariantLabel,
    int Quantity,
    decimal UnitPrice,
    decimal TotalPrice);
