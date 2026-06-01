# Safa Foods Technical Data Model

## 1. Core Tables

### customers

- `id`
- `shopify_customer_id`
- `full_name`
- `phone`
- `email`
- `marketing_opt_in`
- `created_at`
- `updated_at`

### addresses

- `id`
- `customer_id`
- `full_name`
- `phone`
- `address_line_1`
- `address_line_2`
- `landmark`
- `area`
- `city`
- `pincode`
- `latitude`
- `longitude`
- `distance_km`
- `delivery_zone_id`
- `is_default`
- `is_serviceable`
- `created_at`
- `updated_at`

### delivery_zones

- `id`
- `name`
- `min_km`
- `max_km`
- `fee`
- `estimated_min_minutes`
- `estimated_max_minutes`
- `is_active`
- `created_at`
- `updated_at`

### products

- `id`
- `shopify_product_id`
- `slug`
- `name`
- `category`
- `is_subscription_enabled`
- `is_active`
- `created_at`
- `updated_at`

### product_variants

- `id`
- `product_id`
- `shopify_variant_id`
- `label`
- `weight`
- `price`
- `sale_price`
- `sku`
- `is_active`
- `created_at`
- `updated_at`

### orders

- `id`
- `shopify_order_id`
- `customer_id`
- `order_type`
- `subtotal`
- `discount_total`
- `delivery_fee`
- `grand_total`
- `payment_method`
- `payment_status`
- `status`
- `address_id`
- `delivery_zone_id`
- `placed_at`
- `created_at`
- `updated_at`

### order_items

- `id`
- `order_id`
- `product_id`
- `variant_id`
- `quantity`
- `unit_price`
- `total_price`

### subscriptions

- `id`
- `customer_id`
- `product_id`
- `variant_id`
- `plan_code`
- `billing_cycle`
- `deliveries_in_cycle`
- `quantity_per_delivery`
- `plan_price`
- `discount_percent`
- `start_date`
- `end_date`
- `next_delivery_date`
- `next_billing_date`
- `status`
- `address_id`
- `delivery_zone_id`
- `created_at`
- `updated_at`

### subscription_deliveries

- `id`
- `subscription_id`
- `scheduled_date`
- `quantity`
- `delivery_fee`
- `status`
- `fulfilled_order_id`
- `attempt_count`
- `delivered_at`
- `created_at`
- `updated_at`

### notification_logs

- `id`
- `customer_id`
- `channel`
- `event_type`
- `recipient`
- `message_status`
- `provider_reference`
- `payload_json`
- `created_at`

### admin_users

- `id`
- `name`
- `email`
- `role`
- `is_active`
- `last_login_at`
- `created_at`
- `updated_at`

## 2. Important Enums

### order_type

- `one_time`
- `subscription_generated`

### payment_method

- `razorpay`
- `cod`

### payment_status

- `pending`
- `paid`
- `failed`
- `refunded`

### order_status

- `placed`
- `confirmed`
- `preparing`
- `out_for_delivery`
- `delivered`
- `cancelled`
- `failed`

### subscription_status

- `draft`
- `pending_payment`
- `active`
- `paused`
- `cancelled`
- `expired`

### subscription_delivery_status

- `scheduled`
- `confirmed`
- `preparing`
- `out_for_delivery`
- `delivered`
- `skipped`
- `failed`

### admin_role

- `super_admin`
- `store_operator`
- `content_manager`
- `delivery_coordinator`

## 3. Key Relationships

- one customer can have many addresses
- one customer can have many orders
- one customer can have many subscriptions
- one product can have many variants
- one subscription can have many subscription deliveries
- one order can have many order items
- one delivery zone can be linked to many addresses, orders, and subscriptions

## 4. Operational Notes

- Shopify ids should be stored for sync, but operational tables remain under your control
- subscription and delivery logic should not depend fully on Shopify internals
- all status changes should be timestamped and auditable later
