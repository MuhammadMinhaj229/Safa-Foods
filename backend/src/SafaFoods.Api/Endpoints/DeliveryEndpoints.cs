using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;

using SafaFoods.Api.Contracts.Delivery;
using SafaFoods.Core.Options;
using SafaFoods.Core.Services;

namespace SafaFoods.Api.Endpoints;

public static class DeliveryEndpoints
{
    public static RouteGroupBuilder MapDeliveryEndpoints(this RouteGroupBuilder api)
    {
        var group = api.MapGroup("/delivery")
            .WithTags("Delivery");

        group.MapPost("/serviceability", HandleServiceabilityQuote)
            .WithName("GetServiceabilityQuote")
            .WithSummary("Calculates delivery serviceability and fee.")
            .WithDescription("Uses the slab-based Safa Foods MVP delivery rules.");

        return group;
    }

    private static Results<Ok<ServiceabilityQuoteResponse>, ValidationProblem> HandleServiceabilityQuote(
        ServiceabilityQuoteRequest request,
        IDeliveryPricingService deliveryPricingService,
        IOptions<ShopOptions> shopOptions)
    {
        decimal distanceKm = 0;

        if (request.DistanceKm.HasValue)
        {
            distanceKm = request.DistanceKm.Value;
        }
        else if (request.Latitude.HasValue && request.Longitude.HasValue)
        {
            distanceKm = CalculateHaversineDistance(
                shopOptions.Value.Latitude,
                shopOptions.Value.Longitude,
                request.Latitude.Value,
                request.Longitude.Value);
        }
        else
        {
            return TypedResults.ValidationProblem(new Dictionary<string, string[]>
            {
                ["Validation"] = ["Either DistanceKm or (Latitude and Longitude) must be provided."]
            });
        }

        if (distanceKm < 0)
        {
            return TypedResults.ValidationProblem(new Dictionary<string, string[]>
            {
                ["distanceKm"] = ["Distance must be zero or greater."]
            });
        }

        var quote = deliveryPricingService.GetQuote(distanceKm);

        return TypedResults.Ok(new ServiceabilityQuoteResponse(
            quote.Serviceable,
            quote.ZoneLabel,
            quote.Fee,
            quote.DistanceKm,
            quote.EstimatedMinMinutes,
            quote.EstimatedMaxMinutes));
    }

    private static decimal CalculateHaversineDistance(decimal lat1, decimal lon1, decimal lat2, decimal lon2)
    {
        var r = 6371; // radius of Earth in km
        var dLat = ToRadians((double)(lat2 - lat1));
        var dLon = ToRadians((double)(lon2 - lon1));
        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(ToRadians((double)lat1)) * Math.Cos(ToRadians((double)lat2)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return (decimal)(r * c);
    }

    private static double ToRadians(double angleStr) => (Math.PI / 180) * angleStr;
}
