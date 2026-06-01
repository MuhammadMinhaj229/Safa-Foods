namespace SafaFoods.Core.Services.Models;

public sealed record RequestOtpResult(bool Success, string? ErrorMessage = null);

public sealed record VerifyOtpResult(
    bool Success,
    string? AccessToken = null,
    string? RefreshToken = null,
    Guid? CustomerId = null,
    string? ErrorMessage = null);

public sealed record RefreshTokenResult(
    bool Success,
    string? AccessToken = null,
    string? RefreshToken = null,
    string? ErrorMessage = null);
