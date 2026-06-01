using Microsoft.EntityFrameworkCore;

using SafaFoods.Core.Services;
using SafaFoods.Core.Services.Models;
using SafaFoods.Infrastructure.Persistence;

namespace SafaFoods.Infrastructure.Services;

public sealed class InvoiceService(SafaFoodsDbContext dbContext) : IInvoiceService
{
    public async Task<ServiceResult<InvoiceDetailResult>> GetOrderInvoiceAsync(
        Guid orderId,
        CancellationToken cancellationToken = default)
    {
        var order = await dbContext.Orders
            .AsNoTracking()
            .Include(x => x.Customer)
            .Include(x => x.Address)
            .Include(x => x.Items)
                .ThenInclude(x => x.Product)
            .Include(x => x.Items)
                .ThenInclude(x => x.Variant)
            .FirstOrDefaultAsync(x => x.Id == orderId, cancellationToken);

        if (order is null || string.IsNullOrWhiteSpace(order.InvoiceNumber) || order.InvoiceIssuedAt is null)
        {
            return ServiceResult<InvoiceDetailResult>.Fail("invoice_not_found", "Invoice is not available for this order.");
        }

        return ServiceResult<InvoiceDetailResult>.Ok(new InvoiceDetailResult(
            order.Id,
            order.InvoiceNumber,
            order.InvoiceIssuedAt.Value,
            order.Customer.FullName,
            order.Customer.Phone,
            order.Address.AddressLine1,
            order.Address.AddressLine2,
            order.Address.Area,
            order.Address.City,
            order.Address.Pincode,
            order.PaymentMethod,
            order.PaymentStatus,
            order.Subtotal,
            order.DiscountTotal,
            order.DeliveryFee,
            order.GrandTotal,
            order.Items
                .OrderBy(x => x.Product.Name)
                .ThenBy(x => x.Variant.Label)
                .Select(x => new InvoiceDetailItemResult(
                    x.Product.Name,
                    x.Variant.Label,
                    x.Quantity,
                    x.UnitPrice,
                    x.TotalPrice))
                .ToList()));
    }
}
