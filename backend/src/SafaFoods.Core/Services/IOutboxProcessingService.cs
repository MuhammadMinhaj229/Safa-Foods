namespace SafaFoods.Core.Services;

public interface IOutboxProcessingService
{
    Task<int> ProcessPendingAsync(int take, CancellationToken cancellationToken = default);
}
