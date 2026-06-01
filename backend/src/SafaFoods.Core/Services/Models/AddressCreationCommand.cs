namespace SafaFoods.Core.Services.Models;

public sealed record AddressCreationCommand(
    string FullName,
    string Phone,
    string Email,
    string AddressLine1,
    string? AddressLine2,
    string? Landmark,
    string Area,
    string City,
    string Pincode,
    decimal DistanceKm,
    bool IsDefault);
