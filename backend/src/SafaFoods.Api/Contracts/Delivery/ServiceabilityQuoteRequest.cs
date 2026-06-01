namespace SafaFoods.Api.Contracts.Delivery;

public sealed record ServiceabilityQuoteRequest(
    decimal? DistanceKm,
    decimal? Latitude,
    decimal? Longitude);
