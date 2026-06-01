namespace SafaFoods.Api.Contracts.Addresses;

public sealed record CreateAddressResponse(
    Guid AddressId,
    Guid CustomerId,
    bool Serviceable,
    string? ZoneLabel,
    decimal? DeliveryFee,
    decimal DistanceKm);
