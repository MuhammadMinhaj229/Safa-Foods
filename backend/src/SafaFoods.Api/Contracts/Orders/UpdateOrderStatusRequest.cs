using SafaFoods.Core.Enums;

namespace SafaFoods.Api.Contracts.Orders;

public sealed record UpdateOrderStatusRequest(OrderStatus Status);
