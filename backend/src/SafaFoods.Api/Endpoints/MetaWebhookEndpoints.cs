using System.Text.Json;

using Microsoft.AspNetCore.Http.HttpResults;

using SafaFoods.Core.Services;

namespace SafaFoods.Api.Endpoints;

public static class MetaWebhookEndpoints
{
    public static RouteGroupBuilder MapMetaWebhookEndpoints(this RouteGroupBuilder api)
    {
        var group = api.MapGroup("/integrations/meta/whatsapp")
            .WithTags("Meta WhatsApp");

        group.MapGet("/webhook", VerifyWebhookAsync)
            .WithName("VerifyMetaWhatsAppWebhook")
            .WithSummary("Verifies the Meta WhatsApp webhook challenge.");

        group.MapPost("/webhook", ReceiveWebhookAsync)
            .WithName("ReceiveMetaWhatsAppWebhook")
            .WithSummary("Receives Meta WhatsApp webhook payloads.");

        return group;
    }

    private static Results<ContentHttpResult, UnauthorizedHttpResult> VerifyWebhookAsync(
        string? hub_mode,
        string? hub_verify_token,
        string? hub_challenge,
        IMetaWebhookService metaWebhookService)
    {
        if (string.IsNullOrWhiteSpace(hub_mode) ||
            string.IsNullOrWhiteSpace(hub_verify_token) ||
            string.IsNullOrWhiteSpace(hub_challenge))
        {
            return TypedResults.Unauthorized();
        }

        return metaWebhookService.TryValidateVerification(hub_mode, hub_verify_token, hub_challenge, out var challenge)
            ? TypedResults.Text(challenge, "text/plain")
            : TypedResults.Unauthorized();
    }

    private static async Task<Results<Accepted<string>, BadRequest<string>>> ReceiveWebhookAsync(
        JsonElement payload,
        IMetaWebhookService metaWebhookService,
        CancellationToken cancellationToken)
    {
        var result = await metaWebhookService.StoreWhatsAppWebhookAsync(payload, cancellationToken);

        return result.Success
            ? TypedResults.Accepted($"/api/integrations/meta/whatsapp/webhook/{result.Data}", "Webhook accepted.")
            : TypedResults.BadRequest(result.ErrorMessage ?? "Unable to store webhook payload.");
    }
}
