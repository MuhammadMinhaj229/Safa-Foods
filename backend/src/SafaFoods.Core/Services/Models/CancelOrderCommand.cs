namespace SafaFoods.Core.Services.Models;

public sealed record CancelOrderCommand(
    Guid OrderId,
    Guid CustomerId,
    string? Reason);
