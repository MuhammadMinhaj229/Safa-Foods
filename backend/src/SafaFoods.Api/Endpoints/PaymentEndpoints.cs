using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;

using SafaFoods.Api.Contracts.Orders;
using SafaFoods.Api.Contracts.Payments;
using SafaFoods.Api.Contracts.Subscriptions;
using SafaFoods.Api.Configuration;
using SafaFoods.Core.Services;
using SafaFoods.Core.Services.Models;

namespace SafaFoods.Api.Endpoints;

public static class PaymentEndpoints
{
    public static RouteGroupBuilder MapPaymentEndpoints(this RouteGroupBuilder api)
    {
        var group = api.MapGroup("/payments")
            .WithTags("Payments")
            .RequireAuthorization("CustomerPolicy");

        group.MapPost("/orders/{orderId:guid}/submit", SubmitOrderPaymentAsync)
            .WithName("SubmitOrderPaymentReference")
            .WithSummary("Submits a manual payment reference for a prepaid order.");

        group.MapPost("/subscriptions/{subscriptionId:guid}/submit", SubmitSubscriptionPaymentAsync)
            .WithName("SubmitSubscriptionPaymentReference")
            .WithSummary("Submits a manual payment reference for a prepaid subscription.");

        group.MapGet("/orders/{orderId:guid}/instructions", GetOrderPaymentInstructionsAsync)
            .WithName("GetOrderPaymentInstructions")
            .WithSummary("Gets the UPI payment instructions for an order.");

        group.MapGet("/subscriptions/{subscriptionId:guid}/instructions", GetSubscriptionPaymentInstructionsAsync)
            .WithName("GetSubscriptionPaymentInstructions")
            .WithSummary("Gets the UPI payment instructions for a subscription.");

        return group;
    }

