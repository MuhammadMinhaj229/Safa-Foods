using Microsoft.EntityFrameworkCore;
using SafaFoods.Core.Entities;
using SafaFoods.Core.Services;
using SafaFoods.Infrastructure.Persistence;

namespace SafaFoods.Infrastructure.Services;

public sealed class DeliverySlotService(SafaFoodsDbContext dbContext) : IDeliverySlotService
{
    public async Task<IReadOnlyList<DeliverySlotItem>> GetAvailableSlotsAsync(DateTime date, CancellationToken ct = default)
    {
        // Get all active slots from the new DeliverySlots DbSet
        var allSlots = await dbContext.DeliverySlots
            .AsNoTracking()
            .Where(s => s.IsActive)
            .OrderBy(s => s.StartTime)
            .ToListAsync(ct);

        // Get order counts for the specific date per slot
        var dateUtc = DateTime.SpecifyKind(date.Date, DateTimeKind.Utc);
        var orderCounts = await dbContext.Orders
            .Where(o => o.ScheduledDeliveryDate == dateUtc && o.DeliverySlotId != null)
            .GroupBy(o => o.DeliverySlotId)
            .Select(g => new { SlotId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.SlotId!.Value, x => x.Count, ct);

        return allSlots.Select(s =>
        {
            var used = orderCounts.GetValueOrDefault(s.Id, 0);
            return new DeliverySlotItem(
                s.Id,
                s.Label,
                s.StartTime,
                s.EndTime,
                Math.Max(0, s.MaxOrders - used));
        }).ToList();
    }

    public async Task<bool> IsSlotAvailableAsync(Guid slotId, DateTime date, CancellationToken ct = default)
    {
        var slot = await dbContext.DeliverySlots
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == slotId && s.IsActive, ct);

        if (slot is null) return false;

        var dateUtc = DateTime.SpecifyKind(date.Date, DateTimeKind.Utc);
        var count = await dbContext.Orders
            .CountAsync(o => o.ScheduledDeliveryDate == dateUtc && o.DeliverySlotId == slotId, ct);

        return count < slot.MaxOrders;
    }
}
