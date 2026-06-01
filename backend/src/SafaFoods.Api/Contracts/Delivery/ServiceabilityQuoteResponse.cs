namespace SafaFoods.Api.Contracts.Delivery;

public sealed record ServiceabilityQuoteResponse(
    bool Serviceable,
    string? ZoneLabel,
    decimal? Fee,
    decimal DistanceKm,
    int? EstimatedMinMinutes,
    int? EstimatedMaxMinutes);
