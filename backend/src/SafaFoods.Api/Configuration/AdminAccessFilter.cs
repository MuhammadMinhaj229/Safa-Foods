using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;

using SafaFoods.Core.Enums;
using SafaFoods.Core.Options;
using SafaFoods.Core.Services;

namespace SafaFoods.Api.Configuration;

public sealed class AdminAccessFilter(
    IOptions<AdminAccessOptions> options,
    IAdminAuthService adminAuthService) : IEndpointFilter
{
    private const string HeaderName = "X-Admin-Key";

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var httpContext = context.HttpContext;

        if (httpContext.Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
        {
            var authorization = authorizationHeader.ToString();
            if (authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                var token = authorization["Bearer ".Length..].Trim();
                var session = await adminAuthService.ValidateTokenAsync(token, httpContext.RequestAborted);
                if (session is not null)
                {
                    httpContext.Items["AdminRole"] = session.Role;
                    httpContext.Items["AdminUserId"] = session.AdminUserId;
                    return await next(context);
                }
            }
        }

        if (!httpContext.Request.Headers.TryGetValue(HeaderName, out var apiKeyValues))
        {
            return TypedResults.Unauthorized();
        }

        var apiKey = apiKeyValues.ToString();
        var configured = options.Value;

        var role = apiKey switch
        {
            var key when key == configured.OwnerApiKey => AdminRole.SuperAdmin,
            var key when key == configured.StaffApiKey => AdminRole.StoreOperator,
            _ => (AdminRole?)null
        };

        if (role is null)
        {
            return TypedResults.Unauthorized();
        }

        httpContext.Items["AdminRole"] = role.Value;
        return await next(context);
    }
}
