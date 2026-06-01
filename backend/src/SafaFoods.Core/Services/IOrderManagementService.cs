using SafaFoods.Core.Enums;
using SafaFoods.Core.Services.Models;

namespace SafaFoods.Core.Services;

public interface IOrderManagementService
{
    OrderQuoteResult GetQuote(OrderQuoteCommand command);

    Task<ServiceResult<CreateOrderResult>> CreateAsync(
        CreateOrderCommand command,
        CancellationToken cancellationToken = default);

    Task<OrderDetailResult?> GetByIdAsync(
        Guid orderId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<OrderSummaryResult>> GetByCustomerAsync(
        Guid customerId,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<OrderDetailResult>> UpdateStatusAsync(
        Guid orderId,
        OrderStatus nextStatus,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<OrderDetailResult>> CancelAsync(
        CancelOrderCommand command,
        CancellationToken cancellationToken = default);
}
