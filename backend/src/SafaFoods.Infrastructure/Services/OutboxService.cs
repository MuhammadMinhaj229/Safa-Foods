using System.Text.Json;

using SafaFoods.Core.Entities;
using SafaFoods.Core.Services;
using SafaFoods.Infrastructure.Persistence;

namespace SafaFoods.Infrastructure.Services;

public sealed class OutboxService(SafaFoodsDbContext dbContext) : IOutboxService
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public Task PublishAsync(
        string eventType,
        string aggregateType,
        Guid aggregateId,
        object payload,
        CancellationToken cancellationToken = default)
    {
        dbContext.Set<OutboxEvent>().Add(new OutboxEvent
        {
            EventType = eventType,
            AggregateType = aggregateType,
            AggregateId = aggregateId,
            PayloadJson = JsonSerializer.Serialize(payload, JsonOptions),
            Status = "pending"
        });

        return Task.CompletedTask;
    }
}
