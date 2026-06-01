using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SafaFoods.Core.Entities;

namespace SafaFoods.Infrastructure.Persistence.Configurations;

public sealed class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder.ToTable("subscriptions");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.PlanCode).HasMaxLength(50);
        builder.Property(x => x.BillingCycle).HasMaxLength(40);
        builder.Property(x => x.PaymentReference).HasMaxLength(120);
        builder.Property(x => x.PlanPrice).HasPrecision(10, 2);
        builder.Property(x => x.DiscountPercent).HasPrecision(5, 2);
    }
}
