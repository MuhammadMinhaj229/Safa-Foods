"use client";

import Cookies from "js-cookie";
import React, { createContext, useContext, useEffect, useMemo, useState } from "react";

import type { AdminSession, AppRole, AppSession, CustomerSession } from "@/lib/types/session";

interface RoleContextType {
  role: AppRole;
  setRole: (role: AppRole) => void;
  isDev: boolean;
  user: AppSession | null;
  setUser: (user: AppSession | null) => void;
  logout: () => Promise<void>;
  isAuthenticated: boolean;
  customerSession: CustomerSession | null;
  adminSession: AdminSession | null;
}

const RoleContext = createContext<RoleContextType | undefined>(undefined);

const SESSION_STORAGE_KEY = "safa_session";
const ROLE_STORAGE_KEY = "safa_role";
const SESSION_COOKIE_KEY = "safa_session";

function isAppRole(value: unknown): value is AppRole {
  return value === "customer" || value === "admin" || value === "delivery";
}

function isBaseSession(value: unknown): value is {
  accessToken: string;
  refreshToken: string;
  userId: string;
  expiresInSeconds: number;
} {
  if (!value || typeof value !== "object") {
    return false;
  }

  const candidate = value as Record<string, unknown>;

  return (
    typeof candidate.accessToken === "string" &&
    typeof candidate.refreshToken === "string" &&
    typeof candidate.userId === "string" &&
    typeof candidate.expiresInSeconds === "number"
  );
}

function normalizeSession(raw: unknown, fallbackRole: AppRole): AppSession | null {
  if (!isBaseSession(raw)) {
    return null;
  }

  const candidate = raw as Record<string, unknown>;
  const accessToken = candidate.accessToken as string;
  const refreshToken = candidate.refreshToken as string;
  const userId = candidate.userId as string;
  const expiresInSeconds = candidate.expiresInSeconds as number;
  const resolvedRole = fallbackRole === "delivery" ? "customer" : fallbackRole;

  if (resolvedRole === "admin") {
    return {
      accessToken,
      refreshToken,
      userId,
      expiresInSeconds,
      role: "admin",
      email: typeof candidate.email === "string" ? candidate.email : "",
    };
  }

  return {
    accessToken,
    refreshToken,
    userId,
    expiresInSeconds,
    role: "customer",
    identifier: typeof candidate.identifier === "string" ? candidate.identifier : "",
  };
}

function persistRole(role: AppRole) {
  localStorage.setItem(ROLE_STORAGE_KEY, role);
}

function persistSession(session: AppSession | null) {
  if (!session) {
    localStorage.removeItem(SESSION_STORAGE_KEY);
    Cookies.remove(SESSION_COOKIE_KEY);
    return;
  }

  const serialized = JSON.stringify(session);
  localStorage.setItem(SESSION_STORAGE_KEY, serialized);
  Cookies.set(SESSION_COOKIE_KEY, serialized, { expires: session.role === "admin" ? 1 : 7 });
}

export const RoleProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [role, setRoleState] = useState<AppRole>("customer");
  const [user, setUserState] = useState<AppSession | null>(null);
  const isDev = typeof window !== "undefined" && window.location.hostname === "localhost";

  useEffect(() => {
    const savedRole = localStorage.getItem(ROLE_STORAGE_KEY);
    const hydratedRole: AppRole = isAppRole(savedRole) ? savedRole : "customer";

    let hydratedSession: AppSession | null = null;
    const savedSession = localStorage.getItem(SESSION_STORAGE_KEY);
    if (savedSession) {
      try {
        hydratedSession = normalizeSession(JSON.parse(savedSession), hydratedRole);
      } catch {
        hydratedSession = null;
      }
    }

    if (hydratedSession) {
      setUserState(hydratedSession);
      setRoleState(hydratedSession.role);
      persistRole(hydratedSession.role);
      persistSession(hydratedSession);
      return;
    }

    setUserState(null);
    setRoleState(hydratedRole);
  }, []);

  const setRole = (newRole: AppRole) => {
    setRoleState(newRole);
    persistRole(newRole);
  };

  const setUser = (nextUser: AppSession | null) => {
    setUserState(nextUser);

    if (nextUser) {
      setRoleState(nextUser.role);
      persistRole(nextUser.role);
      persistSession(nextUser);
      return;
    }

    persistSession(null);
  };

  const logout = async () => {
    persistSession(null);
    localStorage.removeItem(ROLE_STORAGE_KEY);
    setRoleState("customer");
    setUserState(null);
  };

  const value = useMemo<RoleContextType>(
    () => ({
      role,
      setRole,
      isDev,
      user,
      setUser,
      logout,
      isAuthenticated: user !== null,
      customerSession: user?.role === "customer" ? user : null,
      adminSession: user?.role === "admin" ? user : null,
    }),
    [role, isDev, user]
  );

  return <RoleContext.Provider value={value}>{children}</RoleContext.Provider>;
};

export const useRole = () => {
  const context = useContext(RoleContext);
  if (context === undefined) {
    throw new Error("useRole must be used within a RoleProvider");
  }
  return context;
};
