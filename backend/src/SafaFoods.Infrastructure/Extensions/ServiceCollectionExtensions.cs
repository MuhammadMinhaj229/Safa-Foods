using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using SafaFoods.Core.Services;
using SafaFoods.Core.Options;
using SafaFoods.Infrastructure.Persistence;
using SafaFoods.Infrastructure.Services;

namespace SafaFoods.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSafaFoodsInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Postgres");

        services.AddDbContext<SafaFoodsDbContext>(options =>
            options.UseNpgsql(connectionString, npgsqlOptions =>
                npgsqlOptions.EnableRetryOnFailure()));

        services.AddOptions<WhatsAppOptions>()
            .Bind(configuration.GetSection(WhatsAppOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<UpiOptions>()
            .Bind(configuration.GetSection(UpiOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<JwtOptions>()
            .Bind(configuration.GetSection(JwtOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddScoped<PasswordHasher<SafaFoods.Core.Entities.AdminUser>>();
        services.AddScoped<ICatalogQueryService, CatalogQueryService>();
        services.AddScoped<IAddressManagementService, AddressManagementService>();
        services.AddScoped<IOrderManagementService, OrderManagementService>();
        services.AddScoped<ISubscriptionManagementService, SubscriptionManagementService>();
        services.AddScoped<IPaymentManagementService, PaymentManagementService>();
        services.AddScoped<IAdminQueryService, AdminQueryService>();
        services.AddScoped<IAdminAuditService, AdminAuditService>();
        services.AddScoped<IMetaWebhookService, MetaWebhookService>();
        services.AddScoped<INotificationTemplateRegistry, NotificationTemplateRegistry>();
        services.AddScoped<INotificationPayloadBuilder, NotificationPayloadBuilder>();
        services.AddScoped<INotificationMessageRenderer, NotificationMessageRenderer>();
        services.AddScoped<SimulatedWhatsAppNotificationProvider>();
        services.AddHttpClient<MetaCloudWhatsAppNotificationProvider>();
        services.AddScoped<INotificationDispatchService, NotificationDispatchService>();
        services.AddScoped<IAdminAuthService, AdminAuthService>();
        services.AddScoped<IInvoiceService, InvoiceService>();
        services.AddScoped<ICustomerAuthService, CustomerAuthService>();
        services.AddScoped<ICartService, CartService>();
        services.AddScoped<IWalletService, WalletService>();
        services.AddScoped<IPromotionsService, PromotionsService>();
        services.AddScoped<IDeliverySlotService, DeliverySlotService>();
        services.AddScoped<IIdempotencyService, IdempotencyService>();
        services.AddScoped<IOutboxService, OutboxService>();
        services.AddScoped<IOutboxProcessingService, OutboxProcessingService>();
        services.AddScoped<ISubscriptionReminderService, SubscriptionReminderService>();

        // Phase 4: Background Workers
        services.AddHostedService<BackgroundNotificationService>();

        // Phase 3: Caching
        var redisConnectionString = configuration.GetConnectionString("Redis");
        if (!string.IsNullOrEmpty(redisConnectionString))
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConnectionString;
                options.InstanceName = "SafaFoods_";
            });
        }
        else
        {
            services.AddDistributedMemoryCache();
        }

        services.AddSingleton<CacheService>();

        return services;
    }
}
