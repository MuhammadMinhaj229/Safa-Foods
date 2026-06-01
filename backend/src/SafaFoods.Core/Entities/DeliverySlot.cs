namespace SafaFoods.Core.Entities;

public sealed class DeliverySlot
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Label { get; set; } // e.g. "Morning (7 AM - 9 AM)"
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public int MaxOrders { get; set; } // capacity
    public bool IsActive { get; set; } = true;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public ICollection<Order> Orders { get; set; } = [];
}
