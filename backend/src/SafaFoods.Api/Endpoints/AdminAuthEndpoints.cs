using Microsoft.AspNetCore.Http.HttpResults;

using SafaFoods.Api.Configuration;
using SafaFoods.Api.Contracts.Admin;
using SafaFoods.Core.Enums;
using SafaFoods.Core.Services;

namespace SafaFoods.Api.Endpoints;

public static class AdminAuthEndpoints
{
    public static RouteGroupBuilder MapAdminAuthEndpoints(this RouteGroupBuilder api)
    {
        var group = api.MapGroup("/admin/auth")
            .WithTags("Admin Auth");

        group.MapPost("/login", LoginAsync)
            .WithName("AdminLogin")
            .WithSummary("Logs in an admin user and returns a bearer token.");

        group.MapPost("/bootstrap", BootstrapAsync)
            .AddEndpointFilter<AdminAccessFilter>()
            .WithName("AdminBootstrap")
            .WithSummary("Creates an admin user from a protected bootstrap flow.");

        group.MapPost("/logout", LogoutAsync)
            .AddEndpointFilter<AdminAccessFilter>()
            .WithName("AdminLogout")
            .WithSummary("Revokes the current admin bearer session.");

        group.MapPost("/logout-all", LogoutAllAsync)
            .AddEndpointFilter<AdminAccessFilter>()
            .WithName("AdminLogoutAll")
            .WithSummary("Revokes all active sessions for the current admin user.");

        return group;
    }

    private static async Task<Results<Ok<AdminAuthResponse>, ValidationProblem>> LoginAsync(
        AdminLoginRequest request,
        IAdminAuthService adminAuthService,
        IAdminAuditService adminAuditService,
        CancellationToken cancellationToken)
    {
        var result = await adminAuthService.LoginAsync(request.Email, request.Password, cancellationToken);
        if (!result.Success || result.Data is null)
        {
            return TypedResults.ValidationProblem(new Dictionary<string, string[]>
            {
                ["credentials"] = [result.ErrorMessage ?? "Invalid admin credentials."]
            });
        }

        await adminAuditService.LogAsync(
            result.Data.AdminUserId,
            result.Data.Role,
            "admin_login",
            "admin_user",
            result.Data.AdminUserId,
            $$"""{"email":"{{result.Data.Email}}"}""",
            cancellationToken);

        return TypedResults.Ok(MapResponse(result.Data));
    }

    private static async Task<Results<Ok<AdminAuthResponse>, ValidationProblem>> BootstrapAsync(
        HttpContext httpContext,
        AdminBootstrapRequest request,
        IAdminAuthService adminAuthService,
        IAdminAuditService adminAuditService,
        CancellationToken cancellationToken)
    {
        var result = await adminAuthService.BootstrapAdminAsync(
            request.Name,
            request.Email,
            request.Password,
            request.Role,
            cancellationToken);

        if (!result.Success || result.Data is null)
        {
            return TypedResults.ValidationProblem(new Dictionary<string, string[]>
            {
                ["admin"] = [result.ErrorMessage ?? "Unable to create admin user."]
            });
        }

        await adminAuditService.LogAsync(
            GetAdminUserId(httpContext),
            GetAdminRole(httpContext),
            "admin_bootstrap_created",
            "admin_user",
            result.Data.AdminUserId,
            $$"""{"email":"{{result.Data.Email}}","role":"{{result.Data.Role}}"}""",
            cancellationToken);

        return TypedResults.Ok(MapResponse(result.Data));
    }

    private static async Task<Results<Ok<AdminLogoutResponse>, ValidationProblem>> LogoutAsync(
        HttpContext httpContext,
        IAdminAuthService adminAuthService,
        IAdminAuditService adminAuditService,
        CancellationToken cancellationToken)
    {
        var token = ExtractBearerToken(httpContext);
        if (string.IsNullOrWhiteSpace(token))
        {
            return TypedResults.ValidationProblem(new Dictionary<string, string[]>
            {
                ["authorization"] = ["Bearer token is required for logout."]
            });
        }

        var result = await adminAuthService.LogoutAsync(token, cancellationToken);
        if (!result.Success)
        {
            return TypedResults.ValidationProblem(new Dictionary<string, string[]>
            {
                ["session"] = [result.ErrorMessage ?? "Unable to revoke admin session."]
            });
        }

        await adminAuditService.LogAsync(
            GetAdminUserId(httpContext),
            GetAdminRole(httpContext),
            "admin_logout",
            "admin_session",
            null,
            null,
            cancellationToken);

        return TypedResults.Ok(new AdminLogoutResponse(result.Data));
    }

    private static async Task<Results<Ok<AdminLogoutResponse>, ValidationProblem>> LogoutAllAsync(
        HttpContext httpContext,
        IAdminAuthService adminAuthService,
        IAdminAuditService adminAuditService,
        CancellationToken cancellationToken)
    {
        if (!httpContext.Items.TryGetValue("AdminUserId", out var adminUserIdValue) ||
            adminUserIdValue is not Guid adminUserId ||
            adminUserId == Guid.Empty)
        {
            return TypedResults.ValidationProblem(new Dictionary<string, string[]>
            {
                ["authorization"] = ["Bearer admin session is required for logout-all."]
            });
        }

        var result = await adminAuthService.LogoutAllAsync(adminUserId, cancellationToken);
        if (!result.Success)
        {
            return TypedResults.ValidationProblem(new Dictionary<string, string[]>
            {
                ["session"] = [result.ErrorMessage ?? "Unable to revoke admin sessions."]
            });
        }

        await adminAuditService.LogAsync(
            adminUserId,
            GetAdminRole(httpContext),
            "admin_logout_all",
            "admin_session",
            null,
            $$"""{"revokedSessions":{{result.Data}}}""",
            cancellationToken);

        return TypedResults.Ok(new AdminLogoutResponse(result.Data));
    }

    private static AdminAuthResponse MapResponse(SafaFoods.Core.Services.Models.AdminAuthResult result) =>
        new(
            result.AdminUserId,
            result.Name,
            result.Email,
            result.Role,
            result.AccessToken,
            result.ExpiresAt);

    private static string? ExtractBearerToken(HttpContext httpContext)
    {
        if (!httpContext.Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
        {
            return null;
        }

        var authorization = authorizationHeader.ToString();
        return authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)
            ? authorization["Bearer ".Length..].Trim()
            : null;
    }

    private static Guid? GetAdminUserId(HttpContext httpContext) =>
        httpContext.Items.TryGetValue("AdminUserId", out var value) && value is Guid id
            ? id
            : null;

    private static AdminRole GetAdminRole(HttpContext httpContext) =>
        httpContext.Items.TryGetValue("AdminRole", out var value) && value is AdminRole role
            ? role
            : AdminRole.StoreOperator;
}
