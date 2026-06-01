using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SafaFoods.Core.Entities;

namespace SafaFoods.Infrastructure.Persistence.Configurations;

public sealed class SubscriptionDeliveryConfiguration : IEntityTypeConfiguration<SubscriptionDelivery>
{
    public void Configure(EntityTypeBuilder<SubscriptionDelivery> builder)
    {
        builder.ToTable("subscription_deliveries");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.DeliveryFee).HasPrecision(10, 2);
    }
}
