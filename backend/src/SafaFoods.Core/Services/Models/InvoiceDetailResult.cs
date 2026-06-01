using SafaFoods.Core.Enums;

namespace SafaFoods.Core.Services.Models;

public sealed record InvoiceDetailResult(
    Guid OrderId,
    string InvoiceNumber,
    DateTimeOffset InvoiceIssuedAt,
    string CustomerName,
    string CustomerPhone,
    string AddressLine1,
    string? AddressLine2,
    string Area,
    string City,
    string Pincode,
    PaymentMethod PaymentMethod,
    PaymentStatus PaymentStatus,
    decimal Subtotal,
    decimal DiscountTotal,
    decimal DeliveryFee,
    decimal GrandTotal,
    IReadOnlyList<InvoiceDetailItemResult> Items);
