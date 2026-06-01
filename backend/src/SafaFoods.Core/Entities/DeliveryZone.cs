namespace SafaFoods.Core.Entities;

public sealed class DeliveryZone
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Name { get; set; }
    public decimal MinKm { get; set; }
    public decimal? MaxKm { get; set; }
    public decimal Fee { get; set; }
    public int EstimatedMinMinutes { get; set; }
    public int EstimatedMaxMinutes { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

    public ICollection<Address> Addresses { get; set; } = [];
    public ICollection<Order> Orders { get; set; } = [];
    public ICollection<Subscription> Subscriptions { get; set; } = [];
}
