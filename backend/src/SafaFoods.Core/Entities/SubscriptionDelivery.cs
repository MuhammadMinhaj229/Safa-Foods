using SafaFoods.Core.Enums;

namespace SafaFoods.Core.Entities;

public sealed class SubscriptionDelivery
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid SubscriptionId { get; set; }
    public DateTimeOffset ScheduledDate { get; set; }
    public int Quantity { get; set; }
    public decimal DeliveryFee { get; set; }
    public SubscriptionDeliveryStatus Status { get; set; } = SubscriptionDeliveryStatus.Scheduled;
    public Guid? FulfilledOrderId { get; set; }
    public int AttemptCount { get; set; }
    public DateTimeOffset? DeliveredAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

    public Subscription Subscription { get; set; } = null!;
    public Order? FulfilledOrder { get; set; }
}
