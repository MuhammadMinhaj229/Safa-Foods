namespace SafaFoods.Api.Endpoints;

public static class HealthEndpoints
{
    public static RouteGroupBuilder MapHealthEndpoints(this RouteGroupBuilder api)
    {
        var group = api.MapGroup("/system")
            .WithTags("System");

        group.MapGet("/health", () => TypedResults.Ok(new
        {
            Service = "SafaFoods.Api",
            Status = "Healthy",
            Timestamp = DateTimeOffset.UtcNow
        }))
        .WithName("GetApiHealth")
        .WithSummary("Returns API health metadata.");

        return group;
    }
}
