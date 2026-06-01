import { ok } from "@/lib/api-response";

export async function GET() {
  return ok({
    service: "safa-foods-frontend",
    status: "healthy",
    timestamp: new Date().toISOString(),
  });
}
