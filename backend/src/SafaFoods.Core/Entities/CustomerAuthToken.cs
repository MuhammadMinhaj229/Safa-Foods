namespace SafaFoods.Core.Entities;

/// <summary>
/// A temporary OTP code issued to a customer for phone-based authentication.
/// </summary>
public sealed class CustomerAuthToken
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Phone { get; set; }
    public required string OtpCode { get; set; }
    public DateTimeOffset ExpiresAt { get; set; }
    public bool Used { get; set; } = false;
    public int AttemptCount { get; set; } = 0;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}
