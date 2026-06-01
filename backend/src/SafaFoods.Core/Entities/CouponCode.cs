using SafaFoods.Core.Enums;

namespace SafaFoods.Core.Entities;

/// <summary>
/// A code that a customer enters at checkout to apply a specific Offer.
/// </summary>
public sealed class CouponCode
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Code { get; set; } // e.g., "WELCOME50"
    public Guid OfferId { get; set; }
    public Offer? Offer { get; set; }
    
    public int? UsageLimit { get; set; } // Total times this coupon can be used across all customers
    public int UsageCount { get; set; } = 0;
    
    public int? UsageLimitPerCustomer { get; set; } = 1; // e.g., use once per customer
    
    public bool IsActive { get; set; } = true;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}
