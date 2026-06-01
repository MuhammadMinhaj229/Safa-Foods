using SafaFoods.Core.Enums;

namespace SafaFoods.Core.Services.Models;

public sealed record SubscriptionDeliveryResult(
    Guid DeliveryId,
    DateTimeOffset ScheduledDate,
    int Quantity,
    decimal DeliveryFee,
    SubscriptionDeliveryStatus Status,
    Guid? FulfilledOrderId);
