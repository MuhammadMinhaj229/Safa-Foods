namespace SafaFoods.Api.Contracts.Orders;

public sealed record CancelOrderRequest(
    string? Reason);
