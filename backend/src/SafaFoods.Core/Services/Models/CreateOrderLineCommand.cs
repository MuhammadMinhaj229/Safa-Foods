namespace SafaFoods.Core.Services.Models;

public sealed record CreateOrderLineCommand(
    Guid VariantId,
    int Quantity);
