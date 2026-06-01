using SafaFoods.Core.Enums;

namespace SafaFoods.Api.Contracts.Orders;

public sealed record CreateOrderRequest(
    Guid AddressId,
    PaymentMethod PaymentMethod,
    Guid? DeliverySlotId = null,
    DateTime? ScheduledDeliveryDate = null,
    bool ApplyWalletBalance = false,
    string? CouponCode = null,
    IReadOnlyList<CreateOrderItemRequest> Items = default!);
