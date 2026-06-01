using SafaFoods.Core.Domain;

namespace SafaFoods.Core.Services;

public interface ISubscriptionPricingService
{
    SubscriptionQuote GetQuote(decimal unitPrice, int quantityPerDelivery, string planCode);
}
