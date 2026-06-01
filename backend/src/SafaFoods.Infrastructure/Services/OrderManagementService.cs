using SafaFoods.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using SafaFoods.Core.Entities;
using SafaFoods.Core.Enums;
using SafaFoods.Core.Options;
using SafaFoods.Core.Services;
using SafaFoods.Core.Services.Models;
using SafaFoods.Infrastructure.Persistence;

namespace SafaFoods.Infrastructure.Services;

public sealed class OrderManagementService(
    SafaFoodsDbContext dbContext,
    IDeliveryPricingService deliveryPricingService,
    IPromotionsService promotionsService,
    IWalletService walletService,
    IDeliverySlotService slotService,
    IOutboxService outboxService,
    IOptions<ShopOptions> shopOptions,
    ILogger<OrderManagementService> logger) : IOrderManagementService
{
    private readonly ShopOptions _shopOptions = shopOptions.Value;

    public OrderQuoteResult GetQuote(OrderQuoteCommand command)
    {
        var deliveryQuote = deliveryPricingService.GetQuote(command.DistanceKm);
        var subtotal = command.Items.Sum(x => x.UnitPrice * x.Quantity);
        var discountTotal = 0m;
        var deliveryFee = deliveryQuote.Fee ?? 0m;
        var grandTotal = subtotal - discountTotal + deliveryFee;
        var codAllowed = command.PaymentMethod == PaymentMethod.Cod &&
                         deliveryQuote.Serviceable &&
                         deliveryQuote.ZoneLabel is "Zone A" or "Zone B";

        return new OrderQuoteResult(
            subtotal,
            discountTotal,
            deliveryFee,
            grandTotal,
            codAllowed,
            deliveryQuote.ZoneLabel);
    }

    public async Task<ServiceResult<CreateOrderResult>> CreateAsync(
        CreateOrderCommand command,
        CancellationToken cancellationToken = default)
    {
        var customer = await dbContext.Customers.FirstOrDefaultAsync(
            x => x.Id == command.CustomerId,
            cancellationToken);
        if (customer is null)
        {
            return ServiceResult<CreateOrderResult>.Fail("customer_not_found", "Customer not found.");
        }

        var address = await dbContext.Addresses
            .Include(x => x.DeliveryZone)
            .FirstOrDefaultAsync(
                x => x.Id == command.AddressId && x.CustomerId == command.CustomerId,
                cancellationToken);
        if (address is null)
        {
            return ServiceResult<CreateOrderResult>.Fail("address_not_found", "Address not found.");
        }

        if (!address.IsServiceable || address.DistanceKm is null)
        {
            return ServiceResult<CreateOrderResult>.Fail("address_not_serviceable", "The selected address is not serviceable.");
        }

        var variantIds = command.Items.Select(x => x.VariantId).Distinct().ToArray();
        var variants = await dbContext.ProductVariants
            .Include(x => x.Product)
            .Where(x => variantIds.Contains(x.Id) && x.IsActive)
            .ToListAsync(cancellationToken);

        if (variants.Count != variantIds.Length)
        {
            return ServiceResult<CreateOrderResult>.Fail("invalid_variants", "One or more requested variants are invalid or inactive.");
        }

        var deliveryQuote = deliveryPricingService.GetQuote(address.DistanceKm.Value);
        var codAllowed = command.PaymentMethod == PaymentMethod.Cod &&
                         deliveryQuote.Serviceable &&
                         deliveryQuote.ZoneLabel is "Zone A" or "Zone B";

        if (command.PaymentMethod == PaymentMethod.Cod && !codAllowed)
        {
            return ServiceResult<CreateOrderResult>.Fail("cod_not_allowed", "COD is available only for eligible Zone A and Zone B one-time orders.");
        }

        var lineItems = command.Items.Select(item =>
        {
            var variant = variants.Single(x => x.Id == item.VariantId);
            var unitPrice = variant.SalePrice ?? variant.Price;

            return new
            {
                Variant = variant,
                item.Quantity,
                UnitPrice = unitPrice,
                TotalPrice = unitPrice * item.Quantity
            };
        }).ToList();

        // Phase 3: Delivery Slot Validation
        if (command.DeliverySlotId.HasValue && command.ScheduledDeliveryDate.HasValue)
        {
            var isAvailable = await slotService.IsSlotAvailableAsync(
                command.DeliverySlotId.Value, command.ScheduledDeliveryDate.Value, cancellationToken);
            if (!isAvailable)
            {
                return ServiceResult<CreateOrderResult>.Fail("slot_unavailable", "Selected delivery slot is full or inactive.");
            }
        }

        using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var subtotal = lineItems.Sum(x => x.TotalPrice);
            var discountTotal = 0m;
            
            // Phase 2: Promotions
            if (!string.IsNullOrEmpty(command.CouponCode))
            {
                var promoResult = await promotionsService.EvaluateCouponAsync(
                    command.CouponCode, command.CustomerId, subtotal, cancellationToken);
                if (promoResult.Success)
                {
                    discountTotal = promoResult.ExpectedDiscount;
                }
            }

            var deliveryFee = deliveryQuote.Fee ?? 0m;
            var totalBeforeWallet = subtotal - discountTotal + deliveryFee;
            var walletUsed = 0m;

            // Phase 3: Wallet Integration
            if (command.ApplyWalletBalance)
            {
                var wallet = await walletService.GetBalanceAsync(command.CustomerId, cancellationToken);
                walletUsed = Math.Min(wallet, totalBeforeWallet);
                if (walletUsed > 0)
                {
                    await walletService.DebitAsync(command.CustomerId, walletUsed, $"Order Payment", cancellationToken);
                }
            }

            var grandTotal = totalBeforeWallet - walletUsed;

        var order = new Order
        {
            CustomerId = customer.Id,
            AddressId = address.Id,
            DeliveryZoneId = address.DeliveryZoneId,
            DeliverySlotId = command.DeliverySlotId,
            ScheduledDeliveryDate = command.ScheduledDeliveryDate.HasValue 
                ? DateTime.SpecifyKind(command.ScheduledDeliveryDate.Value.Date, DateTimeKind.Utc) 
                : null,
            OrderType = OrderType.OneTime,
            PaymentMethod = command.PaymentMethod,
            PaymentStatus = (command.PaymentMethod == PaymentMethod.Cod || grandTotal > 0)
                ? PaymentStatus.Pending
                : PaymentStatus.Paid,
            InvoiceNumber = $"INV-{DateTimeOffset.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..8].ToUpperInvariant()}",
            InvoiceIssuedAt = DateTimeOffset.UtcNow,
            Status = OrderStatus.Placed,
            Subtotal = subtotal,
            DiscountTotal = discountTotal,
            DeliveryFee = deliveryFee,
            WalletAmountUsed = walletUsed,
            GrandTotal = grandTotal,
            AppliedCouponCode = command.CouponCode,
            Items = lineItems.Select(item => new OrderItem
            {
                ProductId = item.Variant.ProductId,
                VariantId = item.Variant.Id,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                TotalPrice = item.TotalPrice
            }).ToList()
        };

            dbContext.Orders.Add(order);
            await outboxService.PublishAsync(
                DomainEvents.OrderCreated,
                "order",
                order.Id,
                BuildOrderEventPayload(order, customer.FullName, customer.Phone),
                cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            return ServiceResult<CreateOrderResult>.Ok(new CreateOrderResult(
                order.Id,
                order.Status,
                order.Subtotal,
                order.DiscountTotal,
                order.DeliveryFee,
                order.GrandTotal,
                deliveryQuote.ZoneLabel,
                codAllowed));
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            logger.LogError(ex, "Failed to create order transactionally for Customer {CustomerId}", command.CustomerId);
            return ServiceResult<CreateOrderResult>.Fail("transaction_failed", "An error occurred while processing your order. Please try again.");
        }
    }

    public async Task<ServiceResult<OrderDetailResult>> CancelAsync(
        CancelOrderCommand command,
        CancellationToken cancellationToken = default)
    {
        var order = await dbContext.Orders
            .Include(x => x.DeliveryZone)
            .Include(x => x.Items)
                .ThenInclude(x => x.Product)
            .Include(x => x.Items)
                .ThenInclude(x => x.Variant)
            .FirstOrDefaultAsync(x => x.Id == command.OrderId && x.CustomerId == command.CustomerId, cancellationToken);

        if (order is null)
        {
            return ServiceResult<OrderDetailResult>.Fail("order_not_found", "Order not found.");
        }

        if (order.Status is not (OrderStatus.Placed or OrderStatus.Confirmed))
        {
            return ServiceResult<OrderDetailResult>.Fail("order_cannot_be_cancelled", "Order can only be cancelled before preparation starts.");
        }

        order.Status = OrderStatus.Cancelled;
        order.CancellationReason = string.IsNullOrWhiteSpace(command.Reason) ? "Cancelled by customer" : command.Reason.Trim();
        order.UpdatedAt = DateTimeOffset.UtcNow;
        await outboxService.PublishAsync(
            DomainEvents.OrderCancelled,
            "order",
            order.Id,
            new
            {
                customerId = order.CustomerId,
                customerName = order.Customer.FullName,
                recipient = order.Customer.Phone,
                orderId = order.Id,
                orderCode = BuildOrderCode(order.Id),
                status = order.Status.ToString(),
                statusLabel = ToStatusLabel(order.Status),
                cancellationReason = order.CancellationReason,
                updatedAt = order.UpdatedAt.ToString("O")
            },
            cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        return ServiceResult<OrderDetailResult>.Ok(new OrderDetailResult(
            order.Id,
            order.CustomerId,
            order.AddressId,
            order.Status,
            order.PaymentMethod,
            order.PaymentStatus,
            order.Subtotal,
            order.DiscountTotal,
            order.DeliveryFee,
            order.GrandTotal,
            order.DeliveryZone?.Name,
            order.PlacedAt,
            order.Items
                .OrderBy(item => item.Product.Name)
                .ThenBy(item => item.Variant.Label)
                .Select(item => new OrderDetailItemResult(
                    item.ProductId,
                    item.VariantId,
                    item.Product.Name,
                    item.Variant.Label,
                    item.Quantity,
                    item.UnitPrice,
                    item.TotalPrice))
                .ToList()));
    }

    public async Task<OrderDetailResult?> GetByIdAsync(
        Guid orderId,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Orders
            .AsNoTracking()
            .Where(x => x.Id == orderId)
            .Select(x => new OrderDetailResult(
                x.Id,
                x.CustomerId,
                x.AddressId,
                x.Status,
                x.PaymentMethod,
                x.PaymentStatus,
                x.Subtotal,
                x.DiscountTotal,
                x.DeliveryFee,
                x.GrandTotal,
                x.DeliveryZone != null ? x.DeliveryZone.Name : null,
                x.PlacedAt,
                x.Items
                    .OrderBy(item => item.Product.Name)
                    .ThenBy(item => item.Variant.Label)
                    .Select(item => new OrderDetailItemResult(
                        item.ProductId,
                        item.VariantId,
                        item.Product.Name,
                        item.Variant.Label,
                        item.Quantity,
                        item.UnitPrice,
                        item.TotalPrice))
                    .ToList()))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<OrderSummaryResult>> GetByCustomerAsync(
        Guid customerId,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Orders
            .AsNoTracking()
            .Where(x => x.CustomerId == customerId)
            .OrderByDescending(x => x.PlacedAt)
            .Select(x => new OrderSummaryResult(
                x.Id,
                x.Status,
                x.PaymentMethod,
                x.PaymentStatus,
                x.GrandTotal,
                x.DeliveryZone != null ? x.DeliveryZone.Name : null,
                x.PlacedAt,
                x.Items.Count))
            .ToListAsync(cancellationToken);
    }

    public async Task<ServiceResult<OrderDetailResult>> UpdateStatusAsync(
        Guid orderId,
        OrderStatus nextStatus,
        CancellationToken cancellationToken = default)
    {
        var order = await dbContext.Orders
            .Include(x => x.Customer)
            .Include(x => x.DeliveryZone)
            .Include(x => x.Items)
                .ThenInclude(x => x.Product)
            .Include(x => x.Items)
                .ThenInclude(x => x.Variant)
            .FirstOrDefaultAsync(x => x.Id == orderId, cancellationToken);

        if (order is null)
        {
            return ServiceResult<OrderDetailResult>.Fail("order_not_found", "Order not found.");
        }

        if (!OrderStatusTransitions.CanTransition(order.Status, nextStatus))
        {
            return ServiceResult<OrderDetailResult>.Fail(
                "invalid_status_transition",
                $"Cannot change order status from {order.Status} to {nextStatus}.");
        }

        order.Status = nextStatus;
        order.UpdatedAt = DateTimeOffset.UtcNow;

        if (nextStatus == OrderStatus.Delivered && order.PaymentMethod == PaymentMethod.Cod)
        {
            order.PaymentStatus = PaymentStatus.Paid;
        }

        await outboxService.PublishAsync(
            DomainEvents.OrderStatusChanged,
            "order",
            order.Id,
            new
            {
                customerId = order.CustomerId,
                customerName = order.Customer.FullName,
                recipient = order.Customer.Phone,
                orderId = order.Id,
                orderCode = BuildOrderCode(order.Id),
                status = order.Status.ToString(),
                statusLabel = ToStatusLabel(order.Status),
                paymentStatus = order.PaymentStatus.ToString(),
                updatedAt = order.UpdatedAt.ToString("O")
            },
            cancellationToken);

        if (nextStatus == OrderStatus.Delivered)
        {
            await outboxService.PublishAsync(
                DomainEvents.ReviewRequested,
                "order",
                order.Id,
                BuildReviewRequestedPayload(order),
                cancellationToken);
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return ServiceResult<OrderDetailResult>.Ok(new OrderDetailResult(
            order.Id,
            order.CustomerId,
            order.AddressId,
            order.Status,
            order.PaymentMethod,
            order.PaymentStatus,
            order.Subtotal,
            order.DiscountTotal,
            order.DeliveryFee,
            order.GrandTotal,
            order.DeliveryZone?.Name,
            order.PlacedAt,
            order.Items
                .OrderBy(item => item.Product.Name)
                .ThenBy(item => item.Variant.Label)
                .Select(item => new OrderDetailItemResult(
                    item.ProductId,
                    item.VariantId,
                    item.Product.Name,
                    item.Variant.Label,
                    item.Quantity,
                    item.UnitPrice,
                    item.TotalPrice))
                .ToList()));
    }

    private static object BuildOrderEventPayload(Order order, string customerName, string recipient) => new
    {
        customerId = order.CustomerId,
        customerName,
        recipient,
        orderId = order.Id,
        orderCode = BuildOrderCode(order.Id),
        status = order.Status.ToString(),
        statusLabel = ToStatusLabel(order.Status),
        paymentMethod = order.PaymentMethod.ToString(),
        paymentStatus = order.PaymentStatus.ToString(),
        grandTotal = order.GrandTotal.ToString("0.##"),
        placedAt = order.PlacedAt.ToString("O")
    };

    private static string BuildOrderCode(Guid orderId) =>
        $"#{orderId.ToString("N")[..8].ToUpperInvariant()}";

    private object BuildReviewRequestedPayload(Order order)
    {
        var reviewUrl = BuildReviewUrl(order.Id);

        return new
        {
            customerId = order.CustomerId,
            customerName = order.Customer.FullName,
            recipient = order.Customer.Phone,
            orderId = order.Id,
            orderCode = BuildOrderCode(order.Id),
            reviewUrl,
            deliveredAt = order.UpdatedAt.ToString("O")
        };
    }

    private string BuildReviewUrl(Guid orderId)
    {
        var publicBaseUrl = string.IsNullOrWhiteSpace(_shopOptions.PublicBaseUrl)
            ? "http://localhost:3000"
            : _shopOptions.PublicBaseUrl.Trim().TrimEnd('/');
        var reviewPath = string.IsNullOrWhiteSpace(_shopOptions.ReviewPagePath)
            ? "/profile?tab=orders&action=review"
            : _shopOptions.ReviewPagePath.Trim();
        var separator = reviewPath.Contains('?', StringComparison.Ordinal) ? "&" : "?";

        return $"{publicBaseUrl}{reviewPath}{separator}orderId={orderId}";
    }

    private static string ToStatusLabel(OrderStatus status) => status switch
    {
        OrderStatus.OutForDelivery => "out for delivery",
        OrderStatus.Delivered => "delivered",
        OrderStatus.Confirmed => "confirmed",
        OrderStatus.Preparing => "being prepared",
        OrderStatus.Cancelled => "cancelled",
        OrderStatus.Failed => "failed",
        _ => status.ToString().ToLowerInvariant()
    };
}
