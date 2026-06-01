import { ZodError } from "zod";

import { fail, ok } from "@/lib/api-response";
import { getDeliveryQuote } from "@/server/delivery";
import { serviceabilityRequestSchema } from "@/server/schemas";

export async function POST(request: Request) {
  try {
    const body = await request.json();
    const input = serviceabilityRequestSchema.parse(body);

    return ok(getDeliveryQuote(input.distanceKm));
  } catch (error) {
    if (error instanceof ZodError) {
      return fail("Invalid serviceability request.", 400, error.flatten());
    }

    return fail("Unable to calculate serviceability.", 500);
  }
}
