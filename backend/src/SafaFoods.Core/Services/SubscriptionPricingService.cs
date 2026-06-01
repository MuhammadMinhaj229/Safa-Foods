using SafaFoods.Core.Domain;

namespace SafaFoods.Core.Services;

public sealed class SubscriptionPricingService : ISubscriptionPricingService
{
    private static readonly IReadOnlyDictionary<string, SubscriptionPlan> Plans =
        new Dictionary<string, SubscriptionPlan>(StringComparer.OrdinalIgnoreCase)
        {
            ["monthly_750g_monday"] = new("monthly_750g_monday", "Monthly 750g Monday Plan", "monthly", 4, 0m)
        };

    public SubscriptionQuote GetQuote(decimal unitPrice, int quantityPerDelivery, string planCode)
    {
        if (!Plans.TryGetValue(planCode, out var plan))
        {
            return new SubscriptionQuote(false, "Unsupported subscription plan.", null, null, null, 0, 0m, 0m, 0m, 0m);
        }

        if (unitPrice <= 0)
        {
            return new SubscriptionQuote(false, "Unit price must be greater than zero.", null, null, null, 0, 0m, 0m, 0m, 0m);
        }

        if (quantityPerDelivery <= 0)
        {
            return new SubscriptionQuote(false, "Quantity per delivery must be greater than zero.", null, null, null, 0, 0m, 0m, 0m, 0m);
        }

        if (string.Equals(planCode, "monthly_750g_monday", StringComparison.OrdinalIgnoreCase) &&
            quantityPerDelivery != 750)
        {
            return new SubscriptionQuote(false, "The monthly subscription plan requires 750g per delivery.", null, null, null, 0, 0m, 0m, 0m, 0m);
        }

        // Subscriptions are priced from the monthly per-kg rate. For MVP this is:
        // 750g delivered once every Monday for 4 weeks at Rs. 200 per kg.
        var deliveryWeightKg = decimal.Round(quantityPerDelivery / 1000m, 3, MidpointRounding.AwayFromZero);
        var cycleSubtotal = decimal.Round(
            unitPrice * deliveryWeightKg * plan.DeliveriesInCycle,
            2,
            MidpointRounding.AwayFromZero);
        var discountAmount = decimal.Round(
            cycleSubtotal * (plan.DiscountPercent / 100m),
            2,
            MidpointRounding.AwayFromZero);
        var total = decimal.Round(cycleSubtotal - discountAmount, 2, MidpointRounding.AwayFromZero);

        return new SubscriptionQuote(
            true,
            null,
            plan.Code,
            plan.Name,
            plan.BillingCycle,
            plan.DeliveriesInCycle,
            plan.DiscountPercent,
            cycleSubtotal,
            discountAmount,
            total);
    }
}
