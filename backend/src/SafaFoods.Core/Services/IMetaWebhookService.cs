using System.Text.Json;

using SafaFoods.Core.Services.Models;

namespace SafaFoods.Core.Services;

public interface IMetaWebhookService
{
    bool TryValidateVerification(string mode, string verifyToken, string challenge, out string challengeResponse);

    Task<ServiceResult<Guid>> StoreWhatsAppWebhookAsync(
        JsonElement payload,
        CancellationToken cancellationToken = default);
}
