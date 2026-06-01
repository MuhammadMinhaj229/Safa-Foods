using SafaFoods.Core.Enums;

namespace SafaFoods.Api.Contracts.Subscriptions;

public sealed record SubscriptionDeliveryResponse(
    Guid DeliveryId,
    DateTimeOffset ScheduledDate,
    int Quantity,
    decimal DeliveryFee,
    SubscriptionDeliveryStatus Status,
    Guid? FulfilledOrderId);
