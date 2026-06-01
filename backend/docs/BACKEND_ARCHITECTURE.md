# Safa Foods Backend Architecture

## Purpose

This document defines the backend architecture for Safa Foods as it exists today and the intended scaling path for the next phases. The goal is to keep the system operationally simple for MVP while preserving clean extension points for payments, notifications, admin tooling, and delivery operations.

## Architectural style

The current backend is a `modular monolith` built with:

- `.NET 9`
- `ASP.NET Core Minimal APIs`
- `EF Core`
- `PostgreSQL`

This is the correct structure for Safa Foods at this stage.

Why:

- one deployable backend is easier to ship and debug
- the business logic is still evolving
- delivery, subscription, payment, and admin workflows are tightly coupled
- the system can later be split into services only where there is real pressure

## Solution layout

### `backend/src/SafaFoods.Api`

API host and HTTP boundary.

Responsibilities:

- route groups and endpoint definitions
- request/response contracts
- endpoint filters
- configuration binding
- OpenAPI and health endpoints

Does not own:

- persistence logic
- pricing rules
- workflow transitions

### `backend/src/SafaFoods.Core`

Domain and application contracts.

Responsibilities:

- entities
- enums
- domain rules
- service interfaces
- service result models
- business options

This project is the business language of the system.

### `backend/src/SafaFoods.Infrastructure`

Persistence and service implementations.

Responsibilities:

- `DbContext`
- EF configurations
- migrations
- query services
- command/workflow services
- payment/admin/notification/invoice implementations

This project contains the data-access and operational implementation of the core contracts.

## Request flow

The system currently follows this flow:

1. request enters `SafaFoods.Api`
2. endpoint validates route/body constraints
3. endpoint calls a `Core` service interface
4. `Infrastructure` implementation executes domain + persistence logic
5. result returns as typed API contract

This separation keeps HTTP concerns out of domain logic and EF concerns out of the route handlers.

## Business modules

### 1. Catalog

Purpose:

- expose active products and variants to the storefront
- support product listing and product detail retrieval

Current endpoints:

- `GET /api/catalog/products`
- `GET /api/catalog/products/{slug}`

Primary tables:

- `products`
- `product_variants`

### 2. Delivery

Purpose:

- determine serviceability
- assign delivery zone
- compute delivery fee slab

Current logic:

- zone-based slab pricing
- local-radius serviceability
- delivery quote used by orders and subscriptions

Primary tables:

- `delivery_zones`
- `addresses`

### 3. Addresses

Purpose:

- persist customer delivery addresses
- store resolved delivery-zone metadata
- act as the stable delivery anchor for orders and subscriptions

Current endpoint:

- `POST /api/addresses`

Primary tables:

- `customers`
- `addresses`

### 4. Orders

Purpose:

- quote checkout totals
- create one-time orders
- expose order history and order detail
- support cancellation before preparation
- support invoice retrieval

Current endpoints:

- `POST /api/orders/quote`
- `GET /api/orders?customerId=...`
- `GET /api/orders/{orderId}`
- `GET /api/orders/{orderId}/invoice`
- `POST /api/orders`
- `POST /api/orders/{orderId}/cancel`
- `POST /api/orders/{orderId}/status`

Primary tables:

- `orders`
- `order_items`

### 5. Subscriptions

Purpose:

- create prepaid subscription drafts
- activate after payment confirmation
- generate the delivery schedule
- allow customer pause/resume/cancel

Current endpoints:

- `POST /api/subscriptions/quote`
- `GET /api/subscriptions?customerId=...`
- `GET /api/subscriptions/{subscriptionId}`
- `POST /api/subscriptions`
- `POST /api/subscriptions/{subscriptionId}/status`
- `POST /api/subscriptions/{subscriptionId}/customer-status`

Primary tables:

- `subscriptions`
- `subscription_deliveries`

### 6. Payments

Purpose:

- capture customer-submitted UPI/manual references
- allow admin payment confirmation
- transition prepaid workflows into active/paid state

Current endpoints:

- `POST /api/payments/orders/{orderId}/submit`
- `POST /api/payments/subscriptions/{subscriptionId}/submit`
- `POST /api/admin/orders/{orderId}/payments/confirm`
- `POST /api/admin/subscriptions/{subscriptionId}/payments/confirm`

Primary state today:

- manual UPI-style confirmation
- no Razorpay yet
- payment status is tracked on order/subscription records

### 7. Admin

Purpose:

- secure operational access
- expose dashboard metrics
- expose admin queues for orders, subscriptions, notifications
- provide admin login/bootstrap

Current endpoints:

