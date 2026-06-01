namespace SafaFoods.Core.Entities;

/// <summary>
/// An active JWT-backed session for a logged-in customer.
/// </summary>
public sealed class CustomerSession
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CustomerId { get; set; }
    public required string RefreshTokenHash { get; set; }
    public DateTimeOffset ExpiresAt { get; set; }
    public bool Revoked { get; set; } = false;
    public string? DeviceHint { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public Customer Customer { get; set; } = null!;
}
