using SafaFoods.Core.Enums;

namespace SafaFoods.Core.Services.Models;

public sealed record UpdateSubscriptionOwnershipCommand(
    Guid SubscriptionId,
    Guid CustomerId,
    SubscriptionStatus NextStatus);
