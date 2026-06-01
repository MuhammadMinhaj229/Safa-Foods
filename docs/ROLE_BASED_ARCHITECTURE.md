# Role-Based Architecture

## Purpose

This document defines a strict role architecture for Safa Foods so each dashboard only shows role-relevant work.

Primary rule:

- Admin does not use customer views.
- Rider does not use customer or admin views.
- Customer does not see operations dashboards.

## Role Model

Three primary personas:

- `Customer` (User)
- `Rider` (Delivery Operator)
- `Admin` (Operations)

Recommended admin sub-roles:

- `SuperAdmin`
- `StoreOperator`
- `SupportAgent`
- `FinanceOperator`
- `CatalogManager`

## Access Separation Rules

### Customer

- Access only customer app routes and customer APIs.
- No admin routes.
- No rider routes.
- Ownership-only access to orders, subscriptions, addresses, wallet, and payments.

### Rider

- Access only rider app routes and rider APIs.
- Can see only deliveries assigned to rider.
- Cannot see full customer profile history.
- Cannot see catalog, finance, analytics, or admin settings.

### Admin

- Access only admin app routes and admin APIs.
- Can see operations data based on role scope.
- Must not render customer storefront pages inside admin dashboard.
- Admin dashboard focuses on queues, workflows, and controls, not customer browsing UI.

## Dashboard Architecture

### 1. Customer Dashboard

Modules:

- Profile
- Saved Addresses
- Orders
- Subscriptions
- Payments History
- Notifications History
- Reviews
- Support

Customer actions:

- place order
- cancel eligible order
- submit payment reference
- pause/resume/cancel own subscription
- submit product and service review

### 2. Rider Dashboard

Modules:

- Today Assignments
- Active Delivery Task
- Delivery Timeline
- Failed Attempt Queue
- Completed Deliveries

Rider actions:

- accept assignment
- reject assignment with reason
- mark picked up
- mark out for delivery
- mark delivered
- upload proof of delivery
- mark failed with reason and customer unreachable flag

### 3. Admin Dashboard

Admin home modules:

- Order Operations Queue
- Subscription Operations Queue
- Payment Verification Queue
- Notification Health
- Outbox Failures
- Delivery Exceptions
- Daily Revenue Snapshot

Admin actions:

- confirm or reject payment references
- manage order state transitions
- manage subscription state transitions
- assign and reassign riders
- retry failed notifications/outbox events
- moderate reviews
- manage products, variants, and stock
- manage delivery zones and slot capacity
- monitor audits and reconciliation

## Permission Boundaries By Role

### Customer Permissions

- `customer.profile.read`
- `customer.address.create`
- `customer.address.read`
- `customer.address.update`
- `customer.address.delete`
- `customer.cart.read_write`
- `customer.order.create`
- `customer.order.read_own`
- `customer.order.cancel_own`
- `customer.payment.submit_own`
- `customer.subscription.create`
- `customer.subscription.read_own`
- `customer.subscription.update_own`
- `customer.wallet.read_own`
- `customer.review.create_own`
- `customer.notification.read_own`

### Rider Permissions

- `rider.assignment.read_own`
- `rider.assignment.accept_reject`
- `rider.delivery.status.update_own`
- `rider.delivery.proof.upload`
- `rider.delivery.failure.report`
- `rider.history.read_own`

### Admin Permissions

- `admin.dashboard.read`
- `admin.orders.read_all`
- `admin.orders.update_status`
- `admin.subscriptions.read_all`
- `admin.subscriptions.update_status`
- `admin.payments.verify`
- `admin.notifications.read`
- `admin.notifications.retry`
- `admin.outbox.read`
- `admin.outbox.retry`
- `admin.rider.assignments.manage`
- `admin.catalog.manage`
- `admin.reviews.moderate`
- `admin.delivery_config.manage`
- `admin.audit.read`

## Admin Sub-Role Scope

### SuperAdmin

- full access to all admin permissions

### StoreOperator

- orders, subscriptions, delivery exceptions, notifications
- no finance settlement exports
- no user/session/security settings

### SupportAgent

- read customer order/subscription history
- update support-safe statuses only
- no payment confirmation
- no catalog or pricing changes

### FinanceOperator

- payment verification
- payment ledger and reconciliation
- revenue reports
- no catalog edits

### CatalogManager

- products, variants, categories, stock, offers
- no payment verification
- no rider assignments

## API Ownership Structure

### Customer API Group

- `/api/me/*`
- `/api/cart/*`
- `/api/orders/*` (customer-owned only)
- `/api/subscriptions/*` (customer-owned only)
- `/api/payments/*` (customer-owned flows only)
- `/api/catalog/*` (public read + auth review submit)

### Rider API Group

- `/api/rider/auth/*`
- `/api/rider/assignments/*`
- `/api/rider/deliveries/*`
- `/api/rider/history/*`

### Admin API Group

- `/api/admin/*`
- `/api/admin/auth/*`
- no customer-owned endpoint should double as admin control endpoint

## Data Visibility Rules

### Customer sees

- own profile and own transactions only

### Rider sees

- assignment-level customer delivery info only:
- name
- masked phone where possible
- address
- delivery notes

Rider does not see:

- full customer history
- payment internals
- admin analytics

### Admin sees

- full operational dataset according to sub-role

## Backend Enforcement Rules

- JWT + role claim required on every protected endpoint
- customer endpoints enforce `customer_id` ownership
- rider endpoints enforce `rider_id` assignment ownership
- admin endpoints enforce `admin_role` permission map
- no cross-role endpoint reuse for state-changing operations
- all sensitive operations must write audit logs

## Workflow Ownership

### Customer-owned workflows

- browse, cart, checkout
- own order/subscription management
- payment submission and instructions
- reviews

### Rider-owned workflows

- delivery execution lifecycle
- POD and failure notes

### Admin-owned workflows

- payment verification
- fulfillment control
- rider assignment
- catalog and operational settings
- automation recovery

## Priority Build Tasks For This Architecture

1. Enforce claim-based customer ownership on orders, subscriptions, and payment endpoints.
2. Create rider identity, assignment model, and rider API group.
3. Split status-changing actions into admin and rider endpoint groups.
4. Add permission map for admin sub-roles and route guards.
5. Build role-specific dashboard APIs so each UI renders only relevant modules.
6. Add audit hooks for rider assignment changes, payment confirmations, and retries.

