using SafaFoods.Core.Services.Models;

namespace SafaFoods.Core.Services;

public interface ICustomerAuthService
{
    /// <summary>Generates and sends a 6-digit OTP to the identifier (Phone or Email).</summary>
    Task<RequestOtpResult> RequestOtpAsync(string identifier, CancellationToken ct = default);

    /// <summary>Verifies the OTP code and returns JWT access + refresh tokens.</summary>
    Task<VerifyOtpResult> VerifyOtpAsync(string identifier, string otpCode, string? deviceHint = null, CancellationToken ct = default);

    /// <summary>Issues a new access token from a valid refresh token.</summary>
    Task<RefreshTokenResult> RefreshTokenAsync(string refreshToken, CancellationToken ct = default);

    /// <summary>Revokes the session associated with this refresh token (logout).</summary>
    Task RevokeSessionAsync(string refreshToken, CancellationToken ct = default);
}
