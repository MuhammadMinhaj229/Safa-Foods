using SafaFoods.Core.Enums;
using SafaFoods.Core.Services.Models;

namespace SafaFoods.Core.Services;

public interface ISubscriptionManagementService
{
    Task<ServiceResult<CreateSubscriptionResult>> CreateAsync(
        CreateSubscriptionCommand command,
        CancellationToken cancellationToken = default);

    Task<SubscriptionDetailResult?> GetByIdAsync(
        Guid subscriptionId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SubscriptionSummaryResult>> GetByCustomerAsync(
        Guid customerId,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<SubscriptionDetailResult>> UpdateStatusAsync(
        Guid subscriptionId,
        SubscriptionStatus nextStatus,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<SubscriptionDetailResult>> UpdateStatusForCustomerAsync(
        UpdateSubscriptionOwnershipCommand command,
        CancellationToken cancellationToken = default);
}
