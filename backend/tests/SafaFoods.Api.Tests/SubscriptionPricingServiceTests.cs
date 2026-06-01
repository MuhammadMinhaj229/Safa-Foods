using SafaFoods.Core.Services;

namespace SafaFoods.Api.Tests;

public sealed class SubscriptionPricingServiceTests
{
    private readonly SubscriptionPricingService _service = new();

    [Fact]
    public void GetQuote_ComputesMonthly750GramPlanCorrectly()
    {
        var result = _service.GetQuote(200m, 750, "monthly_750g_monday");

        Assert.True(result.Success);
        Assert.Equal("monthly_750g_monday", result.PlanCode);
        Assert.Equal(4, result.DeliveriesInCycle);
        Assert.Equal(600m, result.CycleSubtotal);
        Assert.Equal(0m, result.DiscountAmount);
        Assert.Equal(600m, result.Total);
    }

    [Fact]
    public void GetQuote_ReturnsFailureForUnknownPlan()
    {
        var result = _service.GetQuote(99m, 1, "daily");

        Assert.False(result.Success);
        Assert.Equal("Unsupported subscription plan.", result.ErrorMessage);
    }

    [Fact]
    public void GetQuote_ReturnsFailureWhenMonthlyPlanDoesNotUse750Grams()
    {
        var result = _service.GetQuote(200m, 500, "monthly_750g_monday");

        Assert.False(result.Success);
        Assert.Equal("The monthly subscription plan requires 750g per delivery.", result.ErrorMessage);
    }
}
