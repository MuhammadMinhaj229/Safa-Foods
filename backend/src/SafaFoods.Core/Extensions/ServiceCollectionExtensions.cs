using Microsoft.Extensions.DependencyInjection;

using SafaFoods.Core.Services;

namespace SafaFoods.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSafaFoodsCore(this IServiceCollection services)
    {
        services.AddSingleton<IDeliveryPricingService, DeliveryPricingService>();
        services.AddSingleton<ISubscriptionPricingService, SubscriptionPricingService>();

        return services;
    }
}
