using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SafaFoods.Core.Entities;

namespace SafaFoods.Infrastructure.Persistence.Configurations;

public sealed class DeliveryZoneConfiguration : IEntityTypeConfiguration<DeliveryZone>
{
    public void Configure(EntityTypeBuilder<DeliveryZone> builder)
    {
        builder.ToTable("delivery_zones");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(80);
        builder.Property(x => x.MinKm).HasPrecision(5, 2);
        builder.Property(x => x.MaxKm).HasPrecision(5, 2);
        builder.Property(x => x.Fee).HasPrecision(8, 2);

        builder.HasData(
            new DeliveryZone
            {
                Id = SafaFoodsSeedIds.ZoneA,
                Name = "Zone A",
                MinKm = 0m,
                MaxKm = 3m,
                Fee = 10m,
                EstimatedMinMinutes = 30,
                EstimatedMaxMinutes = 45,
                IsActive = true,
                CreatedAt = SafaFoodsSeedIds.SeedTimestamp,
                UpdatedAt = SafaFoodsSeedIds.SeedTimestamp
            },
            new DeliveryZone
            {
                Id = SafaFoodsSeedIds.ZoneB,
                Name = "Zone B",
                MinKm = 3m,
                MaxKm = 8m,
                Fee = 20m,
                EstimatedMinMinutes = 45,
                EstimatedMaxMinutes = 70,
                IsActive = true,
                CreatedAt = SafaFoodsSeedIds.SeedTimestamp,
                UpdatedAt = SafaFoodsSeedIds.SeedTimestamp
            },
            new DeliveryZone
            {
                Id = SafaFoodsSeedIds.ZoneC,
                Name = "Zone C",
                MinKm = 8m,
                MaxKm = 12m,
                Fee = 40m,
                EstimatedMinMinutes = 70,
                EstimatedMaxMinutes = 110,
                IsActive = true,
                CreatedAt = SafaFoodsSeedIds.SeedTimestamp,
                UpdatedAt = SafaFoodsSeedIds.SeedTimestamp
            });
    }
}
