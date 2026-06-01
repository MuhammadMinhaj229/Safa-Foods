using SafaFoods.Core.Enums;
using SafaFoods.Core.Services.Models;

namespace SafaFoods.Core.Services;

public interface IAdminAuditService
{
    Task LogAsync(
        Guid? adminUserId,
        AdminRole role,
        string action,
        string entityType,
        Guid? entityId,
        string? metadataJson,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AdminAuditLogItemResult>> GetRecentAsync(
        int take,
        CancellationToken cancellationToken = default);
}
