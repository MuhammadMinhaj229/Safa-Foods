using Microsoft.AspNetCore.Http.HttpResults;

using SafaFoods.Api.Contracts.Addresses;
using SafaFoods.Core.Services;
using SafaFoods.Core.Services.Models;

namespace SafaFoods.Api.Endpoints;

public static class AddressEndpoints
{
    public static RouteGroupBuilder MapAddressEndpoints(this RouteGroupBuilder api)
    {
        var group = api.MapGroup("/addresses")
            .WithTags("Addresses");

        group.MapPost("/", CreateAddressAsync)
            .WithName("CreateAddress")
            .WithSummary("Creates a guest customer record (or reuses by phone/email) and saves an address.");

        return group;
    }

    private static async Task<Results<Created<CreateAddressResponse>, ValidationProblem>> CreateAddressAsync(
        CreateAddressRequest request,
        IAddressManagementService addressManagementService,
        CancellationToken cancellationToken)
    {
        var errors = new Dictionary<string, string[]>();

        if (string.IsNullOrWhiteSpace(request.FullName))
            errors["fullName"] = ["Full name is required."];
        if (string.IsNullOrWhiteSpace(request.Phone))
            errors["phone"] = ["Phone is required."];
        if (string.IsNullOrWhiteSpace(request.Email))
            errors["email"] = ["Email is required."];
        if (string.IsNullOrWhiteSpace(request.AddressLine1))
            errors["addressLine1"] = ["Address line 1 is required."];
        if (string.IsNullOrWhiteSpace(request.Area))
            errors["area"] = ["Area is required."];
        if (string.IsNullOrWhiteSpace(request.City))
            errors["city"] = ["City is required."];
        if (string.IsNullOrWhiteSpace(request.Pincode))
            errors["pincode"] = ["Pincode is required."];
        if (request.DistanceKm < 0)
            errors["distanceKm"] = ["Distance must be zero or greater."];

        if (errors.Count > 0)
        {
            return TypedResults.ValidationProblem(errors);
        }

        var result = await addressManagementService.CreateAsync(
            new AddressCreationCommand(
                request.FullName,
                request.Phone,
                request.Email,
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
            $"/api/addresses/{createdAddress.AddressId}",
            new CreateAddressResponse(
                createdAddress.AddressId,
                createdAddress.CustomerId,
                createdAddress.Serviceable,
                createdAddress.ZoneLabel,
                createdAddress.DeliveryFee,
                createdAddress.DistanceKm));
    }
}
