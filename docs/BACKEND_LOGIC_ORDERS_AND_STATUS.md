# Safa Foods Orders and Status Logic

## 1. Order Types

- `one_time`
- `subscription_generated`

## 2. Payment Methods

- `razorpay`
- `cod`

## 3. Payment Rules

- one-time orders can use Razorpay or COD
- subscriptions use Razorpay only
- COD is disabled for subscription-generated orders

## 4. Order Status Flow

### One-time orders

- `placed`
- `confirmed`
- `preparing`
- `out_for_delivery`
- `delivered`
- `cancelled`

### Subscription-generated orders

- `scheduled`
- `confirmed`
- `preparing`
- `out_for_delivery`
- `delivered`
- `skipped`
- `failed`

## 5. Fulfillment Logic

When a subscription delivery date arrives:

- backend generates an internal order record
- order is linked to subscription delivery
- operations team processes it like a normal order

## 6. Delivery Confirmation

In MVP:

- store operator updates final status manually

Later:

- rider app
- delivery OTP
- proof of delivery

## 7. Notifications

Send on:

- order placed
- order confirmed
- out for delivery
- delivered
- subscription renewal due
