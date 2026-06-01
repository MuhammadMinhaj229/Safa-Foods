namespace SafaFoods.Core.Services.Models;

public sealed record CatalogVariantItem(
    Guid VariantId,
    string Label,
    string Weight,
    decimal Price,
    decimal? SalePrice,
    string Sku,
    int StockQty,
    bool InStock);

public sealed record CatalogProductItem(
    Guid ProductId,
    string Slug,
    string Name,
    string? Description,
    string? ImageUrl,
    string? Category,
    Guid? CategoryId,
    bool IsVegetarian,
    bool IsSubscriptionEnabled,
    string? IngredientsList,
    string? AllergenInfo,
    string? NutritionalInfoJson,
    decimal AverageRating,
    int ReviewCount,
    IReadOnlyList<CatalogVariantItem> Variants);

public sealed record CatalogCategoryItem(
    Guid CategoryId,
    string Slug,
    string Name,
    string? Description,
    string? ImageUrl,
    int SortOrder,
    int ProductCount);

public sealed record ProductReviewItem(
    Guid ReviewId,
    Guid CustomerId,
    string CustomerName,
    int Rating,
    string? Title,
    string? Body,
    DateTimeOffset CreatedAt);

