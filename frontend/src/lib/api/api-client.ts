import type { AppSession } from "@/lib/types/session";

function getApiBaseUrl() {
  const configuredBaseUrl =
    process.env.NEXT_PUBLIC_BACKEND_API_BASE_URL ??
    process.env.NEXT_PUBLIC_API_URL;

  if (!configuredBaseUrl) {
    return "http://localhost:5207/api";
  }

  return `${configuredBaseUrl.replace(/\/+$/, "")}/api`;
}

function getStoredSession(): AppSession | null {
  if (typeof window === "undefined") {
    return null;
  }

  const raw = window.localStorage.getItem("safa_session");
  if (!raw) {
    return null;
  }

  try {
    return JSON.parse(raw) as AppSession;
  } catch {
    return null;
  }
}

function buildHeaders(options: RequestInit): Headers {
  const headers = new Headers(options.headers ?? {});
  const session = getStoredSession();

  if (!headers.has("Content-Type") && options.body) {
    headers.set("Content-Type", "application/json");
  }

  if (session?.accessToken && !headers.has("Authorization")) {
    headers.set("Authorization", `Bearer ${session.accessToken}`);
  }

  return headers;
}

/**
 * API client for Safa Foods.
 * Adds auth headers when a local session exists and normalizes API errors.
 */
export async function apiClient<T>(
  endpoint: string,
  options: RequestInit = {}
): Promise<T> {
  const response = await fetch(`${getApiBaseUrl()}${endpoint}`, {
    ...options,
    headers: buildHeaders(options),
  });

  if (response.status === 401 && typeof window !== "undefined") {
    console.warn("Unauthorized request to:", endpoint);
  }

  if (!response.ok) {
    const errorBody = await response.json().catch(() => ({ message: "System error" }));
    const message =
      typeof errorBody?.message === "string"
        ? errorBody.message
        : typeof errorBody?.title === "string"
          ? errorBody.title
          : "Network response was not ok";

    throw new Error(message);
  }

  if (response.status === 204) {
    return undefined as T;
  }

  return (await response.json()) as T;
}
