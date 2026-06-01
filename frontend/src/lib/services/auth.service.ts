import { apiClient } from "../api/api-client";
import type { AdminSession, CustomerSession } from "@/lib/types/session";

interface CustomerAuthTokenResponse {
  accessToken: string;
  refreshToken: string;
  customerId: string;
  expiresInSeconds: number;
}

interface AdminAuthTokenResponse {
  accessToken: string;
  refreshToken: string;
  adminUserId: string;
  email?: string;
  role?: string;
  expiresInSeconds: number;
}

/**
 * Authentication service for customer OTP and admin login flows.
 */
export const authService = {
  requestCustomerOtp: async (identifier: string): Promise<void> => {
    await apiClient<void>("/auth/customer/request-otp", {
      method: "POST",
      body: JSON.stringify({ identifier }),
    });
  },

  verifyCustomerOtp: async (
    identifier: string,
    otpCode: string
  ): Promise<CustomerSession> => {
    const response = await apiClient<CustomerAuthTokenResponse>("/auth/customer/verify-otp", {
      method: "POST",
      body: JSON.stringify({ identifier, otpCode, deviceHint: "WebApp_Customer" }),
    });

    return {
      accessToken: response.accessToken,
      refreshToken: response.refreshToken,
      userId: response.customerId,
      expiresInSeconds: response.expiresInSeconds,
      role: "customer",
      identifier,
    };
  },

  adminLogin: async (email: string, password: string): Promise<AdminSession> => {
    const response = await apiClient<AdminAuthTokenResponse>("/admin/auth/login", {
      method: "POST",
      body: JSON.stringify({ email, password }),
    });

    return {
      accessToken: response.accessToken,
      refreshToken: response.refreshToken,
      userId: response.adminUserId,
      expiresInSeconds: response.expiresInSeconds,
      role: "admin",
      email: response.email ?? email,
    };
  },

  logout: async (role: "customer" | "admin", refreshToken: string): Promise<void> => {
    try {
      const endpoint = role === "admin" ? "/admin/auth/logout" : "/auth/customer/logout";
      await apiClient<void>(endpoint, {
        method: "POST",
        body: JSON.stringify({ refreshToken }),
      });
    } catch (err) {
      console.warn("Backend logout failed, clearing local session anyway.", err);
    } finally {
      localStorage.removeItem("safa_session");
      localStorage.removeItem("safa_role");
    }
  },
};
