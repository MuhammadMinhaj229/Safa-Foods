using SafaFoods.Core.Domain;
using SafaFoods.Core.Enums;

namespace SafaFoods.Api.Tests;

public sealed class OrderStatusTransitionTests
{
    [Theory]
    [InlineData(OrderStatus.Placed, OrderStatus.Confirmed, true)]
    [InlineData(OrderStatus.Confirmed, OrderStatus.Preparing, true)]
    [InlineData(OrderStatus.Preparing, OrderStatus.OutForDelivery, true)]
    [InlineData(OrderStatus.OutForDelivery, OrderStatus.Delivered, true)]
    [InlineData(OrderStatus.Delivered, OrderStatus.Confirmed, false)]
    [InlineData(OrderStatus.Cancelled, OrderStatus.Placed, false)]
    public void CanTransition_ReturnsExpectedValue(OrderStatus currentStatus, OrderStatus nextStatus, bool expected)
    {
        var actual = OrderStatusTransitions.CanTransition(currentStatus, nextStatus);

        Assert.Equal(expected, actual);
    }
}
