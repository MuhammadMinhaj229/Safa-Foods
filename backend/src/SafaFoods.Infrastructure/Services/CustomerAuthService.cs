using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SafaFoods.Core.Entities;
using SafaFoods.Core.Options;
using SafaFoods.Core.Services;
using SafaFoods.Core.Services.Models;
using SafaFoods.Infrastructure.Persistence;

namespace SafaFoods.Infrastructure.Services;

/**
 * Professional Authentication Service (Unified Identify-Verify)
 * Supports Mobile (WhatsApp) or Email identifers for Artisan Security.
 */
public sealed class CustomerAuthService(
    SafaFoodsDbContext dbContext,
    IOptions<JwtOptions> jwtOptions,
    INotificationDispatchService notifications,
    ILogger<CustomerAuthService> logger) : ICustomerAuthService
{
    private const int OtpLength = 6;
    private const int OtpLifetimeMinutes = 10;
    private const int MaxOtpAttempts = 5;

    public async Task<RequestOtpResult> RequestOtpAsync(string identifier, CancellationToken ct = default)
    {
        // Invalidate older unused tokens for this identifier
        var existing = await dbContext.CustomerAuthTokens
            .Where(t => t.Phone == identifier && !t.Used && t.ExpiresAt > DateTimeOffset.UtcNow)
            .ToListAsync(ct);
        foreach (var t in existing) t.Used = true;

        var otp = GenerateOtp();

        dbContext.CustomerAuthTokens.Add(new CustomerAuthToken
        {
            Phone = identifier, // Reuse Phone column as generic Identifier
            OtpCode = otp,
            ExpiresAt = DateTimeOffset.UtcNow.AddMinutes(OtpLifetimeMinutes)
        });

        await dbContext.SaveChangesAsync(ct);

        // Smart Dispatch based on format
        if (IsEmail(identifier))
        {
            logger.LogInformation("Simulating Email OTP for {Email}: {Otp}", identifier, otp);
            // TODO: notifications.SendEmailOtpAsync(identifier, otp, ct);
        }
        else
        {
            await notifications.SendOtpAsync(identifier, otp, ct);
        }

        return new RequestOtpResult(true);
    }

    public async Task<VerifyOtpResult> VerifyOtpAsync(string identifier, string otpCode, string? deviceHint = null, CancellationToken ct = default)
    {
        var token = await dbContext.CustomerAuthTokens
            .Where(t => t.Phone == identifier && !t.Used && t.ExpiresAt > DateTimeOffset.UtcNow)
            .OrderByDescending(t => t.CreatedAt)
            .FirstOrDefaultAsync(ct);

        if (token is null)
            return new VerifyOtpResult(false, ErrorMessage: "OTP expired or was not issued for this identifier.");

        token.AttemptCount++;

        if (token.AttemptCount > MaxOtpAttempts)
        {
            token.Used = true;
            await dbContext.SaveChangesAsync(ct);
            return new VerifyOtpResult(false, ErrorMessage: "Too many security attempts. Request a new OTP.");
        }

        if (token.OtpCode != otpCode)
        {
            await dbContext.SaveChangesAsync(ct);
            return new VerifyOtpResult(false, ErrorMessage: "Invalid artisan authorization code.");
        }

        token.Used = true;

        // Unified Identity Search: Check Phone OR Email
        var customer = await dbContext.Customers
            .FirstOrDefaultAsync(c => c.Phone == identifier || c.Email == identifier, ct);

        if (customer is null)
        {
            // NEW ARTISAN DISCOVERY (Auto-Sign-up)
            var normalizedIdentifier = identifier.Trim();
            var defaultName = IsEmail(normalizedIdentifier)
                ? normalizedIdentifier.Split('@')[0]
                : normalizedIdentifier;

            customer = new Customer
            {
                FullName = defaultName,
                Phone = IsEmail(normalizedIdentifier) ? string.Empty : normalizedIdentifier,
                Email = IsEmail(normalizedIdentifier) ? normalizedIdentifier : string.Empty,
                ReferralCode = GenerateReferralCode()
            };

            dbContext.Customers.Add(customer);
        }

        var (accessToken, refreshToken, refreshHash) = GenerateTokenPair(customer.Id);

        dbContext.CustomerSessions.Add(new CustomerSession
        {
            CustomerId = customer.Id,
            RefreshTokenHash = refreshHash,
            ExpiresAt = DateTimeOffset.UtcNow.AddDays(jwtOptions.Value.RefreshTokenLifetimeDays),
            DeviceHint = deviceHint
        });

        await dbContext.SaveChangesAsync(ct);

        return new VerifyOtpResult(true, accessToken, refreshToken, customer.Id);
    }

    public async Task<RefreshTokenResult> RefreshTokenAsync(string refreshToken, CancellationToken ct = default)
    {
        var hash = HashRefreshToken(refreshToken);
        var session = await dbContext.CustomerSessions
            .Include(s => s.Customer)
            .FirstOrDefaultAsync(s => s.RefreshTokenHash == hash && !s.Revoked && s.ExpiresAt > DateTimeOffset.UtcNow, ct);

        if (session is null)
            return new RefreshTokenResult(false, ErrorMessage: "Session invalid. Please reconfirm identity.");

        var (newAccessToken, newRefreshToken, newHash) = GenerateTokenPair(session.CustomerId);
        session.RefreshTokenHash = newHash;
        session.ExpiresAt = DateTimeOffset.UtcNow.AddDays(jwtOptions.Value.RefreshTokenLifetimeDays);

        await dbContext.SaveChangesAsync(ct);
        return new RefreshTokenResult(true, newAccessToken, newRefreshToken);
    }

    public async Task RevokeSessionAsync(string refreshToken, CancellationToken ct = default)
    {
        var hash = HashRefreshToken(refreshToken);
        var session = await dbContext.CustomerSessions.FirstOrDefaultAsync(s => s.RefreshTokenHash == hash, ct);
        if (session is { }) { session.Revoked = true; await dbContext.SaveChangesAsync(ct); }
    }

    private (string accessToken, string refreshToken, string refreshHash) GenerateTokenPair(Guid customerId)
    {
        var jwt = jwtOptions.Value;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new[] { 
            new Claim(JwtRegisteredClaimNames.Sub, customerId.ToString()),
            new Claim("customer_id", customerId.ToString()) 
        };
        var token = new JwtSecurityToken(jwt.Issuer, jwt.Audience, claims, expires: DateTime.UtcNow.AddMinutes(jwt.AccessTokenLifetimeMinutes), signingCredentials: credentials);
        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
        var refreshBytes = RandomNumberGenerator.GetBytes(64);
        var refreshToken = Convert.ToBase64String(refreshBytes);
        return (accessToken, refreshToken, HashRefreshToken(refreshToken));
    }

    private static string HashRefreshToken(string t) => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(t))).ToLowerInvariant();
    private static string GenerateOtp() => RandomNumberGenerator.GetInt32(0, 999999).ToString("D6");
    private static bool IsEmail(string i) => i.Contains('@');
    private static string GenerateReferralCode() => new string(RandomNumberGenerator.GetBytes(8).Select(b => "ABCDEFGHJKLMNPQRSTUVWXYZ23456789"[b % 32]).ToArray());
}
