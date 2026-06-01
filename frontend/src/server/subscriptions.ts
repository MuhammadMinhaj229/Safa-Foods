export const SUBSCRIPTION_PLANS = {
  weekly_once: {
    code: "weekly_once",
    name: "Weekly Once",
    deliveriesInCycle: 1,
    billingCycle: "weekly",
    discountPercent: 5,
  },
  weekly_twice: {
    code: "weekly_twice",
    name: "Weekly Twice",
    deliveriesInCycle: 2,
    billingCycle: "weekly",
    discountPercent: 6,
  },
  monthly_four: {
    code: "monthly_four",
    name: "Monthly Four",
    deliveriesInCycle: 4,
    billingCycle: "monthly",
    discountPercent: 8,
  },
} as const;

export type SubscriptionPlanCode = keyof typeof SUBSCRIPTION_PLANS;

export function getSubscriptionQuote(
  unitPrice: number,
  quantityPerDelivery: number,
  planCode: SubscriptionPlanCode,
) {
  const plan = SUBSCRIPTION_PLANS[planCode];
  const cycleSubtotal = unitPrice * quantityPerDelivery * plan.deliveriesInCycle;
  const discountAmount = Number(
    ((cycleSubtotal * plan.discountPercent) / 100).toFixed(2),
  );
  const total = Number((cycleSubtotal - discountAmount).toFixed(2));

  return {
    plan,
    cycleSubtotal: Number(cycleSubtotal.toFixed(2)),
    discountAmount,
    total,
  };
}
