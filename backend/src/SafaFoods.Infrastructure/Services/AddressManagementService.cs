using Microsoft.EntityFrameworkCore;

using SafaFoods.Core.Domain;
using SafaFoods.Core.Entities;
using SafaFoods.Core.Services;
using SafaFoods.Core.Services.Models;
using SafaFoods.Infrastructure.Persistence;

namespace SafaFoods.Infrastructure.Services;

public sealed class AddressManagementService(
    SafaFoodsDbContext dbContext,
    IDeliveryPricingService deliveryPricingService) : IAddressManagementService
{
    public async Task<ServiceResult<AddressCreationResult>> CreateAsync(
        AddressCreationCommand command,
        CancellationToken cancellationToken = default)
    {
        var (quote, zone) = await ResolveQuoteAndZoneAsync(command.DistanceKm, cancellationToken);

        var customer = await dbContext.Customers.FirstOrDefaultAsync(
            x => x.Phone == command.Phone || x.Email == command.Email,
            cancellationToken);

        if (customer is null)
        {
            customer = new Customer
            {
                FullName = command.FullName,
                Phone = command.Phone,
                Email = command.Email
            };
            dbContext.Customers.Add(customer);
        }
        else
        {
            customer.FullName = command.FullName;
            customer.Phone = command.Phone;
            customer.Email = command.Email;
            customer.UpdatedAt = DateTimeOffset.UtcNow;
        }

        var address = BuildAddress(
            customer.Id,
            command.FullName,
            command.Phone,
            command.AddressLine1,
            command.AddressLine2,
            command.Landmark,
            command.Area,
            command.City,
            command.Pincode,
            command.IsDefault,
            quote,
            zone);

        if (command.IsDefault)
        {
            await ClearExistingDefaultAddressAsync(customer.Id, cancellationToken);
        }

        dbContext.Addresses.Add(address);
        await dbContext.SaveChangesAsync(cancellationToken);

        return ServiceResult<AddressCreationResult>.Ok(new AddressCreationResult(
            address.Id,
            customer.Id,
            quote.Serviceable,
            quote.ZoneLabel,
            quote.Fee,
            quote.DistanceKm));
    }

    public async Task<ServiceResult<AddressCreationResult>> CreateForCustomerAsync(
        CustomerAddressCreationCommand command,
        CancellationToken cancellationToken = default)
    {
        var customer = await dbContext.Customers.FirstOrDefaultAsync(
            x => x.Id == command.CustomerId,
            cancellationToken);

        if (customer is null)
        {
            return ServiceResult<AddressCreationResult>.Fail("customer_not_found", "Customer was not found.");
        }

        customer.UpdatedAt = DateTimeOffset.UtcNow;

        var (quote, zone) = await ResolveQuoteAndZoneAsync(command.DistanceKm, cancellationToken);
        if (command.IsDefault)
        {
            await ClearExistingDefaultAddressAsync(customer.Id, cancellationToken);
        }

        var address = BuildAddress(
            customer.Id,
            command.FullName,
            command.Phone,
            command.AddressLine1,
            command.AddressLine2,
            command.Landmark,
            command.Area,
            command.City,
            command.Pincode,
            command.IsDefault,
            quote,
            zone);

        dbContext.Addresses.Add(address);
        await dbContext.SaveChangesAsync(cancellationToken);

        return ServiceResult<AddressCreationResult>.Ok(new AddressCreationResult(
            address.Id,
            customer.Id,
            quote.Serviceable,
            quote.ZoneLabel,
            quote.Fee,
            quote.DistanceKm));
    }

    private async Task<(DeliveryQuote Quote, DeliveryZone? Zone)> ResolveQuoteAndZoneAsync(
        decimal distanceKm,
        CancellationToken cancellationToken)
    {
        var quote = deliveryPricingService.GetQuote(distanceKm);
        var zone = quote.ZoneLabel is null
            ? null
            : await dbContext.DeliveryZones.FirstOrDefaultAsync(
                x => x.Name == quote.ZoneLabel,
                cancellationToken);
        return (quote, zone);
    }

    private async Task ClearExistingDefaultAddressAsync(Guid customerId, CancellationToken cancellationToken)
    {
        var existingDefaults = await dbContext.Addresses
            .Where(x => x.CustomerId == customerId && x.IsDefault)
            .ToListAsync(cancellationToken);

        foreach (var existingAddress in existingDefaults)
        {
            existingAddress.IsDefault = false;
            existingAddress.UpdatedAt = DateTimeOffset.UtcNow;
        }
    }

    private static Address BuildAddress(
        Guid customerId,
        string fullName,
        string phone,
        string addressLine1,
        string? addressLine2,
        string? landmark,
        string area,
        string city,
        string pincode,
        bool isDefault,
        DeliveryQuote quote,
        DeliveryZone? zone) =>
        new()
        {
            CustomerId = customerId,
            FullName = fullName,
            Phone = phone,
            AddressLine1 = addressLine1,
            AddressLine2 = addressLine2,
            Landmark = landmark,
            Area = area,
            City = city,
            Pincode = pincode,
            DistanceKm = quote.DistanceKm,
            IsDefault = isDefault,
            IsServiceable = quote.Serviceable,
            DeliveryZone = zone
        };
}
