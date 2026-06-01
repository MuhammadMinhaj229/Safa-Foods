using Microsoft.EntityFrameworkCore;

using SafaFoods.Core.Domain;
using SafaFoods.Core.Entities;
using SafaFoods.Core.Enums;
using SafaFoods.Core.Services;
using SafaFoods.Infrastructure.Persistence;

namespace SafaFoods.Infrastructure.Services;

public sealed class SubscriptionReminderService(
    SafaFoodsDbContext dbContext,
    IOutboxService outboxService) : ISubscriptionReminderService
{
    public async Task<int> ProcessUpcomingDeliveryRemindersAsync(int take, CancellationToken cancellationToken = default)
    {
        var batchSize = Math.Clamp(take <= 0 ? 20 : take, 1, 100);
        var tomorrow = DateTimeOffset.UtcNow.Date.AddDays(1);
        var tomorrowEnd = tomorrow.AddDays(1);
        var today = DateTimeOffset.UtcNow.Date;

        var subscriptions = await dbContext.Subscriptions
            .Include(x => x.Customer)
            .Include(x => x.Product)
            .Include(x => x.Variant)
            .Where(x => x.Status == SubscriptionStatus.Active)
            .Where(x => x.NextDeliveryDate >= tomorrow && x.NextDeliveryDate < tomorrowEnd)
            .OrderBy(x => x.NextDeliveryDate)
            .Take(batchSize)
            .ToListAsync(cancellationToken);

        if (subscriptions.Count == 0)
        {
            return 0;
        }

        var subscriptionIds = subscriptions.Select(x => x.Id).ToArray();
        var alreadyReminded = await dbContext.OutboxEvents
            .AsNoTracking()
            .Where(x => x.EventType == DomainEvents.SubscriptionUpcomingDelivery)
            .Where(x => subscriptionIds.Contains(x.AggregateId))
            .Where(x => x.OccurredAt >= today)
            .Select(x => x.AggregateId)
            .Distinct()
            .ToListAsync(cancellationToken);

        var remindedIds = alreadyReminded.ToHashSet();
        var published = 0;

        foreach (var subscription in subscriptions.Where(x => !remindedIds.Contains(x.Id)))
        {
            await outboxService.PublishAsync(
                DomainEvents.SubscriptionUpcomingDelivery,
                "subscription",
                subscription.Id,
                new
                {
                    customerId = subscription.CustomerId,
                    customerName = subscription.Customer.FullName,
                    recipient = subscription.Customer.Phone,
                    subscriptionId = subscription.Id,
                    subscriptionCode = BuildSubscriptionCode(subscription.Id),
                    productName = subscription.Product.Name,
                    variantLabel = subscription.Variant.Label,
                    quantityPerDelivery = subscription.QuantityPerDelivery.ToString(),
                    nextDeliveryDate = subscription.NextDeliveryDate.ToString("dd MMM yyyy"),
                    reviewEligible = "false"
                },
                cancellationToken);

            published += 1;
        }

        if (published > 0)
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        return published;
    }

    private static string BuildSubscriptionCode(Guid subscriptionId) =>
        $"SUB-{subscriptionId.ToString("N")[..8].ToUpperInvariant()}";
}
