namespace SafaFoods.Core.Domain;

public sealed record DeliverySlab(
    string Label,
    decimal MinKm,
    decimal? MaxKm,
    decimal Fee,
    int EstimatedMinMinutes,
    int EstimatedMaxMinutes);
