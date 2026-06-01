namespace SafaFoods.Api.Contracts.Orders;

public sealed record CheckoutQuoteItemRequest(
    string ProductName,
    int Quantity,
    decimal UnitPrice);
