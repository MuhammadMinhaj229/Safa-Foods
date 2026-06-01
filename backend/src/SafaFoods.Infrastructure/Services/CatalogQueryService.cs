using Microsoft.EntityFrameworkCore;

using SafaFoods.Core.Enums;
using SafaFoods.Core.Services;
using SafaFoods.Core.Services.Models;
using SafaFoods.Infrastructure.Persistence;

namespace SafaFoods.Infrastructure.Services;

public sealed class CatalogQueryService(SafaFoodsDbContext dbContext, CacheService cache) : ICatalogQueryService
{
    // Category list changes rarely — cache for 30 mins
    private static readonly TimeSpan CategoryTtl = TimeSpan.FromMinutes(30);
    // Product list may change when stock/price updates — cache for 10 mins
    private static readonly TimeSpan ProductListTtl = TimeSpan.FromMinutes(10);
    // Product detail pages — cache for 10 mins
    private static readonly TimeSpan ProductDetailTtl = TimeSpan.FromMinutes(10);
    // Reviews — cache for 5 mins
    private static readonly TimeSpan ReviewsTtl = TimeSpan.FromMinutes(5);

    public async Task<IReadOnlyList<CatalogCategoryItem>> GetCategoriesAsync(CancellationToken cancellationToken = default)
    {
        var cached = await cache.GetAsync<List<CatalogCategoryItem>>(CacheKeys.AllCategories, cancellationToken);
        if (cached is not null) return cached;

        var result = await dbContext.ProductCategories
            .AsNoTracking()
            .Where(c => c.IsActive)
            .OrderBy(c => c.SortOrder)
            .Select(c => new CatalogCategoryItem(
                c.Id, c.Slug, c.Name, c.Description, c.ImageUrl,
                c.SortOrder, c.Products.Count(p => p.IsActive)))
            .ToListAsync(cancellationToken);

        await cache.SetAsync(CacheKeys.AllCategories, result, CategoryTtl, cancellationToken);
        return result;
    }

    public async Task<IReadOnlyList<CatalogProductItem>> GetProductsAsync(string? categorySlug = null, CancellationToken cancellationToken = default)
    {
        var cacheKey = CacheKeys.ProductsByCategory(categorySlug);
        var cached = await cache.GetAsync<List<CatalogProductItem>>(cacheKey, cancellationToken);
        if (cached is not null) return cached;

        var query = dbContext.Products
            .AsNoTracking()
            .Where(x => x.IsActive);

        if (!string.IsNullOrWhiteSpace(categorySlug))
            query = query.Where(x => x.ProductCategory != null && x.ProductCategory.Slug == categorySlug);

        var result = await query
            .Include(x => x.Variants.Where(v => v.IsActive))
            .Include(x => x.Reviews.Where(r => r.IsApproved))
            .OrderBy(x => x.Name)
            .Select(x => MapProduct(x))
            .ToListAsync(cancellationToken);

        await cache.SetAsync(cacheKey, result, ProductListTtl, cancellationToken);
        return result;
    }

    public async Task<IReadOnlyList<CatalogProductItem>> SearchProductsAsync(string query, CancellationToken cancellationToken = default)
    {
        // Fuzzy search using pg_trgm similarity scores.
        // ILike handles basic substring, but similarity handles typos.
        var q = query.ToLower().Trim();
        if (string.IsNullOrEmpty(q)) return [];

        return await dbContext.Products
            .AsNoTracking()
            .Where(x => x.IsActive && (
                EF.Functions.ILike(x.Name, $"%{q}%") ||
                (x.Description != null && EF.Functions.ILike(x.Description, $"%{q}%")) ||
                EF.Functions.TrigramsSimilarity(x.Name, q) > 0.3)) // Trigram filter for fuzzy match
            .Include(x => x.Variants.Where(v => v.IsActive))
            .Include(x => x.Reviews.Where(r => r.IsApproved))
            .OrderByDescending(x => EF.Functions.TrigramsSimilarity(x.Name, q))
            .Select(x => MapProduct(x))
            .ToListAsync(cancellationToken);
    }

    public async Task<CatalogProductItem?> GetProductBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        var cacheKey = CacheKeys.ProductBySlug(slug);
        var cached = await cache.GetAsync<CatalogProductItem>(cacheKey, cancellationToken);
        if (cached is not null) return cached;

        var product = await dbContext.Products
            .AsNoTracking()
            .Where(x => x.IsActive && x.Slug == slug)
            .Include(x => x.Variants.Where(v => v.IsActive))
            .Include(x => x.Reviews.Where(r => r.IsApproved))
                .ThenInclude(r => r.Customer)
            .FirstOrDefaultAsync(cancellationToken);

