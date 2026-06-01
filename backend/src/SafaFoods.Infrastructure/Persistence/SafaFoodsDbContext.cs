using Microsoft.EntityFrameworkCore;

using SafaFoods.Core.Entities;

namespace SafaFoods.Infrastructure.Persistence;

public sealed class SafaFoodsDbContext(DbContextOptions<SafaFoodsDbContext> options) : DbContext(options)
{
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<CustomerAuthToken> CustomerAuthTokens => Set<CustomerAuthToken>();
    public DbSet<CustomerSession> CustomerSessions => Set<CustomerSession>();
    public DbSet<Address> Addresses => Set<Address>();
    public DbSet<DeliveryZone> DeliveryZones => Set<DeliveryZone>();
    public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductVariant> ProductVariants => Set<ProductVariant>();
    public DbSet<ProductReview> ProductReviews => Set<ProductReview>();
    public DbSet<Cart> Carts => Set<Cart>();
    public DbSet<CartItem> CartItems => Set<CartItem>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Subscription> Subscriptions => Set<Subscription>();
    public DbSet<SubscriptionDelivery> SubscriptionDeliveries => Set<SubscriptionDelivery>();
    public DbSet<NotificationLog> NotificationLogs => Set<NotificationLog>();
    public DbSet<OutboxEvent> OutboxEvents => Set<OutboxEvent>();
    public DbSet<AdminUser> AdminUsers => Set<AdminUser>();
    public DbSet<AdminSession> AdminSessions => Set<AdminSession>();
    public DbSet<AdminAuditLog> AdminAuditLogs => Set<AdminAuditLog>();
    public DbSet<IntegrationWebhookEvent> IntegrationWebhookEvents => Set<IntegrationWebhookEvent>();
    public DbSet<Offer> Offers => Set<Offer>();
    public DbSet<CouponCode> CouponCodes => Set<CouponCode>();
    public DbSet<CustomerWallet> CustomerWallets => Set<CustomerWallet>();
    public DbSet<WalletTransaction> WalletTransactions => Set<WalletTransaction>();
    public DbSet<DeliverySlot> DeliverySlots => Set<DeliverySlot>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("pg_trgm");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SafaFoodsDbContext).Assembly);

        modelBuilder.Entity<DeliverySlot>().HasData(
            new DeliverySlot
            {
                Id = SafaFoodsSeedIds.SlotMorning,
                Label = "Morning (7 AM - 10 AM)",
                StartTime = new TimeSpan(7, 0, 0),
                EndTime = new TimeSpan(10, 0, 0),
                MaxOrders = 50
            },
            new DeliverySlot
            {
                Id = SafaFoodsSeedIds.SlotAfternoon,
                Label = "Afternoon (1 PM - 4 PM)",
                StartTime = new TimeSpan(13, 0, 0),
                EndTime = new TimeSpan(16, 0, 0),
                MaxOrders = 50
            },
            new DeliverySlot
            {
                Id = SafaFoodsSeedIds.SlotEvening,
                Label = "Evening (6 PM - 9 PM)",
                StartTime = new TimeSpan(18, 0, 0),
                EndTime = new TimeSpan(21, 0, 0),
                MaxOrders = 30
            }
        );
    }
}
