using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Claims;

using SafaFoods.Api.Contracts.Catalog;
using SafaFoods.Core.Services;

namespace SafaFoods.Api.Endpoints;

public static class CatalogEndpoints
{
    public static RouteGroupBuilder MapCatalogEndpoints(this RouteGroupBuilder api)
    {
        var group = api.MapGroup("/catalog")
            .WithTags("Catalog");

        group.MapGet("/categories", GetCategoriesAsync)
            .WithName("GetCatalogCategories")
            .WithSummary("Returns all active product categories.");

        group.MapGet("/products", GetProductsAsync)
            .WithName("GetCatalogProducts")
            .WithSummary("Returns active products, optionally filtered by category slug.");

        group.MapGet("/search", SearchProductsAsync)
            .WithName("SearchCatalogProducts")
            .WithSummary("Searches products by name, description, or ingredients.");

        group.MapGet("/products/{slug}", GetProductBySlugAsync)
            .WithName("GetCatalogProductBySlug")
            .WithSummary("Returns a single active product by slug.");

        group.MapGet("/products/{productId:guid}/reviews", GetProductReviewsAsync)
            .WithName("GetProductReviews")
            .WithSummary("Returns approved customer reviews for a product.");

        group.MapPost("/products/{productId:guid}/reviews", SubmitReviewAsync)
            .WithName("SubmitProductReview")
            .WithSummary("Submits a verified-purchase review. Requires customer auth.")
            .RequireAuthorization("CustomerPolicy");

        return group;
    }

    private static async Task<Ok<IReadOnlyList<CatalogCategoryResponse>>> GetCategoriesAsync(
        ICatalogQueryService catalogQueryService,
        CancellationToken cancellationToken)
    {
        var categories = await catalogQueryService.GetCategoriesAsync(cancellationToken);
        return TypedResults.Ok(categories.Select(c => new CatalogCategoryResponse(
            c.CategoryId, c.Slug, c.Name, c.Description, c.ImageUrl, c.SortOrder, c.ProductCount))
            .ToList() as IReadOnlyList<CatalogCategoryResponse>);
    }

    private static async Task<Ok<IReadOnlyList<CatalogProductResponse>>> GetProductsAsync(
        ICatalogQueryService catalogQueryService,
        string? category,
        CancellationToken cancellationToken)
    {
        var products = await catalogQueryService.GetProductsAsync(category, cancellationToken);
        return TypedResults.Ok(products.Select(MapProductResponse).ToList() as IReadOnlyList<CatalogProductResponse>);
    }

    private static async Task<Ok<IReadOnlyList<CatalogProductResponse>>> SearchProductsAsync(
        ICatalogQueryService catalogQueryService,
        string q,
        CancellationToken cancellationToken)
    {
        var products = await catalogQueryService.SearchProductsAsync(q, cancellationToken);
        return TypedResults.Ok(products.Select(MapProductResponse).ToList() as IReadOnlyList<CatalogProductResponse>);
    }

    private static async Task<Results<Ok<CatalogProductDetailResponse>, NotFound<string>>> GetProductBySlugAsync(
        string slug,
        ICatalogQueryService catalogQueryService,
        CancellationToken cancellationToken)
    {
        var product = await catalogQueryService.GetProductBySlugAsync(slug, cancellationToken);
        if (product is null) return TypedResults.NotFound("Product not found.");

        return TypedResults.Ok(new CatalogProductDetailResponse(
            product.ProductId, product.Slug, product.Name, product.Description, product.ImageUrl,
            product.Category, product.IsVegetarian, product.IsSubscriptionEnabled,
            product.IngredientsList, product.AllergenInfo, product.NutritionalInfoJson,
            product.AverageRating, product.ReviewCount,
            product.Variants.Select(v => new CatalogProductVariantResponse(
                v.VariantId, v.Label, v.Weight, v.Price, v.SalePrice, v.Sku, v.StockQty, v.InStock)).ToList()));
    }

    private static async Task<Ok<IReadOnlyList<ProductReviewResponse>>> GetProductReviewsAsync(
        Guid productId,
        ICatalogQueryService catalogQueryService,
        CancellationToken cancellationToken)
    {
        var reviews = await catalogQueryService.GetProductReviewsAsync(productId, cancellationToken);
        return TypedResults.Ok(reviews.Select(r => new ProductReviewResponse(
            r.ReviewId, r.CustomerName, r.Rating, r.Title, r.Body, r.CreatedAt))
            .ToList() as IReadOnlyList<ProductReviewResponse>);
    }

    private static async Task<Results<Ok<ProductReviewResponse>, ValidationProblem>> SubmitReviewAsync(
        Guid productId,
        ClaimsPrincipal user,
        SubmitReviewRequest request,
        ICatalogQueryService catalogQueryService,
        CancellationToken cancellationToken)
    {
        var customerId = Guid.Parse(user.FindFirstValue("customer_id")!);
        var result = await catalogQueryService.SubmitReviewAsync(
            productId, customerId, request.OrderId, request.Rating, request.Title, request.Body, cancellationToken);

        if (!result.Success)
            return TypedResults.ValidationProblem(new Dictionary<string, string[]>
                { ["review"] = [result.ErrorMessage ?? "Unable to submit review."] });

        var r = result.Data!;
        return TypedResults.Ok(new ProductReviewResponse(r.ReviewId, r.CustomerName, r.Rating, r.Title, r.Body, r.CreatedAt));
    }

    private static CatalogProductResponse MapProductResponse(Core.Services.Models.CatalogProductItem p) =>
        new(p.ProductId, p.Slug, p.Name, p.Description, p.ImageUrl, p.Category,
            p.IsVegetarian, p.IsSubscriptionEnabled, p.AverageRating, p.ReviewCount,
            p.Variants.Select(v => new CatalogProductVariantResponse(
                v.VariantId, v.Label, v.Weight, v.Price, v.SalePrice, v.Sku, v.StockQty, v.InStock)).ToList());
}
