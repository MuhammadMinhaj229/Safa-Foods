using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;

using SafaFoods.Api.Contracts.Subscriptions;
using SafaFoods.Core.Enums;
using SafaFoods.Core.Services;
using SafaFoods.Core.Services.Models;

namespace SafaFoods.Api.Endpoints;

public static class SubscriptionEndpoints
{
    public static RouteGroupBuilder MapSubscriptionEndpoints(this RouteGroupBuilder api)
    {
        var group = api.MapGroup("/subscriptions")
            .WithTags("Subscriptions");

        group.MapPost("/quote", HandleSubscriptionQuote)
            .WithName("GetSubscriptionQuote")
            .WithSummary("Calculates a prepaid subscription quote.")
            .WithDescription("Supports the MVP subscription plans for paste products.");

        group.MapGet("/", GetCustomerSubscriptionsAsync)
            .WithName("GetCustomerSubscriptions")
            .WithSummary("Returns subscriptions for the authenticated customer.")
            .RequireAuthorization("CustomerPolicy");

        group.MapGet("/{subscriptionId:guid}", GetSubscriptionByIdAsync)
            .WithName("GetSubscriptionById")
            .WithSummary("Returns a single subscription with delivery schedule.")
            .RequireAuthorization("CustomerPolicy");

        group.MapPost("/", CreateSubscriptionAsync)
            .WithName("CreateSubscription")
            .WithSummary("Creates a prepaid subscription draft pending payment confirmation.")
            .RequireAuthorization("CustomerPolicy");

        group.MapPost("/{subscriptionId:guid}/customer-status", UpdateSubscriptionStatusForCustomerAsync)
            .WithName("UpdateSubscriptionStatusForCustomer")
            .WithSummary("Allows a customer to pause, resume, or cancel their subscription.")
            .RequireAuthorization("CustomerPolicy");

        return group;
    }

    private static Results<Ok<SubscriptionQuoteResponse>, ValidationProblem> HandleSubscriptionQuote(
        SubscriptionQuoteRequest request,
        ISubscriptionPricingService subscriptionPricingService)
    {
        var validationErrors = new Dictionary<string, string[]>();

        if (request.UnitPrice <= 0)
        {
            validationErrors["unitPrice"] = ["Unit price must be greater than zero."];
        }

        if (request.QuantityPerDelivery <= 0)
        {
            validationErrors["quantityPerDelivery"] = ["Quantity per delivery must be greater than zero."];
        }

        if (validationErrors.Count > 0)
        {
            return TypedResults.ValidationProblem(validationErrors);
        }

        var quote = subscriptionPricingService.GetQuote(
            request.UnitPrice,
            request.QuantityPerDelivery,
            request.PlanCode);

        if (!quote.Success)
        {
            return TypedResults.ValidationProblem(new Dictionary<string, string[]>
            {
                ["planCode"] = [quote.ErrorMessage ?? "Invalid subscription plan."]
            });
        }

        return TypedResults.Ok(new SubscriptionQuoteResponse(
            quote.PlanCode!,
            quote.PlanName!,
            quote.BillingCycle!,
            quote.DeliveriesInCycle,
            quote.DiscountPercent,
            quote.CycleSubtotal,
            quote.DiscountAmount,
            quote.Total));
    }

    private static async Task<Results<Ok<IReadOnlyList<SubscriptionSummaryResponse>>, ValidationProblem>> GetCustomerSubscriptionsAsync(
        ClaimsPrincipal user,
        ISubscriptionManagementService subscriptionManagementService,
        CancellationToken cancellationToken)
    {
        var customerId = GetCustomerId(user);
        if (customerId == Guid.Empty)
        {
            return TypedResults.ValidationProblem(new Dictionary<string, string[]>
            {
                ["customerId"] = ["Customer id is required."]
            });
        }

        var subscriptions = await subscriptionManagementService.GetByCustomerAsync(customerId, cancellationToken);

        return TypedResults.Ok<IReadOnlyList<SubscriptionSummaryResponse>>(subscriptions
            .Select(x => new SubscriptionSummaryResponse(
                x.SubscriptionId,
                x.ProductId,
                x.VariantId,
                x.ProductName,
                x.VariantLabel,
                x.Status,
                x.PlanCode,
                x.PlanPrice,
                x.StartDate,
                x.NextDeliveryDate))
            .ToList());
    }

