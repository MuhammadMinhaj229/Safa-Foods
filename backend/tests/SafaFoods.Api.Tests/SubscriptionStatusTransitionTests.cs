using SafaFoods.Core.Domain;
using SafaFoods.Core.Enums;

namespace SafaFoods.Api.Tests;

public sealed class SubscriptionStatusTransitionTests
{
    [Theory]
    [InlineData(SubscriptionStatus.Draft, SubscriptionStatus.PendingPayment, true)]
    [InlineData(SubscriptionStatus.PendingPayment, SubscriptionStatus.Active, true)]
    [InlineData(SubscriptionStatus.Active, SubscriptionStatus.Paused, true)]
    [InlineData(SubscriptionStatus.Paused, SubscriptionStatus.Active, true)]
    [InlineData(SubscriptionStatus.Active, SubscriptionStatus.Cancelled, true)]
    [InlineData(SubscriptionStatus.Expired, SubscriptionStatus.Active, false)]
    [InlineData(SubscriptionStatus.Cancelled, SubscriptionStatus.Active, false)]
    public void CanTransition_ReturnsExpectedValue(SubscriptionStatus currentStatus, SubscriptionStatus nextStatus, bool expected)
    {
        var actual = SubscriptionStatusTransitions.CanTransition(currentStatus, nextStatus);

        Assert.Equal(expected, actual);
    }
}
