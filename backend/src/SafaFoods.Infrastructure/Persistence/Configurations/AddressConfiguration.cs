using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SafaFoods.Core.Entities;

namespace SafaFoods.Infrastructure.Persistence.Configurations;

public sealed class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.ToTable("addresses");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.FullName).HasMaxLength(160);
        builder.Property(x => x.Phone).HasMaxLength(20);
        builder.Property(x => x.AddressLine1).HasMaxLength(240);
        builder.Property(x => x.AddressLine2).HasMaxLength(240);
        builder.Property(x => x.Landmark).HasMaxLength(160);
        builder.Property(x => x.Area).HasMaxLength(120);
        builder.Property(x => x.City).HasMaxLength(120);
        builder.Property(x => x.Pincode).HasMaxLength(12);
        builder.Property(x => x.Latitude).HasPrecision(9, 6);
        builder.Property(x => x.Longitude).HasPrecision(9, 6);
        builder.Property(x => x.DistanceKm).HasPrecision(5, 2);
    }
}
