namespace SafaFoods.Core.Services;

public record PromotionEvaluationResult(
    bool Success,
    string? ErrorMessage,
    decimal ExpectedDiscount,
    string? OfferName,
    string? OfferType);

public interface IPromotionsService
{
    Task<PromotionEvaluationResult> EvaluateCouponAsync(string couponCode, Guid customerId, decimal cartSubtotal, CancellationToken ct = default);
    Task TrackCouponUsageAsync(string couponCode, CancellationToken ct = default);
}
