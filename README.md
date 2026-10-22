# Safa Foods & Wholesale — Supermarket Grocery Ordering & Household Supply Platform

[![Brand](https://img.shields.io/badge/Client-Safa_Fresh_Supermarket-green?style=flat-square)](https://alif-growth.netlify.app)
[![Container](https://img.shields.io/badge/Docker-Ready-blue?style=flat-square&logo=docker&logoColor=white)](https://docker.com)

**Safa Foods & Wholesale** is an integrated household grocery supply and order automation platform engineered for local supermarkets and wholesale grain distributors. It transforms chaotic phone-call orders into clean, organized digital dispatches with automated WhatsApp invoice delivery.

---

## Key Capabilities

- **Weekly Household Order Form:** Simplified mobile interface for families to submit repeat weekly grocery lists.
- **Automated SKU & Inventory Reconciliation:** Matches incoming orders against current supermarket stock sheets (`.xlsx`).
- **1-Tap WhatsApp Invoice Dispatch:** Automatically tallies totals and sends payment links (UPI/Cash on Delivery) via WhatsApp.
- **Multi-Service Architecture:** Includes .NET backend solution (`SafaFoods.sln`) and containerized Python order parsing microservices.

---

## How to Run Locally

```bash
git clone https://github.com/MuhammadMinhaj229/Safa-Foods.git
cd Safa-Foods

# Running with Docker
docker build -t safa-foods-app .
docker run -p 5000:5000 safa-foods-app
```

---

## Author
Engineered by **Mohammed Minhaj Mahmood** for Alif Growth Media.
