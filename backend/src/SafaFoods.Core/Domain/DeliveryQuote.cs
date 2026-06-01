namespace SafaFoods.Core.Domain;

public sealed record DeliveryQuote(
    bool Serviceable,
    string? ZoneLabel,
    decimal? Fee,
    decimal DistanceKm,
    int? EstimatedMinMinutes,
    int? EstimatedMaxMinutes);
