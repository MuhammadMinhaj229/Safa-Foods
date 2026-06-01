namespace SafaFoods.Core.Services.Models;

public sealed record AddressCreationResult(
    Guid AddressId,
    Guid CustomerId,
    bool Serviceable,
    string? ZoneLabel,
    decimal? DeliveryFee,
    decimal DistanceKm);
