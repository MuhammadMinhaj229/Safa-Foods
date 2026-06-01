using SafaFoods.Core.Enums;

namespace SafaFoods.Api.Contracts.Subscriptions;

public sealed record CustomerSubscriptionStatusRequest(
    SubscriptionStatus Status);