        if (product is null) return null;

        var mapped = MapProduct(product);
        await cache.SetAsync(cacheKey, mapped, ProductDetailTtl, cancellationToken);
        return mapped;
    }

    public async Task<IReadOnlyList<ProductReviewItem>> GetProductReviewsAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        var cacheKey = CacheKeys.ProductReviews(productId);
        var cached = await cache.GetAsync<List<ProductReviewItem>>(cacheKey, cancellationToken);
        if (cached is not null) return cached;

        var result = await dbContext.ProductReviews
            .AsNoTracking()
            .Where(r => r.ProductId == productId && r.IsApproved)
            .Include(r => r.Customer)
            .OrderByDescending(r => r.CreatedAt)
            .Select(r => new ProductReviewItem(
                r.Id, r.CustomerId, r.Customer.FullName, r.Rating, r.Title, r.Body, r.CreatedAt))
            .ToListAsync(cancellationToken);

        await cache.SetAsync(cacheKey, result, ReviewsTtl, cancellationToken);
        return result;
    }

    public async Task<ServiceResult<ProductReviewItem>> SubmitReviewAsync(
        Guid productId, Guid customerId, Guid orderId,
        int rating, string? title, string? body,
        CancellationToken cancellationToken = default)
    {
        if (rating < 1 || rating > 5)
            return ServiceResult<ProductReviewItem>.Fail("invalid_rating", "Rating must be between 1 and 5.");

        var order = await dbContext.Orders
            .AsNoTracking()
            .Where(o => o.Id == orderId && o.CustomerId == customerId)
            .Select(o => new
            {
                o.Status,
                ContainsProduct = o.Items.Any(i => i.ProductId == productId)
            })
            .FirstOrDefaultAsync(cancellationToken);
        if (order is null)
            return ServiceResult<ProductReviewItem>.Fail("order_not_found", "Order not found for this customer.");
        if (order.Status != OrderStatus.Delivered)
            return ServiceResult<ProductReviewItem>.Fail("order_not_delivered", "Reviews are available only after delivery is completed.");
        if (!order.ContainsProduct)
            return ServiceResult<ProductReviewItem>.Fail("product_not_in_order", "This product was not part of the selected order.");

        var alreadyReviewed = await dbContext.ProductReviews
            .AnyAsync(r => r.ProductId == productId && r.CustomerId == customerId && r.OrderId == orderId, cancellationToken);
        if (alreadyReviewed)
            return ServiceResult<ProductReviewItem>.Fail("duplicate_review", "You have already reviewed this product for this order.");

        var customer = await dbContext.Customers.FindAsync([customerId], cancellationToken);
        if (customer is null)
            return ServiceResult<ProductReviewItem>.Fail("customer_not_found", "Customer not found.");

        var review = new Core.Entities.ProductReview
        {
            ProductId = productId,
            CustomerId = customerId,
            OrderId = orderId,
            Rating = rating,
            Title = title,
            Body = body,
            IsApproved = false  // goes through moderation
        };

        dbContext.ProductReviews.Add(review);
        await dbContext.SaveChangesAsync(cancellationToken);

        // Invalidate review cache so next request gets fresh data
        await cache.RemoveAsync(CacheKeys.ProductReviews(productId), cancellationToken);

        return ServiceResult<ProductReviewItem>.Ok(new ProductReviewItem(
            review.Id, customerId, customer.FullName, rating, title, body, review.CreatedAt));
    }

    private static CatalogProductItem MapProduct(Core.Entities.Product x)
    {
        var approvedReviews = x.Reviews.Where(r => r.IsApproved).ToList();
        var avgRating = approvedReviews.Count > 0 ? (decimal)approvedReviews.Average(r => r.Rating) : 0m;

        return new CatalogProductItem(
            x.Id, x.Slug, x.Name, x.Description, x.ImageUrl,
            x.Category, x.CategoryId, x.IsVegetarian, x.IsSubscriptionEnabled,
            x.IngredientsList, x.AllergenInfo, x.NutritionalInfoJson,
            Math.Round(avgRating, 1), approvedReviews.Count,
            x.Variants
                .Where(v => v.IsActive)
                .OrderBy(v => v.Price)
                .Select(v => new CatalogVariantItem(
                    v.Id, v.Label, v.Weight, v.Price, v.SalePrice, v.Sku,
                    v.StockQty, !v.TrackInventory || v.StockQty > 0))
                .ToList());
    }
}
