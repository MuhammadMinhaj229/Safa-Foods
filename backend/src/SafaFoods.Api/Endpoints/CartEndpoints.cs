using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;

using SafaFoods.Api.Contracts.Cart;
using SafaFoods.Core.Services;

namespace SafaFoods.Api.Endpoints;

public static class CartEndpoints
{
    public static RouteGroupBuilder MapCartEndpoints(this RouteGroupBuilder api)
    {
        var group = api.MapGroup("/cart")
            .WithTags("Cart")
            .RequireAuthorization("CustomerPolicy");

        group.MapGet("/", GetCartAsync)
            .WithName("GetCart")
            .WithSummary("Returns the current customer's cart.");

        group.MapPost("/items", AddItemAsync)
            .WithName("AddCartItem")
            .WithSummary("Adds a product variant to the cart.");

        group.MapPut("/items/{itemId:guid}", UpdateItemAsync)
            .WithName("UpdateCartItem")
            .WithSummary("Updates the quantity of a cart item.");

        group.MapDelete("/items/{itemId:guid}", RemoveItemAsync)
            .WithName("RemoveCartItem")
            .WithSummary("Removes an item from the cart.");

        group.MapDelete("/", ClearCartAsync)
            .WithName("ClearCart")
            .WithSummary("Removes all items from the cart.");

        group.MapPost("/coupon", ApplyCouponAsync)
            .WithName("ApplyCartCoupon")
            .WithSummary("Applies a promotional coupon to the cart.");

        group.MapDelete("/coupon", RemoveCouponAsync)
            .WithName("RemoveCartCoupon")
            .WithSummary("Removes the applied coupon from the cart.");

        return group;
    }

    private static Guid GetCustomerId(ClaimsPrincipal user) =>
        Guid.Parse(user.FindFirstValue("customer_id")!);

    private static async Task<Ok<CartResponse>> GetCartAsync(
        ClaimsPrincipal user,
        ICartService cartService,
        CancellationToken ct)
    {
        var result = await cartService.GetOrCreateCartAsync(GetCustomerId(user), ct);
        return TypedResults.Ok(MapCart(result));
    }

    private static async Task<Results<Ok<CartResponse>, ValidationProblem>> AddItemAsync(
        ClaimsPrincipal user,
        AddCartItemRequest request,
        ICartService cartService,
        CancellationToken ct)
    {
        var result = await cartService.AddItemAsync(GetCustomerId(user), request.ProductId, request.VariantId, request.Quantity, ct);
        if (!result.Success)
            return TypedResults.ValidationProblem(new Dictionary<string, string[]>
                { ["cart"] = [result.ErrorMessage ?? "Unable to add item."] });

        return TypedResults.Ok(MapCart(result.Data!));
    }

    private static async Task<Results<Ok<CartResponse>, ValidationProblem, NotFound<string>>> UpdateItemAsync(
        Guid itemId,
        ClaimsPrincipal user,
        UpdateCartItemRequest request,
        ICartService cartService,
        CancellationToken ct)
    {
        var result = await cartService.UpdateItemQuantityAsync(GetCustomerId(user), itemId, request.Quantity, ct);
        if (!result.Success)
            return result.ErrorCode == "item_not_found"
                ? TypedResults.NotFound("Cart item not found.")
                : TypedResults.ValidationProblem(new Dictionary<string, string[]>
                    { ["cart"] = [result.ErrorMessage ?? "Unable to update item."] });

        return TypedResults.Ok(MapCart(result.Data!));
    }

    private static async Task<Results<Ok<CartResponse>, NotFound<string>>> RemoveItemAsync(
        Guid itemId,
        ClaimsPrincipal user,
        ICartService cartService,
        CancellationToken ct)
    {
        var result = await cartService.RemoveItemAsync(GetCustomerId(user), itemId, ct);
        if (!result.Success)
            return TypedResults.NotFound("Cart item not found.");

        return TypedResults.Ok(MapCart(result.Data!));
    }

    private static async Task<Ok> ClearCartAsync(
        ClaimsPrincipal user,
        ICartService cartService,
        CancellationToken ct)
    {
        await cartService.ClearCartAsync(GetCustomerId(user), ct);
        return TypedResults.Ok();
    }

    private static async Task<Results<Ok<CartResponse>, ValidationProblem>> ApplyCouponAsync(
        ClaimsPrincipal user,
        ApplyCouponRequest request,
        ICartService cartService,
        CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.CouponCode))
            return TypedResults.ValidationProblem(new Dictionary<string, string[]> { ["coupon"] = ["Coupon code is required."] });

        var result = await cartService.ApplyCouponAsync(GetCustomerId(user), request.CouponCode.Trim(), ct);
        if (!result.Success)
            return TypedResults.ValidationProblem(new Dictionary<string, string[]> { ["coupon"] = [result.ErrorMessage ?? "Invalid coupon."] });

        return TypedResults.Ok(MapCart(result.Data!));
    }

    private static async Task<Results<Ok<CartResponse>, ValidationProblem>> RemoveCouponAsync(
        ClaimsPrincipal user,
        ICartService cartService,
        CancellationToken ct)
    {
        var result = await cartService.RemoveCouponAsync(GetCustomerId(user), ct);
        if (!result.Success)
            return TypedResults.ValidationProblem(new Dictionary<string, string[]> { ["cart"] = [result.ErrorMessage ?? "Unable to remove coupon."] });

        return TypedResults.Ok(MapCart(result.Data!));
    }

    private static CartResponse MapCart(Core.Services.Models.CartResult cart) =>
        new(cart.CartId,
            cart.CustomerId,
            cart.Items.Select(i => new CartItemResponse(
                i.ItemId, i.ProductId, i.VariantId, i.ProductName, i.VariantLabel,
                i.ImageUrl, i.Quantity, i.UnitPrice, i.LineTotal, i.InStock)).ToList(),
            cart.Subtotal,
            cart.TotalItems,
            cart.AppliedCouponCode,
            cart.DiscountTotal,
            cart.GrandTotal);
}
