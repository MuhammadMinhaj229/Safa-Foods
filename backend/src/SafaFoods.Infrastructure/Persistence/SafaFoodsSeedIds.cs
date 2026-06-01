namespace SafaFoods.Infrastructure.Persistence;

public static class SafaFoodsSeedIds
{
    public static readonly DateTimeOffset SeedTimestamp = new(2026, 4, 6, 9, 0, 0, TimeSpan.Zero);

    public static readonly Guid ZoneA = Guid.Parse("4f9f0f95-7408-44ad-a6f2-6f30f3e9a101");
    public static readonly Guid ZoneB = Guid.Parse("4f9f0f95-7408-44ad-a6f2-6f30f3e9a102");
    public static readonly Guid ZoneC = Guid.Parse("4f9f0f95-7408-44ad-a6f2-6f30f3e9a103");

    public static readonly Guid ProductGingerGarlic = Guid.Parse("7f8f0f95-7408-44ad-a6f2-6f30f3e9a201");
    public static readonly Guid ProductGarlic = Guid.Parse("7f8f0f95-7408-44ad-a6f2-6f30f3e9a202");
    public static readonly Guid ProductGreenChilli = Guid.Parse("7f8f0f95-7408-44ad-a6f2-6f30f3e9a203");

    public static readonly Guid VariantGingerGarlic200 = Guid.Parse("8f8f0f95-7408-44ad-a6f2-6f30f3e9a301");
    public static readonly Guid VariantGingerGarlic500 = Guid.Parse("8f8f0f95-7408-44ad-a6f2-6f30f3e9a302");
    public static readonly Guid VariantGarlic200 = Guid.Parse("8f8f0f95-7408-44ad-a6f2-6f30f3e9a303");
    public static readonly Guid VariantGarlic500 = Guid.Parse("8f8f0f95-7408-44ad-a6f2-6f30f3e9a304");
    public static readonly Guid VariantGreenChilli150 = Guid.Parse("8f8f0f95-7408-44ad-a6f2-6f30f3e9a305");
    public static readonly Guid VariantGreenChilli300 = Guid.Parse("8f8f0f95-7408-44ad-a6f2-6f30f3e9a306");
    
    public static readonly Guid SlotMorning = Guid.Parse("9f8f0f95-7408-44ad-a6f2-6f30f3e9a401");
    public static readonly Guid SlotAfternoon = Guid.Parse("9f8f0f95-7408-44ad-a6f2-6f30f3e9a402");
    public static readonly Guid SlotEvening = Guid.Parse("9f8f0f95-7408-44ad-a6f2-6f30f3e9a403");
}
