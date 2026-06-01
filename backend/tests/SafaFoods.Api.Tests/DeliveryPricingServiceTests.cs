using SafaFoods.Core.Services;

namespace SafaFoods.Api.Tests;

public sealed class DeliveryPricingServiceTests
{
    private readonly DeliveryPricingService _service = new();

    [Theory]
    [InlineData(2.5, true, "Zone A", 10)]
    [InlineData(5.0, true, "Zone B", 20)]
    [InlineData(10.0, true, "Zone C", 40)]
    [InlineData(15.0, false, null, null)]
    public void GetQuote_ReturnsExpectedServiceability(
        decimal distanceKm,
        bool expectedServiceable,
        string? expectedZone,
        int? expectedFee)
    {
        var result = _service.GetQuote(distanceKm);

        Assert.Equal(expectedServiceable, result.Serviceable);
        Assert.Equal(expectedZone, result.ZoneLabel);
        Assert.Equal(expectedFee, result.Fee is null ? null : (int?)result.Fee.Value);
    }
}
