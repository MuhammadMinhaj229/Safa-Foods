using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;
using SafaFoods.Api.Contracts.Auth;
using SafaFoods.Core.Options;
using SafaFoods.Core.Services;

namespace SafaFoods.Api.Endpoints;

/**
 * Unified Artisan Authentication Endpoints
 * Supports secure identity verification via Mobile/WhatsApp or Email.
 */
public static class CustomerAuthEndpoints
{
    public static RouteGroupBuilder MapCustomerAuthEndpoints(this RouteGroupBuilder api)
    {
        var group = api.MapGroup("/auth/customer")
            .WithTags("Customer Auth");

        group.MapPost("/request-otp", RequestOtpAsync)
            .WithName("CustomerRequestOtp")
            .WithSummary("Sends a 6-digit OTP to the identifier (Mobile or Email).")
            .RequireRateLimiting("otp_policy")
            .AllowAnonymous();

        group.MapPost("/verify-otp", VerifyOtpAsync)
            .WithName("CustomerVerifyOtp")
            .WithSummary("Verifies the OTP and returns high-fidelity JWT sessions.")
            .AllowAnonymous();

        group.MapPost("/refresh", RefreshTokenAsync)
            .WithName("CustomerRefreshToken")
            .WithSummary("Issues a new access session using a valid refresh token.")
            .AllowAnonymous();

        group.MapPost("/logout", LogoutAsync)
            .WithName("CustomerLogout")
            .WithSummary("Revokes the current artisan session.")
            .RequireAuthorization("CustomerPolicy");

        return group;
    }

    private static async Task<Results<Ok, ValidationProblem>> RequestOtpAsync(
        RequestOtpRequest request,
        ICustomerAuthService authService,
        CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.Identifier))
            return TypedResults.ValidationProblem(new Dictionary<string, string[]>
                { ["identifier"] = ["Identity identifier (Phone or Email) is required."] });

        var result = await authService.RequestOtpAsync(request.Identifier.Trim(), ct);
        if (!result.Success)
            return TypedResults.ValidationProblem(new Dictionary<string, string[]>
                { ["otp"] = [result.ErrorMessage ?? "Artisan security failure. Failed to dispatch OTP."] });

        return TypedResults.Ok();
    }

    private static async Task<Results<Ok<AuthTokenResponse>, ValidationProblem>> VerifyOtpAsync(
        VerifyOtpRequest request,
        ICustomerAuthService authService,
        IOptions<JwtOptions> jwtOptions,
        CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.Identifier) || string.IsNullOrWhiteSpace(request.OtpCode))
            return TypedResults.ValidationProblem(new Dictionary<string, string[]>
                { ["Validation"] = ["Identification and OTP code are required for verification."] });

        var result = await authService.VerifyOtpAsync(request.Identifier.Trim(), request.OtpCode.Trim(), request.DeviceHint, ct);

        if (!result.Success)
            return TypedResults.ValidationProblem(new Dictionary<string, string[]>
                { ["otp"] = [result.ErrorMessage ?? "Artisan verification failed."] });

        return TypedResults.Ok(new AuthTokenResponse(
            result.AccessToken!,
            result.RefreshToken!,
            result.CustomerId!.Value,
            jwtOptions.Value.AccessTokenLifetimeMinutes * 60));
    }

    private static async Task<Results<Ok<AuthTokenResponse>, ValidationProblem>> RefreshTokenAsync(
        RefreshTokenRequest request,
        ICustomerAuthService authService,
        IOptions<JwtOptions> jwtOptions,
        CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.RefreshToken))
            return TypedResults.ValidationProblem(new Dictionary<string, string[]>
                { ["refreshToken"] = ["Refresh token is required."] });

        var result = await authService.RefreshTokenAsync(request.RefreshToken, ct);

        if (!result.Success)
            return TypedResults.ValidationProblem(new Dictionary<string, string[]>
                { ["session"] = [result.ErrorMessage ?? "Artisan session rotation failed."] });

        return TypedResults.Ok(new AuthTokenResponse(
            result.AccessToken!,
            result.RefreshToken!,
            Guid.Empty,
            jwtOptions.Value.AccessTokenLifetimeMinutes * 60));
    }

    private static async Task<Ok> LogoutAsync(
        ClaimsPrincipal user,
        RevokeTokenRequest request,
        ICustomerAuthService authService,
        CancellationToken ct)
    {
        await authService.RevokeSessionAsync(request.RefreshToken, ct);
        return TypedResults.Ok();
    }
}
