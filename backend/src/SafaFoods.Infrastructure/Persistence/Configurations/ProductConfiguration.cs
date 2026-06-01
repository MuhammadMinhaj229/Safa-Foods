using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SafaFoods.Core.Entities;

namespace SafaFoods.Infrastructure.Persistence.Configurations;

public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("products");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(160);
        builder.Property(x => x.Slug).HasMaxLength(180);
        builder.Property(x => x.Category).HasMaxLength(120);
        builder.Property(x => x.ShopifyProductId).HasMaxLength(64);
        builder.HasIndex(x => x.Slug).IsUnique();
        builder.HasIndex(x => x.ShopifyProductId).IsUnique();

        builder.HasData(
            new Product
            {
                Id = SafaFoodsSeedIds.ProductGingerGarlic,
                Slug = "premium-ginger-garlic-paste",
                Name = "Premium Ginger Garlic Paste",
                Category = "Fresh Pastes",
                IsSubscriptionEnabled = true,
                IsActive = true,
                CreatedAt = SafaFoodsSeedIds.SeedTimestamp,
                UpdatedAt = SafaFoodsSeedIds.SeedTimestamp
            },
            new Product
            {
                Id = SafaFoodsSeedIds.ProductGarlic,
                Slug = "fresh-garlic-paste",
                Name = "Fresh Garlic Paste",
                Category = "Fresh Pastes",
                IsSubscriptionEnabled = true,
                IsActive = true,
                CreatedAt = SafaFoodsSeedIds.SeedTimestamp,
                UpdatedAt = SafaFoodsSeedIds.SeedTimestamp
            },
            new Product
            {
                Id = SafaFoodsSeedIds.ProductGreenChilli,
                Slug = "green-chilli-paste",
                Name = "Green Chilli Paste",
                Category = "Fresh Pastes",
                IsSubscriptionEnabled = true,
                IsActive = true,
                CreatedAt = SafaFoodsSeedIds.SeedTimestamp,
                UpdatedAt = SafaFoodsSeedIds.SeedTimestamp
            });
    }
}
