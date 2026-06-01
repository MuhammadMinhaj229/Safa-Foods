using System.Text.Json;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using SafaFoods.Core.Entities;
using SafaFoods.Core.Options;
using SafaFoods.Core.Services;
using SafaFoods.Core.Services.Models;
using SafaFoods.Infrastructure.Persistence;

namespace SafaFoods.Infrastructure.Services;

public sealed class MetaWebhookService(
    SafaFoodsDbContext dbContext,
    IOptions<WhatsAppOptions> whatsAppOptions) : IMetaWebhookService
{
    public bool TryValidateVerification(string mode, string verifyToken, string challenge, out string challengeResponse)
    {
        challengeResponse = string.Empty;

        if (!string.Equals(mode, "subscribe", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        var configuredToken = whatsAppOptions.Value.MetaWebhookVerifyToken;
        if (string.IsNullOrWhiteSpace(configuredToken) || !string.Equals(verifyToken, configuredToken, StringComparison.Ordinal))
        {
            return false;
        }

        challengeResponse = challenge;
        return true;
    }

    public async Task<ServiceResult<Guid>> StoreWhatsAppWebhookAsync(
        JsonElement payload,
        CancellationToken cancellationToken = default)
    {
        var payloadJson = payload.GetRawText();
        var externalEventId = TryExtractExternalEventId(payload);

        var entity = new IntegrationWebhookEvent
        {
            Provider = "meta_whatsapp",
            EventType = "webhook_received",
            ExternalEventId = externalEventId,
            PayloadJson = payloadJson,
            ProcessingStatus = "received"
        };

        dbContext.Set<IntegrationWebhookEvent>().Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);

        return ServiceResult<Guid>.Ok(entity.Id);
    }

    private static string? TryExtractExternalEventId(JsonElement payload)
    {
        try
        {
            if (!payload.TryGetProperty("entry", out var entries) || entries.ValueKind != JsonValueKind.Array || entries.GetArrayLength() == 0)
            {
                return null;
            }

            var firstEntry = entries[0];
            if (firstEntry.TryGetProperty("id", out var entryId) && entryId.ValueKind == JsonValueKind.String)
            {
                return entryId.GetString();
            }
        }
        catch
        {
        }

        return null;
    }
}
