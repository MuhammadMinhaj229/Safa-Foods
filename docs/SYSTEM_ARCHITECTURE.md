# Safa Foods System Architecture

## 1. Architecture Goal

The Safa Foods platform should support:

- local ecommerce orders
- prepaid subscriptions
- delivery-zone pricing
- frequent content updates
- WhatsApp-led engagement
- admin-friendly operations

The system should be simple enough for MVP launch and strong enough to scale later.

## 2. Recommended Architecture

### Frontend

- `Next.js`
- `Tailwind CSS`
- `shadcn/ui`

Responsibilities:

- homepage and landing pages
- product catalog
- product detail pages
- cart and checkout UI
- account area
- subscription management UI
- SEO pages
- blog and content pages

### Commerce Backend

- `Shopify`

Responsibilities:

- product catalog
- pricing
- cart
- checkout
- order records
- customer accounts
- inventory basics
- discount codes

### Content Backend

- `Sanity`

Responsibilities:

- homepage sections
- banners
- brand story
- FAQ
- blog
- reviews
- delivery info content
- offer content

### Custom Business Logic Layer

- `Next.js server actions / API routes`
- or separate lightweight backend later

Responsibilities:

- delivery-zone calculation
- subscription schedule creation
- Razorpay integration
- WhatsApp notification triggers
- address serviceability checks
- order-status bridge logic

### Database

- `PostgreSQL`

Responsibilities:

- custom delivery-zone data
- subscription records
- delivery schedules
- notification logs
- custom order status history
- admin operational records not handled by Shopify

## 3. Why Hybrid Architecture Is Best

Do not force everything into Shopify.

Reason:

- Shopify is good for commerce
- Sanity is better for flexible content
- custom backend is needed for local delivery and subscriptions

This split gives:

- faster launch
- safer commerce
- better content management
- more scalable custom logic

## 4. Core System Modules

### Module A: Storefront

User-facing website.

Key pages:

- Home
- Shop
- Product
- About
- Subscription
- FAQ
- Blog
- Contact
- Cart
- Checkout
- My Account
- Order Tracking

### Module B: Commerce

Handles:

- products
- variants
- orders
- promotions
- payments
- customer profiles

### Module C: Delivery Engine

Handles:

- address capture
- shop-distance calculation
- serviceability check
- delivery fee slab assignment
- delivery ETA assignment

### Module D: Subscription Engine

Handles:

- eligible products
- fixed plan selection
- prepaid subscription creation
- delivery schedule generation
- pause/resume/cancel logic
- renewal reminders

### Module E: Content Engine

Handles:

- banners
- campaigns
- homepage blocks
- blog
- FAQs
- recipes
- reviews

### Module F: Notification Engine

Handles:

- WhatsApp confirmations
- order status notifications
- subscription reminders
- delivery updates

## 5. Primary Data Domains

The backend should be organized into these domains:

- Catalog
- Customers
- Addresses
- Delivery Zones
- Orders
- Subscriptions
- Content
- Notifications
- Admin Users

## 6. Entity Model

### Product

Represents a sellable item.

Fields:

- `id`
- `slug`
- `name`
- `short_title`
- `description`
- `ingredients`
- `category_id`
- `is_subscription_enabled`
- `is_active`
- `brand_badges`
- `storage_instructions`
- `shelf_life`
- `seo_title`
- `seo_description`

### ProductVariant

Represents pack size / sellable version.

Fields:

- `id`
- `product_id`
- `label`
- `weight`
- `price`
- `sale_price`
- `sku`
- `stock_qty`
- `is_active`

### Category

Fields:

- `id`
- `name`
- `slug`
- `description`
- `image`
- `is_active`

### Customer

Fields:

- `id`
- `full_name`
- `phone`
- `email`
- `auth_provider`
- `default_address_id`
- `marketing_opt_in`
- `created_at`

### Address

Fields:

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
- `is_serviceable`

### DeliveryZone

Fields:

- `id`
- `name`
- `min_km`
- `max_km`
- `fee`
- `estimated_min_minutes`
- `estimated_max_minutes`
- `is_active`

### Order

Fields:

