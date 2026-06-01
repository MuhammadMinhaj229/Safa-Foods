import { NextResponse } from "next/server";

import { getBackendApiBaseUrl } from "@/lib/backend-api";

export async function POST(request: Request) {
  const baseUrl = getBackendApiBaseUrl();
  if (!baseUrl) {
    return NextResponse.json(
      { ok: false, error: { message: "BACKEND_API_BASE_URL is not configured." } },
      { status: 500 },
    );
  }

  const body = await request.json();
  const authorization = request.headers.get("authorization");
  const targetPath = authorization ? "/api/me/addresses" : "/api/addresses";

  const response = await fetch(`${baseUrl}${targetPath}`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Accept: "application/json",
      Authorization: authorization ?? "",
    },
    body: JSON.stringify(body),
  });

  const text = await response.text();

  return new NextResponse(text, {
    status: response.status,
    headers: {
      "Content-Type": response.headers.get("Content-Type") ?? "application/json",
    },
  });
}
