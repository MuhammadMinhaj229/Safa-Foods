namespace SafaFoods.Core.Services.Models;

public sealed record CreateSubscriptionCommand(
    Guid CustomerId,
    Guid AddressId,
    Guid VariantId,
    string PlanCode,
    int QuantityPerDelivery,
    DateTimeOffset StartDate);
