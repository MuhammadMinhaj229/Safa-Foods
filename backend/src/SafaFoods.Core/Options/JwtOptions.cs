using System.ComponentModel.DataAnnotations;

namespace SafaFoods.Core.Options;

public sealed class JwtOptions
{
    public const string SectionName = "Jwt";

    [Required]
    public required string Secret { get; init; }

    [Required]
    public required string Issuer { get; init; }

    [Required]
    public required string Audience { get; init; }

    /// <summary>Access token lifetime in minutes. Default: 60.</summary>
    public int AccessTokenLifetimeMinutes { get; init; } = 60;

    /// <summary>Refresh token lifetime in days. Default: 30.</summary>
    public int RefreshTokenLifetimeDays { get; init; } = 30;
}