    private static async Task<Results<Ok<OrderDetailResponse>, ValidationProblem, NotFound<string>>> SubmitOrderPaymentAsync(
        Guid orderId,
        ClaimsPrincipal user,
        ConfirmPaymentRequest request,
        IOrderManagementService orderManagementService,
        IPaymentManagementService paymentManagementService,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.PaymentReference))
        {
            return TypedResults.ValidationProblem(new Dictionary<string, string[]>
            {
                ["paymentReference"] = ["Payment reference is required."]
            });
        }

        var order = await orderManagementService.GetByIdAsync(orderId, cancellationToken);
        if (order is null || order.CustomerId != GetCustomerId(user))
        {
            return TypedResults.NotFound("Order not found.");
        }

        var result = await paymentManagementService.SubmitOrderPaymentReferenceAsync(
            orderId,
            request.PaymentReference,
            cancellationToken);

        if (!result.Success || result.Data is null)
        {
            return result.ErrorCode switch
            {
                "order_not_found" => TypedResults.NotFound(result.ErrorMessage ?? "Order not found."),
                _ => TypedResults.ValidationProblem(new Dictionary<string, string[]>
                {
                    ["payment"] = [result.ErrorMessage ?? "Unable to submit order payment reference."]
                })
            };
        }

        return TypedResults.Ok(MapOrderDetail(result.Data));
    }

    private static async Task<Results<Ok<SubscriptionDetailResponse>, ValidationProblem, NotFound<string>>> SubmitSubscriptionPaymentAsync(
        Guid subscriptionId,
        ClaimsPrincipal user,
        ConfirmPaymentRequest request,
        ISubscriptionManagementService subscriptionManagementService,
        IPaymentManagementService paymentManagementService,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.PaymentReference))
        {
            return TypedResults.ValidationProblem(new Dictionary<string, string[]>
            {
                ["paymentReference"] = ["Payment reference is required."]
            });
        }

        var subscription = await subscriptionManagementService.GetByIdAsync(subscriptionId, cancellationToken);
        if (subscription is null || subscription.CustomerId != GetCustomerId(user))
        {
            return TypedResults.NotFound("Subscription not found.");
        }

        var result = await paymentManagementService.SubmitSubscriptionPaymentReferenceAsync(
            subscriptionId,
            request.PaymentReference,
            cancellationToken);

        if (!result.Success || result.Data is null)
        {
            return result.ErrorCode switch
            {
                "subscription_not_found" => TypedResults.NotFound(result.ErrorMessage ?? "Subscription not found."),
                _ => TypedResults.ValidationProblem(new Dictionary<string, string[]>
                {
                    ["payment"] = [result.ErrorMessage ?? "Unable to submit subscription payment reference."]
                })
            };
        }

        return TypedResults.Ok(MapSubscriptionDetail(result.Data));
    }

    private static async Task<Results<Ok<UpiPaymentInstructionsResponse>, ValidationProblem, NotFound<string>>> GetOrderPaymentInstructionsAsync(
        Guid orderId,
        ClaimsPrincipal user,
        IOrderManagementService orderManagementService,
        IPaymentManagementService paymentManagementService,
        CancellationToken cancellationToken)
    {
        var order = await orderManagementService.GetByIdAsync(orderId, cancellationToken);
        if (order is null || order.CustomerId != GetCustomerId(user))
        {
            return TypedResults.NotFound("Order not found.");
        }

        var result = await paymentManagementService.GetOrderPaymentInstructionsAsync(orderId, cancellationToken);

        if (!result.Success || result.Data is null)
        {
            return result.ErrorCode switch
            {
                "order_not_found" => TypedResults.NotFound(result.ErrorMessage ?? "Order not found."),
                _ => TypedResults.ValidationProblem(new Dictionary<string, string[]>
                {
                    ["payment"] = [result.ErrorMessage ?? "Unable to get order payment instructions."]
                })
            };
        }

        return TypedResults.Ok(new UpiPaymentInstructionsResponse(
            result.Data.UpiId,
            result.Data.MerchantName,
            result.Data.Amount,
            result.Data.TransactionReference,
            result.Data.QrImageUrl));
    }

    private static async Task<Results<Ok<UpiPaymentInstructionsResponse>, ValidationProblem, NotFound<string>>> GetSubscriptionPaymentInstructionsAsync(
        Guid subscriptionId,
        ClaimsPrincipal user,
        ISubscriptionManagementService subscriptionManagementService,
        IPaymentManagementService paymentManagementService,
        CancellationToken cancellationToken)
    {
        var subscription = await subscriptionManagementService.GetByIdAsync(subscriptionId, cancellationToken);
        if (subscription is null || subscription.CustomerId != GetCustomerId(user))
        {
            return TypedResults.NotFound("Subscription not found.");
        }

        var result = await paymentManagementService.GetSubscriptionPaymentInstructionsAsync(subscriptionId, cancellationToken);

        if (!result.Success || result.Data is null)
        {
            return result.ErrorCode switch
            {
                "subscription_not_found" => TypedResults.NotFound(result.ErrorMessage ?? "Subscription not found."),
                _ => TypedResults.ValidationProblem(new Dictionary<string, string[]>
                {
                    ["payment"] = [result.ErrorMessage ?? "Unable to get subscription payment instructions."]
                })
            };
        }

        return TypedResults.Ok(new UpiPaymentInstructionsResponse(
            result.Data.UpiId,
            result.Data.MerchantName,
            result.Data.Amount,
            result.Data.TransactionReference,
            result.Data.QrImageUrl));
    }

    private static Guid GetCustomerId(ClaimsPrincipal user) =>
        Guid.Parse(user.FindFirstValue("customer_id")!);

    private static OrderDetailResponse MapOrderDetail(OrderDetailResult order) =>
        new(
            order.OrderId,
            order.CustomerId,
            order.AddressId,
            order.Status,
            order.PaymentMethod,
            order.PaymentStatus,
            order.Subtotal,
            order.DiscountTotal,
            order.DeliveryFee,
            order.GrandTotal,
            order.ZoneLabel,
            order.PlacedAt,
            order.Items.Select(item => new OrderDetailItemResponse(
                item.ProductId,
                item.VariantId,
                item.ProductName,
                item.VariantLabel,
                item.Quantity,
                item.UnitPrice,
                item.TotalPrice)).ToList());

    private static SubscriptionDetailResponse MapSubscriptionDetail(SubscriptionDetailResult subscription) =>
        new(
            subscription.SubscriptionId,
            subscription.CustomerId,
            subscription.AddressId,
            subscription.ProductId,
            subscription.VariantId,
            subscription.ProductName,
            subscription.VariantLabel,
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
            subscription.ZoneLabel,
            subscription.Deliveries.Select(x => new SubscriptionDeliveryResponse(
                x.DeliveryId,
                x.ScheduledDate,
                x.Quantity,
                x.DeliveryFee,
                x.Status,
                x.FulfilledOrderId)).ToList());
}
