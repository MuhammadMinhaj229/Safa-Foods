using SafaFoods.Core.Enums;

namespace SafaFoods.Core.Domain;

public static class OrderStatusTransitions
{
    public static bool CanTransition(OrderStatus currentStatus, OrderStatus nextStatus)
    {
        if (currentStatus == nextStatus)
        {
            return true;
        }

        return currentStatus switch
        {
            OrderStatus.Placed => nextStatus is OrderStatus.Confirmed or OrderStatus.Cancelled or OrderStatus.Failed,
            OrderStatus.Confirmed => nextStatus is OrderStatus.Preparing or OrderStatus.Cancelled or OrderStatus.Failed,
            OrderStatus.Preparing => nextStatus is OrderStatus.OutForDelivery or OrderStatus.Cancelled or OrderStatus.Failed,
            OrderStatus.OutForDelivery => nextStatus is OrderStatus.Delivered or OrderStatus.Failed,
            _ => false
        };
    }
}
