export type AppRole = "customer" | "admin" | "delivery";

export interface CustomerSession {
  accessToken: string;
  refreshToken: string;
  userId: string;
  expiresInSeconds: number;
  role: "customer";
  identifier: string;
}

export interface AdminSession {
  accessToken: string;
  refreshToken: string;
  userId: string;
  expiresInSeconds: number;
  role: "admin";
  email: string;
}

export type AppSession = CustomerSession | AdminSession;

export interface CustomerProfile {
  customerId: string;
  fullName: string;
  phone: string;
  email: string;
  referralCode?: string | null;
  memberSince: string;
}

export interface CustomerProfileOrderSummary {
  orderId: string;
  status: string;
  grandTotal: number;
  paymentStatus: string;
  placedAt: string;
  itemCount: number;
}

export interface CustomerProfileSubscriptionSummary {
  subscriptionId: string;
  productName: string;
  deliveriesInCycle: number;
  status: string;
  startDate: string;
  nextDeliveryDate?: string | null;
}

export interface CustomerProfileAddress {
  addressId: string;
  label?: string | null;
  line1: string;
  line2?: string | null;
  city: string;
  state: string;
  pinCode: string;
  latitude?: number | null;
  longitude?: number | null;
  isDefault: boolean;
}

export interface CustomerProfilePayload {
  profile: CustomerProfile;
  orders: CustomerProfileOrderSummary[];
  subscriptions: CustomerProfileSubscriptionSummary[];
  addresses: CustomerProfileAddress[];
}
