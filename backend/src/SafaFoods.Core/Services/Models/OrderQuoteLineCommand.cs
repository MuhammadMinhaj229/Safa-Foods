namespace SafaFoods.Core.Services.Models;

public sealed record OrderQuoteLineCommand(
    string ProductName,
    int Quantity,
    decimal UnitPrice);
