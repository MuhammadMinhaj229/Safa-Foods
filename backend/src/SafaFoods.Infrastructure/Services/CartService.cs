using Microsoft.EntityFrameworkCore;

using SafaFoods.Core.Entities;
using SafaFoods.Core.Services;
using SafaFoods.Core.Services.Models;
using SafaFoods.Infrastructure.Persistence;

namespace SafaFoods.Infrastructure.Services;

public sealed class CartService(SafaFoodsDbContext dbContext, IPromotionsService promotionsService) : ICartService
{
    public async Task<CartResult> GetOrCreateCartAsync(Guid customerId, CancellationToken ct = default)
    {
        var cart = await GetCartWithItemsAsync(customerId, ct);
        if (cart is null)
        {
            cart = new Cart { CustomerId = customerId };
            dbContext.Carts.Add(cart);
            await dbContext.SaveChangesAsync(ct);
            cart = await GetCartWithItemsAsync(customerId, ct) ?? cart;
        }
        return await MapCartAsync(cart, ct);
    }

    public async Task<ServiceResult<CartResult>> AddItemAsync(Guid customerId, Guid productId, Guid variantId, int quantity, CancellationToken ct = default)
    {
        if (quantity <= 0)
            return ServiceResult<CartResult>.Fail("invalid_quantity", "Quantity must be at least 1.");

        var variant = await dbContext.ProductVariants
            .Include(v => v.Product)
            .FirstOrDefaultAsync(v => v.Id == variantId && v.ProductId == productId && v.IsActive, ct);

        if (variant is null)
            return ServiceResult<CartResult>.Fail("variant_not_found", "Product variant not found or unavailable.");

        if (variant.TrackInventory && variant.StockQty < quantity)
            return ServiceResult<CartResult>.Fail("out_of_stock", $"Only {variant.StockQty} units available.");

        var cart = await GetCartWithItemsAsync(customerId, ct);
        if (cart is null)
        {
            cart = new Cart { CustomerId = customerId };
            dbContext.Carts.Add(cart);
            await dbContext.SaveChangesAsync(ct);
        }

        var existingItem = cart.Items.FirstOrDefault(i => i.VariantId == variantId);
        if (existingItem is not null)
        {
            existingItem.Quantity += quantity;
            if (variant.TrackInventory && existingItem.Quantity > variant.StockQty)
                return ServiceResult<CartResult>.Fail("out_of_stock", $"Cannot add that many. Only {variant.StockQty} units in stock.");
        }
        else
        {
            cart.Items.Add(new CartItem
            {
                CartId = cart.Id,
                ProductId = productId,
                VariantId = variantId,
                Quantity = quantity
            });
        }

        cart.UpdatedAt = DateTimeOffset.UtcNow;
        await dbContext.SaveChangesAsync(ct);

        return ServiceResult<CartResult>.Ok(await MapCartAsync(await GetCartWithItemsAsync(customerId, ct) ?? cart, ct));
    }

    public async Task<ServiceResult<CartResult>> UpdateItemQuantityAsync(Guid customerId, Guid cartItemId, int quantity, CancellationToken ct = default)
    {
        var cart = await GetCartWithItemsAsync(customerId, ct);
        if (cart is null)
            return ServiceResult<CartResult>.Fail("cart_not_found", "Cart not found.");

        var item = cart.Items.FirstOrDefault(i => i.Id == cartItemId);
        if (item is null)
            return ServiceResult<CartResult>.Fail("item_not_found", "Cart item not found.");

        if (quantity <= 0)
        {
            cart.Items.Remove(item);
            dbContext.CartItems.Remove(item);
        }
        else
        {
            item.Quantity = quantity;
        }

        cart.UpdatedAt = DateTimeOffset.UtcNow;
        await dbContext.SaveChangesAsync(ct);

        return ServiceResult<CartResult>.Ok(await MapCartAsync(await GetCartWithItemsAsync(customerId, ct) ?? cart, ct));
    }

