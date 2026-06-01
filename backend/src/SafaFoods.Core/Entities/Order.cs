using SafaFoods.Core.Enums;

namespace SafaFoods.Core.Entities;

public sealed class Order
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? ShopifyOrderId { get; set; }
    public Guid CustomerId { get; set; }
    public OrderType OrderType { get; set; }
    public decimal Subtotal { get; set; }
    public decimal DiscountTotal { get; set; }
    public decimal DeliveryFee { get; set; }
    public decimal WalletAmountUsed { get; set; }
    public decimal GrandTotal { get; set; }
    public string? AppliedCouponCode { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;
    public string? PaymentReference { get; set; }
    public DateTimeOffset? PaidAt { get; set; }
    public string? InvoiceNumber { get; set; }
    public DateTimeOffset? InvoiceIssuedAt { get; set; }
    public string? CancellationReason { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Placed;
    public Guid AddressId { get; set; }
    public Guid? DeliveryZoneId { get; set; }
    public Guid? DeliverySlotId { get; set; }
    public DateTimeOffset? ScheduledDeliveryDate { get; set; }
    public DateTimeOffset PlacedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

    public Customer Customer { get; set; } = null!;
    public Address Address { get; set; } = null!;
    public DeliveryZone? DeliveryZone { get; set; }
    public DeliverySlot? DeliverySlot { get; set; }
    public ICollection<OrderItem> Items { get; set; } = [];
    public ICollection<SubscriptionDelivery> SubscriptionDeliveries { get; set; } = [];
}
