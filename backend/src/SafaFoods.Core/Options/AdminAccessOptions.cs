using System.ComponentModel.DataAnnotations;

namespace SafaFoods.Core.Options;

public sealed class AdminAccessOptions
{
    public const string SectionName = "AdminAccess";

    [Required]
    public required string OwnerApiKey { get; init; }

    [Required]
    public required string StaffApiKey { get; init; }
}
