using SafaFoods.Core.Services.Models;

namespace SafaFoods.Core.Services;

public interface ICartService
{
    /// <summary>Returns the active cart for a customer, creating one if it doesn't exist.</summary>
    Task<CartResult> GetOrCreateCartAsync(Guid customerId, CancellationToken ct = default);

    /// <summary>Adds a product variant to the cart. If the item already exists, increments quantity.</summary>
    Task<ServiceResult<CartResult>> AddItemAsync(Guid customerId, Guid productId, Guid variantId, int quantity, CancellationToken ct = default);

    /// <summary>Updates the quantity of a specific cart item. Removes it if quantity is 0.</summary>
    Task<ServiceResult<CartResult>> UpdateItemQuantityAsync(Guid customerId, Guid cartItemId, int quantity, CancellationToken ct = default);

    /// <summary>Removes a specific item from the cart.</summary>
    Task<ServiceResult<CartResult>> RemoveItemAsync(Guid customerId, Guid cartItemId, CancellationToken ct = default);

    /// <summary>Removes all items from the cart.</summary>
    Task ClearCartAsync(Guid customerId, CancellationToken ct = default);

    /// <summary>Applies a coupon code to the cart.</summary>
    Task<ServiceResult<CartResult>> ApplyCouponAsync(Guid customerId, string couponCode, CancellationToken ct = default);

    /// <summary>Removes any applied coupon code from the cart.</summary>
    Task<ServiceResult<CartResult>> RemoveCouponAsync(Guid customerId, CancellationToken ct = default);
}