- `POST /api/admin/auth/login`
- `POST /api/admin/auth/bootstrap`
- `GET /api/admin/dashboard`
- `GET /api/admin/orders`
- `GET /api/admin/subscriptions`
- `GET /api/admin/notifications`
- `POST /api/admin/notifications/dispatch`

Primary tables:

- `admin_users`
- `admin_sessions`

### 8. Notifications

Purpose:

- record outbound business events
- dispatch queued customer messages

Current state:

- persistence exists
- dispatch is simulated/provider-agnostic
- ready for WhatsApp provider integration

Primary table:

- `notification_logs`

## Domain entities

### Customer-facing entities

- `Customer`
- `Address`
- `Product`
- `ProductVariant`
- `Order`
- `OrderItem`
- `Subscription`
- `SubscriptionDelivery`

### Operations/admin entities

- `AdminUser`
- `AdminSession`
- `NotificationLog`
- `DeliveryZone`

## State machines

### Order lifecycle

- `Placed`
- `Confirmed`
- `Preparing`
- `OutForDelivery`
- `Delivered`
- `Cancelled`
- `Failed`

Rule:

- customer cancellation is allowed only before preparation starts

### Subscription lifecycle

- `Draft`
- `PendingPayment`
- `Active`
- `Paused`
- `Cancelled`
- `Expired`

Rule:

- customer self-service can only move to `Paused`, `Active`, or `Cancelled`

### Payment lifecycle

Order and subscription payment state is tracked separately from operational status.

This is correct because:

- COD can be operationally complete before payment becomes paid
- prepaid flows may be awaiting manual verification
- subscriptions must not become active until payment is confirmed

## Security model

### Current

Admin access supports two mechanisms:

- legacy `X-Admin-Key`
- bearer token backed by `admin_sessions`

This is acceptable for internal MVP operations, but the target direction is:

- bearer token auth only
- owner/staff role policies
- secrets only from environment/user-secret/secret manager

### Immediate rules

- never commit production DB credentials
- never keep real admin keys in tracked config
- rotate owner/staff bootstrap keys once initial admin accounts are created

## Data architecture

PostgreSQL is the system of record.

Design principles:

- normalized transactional tables
- explicit foreign keys
- migration-driven schema evolution
- business timestamps stored as `timestamp with time zone`

Key persisted business facts:

- customer and address history
- quoted and placed order totals
- subscription plan and schedule
- payment references and confirmation state
- notification audit trail
- admin session activity

## Why this is scalable

This backend is scalable because the important boundaries already exist inside the monolith:

- catalog
- delivery
- orders
- subscriptions
- payments
- notifications
- admin

If needed later, the likely extraction order is:

1. notifications worker/service
2. payment orchestration service
3. admin/reporting service

Do not split catalog/orders/subscriptions yet. They are still tightly coupled to one operational model.

## Integration architecture

### Frontend integration

The `frontend` app should consume this backend as the source of truth for:

- catalog
- delivery quote
- address creation
- order creation
- payment submission
- subscription creation and lifecycle
- admin dashboards

### Future external integrations

Planned:

- Razorpay
- WhatsApp provider
- email sender
- analytics/audit exports

These should be added behind service interfaces in `Core` and implemented in `Infrastructure`.

## Observability and operations

Current:

- health endpoint exists
- notification audit log exists

Still needed:

- structured request logging
- admin audit trail for sensitive actions
- payment confirmation audit metadata
- failure dashboards for dispatch/payment jobs

## Recommended next backend phases

### Phase 1: Operational hardening

- move all real secrets to environment-based configuration
- remove long-term dependence on `X-Admin-Key`
- add admin logout/session revoke
- add API-side validation cleanup

### Phase 2: External integrations

- Razorpay one-time orders
- WhatsApp provider integration
- invoice PDF generation

### Phase 3: Admin tooling

- owner/staff dashboard UI
- order work queue filters
- subscription operations screen
- notification retry UI

### Phase 4: Scale and reporting

- business reporting endpoints
- scheduled jobs
- queue-backed notification dispatch
- delivery agent workflow if operations require it

## Architectural decisions

### Keep

- modular monolith
- Minimal APIs
- EF Core migrations
- PostgreSQL as source of truth
- service interfaces in `Core`

### Avoid for now

- microservices
- event bus complexity
- multiple databases
- separate auth server
- dynamic delivery-pricing engines

## Summary

The current Safa Foods backend is correctly positioned as a modular monolith with clean feature boundaries. It is strong enough for MVP and structured well enough for future extraction, provided we continue to keep:

- domain logic in `Core`
- persistence/integrations in `Infrastructure`
- HTTP contracts in `Api`

That is the right professional architecture for this business stage.
