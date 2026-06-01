export type OrderStatus =
  | "placed"
  | "confirmed"
  | "preparing"
  | "out_for_delivery"
  | "delivered"
  | "cancelled"
  | "failed";

export type SubscriptionStatus =
  | "draft"
  | "pending_payment"
  | "active"
  | "paused"
  | "cancelled"
  | "expired";

export type PaymentMethod = "razorpay" | "cod";

export type DeliveryZoneLabel = "Zone A" | "Zone B" | "Zone C";
