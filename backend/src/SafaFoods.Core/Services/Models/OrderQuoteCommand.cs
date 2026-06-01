using SafaFoods.Core.Enums;

namespace SafaFoods.Core.Services.Models;

public sealed record OrderQuoteCommand(
    decimal DistanceKm,
    PaymentMethod PaymentMethod,
    IReadOnlyList<OrderQuoteLineCommand> Items);
