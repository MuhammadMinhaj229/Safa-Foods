namespace SafaFoods.Core.Entities;

public sealed class Address
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CustomerId { get; set; }
    public required string FullName { get; set; }
    public required string Phone { get; set; }
    public required string AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? Landmark { get; set; }
    public required string Area { get; set; }
    public required string City { get; set; }
    public required string Pincode { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public decimal? DistanceKm { get; set; }
    public bool IsDefault { get; set; }
    public bool IsServiceable { get; set; }
    public Guid? DeliveryZoneId { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

    public Customer Customer { get; set; } = null!;
    public DeliveryZone? DeliveryZone { get; set; }
    public ICollection<Order> Orders { get; set; } = [];
    public ICollection<Subscription> Subscriptions { get; set; } = [];
}
