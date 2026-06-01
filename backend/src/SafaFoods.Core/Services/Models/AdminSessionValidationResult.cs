using SafaFoods.Core.Enums;

namespace SafaFoods.Core.Services.Models;

public sealed record AdminSessionValidationResult(
    Guid AdminUserId,
    string Name,
    string Email,
    AdminRole Role,
    DateTimeOffset ExpiresAt);
