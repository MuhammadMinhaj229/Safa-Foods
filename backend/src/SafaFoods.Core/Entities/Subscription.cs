using SafaFoods.Core.Enums;

namespace SafaFoods.Core.Entities;

public sealed class Subscription
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CustomerId { get; set; }
    public Guid ProductId { get; set; }
    public Guid VariantId { get; set; }
    public required string PlanCode { get; set; }
    public required string BillingCycle { get; set; }
    public int DeliveriesInCycle { get; set; }
    public int QuantityPerDelivery { get; set; }
    public decimal PlanPrice { get; set; }
    public decimal DiscountPercent { get; set; }
    public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Upi;
    public string? PaymentReference { get; set; }
    public DateTimeOffset? PaidAt { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }
    public DateTimeOffset NextDeliveryDate { get; set; }
    public DateTimeOffset? NextBillingDate { get; set; }
    public SubscriptionStatus Status { get; set; } = SubscriptionStatus.Draft;
    public Guid AddressId { get; set; }
    public Guid? DeliveryZoneId { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

    public Customer Customer { get; set; } = null!;
    public Product Product { get; set; } = null!;
    public ProductVariant Variant { get; set; } = null!;
    public Address Address { get; set; } = null!;
    public DeliveryZone? DeliveryZone { get; set; }
    public ICollection<SubscriptionDelivery> Deliveries { get; set; } = [];
}
