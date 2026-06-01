using SafaFoods.Core.Enums;

namespace SafaFoods.Api.Contracts.Orders;

public sealed record CheckoutQuoteRequest(
    decimal DistanceKm,
    PaymentMethod PaymentMethod,
    IReadOnlyList<CheckoutQuoteItemRequest> Items);
