using SafaFoods.Core.Enums;

namespace SafaFoods.Core.Services.Models;

public sealed record CreateOrderCommand(
    Guid CustomerId,
    Guid AddressId,
    PaymentMethod PaymentMethod,
    Guid? DeliverySlotId = null,
    DateTime? ScheduledDeliveryDate = null,
    bool ApplyWalletBalance = false,
    string? CouponCode = null,
    IReadOnlyList<CreateOrderLineCommand> Items = default!);