    private static async Task<Results<Ok<SubscriptionDetailResponse>, NotFound<string>>> GetSubscriptionByIdAsync(
        Guid subscriptionId,
        ClaimsPrincipal user,
        ISubscriptionManagementService subscriptionManagementService,
        CancellationToken cancellationToken)
    {
        var subscription = await subscriptionManagementService.GetByIdAsync(subscriptionId, cancellationToken);
        if (subscription is null || subscription.CustomerId != GetCustomerId(user))
        {
            return TypedResults.NotFound("Subscription not found.");
        }

        return TypedResults.Ok(MapSubscriptionDetail(subscription));
    }

    private static async Task<Results<Created<CreateSubscriptionResponse>, ValidationProblem, NotFound<string>>> CreateSubscriptionAsync(
        ClaimsPrincipal user,
        CreateSubscriptionRequest request,
        ISubscriptionManagementService subscriptionManagementService,
        CancellationToken cancellationToken)
    {
        var errors = new Dictionary<string, string[]>();

        if (request.AddressId == Guid.Empty)
            errors["addressId"] = ["Address id is required."];
        if (request.VariantId == Guid.Empty)
            errors["variantId"] = ["Variant id is required."];
        if (request.QuantityPerDelivery <= 0)
            errors["quantityPerDelivery"] = ["Quantity per delivery must be greater than zero."];
        if (string.IsNullOrWhiteSpace(request.PlanCode))
            errors["planCode"] = ["Plan code is required."];

        if (errors.Count > 0)
        {
            return TypedResults.ValidationProblem(errors);
        }

        var customerId = GetCustomerId(user);
        var result = await subscriptionManagementService.CreateAsync(
            new CreateSubscriptionCommand(
                customerId,
                request.AddressId,
                request.VariantId,
                request.PlanCode,
                request.QuantityPerDelivery,
                request.StartDate),
            cancellationToken);

        if (!result.Success || result.Data is null)
        {
            return result.ErrorCode switch
            {
                "customer_not_found" => TypedResults.NotFound(result.ErrorMessage ?? "Customer not found."),
                "address_not_found" => TypedResults.NotFound(result.ErrorMessage ?? "Address not found."),
                "variant_not_found" => TypedResults.NotFound(result.ErrorMessage ?? "Variant not found."),
                _ => TypedResults.ValidationProblem(new Dictionary<string, string[]>
                {
                    ["subscription"] = [result.ErrorMessage ?? "Unable to create subscription."]
                })
            };
        }

        var subscription = result.Data;

        return TypedResults.Created(
            $"/api/subscriptions/{subscription.SubscriptionId}",
            new CreateSubscriptionResponse(
                subscription.SubscriptionId,
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

    private static async Task<Results<Ok<SubscriptionDetailResponse>, ValidationProblem, NotFound<string>>> UpdateSubscriptionStatusForCustomerAsync(
        Guid subscriptionId,
        ClaimsPrincipal user,
        CustomerSubscriptionStatusRequest request,
        ISubscriptionManagementService subscriptionManagementService,
        CancellationToken cancellationToken)
    {
        if (request.Status is not (SubscriptionStatus.Paused or SubscriptionStatus.Active or SubscriptionStatus.Cancelled))
        {
            return TypedResults.ValidationProblem(new Dictionary<string, string[]>
            {
                ["status"] = ["Customers can only pause, resume, or cancel subscriptions."]
            });
        }

        var result = await subscriptionManagementService.UpdateStatusForCustomerAsync(
            new UpdateSubscriptionOwnershipCommand(
                subscriptionId,
                GetCustomerId(user),
                request.Status),
            cancellationToken);

        if (!result.Success || result.Data is null)
        {
            return result.ErrorCode switch
            {
                "subscription_not_found" => TypedResults.NotFound(result.ErrorMessage ?? "Subscription not found."),
                _ => TypedResults.ValidationProblem(new Dictionary<string, string[]>
                {
                    ["subscription"] = [result.ErrorMessage ?? "Unable to update subscription."]
                })
            };
        }

        return TypedResults.Ok(MapSubscriptionDetail(result.Data!));
    }

    private static Guid GetCustomerId(ClaimsPrincipal user) =>
        Guid.Parse(user.FindFirstValue("customer_id")!);
}
