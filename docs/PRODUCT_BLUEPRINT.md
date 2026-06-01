# Safa Foods Product Blueprint

## 1. Business Summary

Safa Foods is a hyperlocal fresh-food and condiments brand serving Warangal and Hanakonda. The first hero product is premium hygienic fresh ginger-garlic paste, followed by fresh garlic paste, green chilli paste, and selected homemade food items.

The website should combine:

- brand trust like Priya Foods
- freshness and local convenience like Batter Bowl
- subscription and repeat ordering
- WhatsApp-led customer engagement
- local delivery operations for a limited geography

Core brand promise:

- Fresh
- Hygienic
- Premium quality
- Homemade-style taste
- Reliable local delivery

## 2. Target Market

- Families in Warangal and Hanakonda
- Daily and weekly kitchen shoppers
- Working households that value convenience
- Customers who prefer fresh local products over factory-packed alternatives
- Repeat buyers who can convert to subscriptions

## 3. Initial Product Catalog

Launch products:

- Premium Ginger Garlic Paste
- Fresh Garlic Paste
- Green Chilli Paste
- Homemade food items

Each product should support:

- name
- short subtitle
- description
- ingredients
- freshness note
- hygiene note
- weight / pack size
- shelf-life
- storage instructions
- price
- delivery estimate
- subscription eligibility
- offer / discount badge
- product images

## 4. Delivery Model

Primary service areas:

- Warangal
- Hanakonda

Store / dispatch reference:

- Near Narsampet Road
- Beside Decent Function Hall

Operational rules:

- Shipping is not hidden
- Delivery charge is based on location
- Recommended MVP slab:
  - `0 to 3 km` -> `Rs. 10`
  - `3 to 8 km` -> `Rs. 20`
  - `8+ km serviceable area` -> `Rs. 40`
- Final payable amount = product total + location-based delivery charge - active discount
- Delivery fee should be charged per order, not per product
- COD is only for immediate orders, not subscriptions
- Delivery status should update customer-facing order status similar to ecommerce norms:
  - order placed
  - confirmed
  - preparing
  - out for delivery
  - delivered

## 5. Payments and Ordering Logic

### Order Types

- Buy now
- Subscription

### Buy Now

- Guest checkout may be supported later, but account-based checkout is better from day one
- COD allowed for eligible current deliveries
- Razorpay allowed

### Subscription

- User creates an account with:
  - full name
  - phone number
  - email
  - delivery address
- User selects subscription-eligible product
- User chooses delivery frequency and duration
- User pays monthly subscription amount through Razorpay
- COD is not allowed for subscriptions

### Subscription Display Requirements

Each product page should clearly show:

- available subscription option
- monthly price
- billing cycle
- delivery frequency
- next delivery date
- included quantity
- pause / resume rules
- cancellation rules
- savings compared to one-time purchase

## 6. Primary Channels

Business and marketing channels:

- WhatsApp
- Instagram
- Facebook

Website requirements for channel integration:

- persistent WhatsApp CTA
- click-to-chat buttons on product and contact pages
- Instagram content embedding or feed blocks later
- Facebook / Meta Pixel integration
- lead capture for remarketing

## 7. Content Strategy

The site should be updated regularly, similar to Priya Foods, with fresh content and active catalog updates.

Content types:

- homepage promotional banners
- offer banners
- seasonal product updates
- product detail updates
- blog articles
- recipe ideas
- storage and usage tips
- freshness and hygiene messaging
- customer reviews

Admin team must be able to update:

- prices
- offers
- stock status
- product images
- homepage sections
- blog content
- FAQ entries

## 8. Must-Have Pages

- Home
- Shop
- Category pages
- Product detail page
- Subscription page
- About
- Contact
- FAQ
- Blog
- Cart
- Checkout
- My Account
- Order Tracking
- Shipping Policy
- Privacy Policy
- Terms and Conditions

## 9. Homepage Structure

Recommended homepage sections:

1. Hero banner
   - premium fresh ginger-garlic paste
   - clear `Order Now` and `Subscribe` CTAs
2. Trust badges
   - fresh
   - hygienic
   - homemade taste
   - local delivery
3. Featured categories
4. Best-selling products
5. Subscription highlight
6. Why Safa Foods
7. Delivery areas
8. Offer / discount strip
9. Customer reviews
10. FAQ preview
11. Blog / recipe highlights
12. Footer with contact and policy links

## 10. Recommended Tech Stack

### Final Recommendation

- Frontend: Next.js
- UI: Tailwind CSS + shadcn/ui
- Commerce: Shopify
- CMS: Sanity
- Payments: Razorpay
- Media: Cloudinary
- Analytics: GA4 + Meta Pixel
- Hosting: Vercel

### Why this stack

- fast mobile performance
- scalable beyond local launch
- strong SEO control
- easy content updates
- reliable admin flows
- easier future integration for subscriptions, offers, and campaigns

### Alternative if full custom ownership is required later

- Next.js
- Medusa
- PostgreSQL
- Sanity
- Razorpay

This is more flexible, but slower to launch than Shopify.

## 11. MVP Feature Set

Phase 1 MVP should include:

- branded homepage
- product catalog
- product detail pages
- account creation
- address capture
- Razorpay checkout
- COD for eligible instant orders
- subscription purchase flow
- delivery charge by area
- order status timeline
- WhatsApp support CTA
- admin product/content updates
- SEO-ready structure

## 12. Non-Negotiable UX Rules

- mobile-first design
- clear pricing
- no hidden charges
- visible delivery area messaging
- visible subscription savings
- fast checkout
- trust-focused product pages
- sticky WhatsApp action
- clean and premium visual design

## 13. Risks To Solve Early

- defining exact delivery charge logic by area
- deciding subscription frequency model
- defining homemade food SKU availability rules
- setting order status update workflow for delivery staff
- determining who updates content and how often
- clarifying shelf-life and food safety statements for every product

## 14. Build Decision

This project should be built as a serious local commerce product, not just a brochure website. The first goal is to dominate repeat fresh-condiment orders in Warangal and Hanakonda with a premium, trustworthy, mobile-first ordering experience.
