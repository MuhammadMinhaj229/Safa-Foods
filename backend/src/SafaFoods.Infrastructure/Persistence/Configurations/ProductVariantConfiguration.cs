using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SafaFoods.Core.Entities;

namespace SafaFoods.Infrastructure.Persistence.Configurations;

public sealed class ProductVariantConfiguration : IEntityTypeConfiguration<ProductVariant>
{
    public void Configure(EntityTypeBuilder<ProductVariant> builder)
    {
        builder.ToTable("product_variants");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Label).HasMaxLength(80);
        builder.Property(x => x.Weight).HasMaxLength(40);
        builder.Property(x => x.Sku).HasMaxLength(80);
        builder.Property(x => x.ShopifyVariantId).HasMaxLength(64);
        builder.Property(x => x.Price).HasPrecision(10, 2);
        builder.Property(x => x.SalePrice).HasPrecision(10, 2);
        builder.HasIndex(x => x.Sku).IsUnique();
        builder.HasIndex(x => x.ShopifyVariantId).IsUnique();

        // Optimistic Concurrency mapping for Postgres xmin
        builder.Property(x => x.Version).IsRowVersion();

        builder.HasData(
            new ProductVariant
            {
                Id = SafaFoodsSeedIds.VariantGingerGarlic200,
                ProductId = SafaFoodsSeedIds.ProductGingerGarlic,
                Label = "200 g",
                Weight = "200 g",
                Price = 99m,
                SalePrice = 94m,
                Sku = "SF-GG-200",
                IsActive = true,
                CreatedAt = SafaFoodsSeedIds.SeedTimestamp,
                UpdatedAt = SafaFoodsSeedIds.SeedTimestamp
            },
            new ProductVariant
            {
                Id = SafaFoodsSeedIds.VariantGingerGarlic500,
                ProductId = SafaFoodsSeedIds.ProductGingerGarlic,
                Label = "500 g",
                Weight = "500 g",
                Price = 219m,
                SalePrice = 209m,
                Sku = "SF-GG-500",
                IsActive = true,
                CreatedAt = SafaFoodsSeedIds.SeedTimestamp,
                UpdatedAt = SafaFoodsSeedIds.SeedTimestamp
            },
            new ProductVariant
            {
                Id = SafaFoodsSeedIds.VariantGarlic200,
                ProductId = SafaFoodsSeedIds.ProductGarlic,
                Label = "200 g",
                Weight = "200 g",
                Price = 89m,
                SalePrice = 84m,
                Sku = "SF-GA-200",
                IsActive = true,
                CreatedAt = SafaFoodsSeedIds.SeedTimestamp,
                UpdatedAt = SafaFoodsSeedIds.SeedTimestamp
            },
            new ProductVariant
            {
                Id = SafaFoodsSeedIds.VariantGarlic500,
                ProductId = SafaFoodsSeedIds.ProductGarlic,
                Label = "500 g",
                Weight = "500 g",
                Price = 199m,
                SalePrice = 189m,
                Sku = "SF-GA-500",
                IsActive = true,
                CreatedAt = SafaFoodsSeedIds.SeedTimestamp,
                UpdatedAt = SafaFoodsSeedIds.SeedTimestamp
            },
            new ProductVariant
            {
                Id = SafaFoodsSeedIds.VariantGreenChilli150,
                ProductId = SafaFoodsSeedIds.ProductGreenChilli,
                Label = "150 g",
                Weight = "150 g",
                Price = 79m,
                SalePrice = 75m,
                Sku = "SF-GC-150",
                IsActive = true,
                CreatedAt = SafaFoodsSeedIds.SeedTimestamp,
                UpdatedAt = SafaFoodsSeedIds.SeedTimestamp
            },
            new ProductVariant
            {
                Id = SafaFoodsSeedIds.VariantGreenChilli300,
                ProductId = SafaFoodsSeedIds.ProductGreenChilli,
                Label = "300 g",
                Weight = "300 g",
                Price = 149m,
                SalePrice = 142m,
                Sku = "SF-GC-300",
                IsActive = true,
                CreatedAt = SafaFoodsSeedIds.SeedTimestamp,
                UpdatedAt = SafaFoodsSeedIds.SeedTimestamp
            });
    }
}
