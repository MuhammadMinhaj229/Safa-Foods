# Backend Execution Tasks

## Purpose

This document is the working backend execution list for Safa Foods.

Role and dashboard architecture reference:

- `docs/ROLE_BASED_ARCHITECTURE.md`

It audits the current backend from three operating perspectives:

- customer
- rider / delivery operator
- admin / operations

It marks what is already covered, what is partial, what is missing, and what should be built next in professional execution order.

## Current Backend Verdict

The backend has a strong foundation:

- modular monolith structure
- catalog, cart, auth, orders, subscriptions, wallet, pricing, notifications
- outbox-based automation
- background notification worker
- admin auth, audit logs, payment confirmation flows

Recent completed hardening:

- Task A1 complete: customer order endpoints are now claim-bound and ownership checked
- Task A2 complete: customer subscription endpoints are now claim-bound and ownership checked
- Task A3 complete: customer payment instruction/submission endpoints are policy-protected with ownership checks
- Task A4 complete (create-path split): guest address capture remains under `/api/addresses`, authenticated saved-address create is under `/api/me/addresses`

The main gaps are not the basic domain model.

The main gaps are:

- incomplete auth boundaries on several customer-owned workflows
- no real rider module yet
- partial admin operations coverage
- missing payment ledger and delivery assignment domain depth
- missing moderation and customer communication history surfaces

## Persona Coverage Audit

### 1. Customer POV

#### Covered

- OTP-based customer auth
- catalog browsing and product search
- product reviews read and write
- cart management
- address creation
- delivery serviceability quote
- order quote and order creation
- order cancellation
- subscription quote and subscription creation
- customer profile summary
- wallet details
- WhatsApp automation for order, payment, subscription, and review events

#### Partial

- profile does not yet expose notification history
- profile does not yet expose payment history
- profile does not yet expose delivery-event history
- review flow exists, but delivery service review is not implemented as a separate domain
- authenticated address update/delete/list preferences are not yet fully expanded beyond current create/list coverage

#### Missing

- notification history endpoint
- payment history endpoint
- review request redemption UX contract from backend perspective
- customer communication preferences / opt-in controls

### 2. Rider / Delivery POV

#### Covered

- none as a real backend persona

#### Partial

- order statuses support fulfillment progression
- subscription deliveries exist in the data model
- delivery slot and serviceability logic exist

#### Missing

- rider auth
- rider session model
- delivery assignment entity
- rider-specific API group
- assigned deliveries list
- pickup accepted / rejected flow
- out-for-delivery transition by rider ownership
- proof of delivery upload contract
- failed delivery reason capture
- retry / reattempt delivery workflow
- rider earnings or settlement model if needed later
- rider audit trail
- rider notification flow

### 3. Admin / Operations POV

#### Covered

- admin bootstrap and login
- admin audit logs
- dashboard summary
- admin order list
- admin subscription list
- payment confirmation for orders and subscriptions
- notification log viewing
- notification dispatch trigger
- notification health metrics
- outbox diagnostics

#### Partial

- admin can view orders and subscriptions, but operational actions are not fully separated from public endpoint groups
- no dedicated admin endpoints yet for catalog CRUD
- no dedicated admin endpoints yet for review moderation
- no dedicated admin endpoints yet for delivery zone management
- no dedicated admin endpoints yet for coupon / offer management
- no dedicated admin endpoint yet for retrying failed outbox events
- no dedicated admin endpoint yet for retrying failed notifications safely

#### Missing

- admin catalog management
- admin variant inventory management
- admin delivery zone management
- admin slot capacity management
- admin review moderation workflow
- admin rider assignment workflow
- admin delivery exception queue
- admin payment ledger
- admin customer communication preferences view
- admin operational reconciliation reports

## Critical Findings

### Critical 1: Customer auth boundary is inconsistent

The highest-risk backend gap has been reduced by recent hardening.

Some customer endpoints are properly JWT claim-bound, especially:

- `/api/me/*`
- `/api/me/wallet`
- `/api/cart/*`
- customer order/subscription/payment creation and read flows
- review submission

Remaining auth work is mainly around edge operations and future rider/admin action surfaces.

### Critical 2: Rider backend is effectively absent

The product has delivery logic, but not a true delivery-operator backend.

That means operations can move status manually, but the rider workflow is not yet a first-class backend module.

### Critical 3: Payment lifecycle lacks ledger depth

Payment references are stored on orders and subscriptions, but there is no dedicated payment entity yet.

This limits:

- reconciliation
- auditability
- multi-attempt tracking
- future gateway integration
- refund handling

### Critical 4: Admin operations are visible but not yet controllable

The admin backend can inspect a good amount of state, but it still lacks full operational control surfaces for:

- catalog
- reviews
- delivery assignments
- outbox retry actions
- failed notification recovery

## Professional Task List

Tasks are listed in execution order.

### Phase A: Security and Ownership Hardening

#### Task A1

Convert customer-owned order endpoints to claim-bound ownership.

Scope:

