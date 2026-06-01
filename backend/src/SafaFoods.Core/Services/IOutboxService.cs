namespace SafaFoods.Core.Services;

public interface IOutboxService
{
    Task PublishAsync(
        string eventType,
        string aggregateType,
        Guid aggregateId,
        object payload,
        CancellationToken cancellationToken = default);
}
