using System.ComponentModel.DataAnnotations;

namespace SafaFoods.Core.Options;

public sealed class WhatsAppOptions
{
    public const string SectionName = "WhatsApp";

    [Required]
    public required string Provider { get; init; } = "simulated";

    public string? BusinessPhoneNumber { get; init; }

    public string? SupportPhoneNumber { get; init; }

    public bool UseDevelopmentRecipientOverride { get; init; }

    public string? DevelopmentRecipientOverride { get; init; }

    public string? MetaWebhookVerifyToken { get; init; }

    public string? MetaAppSecret { get; init; }

    public string? MetaApiVersion { get; init; }

    public string? MetaGraphBaseUrl { get; init; }

    public string? MetaAccessToken { get; init; }

    public string? MetaPhoneNumberId { get; init; }
}
