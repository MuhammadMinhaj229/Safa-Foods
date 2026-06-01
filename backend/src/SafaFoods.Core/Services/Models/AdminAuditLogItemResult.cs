using SafaFoods.Core.Enums;

namespace SafaFoods.Core.Services.Models;

public sealed record AdminAuditLogItemResult(
    Guid AuditLogId,
    Guid? AdminUserId,
    string? AdminName,
    string? AdminEmail,
    AdminRole Role,
    string Action,
    string EntityType,
    Guid? EntityId,
    string? MetadataJson,
    DateTimeOffset CreatedAt);
