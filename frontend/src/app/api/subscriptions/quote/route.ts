import { ZodError } from "zod";

import { fail, ok } from "@/lib/api-response";
import { subscriptionQuoteRequestSchema } from "@/server/schemas";
import { getSubscriptionQuote } from "@/server/subscriptions";

export async function POST(request: Request) {
  try {
    const body = await request.json();
    const input = subscriptionQuoteRequestSchema.parse(body);

    return ok(
      getSubscriptionQuote(
        input.unitPrice,
        input.quantityPerDelivery,
        input.planCode,
      ),
    );
  } catch (error) {
    if (error instanceof ZodError) {
      return fail("Invalid subscription quote request.", 400, error.flatten());
    }

    return fail("Unable to calculate subscription quote.", 500);
  }
}
