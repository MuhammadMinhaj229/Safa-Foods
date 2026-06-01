namespace SafaFoods.Core.Services.Models;

public sealed record AdminOutboxEventItemResult(
    Guid OutboxEventId,
    string EventType,
    string AggregateType,
    Guid AggregateId,
    string Status,
    int RetryCount,
    string? LastError,
    DateTimeOffset OccurredAt,
    DateTimeOffset? ProcessedAt);
