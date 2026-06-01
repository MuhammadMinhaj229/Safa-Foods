namespace SafaFoods.Api.Contracts.Catalog;

// ── Variants ──────────────────────────────────────────────────────────
public sealed record CatalogProductVariantResponse(
    Guid VariantId,
    string Label,
    string Weight,
    decimal Price,
    decimal? SalePrice,
    string Sku,
    int StockQty,
    bool InStock);

// ── Product list item ─────────────────────────────────────────────────
public sealed record CatalogProductResponse(
    Guid ProductId,
    string Slug,
    string Name,
    string? Description,
    string? ImageUrl,
    string? Category,
    bool IsVegetarian,
    bool IsSubscriptionEnabled,
    decimal AverageRating,
    int ReviewCount,
    IReadOnlyList<CatalogProductVariantResponse> Variants);

// ── Product detail ────────────────────────────────────────────────────
public sealed record CatalogProductDetailResponse(
    Guid ProductId,
    string Slug,
    string Name,
    string? Description,
    string? ImageUrl,
    string? Category,
    bool IsVegetarian,
    bool IsSubscriptionEnabled,
    string? IngredientsList,
    string? AllergenInfo,
    string? NutritionalInfoJson,
    decimal AverageRating,
    int ReviewCount,
    IReadOnlyList<CatalogProductVariantResponse> Variants);

// ── Category ─────────────────────────────────────────────────────────
public sealed record CatalogCategoryResponse(
    Guid CategoryId,
    string Slug,
    string Name,
    string? Description,
    string? ImageUrl,
    int SortOrder,
    int ProductCount);

// ── Reviews ──────────────────────────────────────────────────────────
public sealed record ProductReviewResponse(
    Guid ReviewId,
    string CustomerName,
    int Rating,
    string? Title,
    string? Body,
    DateTimeOffset CreatedAt);

public sealed record SubmitReviewRequest(
    Guid OrderId,
    int Rating,
    string? Title,
    string? Body);
