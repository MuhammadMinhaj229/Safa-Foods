using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.Options;

using SafaFoods.Core.Entities;
using SafaFoods.Core.Domain;
using SafaFoods.Core.Enums;
using SafaFoods.Core.Options;
using SafaFoods.Core.Services;
using SafaFoods.Core.Services.Models;
using SafaFoods.Infrastructure.Persistence;

namespace SafaFoods.Infrastructure.Services;

public sealed class PaymentManagementService(
    SafaFoodsDbContext dbContext,
    IOrderManagementService orderManagementService,
    ISubscriptionManagementService subscriptionManagementService,
    IOutboxService outboxService,
    IOptions<UpiOptions> upiOptions) : IPaymentManagementService
{
    private readonly UpiOptions _upiOptions = upiOptions.Value;
    public async Task<ServiceResult<OrderDetailResult>> SubmitOrderPaymentReferenceAsync(
        Guid orderId,
        string paymentReference,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(paymentReference))
        {
            return ServiceResult<OrderDetailResult>.Fail("payment_reference_required", "Payment reference is required.");
        }

        var order = await dbContext.Orders
            .Include(x => x.Customer)
            .FirstOrDefaultAsync(x => x.Id == orderId, cancellationToken);

        if (order is null)
        {
            return ServiceResult<OrderDetailResult>.Fail("order_not_found", "Order not found.");
        }

        if (order.PaymentMethod == PaymentMethod.Cod)
        {
            return ServiceResult<OrderDetailResult>.Fail("invalid_payment_method", "COD orders do not require payment submission.");
        }

        order.PaymentReference = paymentReference.Trim();
        order.UpdatedAt = DateTimeOffset.UtcNow;

        await outboxService.PublishAsync(
            DomainEvents.PaymentSubmitted,
            "order",
            order.Id,
            new
            {
                customerId = order.CustomerId,
                customerName = order.Customer.FullName,
                recipient = order.Customer.Phone,
                entityLabel = "order",
                orderId = order.Id,
                orderCode = $"#{order.Id.ToString("N")[..8].ToUpperInvariant()}",
                paymentMethod = order.PaymentMethod.ToString(),
                paymentReference = order.PaymentReference,
                paymentStatus = order.PaymentStatus.ToString(),
                updatedAt = order.UpdatedAt.ToString("O")
            },
            cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        var updatedOrder = await orderManagementService.GetByIdAsync(orderId, cancellationToken);
        return updatedOrder is null
            ? ServiceResult<OrderDetailResult>.Fail("order_not_found", "Order not found.")
            : ServiceResult<OrderDetailResult>.Ok(updatedOrder);
    }

    public async Task<ServiceResult<OrderDetailResult>> ConfirmOrderPaymentAsync(
        Guid orderId,
        string paymentReference,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(paymentReference))
        {
            return ServiceResult<OrderDetailResult>.Fail("payment_reference_required", "Payment reference is required.");
        }

        var order = await dbContext.Orders
            .Include(x => x.Customer)
            .FirstOrDefaultAsync(x => x.Id == orderId, cancellationToken);

        if (order is null)
        {
            return ServiceResult<OrderDetailResult>.Fail("order_not_found", "Order not found.");
        }

        if (order.PaymentMethod == PaymentMethod.Cod)
        {
            return ServiceResult<OrderDetailResult>.Fail("invalid_payment_method", "COD orders cannot be manually marked as prepaid.");
        }

        order.PaymentStatus = PaymentStatus.Paid;
        order.PaymentReference = paymentReference.Trim();
        order.PaidAt = DateTimeOffset.UtcNow;
        order.UpdatedAt = DateTimeOffset.UtcNow;

        await outboxService.PublishAsync(
            DomainEvents.PaymentConfirmed,
            "order",
            order.Id,
            new
            {
                customerId = order.CustomerId,
                customerName = order.Customer.FullName,
                recipient = order.Customer.Phone,
                entityLabel = "order",
                orderId = order.Id,
                orderCode = $"#{order.Id.ToString("N")[..8].ToUpperInvariant()}",
                paymentMethod = order.PaymentMethod.ToString(),
                paymentReference = order.PaymentReference,
                paymentStatus = order.PaymentStatus.ToString(),
                paidAt = order.PaidAt?.ToString("O")
            },
            cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        var updatedOrder = await orderManagementService.GetByIdAsync(orderId, cancellationToken);
        return updatedOrder is null
            ? ServiceResult<OrderDetailResult>.Fail("order_not_found", "Order not found.")
            : ServiceResult<OrderDetailResult>.Ok(updatedOrder);
    }

    public async Task<ServiceResult<SubscriptionDetailResult>> SubmitSubscriptionPaymentReferenceAsync(
        Guid subscriptionId,
        string paymentReference,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(paymentReference))
        {
            return ServiceResult<SubscriptionDetailResult>.Fail("payment_reference_required", "Payment reference is required.");
        }

        var subscription = await dbContext.Subscriptions
            .Include(x => x.Customer)
            .FirstOrDefaultAsync(x => x.Id == subscriptionId, cancellationToken);

        if (subscription is null)
        {
            return ServiceResult<SubscriptionDetailResult>.Fail("subscription_not_found", "Subscription not found.");
        }

        subscription.PaymentReference = paymentReference.Trim();
        subscription.UpdatedAt = DateTimeOffset.UtcNow;

        await outboxService.PublishAsync(
            DomainEvents.PaymentSubmitted,
            "subscription",
            subscription.Id,
            new
            {
                customerId = subscription.CustomerId,
                customerName = subscription.Customer.FullName,
                recipient = subscription.Customer.Phone,
                entityLabel = "subscription",
                subscriptionId = subscription.Id,
                subscriptionCode = $"SUB-{subscription.Id.ToString("N")[..8].ToUpperInvariant()}",
                paymentReference = subscription.PaymentReference,
                status = subscription.Status.ToString(),
                updatedAt = subscription.UpdatedAt.ToString("O")
            },
            cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        var updatedSubscription = await subscriptionManagementService.GetByIdAsync(subscriptionId, cancellationToken);
        return updatedSubscription is null
            ? ServiceResult<SubscriptionDetailResult>.Fail("subscription_not_found", "Subscription not found.")
            : ServiceResult<SubscriptionDetailResult>.Ok(updatedSubscription);
    }

    public async Task<ServiceResult<SubscriptionDetailResult>> ConfirmSubscriptionPaymentAsync(
        Guid subscriptionId,
        string paymentReference,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(paymentReference))
        {
            return ServiceResult<SubscriptionDetailResult>.Fail("payment_reference_required", "Payment reference is required.");
        }

        var subscription = await dbContext.Subscriptions
            .Include(x => x.Customer)
            .FirstOrDefaultAsync(x => x.Id == subscriptionId, cancellationToken);

        if (subscription is null)
        {
            return ServiceResult<SubscriptionDetailResult>.Fail("subscription_not_found", "Subscription not found.");
        }

        subscription.PaymentReference = paymentReference.Trim();
        subscription.PaidAt = DateTimeOffset.UtcNow;
        subscription.UpdatedAt = DateTimeOffset.UtcNow;

        await outboxService.PublishAsync(
            DomainEvents.PaymentConfirmed,
            "subscription",
            subscription.Id,
            new
            {
                customerId = subscription.CustomerId,
                customerName = subscription.Customer.FullName,
                recipient = subscription.Customer.Phone,
                entityLabel = "subscription",
                subscriptionId = subscription.Id,
                subscriptionCode = $"SUB-{subscription.Id.ToString("N")[..8].ToUpperInvariant()}",
                paymentReference = subscription.PaymentReference,
                status = subscription.Status.ToString(),
                paidAt = subscription.PaidAt?.ToString("O")
            },
            cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        return await subscriptionManagementService.UpdateStatusAsync(
            subscriptionId,
            SubscriptionStatus.Active,
            cancellationToken);
    }

    public async Task<ServiceResult<UpiPaymentInstructionsResult>> GetOrderPaymentInstructionsAsync(
        Guid orderId,
        CancellationToken cancellationToken = default)
    {
        var order = await dbContext.Orders
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == orderId, cancellationToken);

        if (order is null)
        {
            return ServiceResult<UpiPaymentInstructionsResult>.Fail("order_not_found", "Order not found.");
        }

        if (order.PaymentMethod == PaymentMethod.Cod)
        {
            return ServiceResult<UpiPaymentInstructionsResult>.Fail("invalid_payment_method", "COD orders do not support UPI payment instructions.");
        }

        var amountBase = Math.Round(order.GrandTotal, 2);
        var transactionRef = $"O-{orderId.ToString()[..8].ToUpperInvariant()}";

        return ServiceResult<UpiPaymentInstructionsResult>.Ok(new UpiPaymentInstructionsResult(
            _upiOptions.MerchantUpiId,
            _upiOptions.MerchantName,
            amountBase,
            transactionRef,
            _upiOptions.GooglePayQrImagePath));
    }

    public async Task<ServiceResult<UpiPaymentInstructionsResult>> GetSubscriptionPaymentInstructionsAsync(
        Guid subscriptionId,
        CancellationToken cancellationToken = default)
    {
        var subscription = await dbContext.Subscriptions
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == subscriptionId, cancellationToken);

        if (subscription is null)
        {
            return ServiceResult<UpiPaymentInstructionsResult>.Fail("subscription_not_found", "Subscription not found.");
        }

        var amountBase = Math.Round(subscription.PlanPrice, 2);
        var transactionRef = $"S-{subscriptionId.ToString()[..8].ToUpperInvariant()}";

        return ServiceResult<UpiPaymentInstructionsResult>.Ok(new UpiPaymentInstructionsResult(
            _upiOptions.MerchantUpiId,
            _upiOptions.MerchantName,
            amountBase,
            transactionRef,
            _upiOptions.GooglePayQrImagePath));
    }

}
