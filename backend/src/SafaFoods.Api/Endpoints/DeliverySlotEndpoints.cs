using Microsoft.AspNetCore.Mvc;
using SafaFoods.Core.Services;

namespace SafaFoods.Api.Endpoints;

public static class DeliverySlotEndpoints
{
    public static void MapDeliverySlotEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/delivery")
            .WithTags("Delivery Slots");

        group.MapGet("/slots", async (
            [FromQuery] DateTime? date,
            IDeliverySlotService slotService,
            CancellationToken ct) =>
        {
            var targetDate = date ?? DateTime.UtcNow.AddDays(1);
            var slots = await slotService.GetAvailableSlotsAsync(targetDate, ct);
            return TypedResults.Ok(slots);
        });
    }
}
