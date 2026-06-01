# Safa Foods Backend

## Architecture docs

Detailed architecture is documented here:

- `backend/docs/BACKEND_ARCHITECTURE.md`
- `backend/docs/API_ARCHITECTURE.md`
- `backend/docs/DATABASE_ARCHITECTURE.md`

## Stack

- `.NET 9`
- `ASP.NET Core Minimal APIs`
- `EF Core`
- `PostgreSQL`

## Local database

The backend can use the `SAFA_FOODS_POSTGRES` environment variable for EF design-time and database update commands.

For normal app runtime configuration, prefer standard .NET environment variables:

- `ConnectionStrings__Postgres`
- `AdminAccess__OwnerApiKey`
- `AdminAccess__StaffApiKey`
- `WhatsApp__Provider`
- `WhatsApp__BusinessPhoneNumber`
- `WhatsApp__SupportPhoneNumber`
- `WhatsApp__MetaAccessToken`
- `WhatsApp__MetaPhoneNumberId`
- `WhatsApp__MetaWebhookVerifyToken`
- `WhatsApp__MetaAppSecret`
- `WhatsApp__MetaApiVersion`
- `WhatsApp__MetaGraphBaseUrl`

An example file is available at:

- `backend/.env.example`

Example connection string shape:

```txt
Host=localhost;Port=5432;Database=Safa Foods;Username=postgres;Password=your-password
```

## Admin access

Protected admin endpoints require the `X-Admin-Key` header.

Configured keys live in:

- `backend/src/SafaFoods.Api/appsettings.json`
- or environment-specific configuration

Admin bearer-token endpoints now also support:

- `POST /api/admin/auth/logout`
- `POST /api/admin/auth/logout-all`

## WhatsApp dispatch

Notification dispatch is provider-based.

Current providers:

- `simulated`
- `meta`

`simulated` is the safe default for development.
`meta` now has a real HTTP transport shape for the Meta Cloud API, but it still requires valid credentials and a public deployment to be used live.

For local development before a real WhatsApp Business setup exists, notifications can be routed to one fixed number:

- `WhatsApp__UseDevelopmentRecipientOverride=true`
- `WhatsApp__DevelopmentRecipientOverride=your-test-recipient`

## Meta webhook readiness

Future Meta WhatsApp linkage is prepared with:

- `GET /api/integrations/meta/whatsapp/webhook`
- `POST /api/integrations/meta/whatsapp/webhook`

Incoming webhook payloads are stored in the database for later processing and status mapping.

## Key endpoint groups

- `/api/system`
- `/api/catalog`
- `/api/delivery`
- `/api/addresses`
- `/api/orders`
- `/api/subscriptions`
- `/api/payments`
- `/api/admin`

## Current operational flow

- customers create addresses and one-time orders
- customers submit manual prepaid references for UPI-style payments
- staff confirms payments through admin endpoints
- subscriptions activate after payment confirmation
- notifications are logged and can be dispatched through the admin notification dispatch endpoint
