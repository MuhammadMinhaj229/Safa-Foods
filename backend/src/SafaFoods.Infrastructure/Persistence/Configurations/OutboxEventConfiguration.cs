using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SafaFoods.Core.Entities;

namespace SafaFoods.Infrastructure.Persistence.Configurations;

public sealed class OutboxEventConfiguration : IEntityTypeConfiguration<OutboxEvent>
{
    public void Configure(EntityTypeBuilder<OutboxEvent> builder)
    {
        builder.ToTable("outbox_events");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.EventType).HasMaxLength(120);
        builder.Property(x => x.AggregateType).HasMaxLength(80);
        builder.Property(x => x.Status).HasMaxLength(40);
        builder.Property(x => x.LastError).HasMaxLength(1000);

        builder.HasIndex(x => new { x.Status, x.OccurredAt });
        builder.HasIndex(x => new { x.AggregateType, x.AggregateId, x.OccurredAt });
    }
}
