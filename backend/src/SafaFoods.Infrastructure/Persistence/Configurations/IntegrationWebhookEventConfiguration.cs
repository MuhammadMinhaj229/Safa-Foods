using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SafaFoods.Core.Entities;

namespace SafaFoods.Infrastructure.Persistence.Configurations;

public sealed class IntegrationWebhookEventConfiguration : IEntityTypeConfiguration<IntegrationWebhookEvent>
{
    public void Configure(EntityTypeBuilder<IntegrationWebhookEvent> builder)
    {
        builder.ToTable("integration_webhook_events");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Provider).HasMaxLength(50);
        builder.Property(x => x.EventType).HasMaxLength(120);
        builder.Property(x => x.ExternalEventId).HasMaxLength(120);
        builder.Property(x => x.PayloadJson).HasColumnType("text");
        builder.Property(x => x.ProcessingStatus).HasMaxLength(40);
        builder.Property(x => x.ReceivedAt).HasColumnType("timestamp with time zone");
        builder.Property(x => x.ProcessedAt).HasColumnType("timestamp with time zone");
        builder.HasIndex(x => new { x.Provider, x.ReceivedAt });
        builder.HasIndex(x => x.ExternalEventId);
    }
}
