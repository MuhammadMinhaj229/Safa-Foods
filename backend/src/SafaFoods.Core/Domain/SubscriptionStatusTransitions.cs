using SafaFoods.Core.Enums;

namespace SafaFoods.Core.Domain;

public static class SubscriptionStatusTransitions
{
    public static bool CanTransition(SubscriptionStatus currentStatus, SubscriptionStatus nextStatus)
    {
        if (currentStatus == nextStatus)
        {
            return true;
        }

        return currentStatus switch
        {
            SubscriptionStatus.Draft => nextStatus is SubscriptionStatus.PendingPayment or SubscriptionStatus.Cancelled,
            SubscriptionStatus.PendingPayment => nextStatus is SubscriptionStatus.Active or SubscriptionStatus.Cancelled,
            SubscriptionStatus.Active => nextStatus is SubscriptionStatus.Paused or SubscriptionStatus.Cancelled or SubscriptionStatus.Expired,
            SubscriptionStatus.Paused => nextStatus is SubscriptionStatus.Active or SubscriptionStatus.Cancelled or SubscriptionStatus.Expired,
            _ => false
        };
    }
}