    public async Task<ServiceResult<CartResult>> RemoveItemAsync(Guid customerId, Guid cartItemId, CancellationToken ct = default)
    {
        var cart = await GetCartWithItemsAsync(customerId, ct);
        if (cart is null)
            return ServiceResult<CartResult>.Fail("cart_not_found", "Cart not found.");

        var item = cart.Items.FirstOrDefault(i => i.Id == cartItemId);
        if (item is null)
            return ServiceResult<CartResult>.Fail("item_not_found", "Cart item not found.");

        cart.Items.Remove(item);
        dbContext.CartItems.Remove(item);
        cart.UpdatedAt = DateTimeOffset.UtcNow;
        await dbContext.SaveChangesAsync(ct);

        return ServiceResult<CartResult>.Ok(await MapCartAsync(await GetCartWithItemsAsync(customerId, ct) ?? cart, ct));
    }

    public async Task ClearCartAsync(Guid customerId, CancellationToken ct = default)
    {
        var cart = await GetCartWithItemsAsync(customerId, ct);
        if (cart is null) return;

        dbContext.CartItems.RemoveRange(cart.Items);
        cart.UpdatedAt = DateTimeOffset.UtcNow;
        await dbContext.SaveChangesAsync(ct);
    }

    public async Task<ServiceResult<CartResult>> ApplyCouponAsync(Guid customerId, string couponCode, CancellationToken ct = default)
    {
        var cart = await GetCartWithItemsAsync(customerId, ct);
        if (cart is null)
            return ServiceResult<CartResult>.Fail("cart_not_found", "Cart not found.");

        var subtotal = cart.Items.Sum(i => (i.Variant.SalePrice ?? i.Variant.Price) * i.Quantity);
        var eval = await promotionsService.EvaluateCouponAsync(couponCode, customerId, subtotal, ct);

        if (!eval.Success)
            return ServiceResult<CartResult>.Fail("coupon_invalid", eval.ErrorMessage ?? "Invalid coupon.");

        cart.CouponCode = couponCode;
        cart.UpdatedAt = DateTimeOffset.UtcNow;
        await dbContext.SaveChangesAsync(ct);

        return ServiceResult<CartResult>.Ok(await MapCartAsync(cart, ct));
    }

    public async Task<ServiceResult<CartResult>> RemoveCouponAsync(Guid customerId, CancellationToken ct = default)
    {
        var cart = await GetCartWithItemsAsync(customerId, ct);
        if (cart is null)
            return ServiceResult<CartResult>.Fail("cart_not_found", "Cart not found.");

        cart.CouponCode = null;
        cart.UpdatedAt = DateTimeOffset.UtcNow;
        await dbContext.SaveChangesAsync(ct);

        return ServiceResult<CartResult>.Ok(await MapCartAsync(cart, ct));
    }

    private async Task<Cart?> GetCartWithItemsAsync(Guid customerId, CancellationToken ct) =>
        await dbContext.Carts
            .Include(c => c.Items)
                .ThenInclude(i => i.Variant)
            .Include(c => c.Items)
                .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.CustomerId == customerId, ct);

    private async Task<CartResult> MapCartAsync(Cart cart, CancellationToken ct)
    {
        var items = cart.Items.Select(i =>
        {
            var price = i.Variant.SalePrice ?? i.Variant.Price;
            var inStock = !i.Variant.TrackInventory || i.Variant.StockQty > 0;
            return new CartItemResult(
                i.Id,
                i.ProductId,
                i.VariantId,
                i.Product.Name,
                i.Variant.Label,
                i.Product.ImageUrl,
                i.Quantity,
                price,
                price * i.Quantity,
                inStock);
        }).ToList();

        var subtotal = items.Sum(i => i.LineTotal);
        decimal discount = 0;
        string? appliedCoupon = null;

        if (!string.IsNullOrWhiteSpace(cart.CouponCode))
        {
            var eval = await promotionsService.EvaluateCouponAsync(cart.CouponCode, cart.CustomerId, subtotal, ct);
            if (eval.Success)
            {
                appliedCoupon = cart.CouponCode;
                discount = eval.ExpectedDiscount;
            }
        }

        var grandTotal = Math.Max(0, subtotal - discount);

        return new CartResult(
            cart.Id,
            cart.CustomerId,
            items,
            subtotal,
            items.Sum(i => i.Quantity),
            appliedCoupon,
            discount,
            grandTotal);
    }
}
