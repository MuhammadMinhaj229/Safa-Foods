namespace SafaFoods.Api.Contracts.Admin;

public sealed record AdminOutboxEventItemResponse(
    Guid OutboxEventId,
    string EventType,
    string AggregateType,
    Guid AggregateId,
    string Status,
    int RetryCount,
    string? LastError,
    DateTimeOffset OccurredAt,
    DateTimeOffset? ProcessedAt);
