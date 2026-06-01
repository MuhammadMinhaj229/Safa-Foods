# Safa Foods API Architecture

## API style

- REST-style route groups
- Minimal API handlers
- typed request/response contracts
- JSON enums serialized as strings

Base path:

- `/api`

## Route groups

### System

- `GET /api/system/health`

Purpose:

- liveness and quick service verification

### Catalog

- `GET /api/catalog/products`
- `GET /api/catalog/products/{slug}`

Purpose:

- storefront read-only product access

### Delivery

- `POST /api/delivery/serviceability`

Purpose:

- serviceability quote by distance/address metadata

### Addresses

- `POST /api/addresses`

Purpose:

- persist customer address with delivery zone/serviceability state

### Orders

- `POST /api/orders/quote`
- `GET /api/orders`
- `GET /api/orders/{orderId}`
- `GET /api/orders/{orderId}/invoice`
- `POST /api/orders`
- `POST /api/orders/{orderId}/cancel`
- `POST /api/orders/{orderId}/status`

### Subscriptions

- `POST /api/subscriptions/quote`
- `GET /api/subscriptions`
- `GET /api/subscriptions/{subscriptionId}`
- `POST /api/subscriptions`
- `POST /api/subscriptions/{subscriptionId}/status`
- `POST /api/subscriptions/{subscriptionId}/customer-status`

### Payments

- `POST /api/payments/orders/{orderId}/submit`
- `POST /api/payments/subscriptions/{subscriptionId}/submit`

### Admin Auth

- `POST /api/admin/auth/login`
- `POST /api/admin/auth/bootstrap`

### Admin Operations

- `GET /api/admin/dashboard`
- `GET /api/admin/orders`
- `GET /api/admin/subscriptions`
- `GET /api/admin/notifications`
- `POST /api/admin/notifications/dispatch`
- `POST /api/admin/orders/{orderId}/payments/confirm`
- `POST /api/admin/subscriptions/{subscriptionId}/payments/confirm`

## Authorization boundaries

### Public/customer endpoints

- catalog
- delivery
- addresses
- customer orders
- customer subscriptions
- customer payment submission

### Protected admin endpoints

- `/api/admin/*`
- admin bootstrap currently protected by admin filter

Admin filter supports:

- bearer token via `Authorization: Bearer <token>`
- legacy `X-Admin-Key`

## Error handling model

Current pattern:

- `404` for missing resources
- `400`-style validation responses via `ValidationProblem`
- typed success results

Recommended next improvement:

- standardized business error codes in a shared problem-details contract

## Contract rules

- HTTP DTOs stay in `SafaFoods.Api/Contracts`
- DTOs must not expose EF entities directly
- endpoint handlers remain thin
- domain/service models stay in `Core`

## API versioning

Current phase:

- no explicit version segment yet

Recommendation:

- stay unversioned for MVP
- introduce `/api/v1` only when breaking public-client changes appear

## Frontend contract guidance

The frontend should treat the backend as the source of truth for:

- totals
- serviceability
- COD eligibility
- subscription state
- order status
- admin data

The frontend should not recompute business-critical totals independently.
