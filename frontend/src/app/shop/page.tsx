"use client";

import { useEffect, useState } from "react";
import Link from "next/link";

import { ProductCard } from "@/components/shop/ProductCard";
import { homeCollections, subscriptionPlans } from "@/data/site";
import { Product } from "@/lib/api/types";
import { catalogService } from "@/lib/services/catalog.service";

export default function ShopPage() {
  const [products, setProducts] = useState<Product[]>([]);

  useEffect(() => {
    catalogService.getProducts().then(setProducts);
  }, []);

  return (
    <main className="bg-[#f6f1e7] px-4 pt-40 pb-20 sm:px-6 lg:px-8">
      <div className="mx-auto max-w-7xl space-y-16">
        <section className="overflow-hidden rounded-[2rem] border border-[#e3d9c8] bg-white shadow-[0_22px_70px_rgba(45,32,16,0.06)]">
          <img
            src="https://images.unsplash.com/photo-1515003197210-e0cd71810b5f?auto=format&fit=crop&q=80&w=1600"
            alt="Safa Foods collections"
            className="h-[18rem] w-full object-cover sm:h-[24rem]"
          />
          <div className="grid gap-8 bg-[#173d1d] px-6 py-10 text-[#ecdfbf] sm:grid-cols-2 lg:grid-cols-4 lg:px-10">
            {["All Natural", "Hygienic", "Social Impact", "Daily Fresh"].map((item) => (
              <div key={item} className="text-center">
                <p className="text-[11px] font-bold uppercase tracking-[0.4em]">{item}</p>
              </div>
            ))}
          </div>
        </section>

        <section className="grid gap-14 lg:grid-cols-[0.34fr_1fr]">
          <aside className="space-y-8">
            <div>
              <h1 className="font-serif text-6xl italic text-[#1f3322]">Collections</h1>
              <div className="mt-4 h-px bg-[#e3d9c8]" />
            </div>
            <div className="space-y-6">
              {homeCollections.map((collection, index) => (
                <Link
                  key={collection.name}
                  href={collection.href}
                  className="block text-[15px] font-bold uppercase tracking-[0.28em] text-[#1f3322]"
                >
                  <span className={index === 0 ? "border-l border-[#b49761] pl-4" : ""}>
                    {collection.name}
                  </span>
                </Link>
              ))}
            </div>

            <div className="rounded-[2rem] bg-[#173d1d] px-8 py-10 text-white shadow-[0_24px_60px_rgba(24,46,28,0.18)]">
              <h2 className="font-serif text-5xl italic leading-tight">Fresh Subscriptions</h2>
              <p className="mt-5 text-lg leading-8 text-[#dde4d7]">
                Get weekly paste deliveries at a 15% discount. Cancel anytime.
              </p>
              <Link
                href="/subscription"
                className="mt-8 inline-flex w-full items-center justify-center border border-[#5b6d4f] px-8 py-4 text-[11px] font-bold uppercase tracking-[0.34em] text-[#e9d59b] transition hover:bg-white/5"
              >
                View Plans
              </Link>
            </div>
          </aside>

          <div className="space-y-10">
            <div className="flex flex-col gap-6 border-b border-[#e3d9c8] pb-6 sm:flex-row sm:items-end sm:justify-between">
              <h2 className="font-serif text-7xl italic text-[#1f3322]">All Pastes</h2>
              <p className="text-[12px] font-bold uppercase tracking-[0.32em] text-[#8d866f]">
                Sort: Newest First
              </p>
            </div>

            <div className="grid gap-10 md:grid-cols-2">
              {products.map((product) => (
                <ProductCard key={product.id} product={product} />
              ))}
            </div>
          </div>
        </section>

        <section className="rounded-[2rem] border border-[#e4dccb] bg-white px-6 py-14 shadow-[0_14px_40px_rgba(43,30,16,0.05)] sm:px-10">
          <div className="mx-auto max-w-5xl text-center">
            <h2 className="font-serif text-[3.6rem] italic leading-none text-[#1f3322] sm:text-[5rem]">
              Never Run Out —
              <span className="block text-[#b49761]">Subscribe & Save</span>
            </h2>
            <p className="mx-auto mt-6 max-w-3xl text-xl leading-9 text-[#6d7266]">
              Fresh paste delivered to your door on your schedule. Cancel anytime, no
              contracts, just real flavour.
            </p>
          </div>

          <div className="mt-12 grid gap-8 lg:grid-cols-3">
            {subscriptionPlans.map((plan) => (
              <article
                key={plan.code}
                className={`border px-8 py-10 text-center ${
                  plan.featured
                    ? "border-[#173d1d] bg-[#173d1d] text-white"
                    : "border-[#ece3d2] bg-[#fbf9f4] text-[#1f3322]"
                }`}
              >
                <h3 className="font-serif text-5xl italic">{plan.name}</h3>
                <p className={`mt-5 text-lg leading-8 ${plan.featured ? "text-[#d9ddd2]" : "text-[#6d7266]"}`}>
                  {plan.summary}
                </p>
                <div className={`mx-auto mt-8 max-w-[12rem] px-5 py-5 text-[12px] font-bold uppercase tracking-[0.18em] ${plan.featured ? "bg-[#b49761] text-[#17311d]" : "bg-white"}`}>
                  Includes
                  <div className="mt-2 text-xl tracking-normal">{plan.includes}</div>
                </div>
                <p className="mt-8 text-5xl font-bold tracking-[-0.04em]">
                  {plan.price?.replace(" / month", "")}
                  <span className="text-xl font-medium opacity-70">/month</span>
                </p>
                <Link
                  href="/subscription"
                  className={`mt-8 inline-flex items-center justify-center px-8 py-4 text-[11px] font-bold uppercase tracking-[0.34em] ${
                    plan.featured ? "bg-[#b49761] text-[#17311d]" : "bg-[#173d1d] text-white"
                  }`}
                >
                  Select Subscription
                </Link>
              </article>
            ))}
          </div>
        </section>
      </div>
    </main>
  );
}
