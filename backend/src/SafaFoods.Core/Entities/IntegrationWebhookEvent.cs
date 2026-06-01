namespace SafaFoods.Core.Entities;

public sealed class IntegrationWebhookEvent
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Provider { get; set; }
    public required string EventType { get; set; }
    public string? ExternalEventId { get; set; }
    public required string PayloadJson { get; set; }
    public required string ProcessingStatus { get; set; }
    public DateTimeOffset ReceivedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? ProcessedAt { get; set; }
}
