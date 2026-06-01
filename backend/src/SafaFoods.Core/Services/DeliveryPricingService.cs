using SafaFoods.Core.Domain;

namespace SafaFoods.Core.Services;

public sealed class DeliveryPricingService : IDeliveryPricingService
{
    private static readonly IReadOnlyList<DeliverySlab> Slabs =
    [
        new("Zone A", 0m, 3m, 10m, 30, 45),
        new("Zone B", 3m, 8m, 20m, 45, 70),
        new("Zone C", 8m, 12m, 40m, 70, 110)
    ];

    public DeliveryQuote GetQuote(decimal distanceKm)
    {
        var normalizedDistance = decimal.Round(distanceKm, 2, MidpointRounding.AwayFromZero);
        var slab = Slabs.FirstOrDefault(candidate =>
            normalizedDistance >= candidate.MinKm &&
            (candidate.MaxKm is null || normalizedDistance <= candidate.MaxKm.Value));

        if (slab is null)
        {
            return new DeliveryQuote(false, null, null, normalizedDistance, null, null);
        }

        return new DeliveryQuote(
            true,
            slab.Label,
            slab.Fee,
            normalizedDistance,
            slab.EstimatedMinMinutes,
            slab.EstimatedMaxMinutes);
    }
}
