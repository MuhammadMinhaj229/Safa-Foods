# Safa Foods Execution Master Plan

## 1. Purpose

This document is the working product, business, and technical execution plan for Safa Foods based on the current codebase direction.

It replaces the earlier assumption that Safa Foods should launch as a Shopify-first hybrid platform.

The current strategic direction is:

- custom storefront
- custom business backend
- PostgreSQL as source of truth
- event-driven automation
- WhatsApp-led customer communication

This is the correct model for a hyperlocal food commerce business where delivery, subscriptions, payments, and customer trust are tightly coupled.

## 2. Product Principle

Safa Foods should operate on one simple professional principle:

`Database as source of truth -> domain events -> automation workers -> customer communication`

This means:

- the website is the customer experience layer
- the backend is the business brain
- PostgreSQL stores the truth
- background workers handle automated communication and scheduled actions
- WhatsApp is the primary customer update channel

## 3. Platform Positioning

Safa Foods is not just an ecommerce website.

It is a `hyperlocal food commerce operations platform`.

The platform must support:

- product discovery
- account creation and login
- address capture and serviceability
- one-time orders
- subscriptions
- delivery-zone pricing
- COD and QR-based payments
- operational order tracking
- customer WhatsApp updates
- reviews and retention
- admin and delivery workflows

## 4. Core Business Logic

### One-time order flow

1. customer browses products
2. customer adds items to cart
3. customer logs in or signs up at checkout
4. customer selects or creates address
5. system checks serviceability and delivery fee
6. customer chooses payment method
7. order is created
8. admin or operations confirms and processes order
9. customer receives WhatsApp status updates
10. delivered order triggers review request

### Supported one-time order payment methods

- `COD`
- `Safa Foods QR / UPI prepaid`

### Subscription flow

Subscription logic must remain operationally safe in MVP.

Do not assume automatic recurring debit unless a real payment mandate exists.

Recommended MVP subscription payment models:

- `COD per delivery`
- `wallet / prepaid balance`
- `manual QR / UPI payment before dispatch`

Later, add:

- `Razorpay recurring / mandate-based autopay`

### Subscription customer journey

1. customer logs in or signs up
2. customer selects subscription-eligible product
3. customer chooses quantity and frequency
4. customer chooses delivery address
5. customer selects allowed payment model
6. subscription is created
7. system schedules delivery cycles
8. customer receives WhatsApp reminders and updates
9. customer can view all subscription records in profile
10. customer can pause, resume, skip, or cancel based on business rules

## 5. Customer Experience Model

The customer-facing product should feel simple and premium.

### Core pages

- Home
- Shop
- Product detail
- Cart
- Checkout
- Login
- Signup
- Profile
- Orders
- Subscriptions
- Reviews
- Contact
- FAQ
- Policies

### Profile area responsibilities

The profile section should become the customer control center.

It should contain:

- account details
- saved addresses
- order history
- subscription history
- payment history
- notification history
- submitted reviews
- subscription controls

## 6. Operations Model

### Admin responsibilities

Admin users must be able to:

- manage catalog
- update prices
- enable or disable products
- manage delivery zones
- confirm QR / UPI payments
- review orders by status
- manage subscriptions
- trigger or retry notifications
- moderate reviews
- view operational analytics

### Delivery responsibilities

Delivery workflow should support:

- assigned deliveries
- status changes
- proof of delivery if needed
- failed delivery notes
- customer contact coordination

## 7. Current Technology Direction

The current project should continue on the existing stack.

### Frontend

- `Next.js 16`
- `React 19`
- `TypeScript`
- `Tailwind CSS`

### Backend

- `.NET 9`
- `ASP.NET Core Minimal APIs`
- `EF Core`
- modular monolith architecture

### Data and infrastructure

- `PostgreSQL`
- `Redis` when enabled
- background workers inside backend first
- WhatsApp provider integration through abstraction

## 8. Architectural Decision

Do not rebuild the backend in another stack.

Do not move to Node.js or Django for notification orchestration.

The current `.NET + PostgreSQL + Next.js` architecture is already strong enough for this business stage.

The correct plan is:

- keep the modular monolith
- strengthen business modules
- add event-driven automation
- add production hardening gradually

## 9. Domain Modules

The platform should be organized around these domains:

- Catalog
- Customers
- Authentication
- Addresses
- Delivery
- Orders
- Subscriptions
- Payments
- Notifications
- Reviews
- Admin
- Delivery Operations
- Analytics

## 10. Event-Driven Automation Model

The platform should follow an event-driven automation model.

Every important business action should create a domain event.

### Event principles

- API writes business state first
- event is recorded in the same persistence flow
- worker processes event later
- notification or scheduled action happens asynchronously
- all results are logged

### Core event types

- `order.created`
- `order.confirmed`
- `order.preparing`
- `order.out_for_delivery`
- `order.delivered`
- `order.failed`
- `payment.submitted`
- `payment.confirmed`
- `subscription.created`
- `subscription.activated`
- `subscription.upcoming_delivery`
- `subscription.paused`
- `subscription.resumed`
- `subscription.cancelled`
- `review.requested`

## 11. Notification and WhatsApp Architecture

WhatsApp should be treated as a delivery channel, not as the source of business truth.

### Notification architecture

`business state change -> event/outbox record -> worker -> template rendering -> provider send -> notification log`

### Why this is correct

- keeps endpoints fast
- avoids request blocking
- allows retries
- allows auditing
- supports future channels beyond WhatsApp

### WhatsApp use cases

WhatsApp should send:

