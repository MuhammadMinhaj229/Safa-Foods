using SafaFoods.Core.Services.Models;

namespace SafaFoods.Core.Services;

public interface IInvoiceService
{
    Task<ServiceResult<InvoiceDetailResult>> GetOrderInvoiceAsync(
        Guid orderId,
        CancellationToken cancellationToken = default);
}
