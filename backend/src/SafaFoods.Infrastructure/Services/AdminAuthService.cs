using System.Security.Cryptography;
using System.Text;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using SafaFoods.Core.Entities;
using SafaFoods.Core.Enums;
using SafaFoods.Core.Services;
using SafaFoods.Core.Services.Models;
using SafaFoods.Infrastructure.Persistence;

namespace SafaFoods.Infrastructure.Services;

public sealed class AdminAuthService(
    SafaFoodsDbContext dbContext,
    PasswordHasher<AdminUser> passwordHasher) : IAdminAuthService
{
    public async Task<ServiceResult<AdminAuthResult>> BootstrapAdminAsync(
        string name,
        string email,
        string password,
        AdminRole role,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(name))
        {
            return ServiceResult<AdminAuthResult>.Fail("invalid_admin_input", "Name, email, and password are required.");
        }

        var normalizedEmail = email.Trim().ToLowerInvariant();
        var existing = await dbContext.AdminUsers.FirstOrDefaultAsync(x => x.Email == normalizedEmail, cancellationToken);
        if (existing is not null)
        {
            return ServiceResult<AdminAuthResult>.Fail("admin_exists", "Admin user already exists for this email.");
        }

        var admin = new AdminUser
        {
            Name = name.Trim(),
            Email = normalizedEmail,
            Role = role,
            IsActive = true
        };
        admin.PasswordHash = passwordHasher.HashPassword(admin, password);

        dbContext.AdminUsers.Add(admin);
        await dbContext.SaveChangesAsync(cancellationToken);

        return await CreateSessionAsync(admin, cancellationToken);
    }

    public async Task<ServiceResult<AdminAuthResult>> LoginAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default)
    {
        var normalizedEmail = email.Trim().ToLowerInvariant();
        var admin = await dbContext.AdminUsers.FirstOrDefaultAsync(x => x.Email == normalizedEmail, cancellationToken);

        if (admin is null || string.IsNullOrWhiteSpace(admin.PasswordHash))
        {
            return ServiceResult<AdminAuthResult>.Fail("invalid_credentials", "Invalid admin credentials.");
        }

        if (!admin.IsActive)
        {
            return ServiceResult<AdminAuthResult>.Fail("admin_inactive", "Admin account is inactive.");
        }

        var verification = passwordHasher.VerifyHashedPassword(admin, admin.PasswordHash, password);
        if (verification == PasswordVerificationResult.Failed)
        {
            return ServiceResult<AdminAuthResult>.Fail("invalid_credentials", "Invalid admin credentials.");
        }

        if (verification == PasswordVerificationResult.SuccessRehashNeeded)
        {
            admin.PasswordHash = passwordHasher.HashPassword(admin, password);
        }

        admin.LastLoginAt = DateTimeOffset.UtcNow;
        admin.UpdatedAt = DateTimeOffset.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);

        return await CreateSessionAsync(admin, cancellationToken);
    }

    public async Task<AdminSessionValidationResult?> ValidateTokenAsync(
        string token,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return null;
        }

        var tokenHash = HashToken(token);
        var session = await dbContext.AdminSessions
            .AsNoTracking()
            .Include(x => x.AdminUser)
            .FirstOrDefaultAsync(
                x => x.TokenHash == tokenHash &&
                     x.RevokedAt == null &&
                     x.ExpiresAt > DateTimeOffset.UtcNow &&
                     x.AdminUser.IsActive,
                cancellationToken);

        return session is null
            ? null
            : new AdminSessionValidationResult(
                session.AdminUserId,
                session.AdminUser.Name,
                session.AdminUser.Email,
                session.AdminUser.Role,
                session.ExpiresAt);
    }

    public async Task<ServiceResult<int>> LogoutAsync(
        string token,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return ServiceResult<int>.Fail("token_required", "Bearer token is required.");
        }

        var tokenHash = HashToken(token);
        var session = await dbContext.AdminSessions
            .FirstOrDefaultAsync(
                x => x.TokenHash == tokenHash &&
                     x.RevokedAt == null,
                cancellationToken);

        if (session is null)
        {
            return ServiceResult<int>.Fail("session_not_found", "Admin session was not found.");
        }

        session.RevokedAt = DateTimeOffset.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);

        return ServiceResult<int>.Ok(1);
    }

    public async Task<ServiceResult<int>> LogoutAllAsync(
        Guid adminUserId,
        CancellationToken cancellationToken = default)
    {
        if (adminUserId == Guid.Empty)
        {
            return ServiceResult<int>.Fail("admin_user_required", "Admin user id is required.");
        }

        var activeSessions = await dbContext.AdminSessions
            .Where(x => x.AdminUserId == adminUserId && x.RevokedAt == null)
            .ToListAsync(cancellationToken);

        if (activeSessions.Count == 0)
        {
            return ServiceResult<int>.Ok(0);
        }

        var revokedAt = DateTimeOffset.UtcNow;
        foreach (var session in activeSessions)
        {
            session.RevokedAt = revokedAt;
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return ServiceResult<int>.Ok(activeSessions.Count);
    }

    private async Task<ServiceResult<AdminAuthResult>> CreateSessionAsync(
        AdminUser admin,
        CancellationToken cancellationToken)
    {
        await RevokeExpiredSessionsAsync(admin.Id, cancellationToken);

        var rawToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(48));
        var expiresAt = DateTimeOffset.UtcNow.AddDays(7);

        var session = new AdminSession
        {
            AdminUserId = admin.Id,
            TokenHash = HashToken(rawToken),
            ExpiresAt = expiresAt
        };

        dbContext.AdminSessions.Add(session);
        await dbContext.SaveChangesAsync(cancellationToken);

        return ServiceResult<AdminAuthResult>.Ok(new AdminAuthResult(
            admin.Id,
            admin.Name,
            admin.Email,
            admin.Role,
            rawToken,
            expiresAt));
    }

    private async Task RevokeExpiredSessionsAsync(Guid adminUserId, CancellationToken cancellationToken)
    {
        var expiredSessions = await dbContext.AdminSessions
            .Where(x => x.AdminUserId == adminUserId &&
                        x.RevokedAt == null &&
                        x.ExpiresAt <= DateTimeOffset.UtcNow)
            .ToListAsync(cancellationToken);

        if (expiredSessions.Count == 0)
        {
            return;
        }

        var revokedAt = DateTimeOffset.UtcNow;
        foreach (var session in expiredSessions)
        {
            session.RevokedAt = revokedAt;
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static string HashToken(string token)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(token));
        return Convert.ToHexString(bytes);
    }
}