- require `CustomerPolicy`
- remove request/query `CustomerId` dependency from customer order APIs
- derive customer identity from JWT claim

Endpoints impacted:

- `/api/orders`
- `/api/orders/{id}`
- `/api/orders/{id}/cancel`

#### Task A2

Convert customer-owned subscription endpoints to claim-bound ownership.

Scope:

- require `CustomerPolicy`
- remove request/query `CustomerId` dependency from customer subscription APIs
- enforce subscription ownership consistently

Endpoints impacted:

- `/api/subscriptions`
- `/api/subscriptions/{id}`
- `/api/subscriptions/{id}/customer-status`

#### Task A3

Protect payment instruction and payment submission endpoints with customer ownership checks.

Scope:

- require `CustomerPolicy` for customer-triggered payment endpoints
- ensure customer can only access their own order/subscription payment flows
- keep admin confirmation flows separate

Endpoints impacted:

- `/api/payments/orders/{id}/submit`
- `/api/payments/orders/{id}/instructions`
- `/api/payments/subscriptions/{id}/submit`
- `/api/payments/subscriptions/{id}/instructions`

#### Task A4

Split guest address capture from authenticated address book logic.

Scope:

- preserve guest checkout support if desired
- add authenticated saved-address creation/update/delete endpoints
- stop conflating signup/address creation with address book management

Status:

- guest create endpoint retained: `/api/addresses`
- authenticated create endpoint added: `/api/me/addresses`
- update/delete for authenticated address book is pending

### Phase B: Payment and Commerce Integrity

#### Task B1

Introduce a dedicated payment ledger entity.

Scope:

- payment records table
- payment type, amount, reference, status
- parent link to order or subscription
- multiple attempts and audit trail

#### Task B2

Add payment reconciliation states.

Scope:

- submitted
- under_review
- confirmed
- rejected
- refunded

#### Task B3

Expose customer payment history in profile APIs.

#### Task B4

Expose admin payment ledger and filters.

### Phase C: Rider / Delivery Module

#### Task C1

Create delivery-operator domain model.

Scope:

- rider entity or delivery operator entity
- auth/session model
- assignment status

#### Task C2

Create delivery assignment entity and workflow.

Scope:

- assign order/subscription delivery to rider
- accepted / declined / reassigned
- timestamps and notes

#### Task C3

Add rider API surface.

Scope:

- assigned deliveries
- active route / task
- mark picked up
- mark out for delivery
- mark delivered
- mark failed attempt

#### Task C4

Add proof-of-delivery contract.

Scope:

- proof image reference or upload metadata
- delivery note
- failed reason

#### Task C5

Add rider audit trail and operational visibility.

### Phase D: Admin Operations Completion

#### Task D1

Move operational status-changing actions behind admin-specific or rider-specific endpoint groups.

Reason:

Public order endpoints should not double as admin control surfaces.

Status:

- completed for admin operations: order/subscription status-changing routes now live under `/api/admin/*`
- rider-specific status surfaces remain pending until rider module is implemented

#### Task D2

Add admin catalog management APIs.

Scope:

- product create/update
- variant create/update
- stock enable/disable
- subscription eligibility toggle

#### Task D3

Add admin review moderation APIs.

Scope:

- pending reviews
- approve / reject
- moderation notes

#### Task D4

Add admin delivery zone and slot management APIs.

Scope:

- zone pricing
- serviceability limits
- slot capacity changes

#### Task D5

Add admin retry operations for failed outbox events and failed notifications.

Scope:

- reset failed outbox event to pending
- requeue failed notification safely
- audit each retry action

### Phase E: Customer Communication Completion

#### Task E1

Add customer notification history endpoint.

#### Task E2

Add communication preference entity and API.

Scope:

- WhatsApp opt-in
- future SMS/email readiness

#### Task E3

Add richer review request contract.

Scope:

- product-specific review links
- optional delivery-service review
- review eligibility expiry rules

#### Task E4

Add webhook delivery-state reconciliation for WhatsApp messages where supported.

### Phase F: Production Hardening

#### Task F1

Fix EF Core relationship drift warnings around wallet entities.

#### Task F2

Add integration tests for:

- auth ownership
- payment submission
- payment confirmation
- outbox generation
- review request generation
- subscription reminder generation

#### Task F3

Add idempotency rules to admin retry flows and payment submission flows.

#### Task F4

Add structured operational metrics and alertable counters for:

- failed notifications
- failed outbox events
- stuck queued notifications
- payment review backlog

## Recommended Immediate Build Order

Build next in this exact order:

1. `Task D1`
2. `Task B1`
3. `Task C1`
4. `Task C2`
5. `Task D5`
6. `Task E1`
7. `Task F2`

## Definition of Backend Done

The backend should be considered professionally complete for MVP only when:

- customer-owned workflows are fully auth-bound
- admin operations are separated from customer/public routes
- rider workflows are first-class backend flows
- payment references are stored in a ledger, not only on aggregate entities
- notification failures are operable
- reviews, payments, notifications, and subscriptions are visible in profile/admin
- automated flows are covered by integration tests
