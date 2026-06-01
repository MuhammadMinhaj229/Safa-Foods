using SafaFoods.Core.Enums;

namespace SafaFoods.Api.Contracts.Admin;

public sealed record AdminAuthResponse(
    Guid AdminUserId,
    string Name,
    string Email,
    AdminRole Role,
    string AccessToken,
    DateTimeOffset ExpiresAt);
