using Microsoft.EntityFrameworkCore;

using SafaFoods.Core.Domain;
using SafaFoods.Core.Entities;
using SafaFoods.Core.Enums;
using SafaFoods.Core.Services;
using SafaFoods.Core.Services.Models;
using SafaFoods.Infrastructure.Persistence;

namespace SafaFoods.Infrastructure.Services;

public sealed class SubscriptionManagementService(
    SafaFoodsDbContext dbContext,
    IDeliveryPricingService deliveryPricingService,
    ISubscriptionPricingService subscriptionPricingService,
    IOutboxService outboxService) : ISubscriptionManagementService
{
    public async Task<ServiceResult<CreateSubscriptionResult>> CreateAsync(
        CreateSubscriptionCommand command,
        CancellationToken cancellationToken = default)
    {
        var customer = await dbContext.Customers
            .FirstOrDefaultAsync(x => x.Id == command.CustomerId, cancellationToken);
        if (customer is null)
        {
            return ServiceResult<CreateSubscriptionResult>.Fail("customer_not_found", "Customer not found.");
        }

        var address = await dbContext.Addresses
            .Include(x => x.DeliveryZone)
            .FirstOrDefaultAsync(
                x => x.Id == command.AddressId && x.CustomerId == command.CustomerId,
                cancellationToken);
        if (address is null)
        {
            return ServiceResult<CreateSubscriptionResult>.Fail("address_not_found", "Address not found.");
        }

        if (!address.IsServiceable || address.DistanceKm is null)
        {
            return ServiceResult<CreateSubscriptionResult>.Fail(
                "address_not_serviceable",
                "Subscriptions are available only for serviceable addresses.");
        }

        var variant = await dbContext.ProductVariants
            .Include(x => x.Product)
            .FirstOrDefaultAsync(x => x.Id == command.VariantId && x.IsActive, cancellationToken);
        if (variant is null)
        {
            return ServiceResult<CreateSubscriptionResult>.Fail("variant_not_found", "Product variant not found.");
        }

        if (!variant.Product.IsSubscriptionEnabled)
        {
            return ServiceResult<CreateSubscriptionResult>.Fail(
                "subscription_not_supported",
                "The selected product does not support subscriptions.");
        }

        // MVP subscriptions use a business-defined monthly kilogram price rather than the
        // retail variant sale price so repeat customers get a consistent plan amount.
        const decimal subscriptionUnitPricePerKg = 200m;
        var quote = subscriptionPricingService.GetQuote(subscriptionUnitPricePerKg, command.QuantityPerDelivery, command.PlanCode);
        if (!quote.Success || quote.PlanCode is null || quote.BillingCycle is null)
        {
            return ServiceResult<CreateSubscriptionResult>.Fail(
                "invalid_plan",
                quote.ErrorMessage ?? "Unsupported subscription plan.");
        }

        var startDate = NormalizeDate(command.StartDate);
        var subscription = new Subscription
        {
            CustomerId = customer.Id,
            ProductId = variant.ProductId,
            VariantId = variant.Id,
            PlanCode = quote.PlanCode,
            BillingCycle = quote.BillingCycle,
            DeliveriesInCycle = quote.DeliveriesInCycle,
            QuantityPerDelivery = command.QuantityPerDelivery,
            PlanPrice = quote.Total,
            DiscountPercent = quote.DiscountPercent,
            StartDate = startDate,
            EndDate = null,
            NextDeliveryDate = startDate,
            NextBillingDate = null,
            Status = SubscriptionStatus.PendingPayment,
            AddressId = address.Id,
            DeliveryZoneId = address.DeliveryZoneId
        };

        dbContext.Subscriptions.Add(subscription);
        await outboxService.PublishAsync(
            DomainEvents.SubscriptionCreated,
            "subscription",
            subscription.Id,
            new
            {
                customerId = subscription.CustomerId,
                customerName = customer.FullName,
                recipient = customer.Phone,
                subscriptionId = subscription.Id,
                subscriptionCode = BuildSubscriptionCode(subscription.Id),
                productId = subscription.ProductId,
                variantId = subscription.VariantId,
                status = subscription.Status.ToString(),
                planCode = subscription.PlanCode,
                planPrice = subscription.PlanPrice.ToString("0.##"),
                nextDeliveryDate = subscription.NextDeliveryDate.ToString("dd MMM yyyy")
            },
            cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return ServiceResult<CreateSubscriptionResult>.Ok(new CreateSubscriptionResult(
            subscription.Id,
            subscription.Status,
            subscription.PlanCode,
            subscription.BillingCycle,
            subscription.DeliveriesInCycle,
            subscription.QuantityPerDelivery,
            subscription.PlanPrice,
            subscription.DiscountPercent,
            subscription.StartDate,
            subscription.NextDeliveryDate));
    }

    public async Task<SubscriptionDetailResult?> GetByIdAsync(
        Guid subscriptionId,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Subscriptions
            .AsNoTracking()
            .Where(x => x.Id == subscriptionId)
            .Select(x => new SubscriptionDetailResult(
                x.Id,
                x.CustomerId,
                x.AddressId,
                x.ProductId,
                x.VariantId,
                x.Product.Name,
                x.Variant.Label,
                x.Status,
                x.PlanCode,
                x.BillingCycle,
                x.DeliveriesInCycle,
                x.QuantityPerDelivery,
                x.PlanPrice,
                x.DiscountPercent,
                x.StartDate,
                x.NextDeliveryDate,
                x.NextBillingDate,
                x.EndDate,
                x.DeliveryZone != null ? x.DeliveryZone.Name : null,
                x.Deliveries
                    .OrderBy(delivery => delivery.ScheduledDate)
                    .Select(delivery => new SubscriptionDeliveryResult(
                        delivery.Id,
                        delivery.ScheduledDate,
                        delivery.Quantity,
                        delivery.DeliveryFee,
                        delivery.Status,
                        delivery.FulfilledOrderId))
                    .ToList()))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<SubscriptionSummaryResult>> GetByCustomerAsync(
        Guid customerId,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Subscriptions
            .AsNoTracking()
            .Where(x => x.CustomerId == customerId)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new SubscriptionSummaryResult(
                x.Id,
                x.ProductId,
                x.VariantId,
                x.Product.Name,
                x.Variant.Label,
                x.Status,
                x.PlanCode,
                x.PlanPrice,
                x.StartDate,
                x.NextDeliveryDate))
            .ToListAsync(cancellationToken);
    }

    public async Task<ServiceResult<SubscriptionDetailResult>> UpdateStatusAsync(
        Guid subscriptionId,
        SubscriptionStatus nextStatus,
        CancellationToken cancellationToken = default)
    {
        var subscription = await dbContext.Subscriptions
            .Include(x => x.Product)
            .Include(x => x.Variant)
            .Include(x => x.Customer)
            .Include(x => x.DeliveryZone)
            .Include(x => x.Address)
            .Include(x => x.Deliveries)
            .FirstOrDefaultAsync(x => x.Id == subscriptionId, cancellationToken);

        if (subscription is null)
        {
            return ServiceResult<SubscriptionDetailResult>.Fail("subscription_not_found", "Subscription not found.");
        }

        if (!SubscriptionStatusTransitions.CanTransition(subscription.Status, nextStatus))
        {
            return ServiceResult<SubscriptionDetailResult>.Fail(
                "invalid_status_transition",
                $"Cannot change subscription status from {subscription.Status} to {nextStatus}.");
        }

        var previousStatus = subscription.Status;
        subscription.Status = nextStatus;
        subscription.UpdatedAt = DateTimeOffset.UtcNow;

        if (nextStatus == SubscriptionStatus.Active)
        {
            EnsureDeliverySchedule(subscription);
            subscription.EndDate = null;
        }

        if (nextStatus is SubscriptionStatus.Cancelled or SubscriptionStatus.Expired)
        {
            subscription.EndDate = DateTimeOffset.UtcNow;

            foreach (var delivery in subscription.Deliveries
                         .Where(x => x.Status is SubscriptionDeliveryStatus.Scheduled or SubscriptionDeliveryStatus.Confirmed)
                         .Where(x => x.ScheduledDate >= NormalizeDate(DateTimeOffset.UtcNow)))
            {
                delivery.Status = SubscriptionDeliveryStatus.Skipped;
                delivery.UpdatedAt = DateTimeOffset.UtcNow;
            }
        }

        var eventType = nextStatus switch
        {
            SubscriptionStatus.Active when previousStatus == SubscriptionStatus.Paused => DomainEvents.SubscriptionResumed,
            SubscriptionStatus.Active => DomainEvents.SubscriptionActivated,
            SubscriptionStatus.Paused => DomainEvents.SubscriptionPaused,
            SubscriptionStatus.Cancelled => DomainEvents.SubscriptionCancelled,
            _ => DomainEvents.SubscriptionStatusChanged
        };

        await outboxService.PublishAsync(
            eventType,
            "subscription",
            subscription.Id,
            new
            {
                customerId = subscription.CustomerId,
                customerName = subscription.Customer.FullName,
                recipient = subscription.Customer.Phone,
                subscriptionId = subscription.Id,
                subscriptionCode = BuildSubscriptionCode(subscription.Id),
                status = subscription.Status.ToString(),
                planCode = subscription.PlanCode,
                nextDeliveryDate = subscription.NextDeliveryDate.ToString("dd MMM yyyy"),
                nextBillingDate = subscription.NextBillingDate?.ToString("dd MMM yyyy"),
                endDate = subscription.EndDate?.ToString("dd MMM yyyy"),
                updatedAt = subscription.UpdatedAt.ToString("O")
            },
            cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        return ServiceResult<SubscriptionDetailResult>.Ok(new SubscriptionDetailResult(
            subscription.Id,
            subscription.CustomerId,
            subscription.AddressId,
            subscription.ProductId,
            subscription.VariantId,
            subscription.Product.Name,
            subscription.Variant.Label,
            subscription.Status,
            subscription.PlanCode,
            subscription.BillingCycle,
            subscription.DeliveriesInCycle,
            subscription.QuantityPerDelivery,
            subscription.PlanPrice,
            subscription.DiscountPercent,
            subscription.StartDate,
            subscription.NextDeliveryDate,
            subscription.NextBillingDate,
            subscription.EndDate,
            subscription.DeliveryZone?.Name,
            subscription.Deliveries
                .OrderBy(x => x.ScheduledDate)
                .Select(x => new SubscriptionDeliveryResult(
                    x.Id,
                    x.ScheduledDate,
                    x.Quantity,
                    x.DeliveryFee,
                    x.Status,
                    x.FulfilledOrderId))
                .ToList()));
    }

    public async Task<ServiceResult<SubscriptionDetailResult>> UpdateStatusForCustomerAsync(
        UpdateSubscriptionOwnershipCommand command,
        CancellationToken cancellationToken = default)
    {
        var subscription = await dbContext.Subscriptions
            .FirstOrDefaultAsync(
                x => x.Id == command.SubscriptionId && x.CustomerId == command.CustomerId,
                cancellationToken);

        if (subscription is null)
        {
            return ServiceResult<SubscriptionDetailResult>.Fail("subscription_not_found", "Subscription not found.");
        }

        return await UpdateStatusAsync(command.SubscriptionId, command.NextStatus, cancellationToken);
    }

    private void EnsureDeliverySchedule(Subscription subscription)
    {
        if (subscription.Deliveries.Any(x => x.Status is not SubscriptionDeliveryStatus.Skipped))
        {
            subscription.NextDeliveryDate = subscription.Deliveries
                .Where(x => x.Status is SubscriptionDeliveryStatus.Scheduled or SubscriptionDeliveryStatus.Confirmed or SubscriptionDeliveryStatus.Preparing or SubscriptionDeliveryStatus.OutForDelivery)
                .OrderBy(x => x.ScheduledDate)
                .Select(x => x.ScheduledDate)
                .FirstOrDefault(subscription.StartDate);
            return;
        }

        var deliveryQuote = deliveryPricingService.GetQuote(subscription.Address.DistanceKm ?? 0m);
        var dates = BuildScheduleDates(subscription.StartDate, subscription.PlanCode, subscription.DeliveriesInCycle);

        foreach (var scheduledDate in dates)
        {
            subscription.Deliveries.Add(new SubscriptionDelivery
            {
                SubscriptionId = subscription.Id,
                ScheduledDate = scheduledDate,
                Quantity = subscription.QuantityPerDelivery,
                DeliveryFee = deliveryQuote.Fee ?? 0m,
                Status = SubscriptionDeliveryStatus.Scheduled
            });
        }

        subscription.NextDeliveryDate = dates[0];
    }

    private static List<DateTimeOffset> BuildScheduleDates(DateTimeOffset startDate, string planCode, int deliveriesInCycle)
    {
        var normalizedStartDate = NormalizeDate(startDate);
        var firstMonday = GetNextOrSameDayOfWeek(normalizedStartDate, DayOfWeek.Monday);

        return planCode.ToLowerInvariant() switch
        {
            "monthly_750g_monday" => Enumerable.Range(0, deliveriesInCycle)
                .Select(index => firstMonday.AddDays(index * 7))
                .ToList(),
            _ => Enumerable.Range(0, deliveriesInCycle)
                .Select(index => firstMonday.AddDays(index * 7))
                .ToList()
        };
    }

    private static DateTimeOffset GetNextOrSameDayOfWeek(DateTimeOffset value, DayOfWeek dayOfWeek)
    {
        var daysToAdd = ((int)dayOfWeek - (int)value.DayOfWeek + 7) % 7;
        return value.AddDays(daysToAdd);
    }

    private static DateTimeOffset NormalizeDate(DateTimeOffset value) =>
        new(value.Year, value.Month, value.Day, 9, 0, 0, value.Offset);

    private static string BuildSubscriptionCode(Guid subscriptionId) =>
        $"SUB-{subscriptionId.ToString("N")[..8].ToUpperInvariant()}";
}
