using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;

using SafaFoods.Api.Contracts.Orders;
using SafaFoods.Core.Services;
using SafaFoods.Core.Services.Models;

namespace SafaFoods.Api.Endpoints;

public static class OrderEndpoints
{
    public static RouteGroupBuilder MapOrderEndpoints(this RouteGroupBuilder api)
    {
        var group = api.MapGroup("/orders")
            .WithTags("Orders");

        group.MapPost("/quote", GetCheckoutQuote)
            .WithName("GetCheckoutQuote")
            .WithSummary("Calculates checkout totals from items and delivery distance.");

        group.MapGet("/", GetCustomerOrdersAsync)
            .WithName("GetCustomerOrders")
            .WithSummary("Returns one-time orders for the authenticated customer.")
            .RequireAuthorization("CustomerPolicy");

        group.MapGet("/{orderId:guid}", GetOrderByIdAsync)
            .WithName("GetOrderById")
            .WithSummary("Returns a single order by id.")
            .RequireAuthorization("CustomerPolicy");

        group.MapGet("/{orderId:guid}/invoice", GetOrderInvoiceAsync)
            .WithName("GetOrderInvoice")
            .WithSummary("Returns invoice data for a placed order.")
            .RequireAuthorization("CustomerPolicy");

        group.MapPost("/", CreateOrderAsync)
            .WithName("CreateOrder")
            .WithSummary("Creates a local Safa Foods one-time order.")
            .RequireAuthorization("CustomerPolicy");

        group.MapPost("/{orderId:guid}/cancel", CancelOrderAsync)
            .WithName("CancelOrder")
            .WithSummary("Cancels a customer order before preparation starts.")
            .RequireAuthorization("CustomerPolicy");

        return group;
    }

    private static Results<Ok<CheckoutQuoteResponse>, ValidationProblem> GetCheckoutQuote(
        CheckoutQuoteRequest request,
        IOrderManagementService orderManagementService)
    {
        var errors = new Dictionary<string, string[]>();

        if (request.Items.Count == 0)
            errors["items"] = ["At least one item is required."];
        if (request.Items.Any(x => x.Quantity <= 0))
            errors["quantity"] = ["Item quantity must be greater than zero."];
        if (request.Items.Any(x => x.UnitPrice <= 0))
            errors["unitPrice"] = ["Item unit price must be greater than zero."];

        if (errors.Count > 0)
        {
            return TypedResults.ValidationProblem(errors);
        }

        var quote = orderManagementService.GetQuote(new OrderQuoteCommand(
            request.DistanceKm,
            request.PaymentMethod,
            request.Items.Select(x => new OrderQuoteLineCommand(
                x.ProductName,
                x.Quantity,
                x.UnitPrice)).ToList()));

        return TypedResults.Ok(new CheckoutQuoteResponse(
            quote.Subtotal,
            quote.DiscountTotal,
            quote.DeliveryFee,
            quote.GrandTotal,
            quote.CodAllowed,
            quote.ZoneLabel));
    }

    private static async Task<Results<Created<CreateOrderResponse>, ValidationProblem, NotFound<string>>> CreateOrderAsync(
        ClaimsPrincipal user,
        CreateOrderRequest request,
        IOrderManagementService orderManagementService,
        CancellationToken cancellationToken)
    {
        var errors = new Dictionary<string, string[]>();

        if (request.Items.Count == 0)
            errors["items"] = ["At least one order item is required."];
        if (request.Items.Any(x => x.Quantity <= 0))
            errors["quantity"] = ["Item quantity must be greater than zero."];

        if (errors.Count > 0)
        {
            return TypedResults.ValidationProblem(errors);
        }

        var customerId = GetCustomerId(user);
        var result = await orderManagementService.CreateAsync(
            new CreateOrderCommand(
                customerId,
                request.AddressId,
                request.PaymentMethod,
                request.DeliverySlotId,
                request.ScheduledDeliveryDate,
                request.ApplyWalletBalance,
                request.CouponCode,
                request.Items.Select(x => new CreateOrderLineCommand(
                    x.VariantId,
                    x.Quantity)).ToList()),
            cancellationToken);

        if (!result.Success || result.Data is null)
        {
            return result.ErrorCode switch
            {
                "customer_not_found" => TypedResults.NotFound(result.ErrorMessage ?? "Customer not found."),
                "address_not_found" => TypedResults.NotFound(result.ErrorMessage ?? "Address not found."),
                _ => TypedResults.ValidationProblem(new Dictionary<string, string[]>
                {
                    ["order"] = [result.ErrorMessage ?? "Unable to create order."]
                })
            };
        }

        var order = result.Data;

        return TypedResults.Created(
            $"/api/orders/{order.OrderId}",
            new CreateOrderResponse(
                order.OrderId,
                order.Status,
                order.Subtotal,
                order.DiscountTotal,
                order.DeliveryFee,
                order.GrandTotal,
                order.ZoneLabel,
                order.CodAllowed));
    }

