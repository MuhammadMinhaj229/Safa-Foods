using Microsoft.EntityFrameworkCore;

using SafaFoods.Core.Entities;
using SafaFoods.Core.Enums;
using SafaFoods.Core.Services;
using SafaFoods.Core.Services.Models;
using SafaFoods.Infrastructure.Persistence;

namespace SafaFoods.Infrastructure.Services;

public sealed class AdminAuditService(SafaFoodsDbContext dbContext) : IAdminAuditService
{
    public async Task LogAsync(
        Guid? adminUserId,
        AdminRole role,
        string action,
        string entityType,
        Guid? entityId,
        string? metadataJson,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(action) || string.IsNullOrWhiteSpace(entityType))
        {
            return;
        }

        dbContext.AdminAuditLogs.Add(new AdminAuditLog
        {
            AdminUserId = adminUserId,
            Role = role,
            Action = action.Trim(),
            EntityType = entityType.Trim(),
            EntityId = entityId,
            MetadataJson = string.IsNullOrWhiteSpace(metadataJson) ? null : metadataJson.Trim()
        });

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<AdminAuditLogItemResult>> GetRecentAsync(
        int take,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.AdminAuditLogs
            .AsNoTracking()
            .Include(x => x.AdminUser)
            .OrderByDescending(x => x.CreatedAt)
            .Take(Math.Clamp(take <= 0 ? 20 : take, 1, 100))
            .Select(x => new AdminAuditLogItemResult(
                x.Id,
                x.AdminUserId,
                x.AdminUser != null ? x.AdminUser.Name : null,
                x.AdminUser != null ? x.AdminUser.Email : null,
                x.Role,
                x.Action,
                x.EntityType,
                x.EntityId,
                x.MetadataJson,
                x.CreatedAt))
            .ToListAsync(cancellationToken);
    }
}
