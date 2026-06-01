# Safa Foods

Professional monorepo structure for the Safa Foods commerce platform.

## Project layout

- `frontend/`
  Next.js storefront and frontend API proxy routes
- `backend/`
  .NET backend, PostgreSQL migrations, SQL scripts, and backend architecture docs
- `docs/`
  business requirements, product planning, and early project specifications
- `.logs/`
  local build and diagnostic artifacts

## Working structure

### Frontend

- app: `frontend/`
- stack: `Next.js`, `TypeScript`, `Tailwind`, `Prisma` support for frontend-side development utilities

### Backend

- app: `backend/`
- stack: `.NET 9`, `ASP.NET Core Minimal APIs`, `EF Core`, `PostgreSQL`

## Key docs

- backend architecture: `backend/docs/BACKEND_ARCHITECTURE.md`
- API architecture: `backend/docs/API_ARCHITECTURE.md`
- database architecture: `backend/docs/DATABASE_ARCHITECTURE.md`
- product blueprint: `docs/PRODUCT_BLUEPRINT.md`
- MVP roadmap: `docs/MVP_ROADMAP.md`

## Goal

This repository is organized as a clean two-application structure:

- `frontend` for the customer/admin web experience
- `backend` for the business system, operations logic, and database lifecycle

That is the right professional baseline for this project.

## Local run

Run both frontend and backend from the repo root:

```powershell
npm install
npm run dev
```

Individual app commands:

```powershell
npm run dev:frontend
npm run dev:backend
```
