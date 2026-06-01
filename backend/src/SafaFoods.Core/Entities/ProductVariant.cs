namespace SafaFoods.Core.Entities;

public sealed class ProductVariant
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ProductId { get; set; }
    public string? ShopifyVariantId { get; set; }
    public required string Label { get; set; }
    public required string Weight { get; set; }
    public decimal Price { get; set; }
    public decimal? SalePrice { get; set; }
    public required string Sku { get; set; }
    public int StockQty { get; set; } = 0;
    public uint Version { get; set; } // xmin for Postgres RowVersion
    public bool TrackInventory { get; set; } = false;
    public bool IsActive { get; set; } = true;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

    public Product Product { get; set; } = null!;
    public ICollection<OrderItem> OrderItems { get; set; } = [];
    public ICollection<Subscription> Subscriptions { get; set; } = [];
    public ICollection<CartItem> CartItems { get; set; } = [];
}
