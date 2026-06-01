using Microsoft.EntityFrameworkCore;

using SafaFoods.Core.Services;
using SafaFoods.Infrastructure.Persistence;

namespace SafaFoods.Infrastructure.Services;

public sealed class PromotionsService(SafaFoodsDbContext dbContext) : IPromotionsService
{
    public async Task<PromotionEvaluationResult> EvaluateCouponAsync(string couponCode, Guid customerId, decimal cartSubtotal, CancellationToken ct = default)
    {
        var coupon = await dbContext.CouponCodes
            .Include(c => c.Offer)
            .FirstOrDefaultAsync(c => c.Code.ToUpper() == couponCode.ToUpper(), ct);

        if (coupon is null || !coupon.IsActive || coupon.Offer is null || !coupon.Offer.IsActive)
            return new PromotionEvaluationResult(false, "Invalid or expired coupon code.", 0, null, null);

        var now = DateTimeOffset.UtcNow;
        if ((coupon.Offer.StartsAt.HasValue && coupon.Offer.StartsAt.Value > now) ||
            (coupon.Offer.EndsAt.HasValue && coupon.Offer.EndsAt.Value < now))
            return new PromotionEvaluationResult(false, "Coupon code is expired or not yet active.", 0, null, null);

        if (coupon.UsageLimit.HasValue && coupon.UsageCount >= coupon.UsageLimit.Value)
            return new PromotionEvaluationResult(false, "Coupon redemption limit has been reached.", 0, null, null);

        if (cartSubtotal < coupon.Offer.MinimumOrderValue)
            return new PromotionEvaluationResult(false, $"Minimum order value of {coupon.Offer.MinimumOrderValue:C} required.", 0, null, null);

        // Check customer usage limit
        if (coupon.UsageLimitPerCustomer.HasValue)
        {
            var customerUsage = await dbContext.Orders
                .CountAsync(o => o.CustomerId == customerId && o.AppliedCouponCode != null && o.AppliedCouponCode.ToUpper() == coupon.Code.ToUpper(), ct);

            if (customerUsage >= coupon.UsageLimitPerCustomer.Value)
                return new PromotionEvaluationResult(false, "You have already used this coupon code.", 0, null, null);
        }

        // Calculate discount
        decimal discount = 0;
        var type = coupon.Offer.OfferType.ToLowerInvariant();

        if (type == "flat_discount")
            discount = coupon.Offer.DiscountValue;
        else if (type == "percent_discount")
            discount = cartSubtotal * (coupon.Offer.DiscountValue / 100);
        else if (type == "free_delivery")
            return new PromotionEvaluationResult(true, null, 0, coupon.Offer.Name, "free_delivery");

        if (coupon.Offer.MaximumDiscount.HasValue && discount > coupon.Offer.MaximumDiscount.Value)
            discount = coupon.Offer.MaximumDiscount.Value;

        if (discount > cartSubtotal)
            discount = cartSubtotal; // Cannot be negative

        return new PromotionEvaluationResult(true, null, discount, coupon.Offer.Name, type);
    }

    public async Task TrackCouponUsageAsync(string couponCode, CancellationToken ct = default)
    {
        var coupon = await dbContext.CouponCodes.FirstOrDefaultAsync(c => c.Code.ToUpper() == couponCode.ToUpper(), ct);
        if (coupon is not null)
        {
            coupon.UsageCount++;
            await dbContext.SaveChangesAsync(ct);
        }
    }
}
