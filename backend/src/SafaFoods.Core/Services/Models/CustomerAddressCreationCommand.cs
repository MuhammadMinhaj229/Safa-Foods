namespace SafaFoods.Core.Services.Models;

public sealed record CustomerAddressCreationCommand(
    Guid CustomerId,
    string FullName,
    string Phone,
    string AddressLine1,
    string? AddressLine2,
    string? Landmark,
    string Area,
    string City,
    string Pincode,
    decimal DistanceKm,
    bool IsDefault);
