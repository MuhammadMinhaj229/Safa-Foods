namespace SafaFoods.Core.Entities;

/// <summary>
/// A customer's shopping cart. One active cart per customer.
/// </summary>
public sealed class Cart
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CustomerId { get; set; }
    public string? CouponCode { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

    public Customer Customer { get; set; } = null!;
    public ICollection<CartItem> Items { get; set; } = [];
}