- order placed
- order confirmed
- payment pending
- payment confirmed
- preparing
- out for delivery
- delivered
- subscription reminder
- subscription status updates
- review request

### Channel abstraction

Even if WhatsApp is first, the design should still support:

- WhatsApp
- SMS later
- email later
- in-app notifications later

## 12. Payment Logic

### MVP payment model

One-time orders:

- `COD`
- `QR / UPI prepaid`

Subscriptions:

- `COD per cycle`
- `wallet / prepaid balance`
- `manual QR / UPI before dispatch`

### Later payment model

- Razorpay one-time checkout
- Razorpay recurring subscription billing
- payment webhooks
- reconciliation workflows

### Payment rules

- no fake auto-deduction without real mandate support
- every payment reference must be auditable
- subscriptions must enforce allowed payment rules clearly
- payment confirmation should change business state explicitly

## 13. Review System Logic

Reviews should be tied to actual fulfilled orders or deliveries.

### Review flow

1. order reaches delivered state
2. system triggers `review.requested`
3. customer receives WhatsApp link to website
4. website stores review
5. review is linked to:
   - product
   - order
   - delivery experience

### Review types

- product rating
- product comment
- delivery/service rating
- delivery/service comment

## 14. Data Model Direction

The current data model should evolve to include operational automation tables clearly.

### Core entities

- `users`
- `addresses`
- `products`
- `product_variants`
- `orders`
- `order_items`
- `subscriptions`
- `subscription_deliveries`
- `delivery_zones`
- `admin_users`
- `admin_sessions`
- `admin_audit_logs`

### Payment and automation entities

- `payments`
- `outbox_events`
- `notifications`
- `notification_templates`
- `communication_preferences`
- `reviews`

### Suggested outbox event fields

- `id`
- `event_type`
- `aggregate_type`
- `aggregate_id`
- `payload_json`
- `status`
- `occurred_at`
- `processed_at`
- `retry_count`
- `last_error`

### Suggested notification fields

- `id`
- `user_id`
- `event_id`
- `channel`
- `template_key`
- `provider`
- `recipient`
- `provider_message_id`
- `status`
- `sent_at`
- `delivered_at`
- `failed_at`
- `error_message`

## 15. Customer and Admin Rules To Lock

These decisions must be finalized before deeper automation:

- signup/login requirement at checkout
- COD eligibility by order type
- subscription payment rules
- QR payment confirmation rules
- serviceability rules
- delivery cut-off windows
- pause / resume / skip cut-off rules
- cancellation and refund policy
- review eligibility window

## 16. API and Workflow Direction

### Customer APIs

- catalog browsing
- profile retrieval
- address management
- order creation
- order history
- subscription creation
- subscription lifecycle actions
- review submission

### Admin APIs

- order list and filtering
- order status updates
- payment confirmation
- subscription operations
- delivery zone management
- notification monitoring
- review moderation

### Delivery APIs

- assigned deliveries
- delivery status updates
- proof or failure reason capture

## 17. Execution Phases

### Phase 1: Core commerce MVP

Deliver:

- customer auth
- cart and checkout
- address creation and serviceability
- one-time orders
- COD and QR payment flow
- order tracking states
- basic profile area

### Phase 2: Notification automation

Deliver:

- outbox event table
- notification worker
- WhatsApp template integration
- order lifecycle updates
- notification logs
- failure and retry handling

### Phase 3: Subscription engine

Deliver:

- subscription creation
- schedule generation
- pause/resume/skip/cancel logic
- reminder jobs
- profile history for subscriptions

### Phase 4: Operations hardening

Deliver:

- payment confirmation dashboard
- admin queue filters
- delivery operations workflow
- admin audit expansion
- review workflow
- policy and trust pages

### Phase 5: Production hardening

Deliver:

- Redis-backed cache strategy
- webhook verification
- integration tests
- endpoint tests
- CI/CD
- structured monitoring
- secrets hardening

### Phase 6: Scale and growth

Deliver:

- queue-backed workers if needed
- analytics and retention reporting
- offer engine
- richer review and content systems
- campaign and remarketing workflows

## 18. Quality Standards

### Engineering quality

- strong typing
- input validation
- no business logic in UI
- no direct provider calls from API endpoints
- audit trail for sensitive operations
- idempotency for critical payment and notification actions

### Testing quality

Required layers:

- unit tests
- service tests
- API integration tests
- notification workflow tests
- payment tests
- subscription tests
- frontend journey tests for checkout, profile, and subscriptions

### Operational quality

Track:

- order creation rate
- payment success/failure rate
- notification success/failure rate
- delivery completion rate
- subscription churn and pause rates
- review submission rate
- API latency and error rates

## 19. Final Strategic Decision

Safa Foods should continue as:

`a custom hyperlocal food commerce platform with event-driven operations and WhatsApp-led communication`

This is the correct scalable direction because:

- the business depends on local delivery logic
- subscriptions need custom workflow control
- payment and fulfillment rules are operationally specific
- WhatsApp is central to customer trust and retention
- admin and delivery operations need custom tooling

## 20. Immediate Next Build Priorities

The next practical priorities should be:

1. finalize business rules for COD, QR, subscriptions, and review flow
2. complete customer profile + auth boundary
3. complete order lifecycle and payment confirmation flow
4. add event/outbox architecture
5. add WhatsApp notification worker
6. build subscription scheduling and reminder logic
7. expand admin operations screens around orders, payments, subscriptions, and notifications

This document should be treated as the current source of truth for product execution and technical planning.
