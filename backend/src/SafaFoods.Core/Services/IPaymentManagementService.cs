using SafaFoods.Core.Services.Models;

namespace SafaFoods.Core.Services;

public interface IPaymentManagementService
{
    Task<ServiceResult<OrderDetailResult>> SubmitOrderPaymentReferenceAsync(
        Guid orderId,
        string paymentReference,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<OrderDetailResult>> ConfirmOrderPaymentAsync(
        Guid orderId,
        string paymentReference,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<SubscriptionDetailResult>> SubmitSubscriptionPaymentReferenceAsync(
        Guid subscriptionId,
        string paymentReference,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<SubscriptionDetailResult>> ConfirmSubscriptionPaymentAsync(
        Guid subscriptionId,
        string paymentReference,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<UpiPaymentInstructionsResult>> GetOrderPaymentInstructionsAsync(
        Guid orderId,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<UpiPaymentInstructionsResult>> GetSubscriptionPaymentInstructionsAsync(
        Guid subscriptionId,
        CancellationToken cancellationToken = default);
}
