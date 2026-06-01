namespace SafaFoods.Core.Entities;

public sealed class Product
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? ShopifyProductId { get; set; }
    public required string Slug { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public string? Category { get; set; } // legacy text fallback
    public Guid? CategoryId { get; set; } // FK to ProductCategory
    public bool IsVegetarian { get; set; } = true;
    public bool IsSubscriptionEnabled { get; set; }
    public bool IsActive { get; set; } = true;
    public string? NutritionalInfoJson { get; set; }  // JSON blob: {"calories":"50kcal", ...}
    public string? IngredientsList { get; set; }
    public string? AllergenInfo { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

    public ProductCategory? ProductCategory { get; set; }
    public ICollection<ProductVariant> Variants { get; set; } = [];
    public ICollection<OrderItem> OrderItems { get; set; } = [];
    public ICollection<Subscription> Subscriptions { get; set; } = [];
    public ICollection<ProductReview> Reviews { get; set; } = [];
    public ICollection<CartItem> CartItems { get; set; } = [];
}
