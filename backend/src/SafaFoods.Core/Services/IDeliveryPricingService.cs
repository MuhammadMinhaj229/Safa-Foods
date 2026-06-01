using SafaFoods.Core.Domain;

namespace SafaFoods.Core.Services;

public interface IDeliveryPricingService
{
    DeliveryQuote GetQuote(decimal distanceKm);
}
