using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SafaFoods.Core.Entities;

namespace SafaFoods.Infrastructure.Persistence.Configurations;

public sealed class NotificationLogConfiguration : IEntityTypeConfiguration<NotificationLog>
{
    public void Configure(EntityTypeBuilder<NotificationLog> builder)
    {
        builder.ToTable("notification_logs");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.EventType).HasMaxLength(80);
        builder.Property(x => x.Recipient).HasMaxLength(160);
        builder.Property(x => x.MessageStatus).HasMaxLength(50);
        builder.Property(x => x.ProviderReference).HasMaxLength(120);
    }
}
