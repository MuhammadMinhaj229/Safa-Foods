using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SafaFoods.Api.Contracts.Addresses;
using SafaFoods.Core.Services;
using SafaFoods.Core.Services.Models;
using SafaFoods.Infrastructure.Persistence;

namespace SafaFoods.Api.Endpoints;

public static class CustomerProfileEndpoints
{
    public static RouteGroupBuilder MapCustomerProfileEndpoints(this RouteGroupBuilder api)
    {
        var group = api.MapGroup("/me")
            .WithTags("Customer Profile")
            .RequireAuthorization("CustomerPolicy");

        group.MapGet("/profile", GetProfileAsync)
            .WithName("GetCustomerProfile")
            .WithSummary("Returns the authenticated customer's profile.");

        group.MapGet("/orders", GetOrdersAsync)
            .WithName("GetMyOrders")
            .WithSummary("Returns the customer's order history.");

        group.MapGet("/subscriptions", GetSubscriptionsAsync)
            .WithName("GetMySubscriptions")
            .WithSummary("Returns the customer's active subscriptions.");

        group.MapGet("/addresses", GetAddressesAsync)
            .WithName("GetMyAddresses")
            .WithSummary("Returns the customer's saved addresses.");

        group.MapPost("/addresses", CreateAddressAsync)
            .WithName("CreateMyAddress")
            .WithSummary("Creates a saved address for the authenticated customer.");

        return group;
    }

    private static Guid GetCustomerId(ClaimsPrincipal user) =>
        Guid.Parse(user.FindFirstValue("customer_id")!);

    private static async Task<Results<Ok<CustomerProfileResponse>, NotFound>> GetProfileAsync(
        ClaimsPrincipal user,
        SafaFoodsDbContext db,
        CancellationToken ct)
    {
        var id = GetCustomerId(user);
        var customer = await db.Customers.FindAsync([id], ct);
        if (customer is null) return TypedResults.NotFound();

        return TypedResults.Ok(new CustomerProfileResponse(
            customer.Id, customer.FullName, customer.Phone, customer.Email,
            customer.ReferralCode, customer.CreatedAt));
    }

    private static async Task<Ok<IReadOnlyList<CustomerOrderSummaryResponse>>> GetOrdersAsync(
        ClaimsPrincipal user,
        SafaFoodsDbContext db,
        CancellationToken ct)
    {
        var id = GetCustomerId(user);
        var orders = await db.Orders
            .Where(o => o.CustomerId == id)
            .OrderByDescending(o => o.PlacedAt)
            .Select(o => new CustomerOrderSummaryResponse(
                o.Id, o.Status.ToString(), o.GrandTotal, o.PaymentStatus.ToString(),
                o.PlacedAt, o.Items.Count))
            .ToListAsync(ct);

        return TypedResults.Ok(orders as IReadOnlyList<CustomerOrderSummaryResponse>);
    }

    private static async Task<Ok<IReadOnlyList<CustomerSubscriptionSummaryResponse>>> GetSubscriptionsAsync(
        ClaimsPrincipal user,
        SafaFoodsDbContext db,
        CancellationToken ct)
    {
        var id = GetCustomerId(user);
        var subs = await db.Subscriptions
            .Include(s => s.Product)
            .Where(s => s.CustomerId == id)
            .OrderByDescending(s => s.StartDate)
            .Select(s => new CustomerSubscriptionSummaryResponse(
                s.Id, s.Product.Name, s.DeliveriesInCycle, s.Status.ToString(),
                s.StartDate, s.NextDeliveryDate))
            .ToListAsync(ct);

        return TypedResults.Ok(subs as IReadOnlyList<CustomerSubscriptionSummaryResponse>);
    }

