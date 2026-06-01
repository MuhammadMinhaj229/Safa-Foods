# Safa Foods Database Architecture

## Database engine

- `PostgreSQL`

## Schema management

- `EF Core` migrations
- migration source under `backend/src/SafaFoods.Infrastructure/Persistence/Migrations`
- exported SQL scripts under `backend/sql`

## Core transactional tables

### `customers`

Stores customer identity and contact details.

### `addresses`

Stores delivery addresses for customers.

Important persisted attributes:

- area
- city
- pincode
- distance in km
- serviceable flag
- delivery-zone mapping

### `delivery_zones`

Stores pricing/serviceability zones.

### `products`

Stores product-level catalog info.

Examples:

- Fresh Ginger Garlic Paste
- Fresh Garlic Paste
- Green Chilli Paste

### `product_variants`

Stores saleable pack variants.

Examples:

- `250g`
- `500g`
- `750g`

### `orders`

Stores one-time orders.

Key columns:

- order type
- payment method
- payment status
- status
- subtotal
- discount total
- delivery fee
- grand total
- invoice number
- invoice issued at
- cancellation reason

### `order_items`

Stores item rows for each order.

### `subscriptions`

Stores recurring plan instances.

Key columns:

- plan code
- billing cycle
- deliveries in cycle
- quantity per delivery
- plan price
- status
- payment reference
- next delivery date
- next billing date

### `subscription_deliveries`

Stores generated delivery schedule rows for each subscription.

### `notification_logs`

Stores outbound notification events and dispatch status.

### `admin_users`

Stores owner/staff users.

Key columns:

- name
- email
- password hash
- role
- is active
- last login at

### `admin_sessions`

Stores bearer-token-backed admin sessions.

Key columns:

- token hash
- admin user id
- expires at
- revoked at

## Integrity model

The schema uses:

- primary keys on all transactional tables
- foreign keys for entity relationships
- unique indexes where identity or session uniqueness matters

Important unique constraints:

- admin email
- admin session token hash
- invoice number

## Migration artifacts for pgAdmin

You can inspect the SQL directly in:

- `backend/sql/pgadmin-full-migrations.sql`
- `backend/sql/20260407053332-admin-sessions-invoices-customer-actions.sql`

## Recommended future DB additions

- audit table for admin actions
- payment transactions table once Razorpay is live
- webhook event table for external providers
- delivery-attempt table if rider workflows become more detailed
