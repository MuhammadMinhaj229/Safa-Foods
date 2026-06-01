namespace SafaFoods.Core.Entities;

/// <summary>
/// A verified-purchase review left by a customer for a product.
/// </summary>
public sealed class ProductReview
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ProductId { get; set; }
    public Guid CustomerId { get; set; }
    public Guid OrderId { get; set; }
    public int Rating { get; set; } // 1–5
    public string? Title { get; set; }
    public string? Body { get; set; }
    public bool IsApproved { get; set; } = false;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public Product Product { get; set; } = null!;
    public Customer Customer { get; set; } = null!;
    public Order Order { get; set; } = null!;
}
