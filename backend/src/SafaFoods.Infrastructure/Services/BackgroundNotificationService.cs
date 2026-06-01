using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SafaFoods.Core.Services;

namespace SafaFoods.Infrastructure.Services;

public sealed class BackgroundNotificationService(
    IServiceProvider serviceProvider,
    ILogger<BackgroundNotificationService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Background Notification Service is starting.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
                var outboxProcessingService = scope.ServiceProvider.GetRequiredService<IOutboxProcessingService>();
                var subscriptionReminderService = scope.ServiceProvider.GetRequiredService<ISubscriptionReminderService>();
                var dispatchService = scope.ServiceProvider.GetRequiredService<INotificationDispatchService>();

                var remindersCreated = await subscriptionReminderService.ProcessUpcomingDeliveryRemindersAsync(20, stoppingToken);
                if (remindersCreated > 0)
                {
                    logger.LogInformation("Created {Count} subscription reminder events.", remindersCreated);
                }

                var created = await outboxProcessingService.ProcessPendingAsync(50, stoppingToken);
                if (created > 0)
                {
                    logger.LogInformation("Created {Count} queued notifications from outbox events.", created);
                }

                var results = await dispatchService.DispatchPendingAsync(20, stoppingToken);
                
                if (results.Count > 0)
                {
                    logger.LogInformation("Dispatched {Count} pending notifications.", results.Count);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while dispatching background notifications.");
            }

            // Wait 30 seconds before next poll
            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }

        logger.LogInformation("Background Notification Service is stopping.");
    }
}