    private static async Task<Results<Ok<IReadOnlyList<OrderSummaryResponse>>, ValidationProblem>> GetCustomerOrdersAsync(
        ClaimsPrincipal user,
        IOrderManagementService orderManagementService,
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

        var orders = await orderManagementService.GetByCustomerAsync(customerId, cancellationToken);

        return TypedResults.Ok<IReadOnlyList<OrderSummaryResponse>>(orders
            .Select(x => new OrderSummaryResponse(
                x.OrderId,
                x.Status,
                x.PaymentMethod,
                x.PaymentStatus,
                x.GrandTotal,
                x.ZoneLabel,
                x.PlacedAt,
                x.ItemCount))
            .ToList());
    }

    private static async Task<Results<Ok<OrderDetailResponse>, NotFound<string>>> GetOrderByIdAsync(
        Guid orderId,
        ClaimsPrincipal user,
        IOrderManagementService orderManagementService,
        CancellationToken cancellationToken)
    {
        var order = await orderManagementService.GetByIdAsync(orderId, cancellationToken);
        if (order is null || order.CustomerId != GetCustomerId(user))
        {
            return TypedResults.NotFound("Order not found.");
        }

        return TypedResults.Ok(MapOrderDetail(order));
    }

    private static async Task<Results<Ok<OrderDetailResponse>, ValidationProblem, NotFound<string>>> CancelOrderAsync(
        Guid orderId,
        ClaimsPrincipal user,
        CancelOrderRequest request,
        IOrderManagementService orderManagementService,
        CancellationToken cancellationToken)
    {
        var result = await orderManagementService.CancelAsync(
            new CancelOrderCommand(orderId, GetCustomerId(user), request.Reason),
            cancellationToken);

        if (!result.Success || result.Data is null)
        {
            return result.ErrorCode switch
            {
                "order_not_found" => TypedResults.NotFound(result.ErrorMessage ?? "Order not found."),
                _ => TypedResults.ValidationProblem(new Dictionary<string, string[]>
                {
                    ["order"] = [result.ErrorMessage ?? "Unable to cancel order."]
                })
            };
        }

        return TypedResults.Ok(MapOrderDetail(result.Data));
    }

    private static async Task<Results<Ok<InvoiceDetailResponse>, NotFound<string>>> GetOrderInvoiceAsync(
        Guid orderId,
        ClaimsPrincipal user,
        IOrderManagementService orderManagementService,
        IInvoiceService invoiceService,
        CancellationToken cancellationToken)
    {
        var order = await orderManagementService.GetByIdAsync(orderId, cancellationToken);
        if (order is null || order.CustomerId != GetCustomerId(user))
        {
            return TypedResults.NotFound("Invoice not found.");
        }

        var result = await invoiceService.GetOrderInvoiceAsync(orderId, cancellationToken);
        if (!result.Success || result.Data is null)
        {
            return TypedResults.NotFound(result.ErrorMessage ?? "Invoice not found.");
        }

        var invoice = result.Data;
        return TypedResults.Ok(new InvoiceDetailResponse(
            invoice.OrderId,
            invoice.InvoiceNumber,
            invoice.InvoiceIssuedAt,
            invoice.CustomerName,
            invoice.CustomerPhone,
            invoice.AddressLine1,
            invoice.AddressLine2,
            invoice.Area,
            invoice.City,
            invoice.Pincode,
            invoice.PaymentMethod,
            invoice.PaymentStatus,
            invoice.Subtotal,
            invoice.DiscountTotal,
            invoice.DeliveryFee,
            invoice.GrandTotal,
            invoice.Items.Select(x => new InvoiceDetailItemResponse(
                x.ProductName,
                x.VariantLabel,
            x.Quantity,
            x.UnitPrice,
            x.TotalPrice)).ToList()));
    }

    private static Guid GetCustomerId(ClaimsPrincipal user) =>
        Guid.Parse(user.FindFirstValue("customer_id")!);

    private static OrderDetailResponse MapOrderDetail(SafaFoods.Core.Services.Models.OrderDetailResult order) =>
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
}
