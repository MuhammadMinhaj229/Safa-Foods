namespace SafaFoods.Api.Contracts.Orders;

public sealed record CreateOrderItemRequest(
    Guid VariantId,
    int Quantity);
