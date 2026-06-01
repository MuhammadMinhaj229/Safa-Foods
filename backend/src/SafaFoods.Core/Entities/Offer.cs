using SafaFoods.Core.Enums;

namespace SafaFoods.Core.Entities;

/// <summary>
/// A global or targeted offer (e.g., Free Delivery over ₹500, Flat 10% off).
/// </summary>
public sealed class Offer
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required string OfferType { get; set; } // e.g., "flat_discount", "percent_discount", "free_delivery"
    public decimal DiscountValue { get; set; }
    public decimal MinimumOrderValue { get; set; } = 0;
    public decimal? MaximumDiscount { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTimeOffset? StartsAt { get; set; }
    public DateTimeOffset? EndsAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}
