using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SafaFoods.Core.Entities;

namespace SafaFoods.Infrastructure.Persistence.Configurations;

public sealed class CustomerWalletConfiguration : IEntityTypeConfiguration<CustomerWallet>
{
    public void Configure(EntityTypeBuilder<CustomerWallet> builder)
    {
        builder.ToTable("customer_wallets");
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Balance).HasDefaultValue(0);
        
        // Optimistic Concurrency mapping for Postgres xmin
        builder.Property(x => x.Version).IsRowVersion();

        builder.HasOne(x => x.Customer)
            .WithOne()
            .HasForeignKey<CustomerWallet>(x => x.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.HasMany(x => x.Transactions)
            .WithOne()
            .HasForeignKey(x => x.WalletId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
