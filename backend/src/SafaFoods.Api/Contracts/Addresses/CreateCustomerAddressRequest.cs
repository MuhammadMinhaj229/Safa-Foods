namespace SafaFoods.Api.Contracts.Addresses;

public sealed record CreateCustomerAddressRequest(
    string FullName,
    string Phone,
    string AddressLine1,
    string? AddressLine2,
    string? Landmark,
    string Area,
    string City,
    string Pincode,
    decimal DistanceKm,
    bool IsDefault = false);
