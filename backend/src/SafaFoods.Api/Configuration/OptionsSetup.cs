using SafaFoods.Core.Options;

namespace SafaFoods.Api.Configuration;

public static class OptionsSetup
{
    public static IServiceCollection AddConfiguredOptions(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddOptions<ShopOptions>()
            .Bind(configuration.GetSection(ShopOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services
            .AddOptions<AdminAccessOptions>()
            .Bind(configuration.GetSection(AdminAccessOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddScoped<AdminAccessFilter>();

        return services;
    }
}
