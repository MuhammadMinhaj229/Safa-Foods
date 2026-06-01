namespace SafaFoods.Core.Entities;

public sealed class OutboxEvent
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string EventType { get; set; }
    public required string AggregateType { get; set; }
    public Guid AggregateId { get; set; }
    public required string PayloadJson { get; set; }
    public required string Status { get; set; } = "pending";
    public int RetryCount { get; set; }
    public string? LastError { get; set; }
    public DateTimeOffset OccurredAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? ProcessedAt { get; set; }
}
