using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;
using SafaFoods.Api.Contracts.Store;
using SafaFoods.Core.Options;

namespace SafaFoods.Api.Endpoints;

public static class StoreEndpoints
{
    public static RouteGroupBuilder MapStoreEndpoints(this RouteGroupBuilder api)
    {
        var group = api.MapGroup("/store")
            .WithTags("Store");

        group.MapGet("/config", GetStoreConfig)
            .WithName("GetStoreConfig")
            .WithSummary("Returns the public store configuration including trust and compliance metadata.");

        return group;
    }

    private static Ok<StoreConfigurationResponse> GetStoreConfig(IOptions<ShopOptions> options)
    {
        var shop = options.Value;
        
        return TypedResults.Ok(new StoreConfigurationResponse(
            shop.Name,
            shop.City,
            shop.ServiceArea,
            shop.AddressHint,
            shop.Latitude,
            shop.Longitude,
            shop.FssaiLicense,
            shop.GstNumber,
            shop.SupportEmail,
            shop.ReturnPolicyUrl,
            shop.TermsOfServiceUrl
        ));
    }
}
