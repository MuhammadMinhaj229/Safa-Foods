namespace SafaFoods.Core.Services;

public interface ISubscriptionReminderService
{
    Task<int> ProcessUpcomingDeliveryRemindersAsync(int take, CancellationToken cancellationToken = default);
}
