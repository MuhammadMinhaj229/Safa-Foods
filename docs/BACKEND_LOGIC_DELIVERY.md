# Safa Foods Delivery Backend Logic

## 1. Current Business Rule

Store coverage is hyperlocal around the shop.

Base service rule:

- Deliver within roughly 8 km from the shop
- For nearby locations, add a small delivery fee
- For farther locations, add a higher delivery fee

Current proposed rule from business:

- Near area: add `Rs. 10` to `Rs. 20`
- Farther area: add `Rs. 40`

## 2. Recommended MVP Rule

Do not start with Instamart-style dynamic pricing in version 1.

Reason:

- it adds operational complexity
- it requires tighter geo data and demand logic
- it is harder to explain to customers
- it can create support issues if charges change unexpectedly

Use a simple, transparent slab model first.

### Delivery Fee Slabs

- Zone A: `0 to 3 km` -> `Rs. 10`
- Zone B: `3 to 8 km` -> `Rs. 20`
- Zone C: `8+ km but serviceable` -> `Rs. 40`

This is the best MVP approach because it is easy to operate, easy to show in checkout, and easy to explain on the website.

## 3. Better Alternative To "40 for every product"

Do not charge `Rs. 40 per product`.

That will feel expensive very quickly and will reduce conversion.

Better rule:

- delivery fee is charged per order
- not per product

Example:

- 1 jar order to a far zone -> `Rs. 40` delivery
- 3 jars order to the same far zone -> still `Rs. 40` delivery

This is how customers usually expect local commerce pricing to work.

If needed, you can later add:

- very small basket handling fee
- free delivery threshold
- surge fee during peak hours

But those should not be in MVP.

## 4. Recommended Checkout Formula

Final payable amount:

`order subtotal - discount + delivery fee`

Where:

- `order subtotal` = product prices x quantities
- `discount` = active coupon / offer / subscription saving
- `delivery fee` = based on delivery zone slab

Do not hide charges.

Checkout should clearly show:

- subtotal
- discount
- delivery fee
- total

## 5. Serviceability Logic

At checkout, backend should determine whether an address is serviceable.

### Required inputs

- customer name
- phone number
- email
- full address
- landmark
- pincode
- lat/lng from map pin or address lookup

### Serviceability decision

1. Calculate distance from shop coordinates
2. Assign zone
3. Return:
   - `serviceable: true/false`
   - `zone: A/B/C`
   - `delivery_fee`
   - `estimated_delivery_time`

### Shop location source

Use the fixed shop coordinates for:

- near Narsampet Road
- beside Decent Function Hall

Exact coordinates should be saved in admin config.

## 6. Data Model

### DeliveryZone

- `id`
- `name`
- `min_km`
- `max_km`
- `delivery_fee`
- `estimated_minutes_min`
- `estimated_minutes_max`
- `is_active`

### DeliveryAddress

- `id`
- `user_id`
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
- `zone_id`
- `is_serviceable`

### Order

- `id`
- `user_id`
- `order_type` (`one_time` or `subscription`)
- `subtotal`
- `discount_total`
- `delivery_fee`
- `grand_total`
- `payment_method`
- `payment_status`
- `order_status`
- `delivery_address_id`
- `zone_id`

## 7. COD Rules

Recommended COD logic:

- COD allowed only for `one_time` orders
- COD not allowed for subscriptions
- COD allowed only if address is serviceable
- optional: COD allowed only above a minimum order amount

Suggested MVP rule:

- COD available only for one-time orders in Zone A and Zone B
- Zone C can be prepaid only if you want to reduce delivery risk

That is optional, but operationally safer.

## 8. Subscription Delivery Logic

Subscriptions should not behave like instant delivery orders.

Each subscription must store:

- user
- product
- quantity
- billing cycle
- delivery frequency
- next billing date
- next delivery date
- subscription status

For subscription orders:

- payment is prepaid through Razorpay
- delivery fee should be included in plan pricing or shown clearly before purchase
- backend should create scheduled orders automatically

### Recommended MVP subscription model

Keep subscriptions simple:

- weekly plan
- monthly plan

For example:

- weekly: 1 or more deliveries per week
- monthly: fixed number of deliveries in a month

Do not start with too many custom rules.

## 9. Admin Logic

Admin panel must support:

- updating zone ranges
- changing zone delivery fees
- marking areas serviceable / not serviceable
- changing estimated delivery times
- viewing orders by zone
- viewing subscription schedule
- manually updating order status

## 10. Rider / Delivery Status Logic

MVP order status flow:

- `placed`
- `confirmed`
- `preparing`
- `out_for_delivery`
- `delivered`
- `cancelled`

When order becomes `out_for_delivery`, customer should receive:

- WhatsApp message
- optional SMS later

When order becomes `delivered`, customer should receive:

- confirmation notification

In MVP, delivery person does not need a full app.

Better initial approach:

- admin or store operator updates status from panel
- later add rider dashboard or delivery OTP

## 11. Discount Logic

Use simple offer rules first, similar in feel to Instamart but not equal in complexity.

Recommended offer types:

- product-level discount
- cart-level discount
- free delivery above threshold
- subscription savings badge

Examples:

- `Rs. 20 off on 2 jars`
- `Free delivery above Rs. 299`
- `Save 8% with monthly subscription`

## 12. Final Recommendation

For MVP, use this rule set:

- delivery fee is per order, not per product
- `0 to 3 km = Rs. 10`
- `3 to 8 km = Rs. 20`
- `8+ km serviceable areas = Rs. 40`
- COD only for one-time orders
- subscriptions are prepaid only
- show delivery fee transparently before payment

This is much better than trying to copy Instamart logic on day one.
