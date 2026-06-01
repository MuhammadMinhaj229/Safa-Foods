using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SafaFoods.Core.Entities;

namespace SafaFoods.Infrastructure.Persistence.Configurations;

public sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("orders");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.ShopifyOrderId).HasMaxLength(64);
        builder.Property(x => x.PaymentReference).HasMaxLength(120);
        builder.Property(x => x.InvoiceNumber).HasMaxLength(40);
        builder.Property(x => x.CancellationReason).HasMaxLength(240);
        builder.Property(x => x.Subtotal).HasPrecision(10, 2);
        builder.Property(x => x.DiscountTotal).HasPrecision(10, 2);
        builder.Property(x => x.DeliveryFee).HasPrecision(10, 2);
        builder.Property(x => x.GrandTotal).HasPrecision(10, 2);
        builder.HasIndex(x => x.ShopifyOrderId).IsUnique();
        builder.HasIndex(x => x.InvoiceNumber).IsUnique();
        builder.HasIndex(x => x.CustomerId); // Critical for "My Orders" performance
        builder.HasIndex(x => x.Status);     // Critical for Admin Dashboard performance
    }
}
