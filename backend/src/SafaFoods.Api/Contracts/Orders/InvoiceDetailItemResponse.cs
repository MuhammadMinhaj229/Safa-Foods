namespace SafaFoods.Api.Contracts.Orders;

public sealed record InvoiceDetailItemResponse(
    string ProductName,
    string VariantLabel,
    int Quantity,
    decimal UnitPrice,
    decimal TotalPrice);