    private static async Task<Ok<IReadOnlyList<CustomerAddressResponse>>> GetAddressesAsync(
        ClaimsPrincipal user,
        SafaFoodsDbContext db,
        CancellationToken ct)
    {
        var id = GetCustomerId(user);
        var addresses = await db.Addresses
            .Where(a => a.CustomerId == id)
            .Select(a => new CustomerAddressResponse(
                a.Id, a.Landmark, a.AddressLine1, a.AddressLine2, a.City, a.Area, a.Pincode,
                a.Latitude, a.Longitude, a.IsDefault))
            .ToListAsync(ct);

        return TypedResults.Ok(addresses as IReadOnlyList<CustomerAddressResponse>);
    }

    private static async Task<Results<Created<CreateAddressResponse>, ValidationProblem>> CreateAddressAsync(
        ClaimsPrincipal user,
        CreateCustomerAddressRequest request,
        IAddressManagementService addressManagementService,
        CancellationToken cancellationToken)
    {
        var errors = ValidateAddressRequest(
            request.FullName,
            request.Phone,
            request.AddressLine1,
            request.Area,
            request.City,
            request.Pincode,
            request.DistanceKm);

        if (errors.Count > 0)
        {
            return TypedResults.ValidationProblem(errors);
        }

        var customerId = GetCustomerId(user);

        var result = await addressManagementService.CreateForCustomerAsync(
            new CustomerAddressCreationCommand(
                customerId,
                request.FullName,
                request.Phone,
                request.AddressLine1,
                request.AddressLine2,
                request.Landmark,
                request.Area,
                request.City,
                request.Pincode,
                request.DistanceKm,
                request.IsDefault),
            cancellationToken);

        if (!result.Success || result.Data is null)
        {
            return TypedResults.ValidationProblem(new Dictionary<string, string[]>
            {
                ["address"] = [result.ErrorMessage ?? "Unable to create address."]
            });
        }

        var createdAddress = result.Data;

        return TypedResults.Created(
            $"/api/me/addresses/{createdAddress.AddressId}",
            new CreateAddressResponse(
                createdAddress.AddressId,
                createdAddress.CustomerId,
                createdAddress.Serviceable,
                createdAddress.ZoneLabel,
                createdAddress.DeliveryFee,
                createdAddress.DistanceKm));
    }

    private static Dictionary<string, string[]> ValidateAddressRequest(
        string fullName,
        string phone,
        string addressLine1,
        string area,
        string city,
        string pincode,
        decimal distanceKm)
    {
        var errors = new Dictionary<string, string[]>();

        if (string.IsNullOrWhiteSpace(fullName))
            errors["fullName"] = ["Full name is required."];
        if (string.IsNullOrWhiteSpace(phone))
            errors["phone"] = ["Phone is required."];
        if (string.IsNullOrWhiteSpace(addressLine1))
            errors["addressLine1"] = ["Address line 1 is required."];
        if (string.IsNullOrWhiteSpace(area))
            errors["area"] = ["Area is required."];
        if (string.IsNullOrWhiteSpace(city))
            errors["city"] = ["City is required."];
        if (string.IsNullOrWhiteSpace(pincode))
            errors["pincode"] = ["Pincode is required."];
        if (distanceKm < 0)
            errors["distanceKm"] = ["Distance must be zero or greater."];

        return errors;
    }
}

// ── Response Types ─────────────────────────────────────────────────────────
public sealed record CustomerProfileResponse(
    Guid CustomerId,
    string FullName,
    string Phone,
    string Email,
    string? ReferralCode,
    DateTimeOffset MemberSince);

public sealed record CustomerOrderSummaryResponse(
    Guid OrderId,
    string Status,
    decimal GrandTotal,
    string PaymentStatus,
    DateTimeOffset PlacedAt,
    int ItemCount);

public sealed record CustomerSubscriptionSummaryResponse(
    Guid SubscriptionId,
    string ProductName,
    int DeliveriesInCycle,
    string Status,
    DateTimeOffset StartDate,
    DateTimeOffset? NextDeliveryDate);

public sealed record CustomerAddressResponse(
    Guid AddressId,
    string? Label,
    string Line1,
    string? Line2,
    string City,
    string State,
    string PinCode,
    decimal? Latitude,
    decimal? Longitude,
    bool IsDefault);
