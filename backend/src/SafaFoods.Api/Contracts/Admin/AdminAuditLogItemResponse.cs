using SafaFoods.Core.Enums;

namespace SafaFoods.Api.Contracts.Admin;

public sealed record AdminAuditLogItemResponse(
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
