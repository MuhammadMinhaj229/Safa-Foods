# API Design

## Current routes

- `GET /api/health`
- `POST /api/delivery/serviceability`
- `POST /api/subscriptions/quote`

## Planned next routes

- `POST /api/checkout/quote`
- `POST /api/addresses`
- `GET /api/orders/:id`
- `PATCH /api/orders/:id/status`
- `POST /api/subscriptions`
- `PATCH /api/subscriptions/:id`
- `POST /api/webhooks/razorpay`
- `POST /api/webhooks/shopify`

## Notes

- Delivery serviceability is currently distance-driven for MVP.
- Subscription quoting is fixed-plan and prepaid by design.
- Payment and commerce provider integrations should be added only after env setup and DB migrations are live.