- `id`
- `customer_id`
- `order_type`
- `source`
- `subtotal`
- `discount_total`
- `delivery_fee`
- `grand_total`
- `payment_method`
- `payment_status`
- `status`
- `address_id`
- `delivery_zone_id`
- `notes`
- `placed_at`

### OrderItem

Fields:

- `id`
- `order_id`
- `product_id`
- `variant_id`
- `quantity`
- `unit_price`
- `total_price`

### Subscription

Fields:

- `id`
- `customer_id`
- `product_id`
- `variant_id`
- `plan_code`
- `quantity_per_delivery`
- `billing_cycle`
- `deliveries_in_cycle`
- `start_date`
- `end_date`
- `next_delivery_date`
- `next_billing_date`
- `plan_price`
- `discount_percent`
- `status`
- `address_id`
- `delivery_zone_id`

### SubscriptionDelivery

Fields:

- `id`
- `subscription_id`
- `scheduled_date`
- `quantity`
- `delivery_fee`
- `status`
- `fulfilled_order_id`
- `attempt_count`
- `delivered_at`

### NotificationLog

Fields:

- `id`
- `customer_id`
- `channel`
- `event_type`
- `recipient`
- `message_status`
- `provider_reference`
- `created_at`

## 7. Admin Role Model

Keep admin permissions simple in MVP.

### Super Admin

Can manage:

- system settings
- delivery zones
- staff accounts
- products
- subscriptions
- orders
- content

### Store Operator

Can manage:

- orders
- order statuses
- customer support actions
- subscriptions
- delivery schedules

### Content Manager

Can manage:

- banners
- homepage blocks
- blog
- FAQ
- offer content

### Delivery Coordinator

Can manage:

- out-for-delivery updates
- delivery completion
- failed delivery notes

## 8. Admin System Responsibilities

Admin must be able to:

- add products
- edit prices
- upload product images
- enable subscription for selected items
- create and update offers
- update homepage banners
- update blogs and FAQs
- configure delivery fees
- view orders by zone
- update delivery status
- view active subscriptions
- pause or cancel subscriptions

## 9. Content Model In Sanity

Recommended content types:

- `homepage`
- `banner`
- `announcement_bar`
- `category_page_content`
- `product_story_block`
- `faq_item`
- `blog_post`
- `review`
- `policy_page`
- `contact_page`

This gives Priya Foods-style content flexibility without coupling content updates to code releases.

## 10. Commerce Ownership Split

Use this split:

- Shopify owns: catalog, checkout, customer order core
- PostgreSQL owns: delivery engine, subscriptions, notification logs, operational data
- Sanity owns: editorial content
- Next.js composes everything into one storefront

## 11. API / Service Boundaries

### Public frontend reads

- product catalog
- category listing
- homepage content
- blog content
- FAQs

### Authenticated customer actions

- manage account
- save addresses
- create order
- buy subscription
- pause subscription
- cancel subscription
- view order history

### Admin actions

- update order status
- manage products
- manage subscriptions
- manage content
- manage delivery zones

## 12. Event Flow Examples

### One-time order flow

1. Customer selects products
2. Customer enters address
3. Delivery engine calculates zone and fee
4. Checkout shows final total
5. Payment succeeds or COD is selected
6. Order created
7. Operator processes order
8. Customer gets status updates

### Subscription flow

1. Customer logs in
2. Customer selects subscription plan
3. Address is validated
4. Razorpay payment succeeds
5. Subscription record is created
6. Future delivery schedule is generated
7. Reminder and fulfillment workflow begins

## 13. Security and Operational Controls

MVP should include:

- protected admin routes
- audit trail for status changes
- input validation for addresses and payments
- webhook verification for payment events
- image upload validation
- role-based admin permissions

## 14. Testing Areas

Critical backend tests:

- address serviceability
- delivery fee slab calculation
- COD restriction checks
- subscription activation
- subscription pause/resume
- scheduled delivery generation
- order status transitions
- Razorpay callback validation

## 15. Final Architecture Recommendation

Build Safa Foods as a hybrid commerce platform:

- `Next.js` for storefront and custom app logic
- `Shopify` for commerce core
- `Sanity` for content
- `PostgreSQL` for subscription and delivery operations

This is the best balance of launch speed, operational safety, and long-term scalability.
