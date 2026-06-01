using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;

using SafaFoods.Core.Services;

namespace SafaFoods.Api.Endpoints;

public static class WalletEndpoints
{
    public static RouteGroupBuilder MapWalletEndpoints(this RouteGroupBuilder api)
    {
        var group = api.MapGroup("/me/wallet")
            .WithTags("Customer Wallet")
            .RequireAuthorization("CustomerPolicy");

        group.MapGet("/", GetWalletAsync)
            .WithName("GetCustomerWallet")
            .WithSummary("Returns the customer's wallet balance and recent transactions.");

        return group;
    }

    private static async Task<Ok<WalletDetails>> GetWalletAsync(
        ClaimsPrincipal user,
        IWalletService walletService,
        CancellationToken ct)
    {
        var customerId = Guid.Parse(user.FindFirstValue("customer_id")!);
        var details = await walletService.GetWalletDetailsAsync(customerId, ct);
        return TypedResults.Ok(details);
    }
}
