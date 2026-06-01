using SafaFoods.Core.Enums;
using SafaFoods.Core.Services.Models;

namespace SafaFoods.Core.Services;

public interface IAdminAuthService
{
    Task<ServiceResult<AdminAuthResult>> BootstrapAdminAsync(
        string name,
        string email,
        string password,
        AdminRole role,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<AdminAuthResult>> LoginAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default);

    Task<AdminSessionValidationResult?> ValidateTokenAsync(
        string token,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<int>> LogoutAsync(
        string token,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<int>> LogoutAllAsync(
        Guid adminUserId,
        CancellationToken cancellationToken = default);
}
