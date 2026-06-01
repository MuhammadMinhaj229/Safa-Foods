using System.ComponentModel.DataAnnotations;

namespace SafaFoods.Core.Options;

public sealed class ShopOptions
{
    public const string SectionName = "Shop";

    [Required]
    public required string Name { get; init; }

    [Required]
    public required string City { get; init; }

    [Required]
    public required string ServiceArea { get; init; }

    [Required]
    public required string AddressHint { get; init; }

    [Range(-90, 90)]
    public decimal Latitude { get; init; }

    [Range(-180, 180)]
    public decimal Longitude { get; init; }

    public string? FssaiLicense { get; init; }
    public string? GstNumber { get; init; }
    public string? SupportEmail { get; init; }
    public string? ReturnPolicyUrl { get; init; }
    public string? TermsOfServiceUrl { get; init; }
    public string? PublicBaseUrl { get; init; }
    public string? ReviewPagePath { get; init; }
}
