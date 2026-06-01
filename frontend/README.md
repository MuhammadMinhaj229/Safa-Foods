# Safa Foods Frontend

Customer-facing storefront and frontend integration layer for Safa Foods.

## Stack

- `Next.js`
- `TypeScript`
- `Tailwind CSS`
- `.NET API` integration layer

## Local development

First, run the development server:

```bash
npm run dev
# or
yarn dev
# or
pnpm dev
# or
bun dev
```

Open `http://localhost:3000` in the browser.

Main responsibilities:

- homepage and brand experience
- shop and product pages
- subscription and contact pages
- client-side consumption of the `.NET` backend APIs
- selective Next.js proxy routes where the frontend needs server-side forwarding

## Important paths

- app routes: `frontend/src/app`
- shared components: `frontend/src/components`
- business content/data: `frontend/src/data`
- backend integration helpers: `frontend/src/lib`

## Commands

- `npm run dev`
- `npm run build`
- `npm run lint`

## Notes

- environment examples are in `frontend/.env.example`
- this app is not a second backend and should not own database access
- backend service integration is documented in `backend/docs/API_ARCHITECTURE.md`
