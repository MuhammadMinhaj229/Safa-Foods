using SafaFoods.Core.Enums;

namespace SafaFoods.Core.Entities;

public sealed class NotificationLog
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid? CustomerId { get; set; }
    public NotificationChannel Channel { get; set; }
    public required string EventType { get; set; }
    public required string Recipient { get; set; }
    public required string MessageStatus { get; set; }
    public string? ProviderReference { get; set; }
    public string? PayloadJson { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public Customer? Customer { get; set; }
}
