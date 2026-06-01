using SafaFoods.Core.Enums;

namespace SafaFoods.Core.Services.Models;

public sealed record AdminAuthResult(
    Guid AdminUserId,
    string Name,
    string Email,
    AdminRole Role,
    string AccessToken,
    DateTimeOffset ExpiresAt);
