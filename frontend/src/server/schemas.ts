import { z } from "zod";

import { SUBSCRIPTION_PLANS } from "@/server/subscriptions";

export const serviceabilityRequestSchema = z.object({
  distanceKm: z.coerce.number().min(0).max(50),
});

export const subscriptionQuoteRequestSchema = z.object({
  unitPrice: z.coerce.number().positive(),
  quantityPerDelivery: z.coerce.number().int().min(1).max(20),
  planCode: z.enum(Object.keys(SUBSCRIPTION_PLANS) as [keyof typeof SUBSCRIPTION_PLANS, ...Array<keyof typeof SUBSCRIPTION_PLANS>]),
});
