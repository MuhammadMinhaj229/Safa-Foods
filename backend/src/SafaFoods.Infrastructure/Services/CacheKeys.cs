namespace SafaFoods.Infrastructure.Services;

/// <summary>
/// Strongly-typed cache keys to prevent typos and aid cache invalidation.
/// </summary>
public static class CacheKeys
{
    // Catalog
    public const string AllCategories = "catalog:categories:all";
    public static string ProductsByCategory(string? slug) => $"catalog:products:cat:{slug ?? "all"}";
    public static string ProductBySlug(string slug) => $"catalog:products:slug:{slug}";
    public static string ProductReviews(Guid productId) => $"catalog:reviews:{productId}";

    // Delivery
    public const string AllDeliveryZones = "delivery:zones:all";
}
