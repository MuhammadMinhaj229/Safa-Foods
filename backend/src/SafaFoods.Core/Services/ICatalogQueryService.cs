using SafaFoods.Core.Services.Models;

namespace SafaFoods.Core.Services;

public interface ICatalogQueryService
{
    Task<IReadOnlyList<CatalogCategoryItem>> GetCategoriesAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<CatalogProductItem>> GetProductsAsync(string? categorySlug = null, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<CatalogProductItem>> SearchProductsAsync(string query, CancellationToken cancellationToken = default);

    Task<CatalogProductItem?> GetProductBySlugAsync(string slug, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ProductReviewItem>> GetProductReviewsAsync(Guid productId, CancellationToken cancellationToken = default);

    Task<ServiceResult<ProductReviewItem>> SubmitReviewAsync(Guid productId, Guid customerId, Guid orderId, int rating, string? title, string? body, CancellationToken cancellationToken = default);
}
