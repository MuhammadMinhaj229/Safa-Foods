namespace SafaFoods.Api.Contracts.Auth;

/**
 * High-Scale Identity Contracts
 * Supports flexible authentication via Phone or Email (Identifier).
 */
public sealed record RequestOtpRequest(string Identifier);

public sealed record VerifyOtpRequest(string Identifier, string OtpCode, string? DeviceHint = null);

public sealed record RefreshTokenRequest(string RefreshToken);

public sealed record AuthTokenResponse(
    string AccessToken, 
    string RefreshToken, 
    Guid CustomerId, 
    int ExpiresInSeconds);

public sealed record RevokeTokenRequest(string RefreshToken);
