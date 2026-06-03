# SAFA Static Landing Page

Production-ready static landing page for **SAFA**, a local household essentials platform delivering fresh vegetables, fruits, eggs, and daily essentials with future expansion into Safa Foods and Safa Services.

## Architecture

No framework and no build step.

```text
index.html
thank-you.html
privacy-policy.html
terms.html
robots.txt
sitemap.xml
_redirects
css/
  style.css
js/
  main.js
assets/
  logo.png
  hero/
  products/
  icons/
```

## Goals

- Present SAFA as a premium, trustworthy local essentials brand.
- Convert visitors into WhatsApp leads and first-time orders.
- Communicate convenience, quality, and reliability.
- Stay lightweight for Netlify static deployment.
- Support future expansion into Safa Fresh, Safa Foods, and Safa Services.

## Maintenance

- Keep the primary landing page in `index.html`.
- Keep visual system and responsive layout in `css/style.css`.
- Keep WhatsApp logic, quick order generation, mobile nav, and active-section highlighting in `js/main.js`.
- Replace the placeholder WhatsApp number in `js/main.js` before launch.
- Update `sitemap.xml`, canonical URLs, and Open Graph URLs when the production domain is finalized.
