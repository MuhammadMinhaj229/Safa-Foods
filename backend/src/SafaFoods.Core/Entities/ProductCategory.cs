namespace SafaFoods.Core.Entities;

/// <summary>
/// A product category used for navigation, filtering and merchandising (e.g. Pastes, Pickles, Masalas).
/// </summary>
public sealed class ProductCategory
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Slug { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public int SortOrder { get; set; } = 0;
    public bool IsActive { get; set; } = true;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public ICollection<Product> Products { get; set; } = [];
}
