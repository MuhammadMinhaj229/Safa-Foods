namespace SafaFoods.Core.Services;

public sealed record DeliverySlotItem(
    Guid Id,
    string Label,
    TimeSpan StartTime,
    TimeSpan EndTime,
    int AvailableCapacity);

public interface IDeliverySlotService
{
    /// <summary>
    /// Fetches all active delivery slots and their remaining capacity for a specific date.
    /// </summary>
    Task<IReadOnlyList<DeliverySlotItem>> GetAvailableSlotsAsync(DateTime date, CancellationToken ct = default);

    /// <summary>
    /// Validates if a specific slot can still accept orders for the given date.
    /// </summary>
    Task<bool> IsSlotAvailableAsync(Guid slotId, DateTime date, CancellationToken ct = default);
}
