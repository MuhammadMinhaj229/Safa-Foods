"use client";

import Link from "next/link";
import { HeartHandshake, Leaf, ShieldCheck, Truck } from "lucide-react";

import { Hero } from "@/components/home/Hero";
import { ProductCard } from "@/components/shop/ProductCard";
import {
  heroHighlights,
  homeCollections,
  journalEntries,
  products,
  promisePoints,
  subscriptionPlans,
} from "@/data/site";

const promiseIcons = [Leaf, ShieldCheck, HeartHandshake, Truck];

export const CustomerHome = () => {
  return (
    <div className="bg-[#f6f1e7] text-[#1f3322]">
      <Hero />
      <PromiseBand />
      <CollectionsSection />
      <SubscriptionSpotlight />
      <KitchenWisdomSection />
    </div>
  );
};

function PromiseBand() {
  return (
    <section className="bg-[#173d1d] px-4 py-12 text-[#efe5cf] sm:px-6 lg:px-8">
      <div className="mx-auto grid max-w-7xl gap-8 md:grid-cols-2 xl:grid-cols-4">
        {promisePoints.map((point, index) => {
          const Icon = promiseIcons[index];

          return (
            <div
              key={point.title}
              className="flex flex-col items-center gap-4 text-center sm:items-start sm:text-left"
            >
              <div className="flex h-16 w-16 items-center justify-center rounded-full border border-[#7d8c5f]/35 text-[#c1a566]">
                <Icon size={24} strokeWidth={1.5} />
              </div>
              <div>
                <p className="text-[12px] font-bold uppercase tracking-[0.38em]">{point.title}</p>
                <p className="mt-3 max-w-xs text-sm leading-7 text-[#d7d1c1]">{point.description}</p>
              </div>
            </div>
          );
        })}
      </div>
    </section>
  );
}

function CollectionsSection() {
  return (
    <section className="px-4 py-20 sm:px-6 lg:px-8">
      <div className="mx-auto max-w-7xl">
        <div className="grid gap-12 lg:grid-cols-[0.34fr_1fr] xl:gap-16">
          <div className="space-y-8">
            <div>
              <h2 className="font-serif text-5xl italic text-[#1f3322] sm:text-6xl">Collections</h2>
              <div className="mt-4 h-px bg-[#e2d8c7]" />
            </div>

            <nav className="space-y-6">
              {homeCollections.map((collection, index) => (
                <article key={collection.name} className="border-b border-[#ece2d2] pb-6">
                  <Link
                    href={collection.href}
                    className="block text-[15px] font-bold uppercase tracking-[0.28em] text-[#1f3322] transition hover:text-[#b49761]"
                  >
                    <span className={index === 0 ? "border-l border-[#b49761] pl-4" : ""}>
                      {collection.name}
                    </span>
                  </Link>
                  <p className="mt-3 max-w-xs text-base leading-7 text-[#6d7266]">
                    {collection.description}
                  </p>
                </article>
              ))}
            </nav>

            <div className="overflow-hidden rounded-[1.75rem] bg-[#173d1d] px-8 py-10 text-white shadow-[0_24px_60px_rgba(24,46,28,0.18)]">
              <h3 className="font-serif text-5xl italic leading-tight">Fresh Subscriptions</h3>
              <p className="mt-5 max-w-sm text-base leading-8 text-[#dde4d7]">
                Get weekly paste deliveries at a discount. Clear Monday scheduling, simple
                renewal, and local support.
              </p>
              <Link
                href="/subscription"
                className="mt-8 inline-flex w-full items-center justify-center border border-[#5b6d4f] px-8 py-4 text-[11px] font-bold uppercase tracking-[0.34em] text-[#e9d59b] transition hover:bg-white/5"
              >
                View Plans
              </Link>
            </div>
          </div>

          <div className="space-y-10">
            <div className="grid gap-8 lg:grid-cols-[1.1fr_0.9fr] xl:items-stretch">
              <div className="overflow-hidden rounded-[2rem] border border-[#e8decc] bg-white shadow-[0_18px_55px_rgba(38,29,16,0.06)]">
                <img
                  src="https://images.unsplash.com/photo-1515003197210-e0cd71810b5f?auto=format&fit=crop&q=80&w=1400"
                  alt="Safa collection feature"
                  className="h-72 w-full object-cover"
                />
                <div className="px-8 py-8">
                  <p className="text-[11px] font-bold uppercase tracking-[0.38em] text-[#b49761]">
                    This Week&apos;s Kitchen Base
                  </p>
                  <h3 className="mt-4 font-serif text-4xl italic leading-tight text-[#1f3322] sm:text-5xl">
                    Fresh paste, built for real home cooking
                  </h3>
                  <p className="mt-5 max-w-2xl text-lg leading-8 text-[#6d7266]">
                    Start with our hero ginger garlic paste, then build your kitchen with
                    garlic-only, green chilli, and future combo packs.
                  </p>
                </div>
              </div>

              <div className="rounded-[2rem] border border-[#e8decc] bg-[#faf6ee] px-8 py-9 shadow-[0_18px_55px_rgba(38,29,16,0.05)]">
                <p className="text-[11px] font-bold uppercase tracking-[0.38em] text-[#b49761]">
                  Why Homes Repeat Order
                </p>
                <div className="mt-8 space-y-8">
                  {[
                    "Balanced kitchen-ready flavour for curry, biryani, and marinades.",
                    "Clear weekly delivery rhythm for subscription customers.",
                    "Small-batch freshness instead of warehouse-style stock aging.",
                  ].map((line) => (
                    <div key={line} className="border-b border-[#e4dccb] pb-6 last:border-b-0 last:pb-0">
                      <p className="text-lg leading-8 text-[#4f5a50]">{line}</p>
                    </div>
                  ))}
                </div>
              </div>
            </div>

            <div className="grid gap-8 md:grid-cols-2">
              {products.map((product) => (
                <ProductCard
                  key={product.slug ?? product.name}
                  product={{
                    id: product.slug ?? product.name,
                    name: product.name,
                    price: parseDisplayPrice(product.price),
                    imagePath: imageForProduct(product.name),
                    sizes: product.sizes,
                    tag: product.badges[0],
                    specs: product.subtitle,
                  }}
                />
              ))}
            </div>

            <div className="overflow-hidden rounded-[2rem] border border-[#e5dccf] bg-white px-6 py-10 shadow-[0_18px_55px_rgba(38,29,16,0.06)] sm:px-8">
              <div className="flex flex-wrap items-center justify-between gap-6">
                <div>
                  <p className="text-[11px] font-bold uppercase tracking-[0.38em] text-[#b49761]">
                    Subscription Plans
                  </p>
                  <h3 className="mt-4 font-serif text-4xl italic text-[#1f3322] sm:text-5xl">
                    Never Run Out
                  </h3>
                </div>
                <Link
                  href="/subscription"
                  className="inline-flex items-center justify-center border border-[#d1c3a8] px-8 py-4 text-[11px] font-bold uppercase tracking-[0.34em] text-[#1f3322] transition hover:bg-[#f7f2e9]"
                >
                  Compare Plans
                </Link>
              </div>

              <div className="mt-10 grid gap-6 lg:grid-cols-3">
                {subscriptionPlans.map((plan) => (
                  <article
                    key={plan.code}
                    className={`border px-6 py-8 text-center ${
                      plan.featured
                        ? "border-[#173d1d] bg-[#173d1d] text-white"
                        : "border-[#ece3d2] bg-[#fbf9f4] text-[#1f3322]"
                    }`}
                  >
                    <h4 className="font-serif text-4xl italic">{plan.name}</h4>
                    <p
                      className={`mt-5 text-sm leading-7 ${
                        plan.featured ? "text-[#dfddcf]" : "text-[#717565]"
                      }`}
                    >
                      {plan.summary}
                    </p>
                    <div
                      className={`mx-auto mt-8 max-w-[11rem] px-5 py-5 text-center text-[12px] font-bold uppercase tracking-[0.18em] ${
                        plan.featured ? "bg-[#b49761] text-[#17311d]" : "bg-white"
                      }`}
                    >
                      Includes
                      <div className="mt-2 text-xl tracking-normal">{plan.includes}</div>
                    </div>
                    <p className="mt-8 text-5xl font-bold tracking-[-0.04em]">
                      {plan.price?.replace(" / month", "")}
                      <span className="text-xl font-medium opacity-70">/month</span>
                    </p>
                    <p
                      className={`mt-5 text-[11px] font-bold uppercase tracking-[0.34em] ${
                        plan.featured ? "text-[#dfddcf]" : "text-[#9c9b8e]"
                      }`}
                    >
                      {plan.savings}
                    </p>
                  </article>
                ))}
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>
  );
}

function SubscriptionSpotlight() {
  return (
    <section className="px-4 py-10 sm:px-6 lg:px-8">
      <div className="mx-auto max-w-7xl rounded-[2rem] border border-[#ece2d0] bg-white px-8 py-16 shadow-[0_16px_40px_rgba(43,30,16,0.05)]">
        <div className="mx-auto max-w-4xl text-center">
          <p className="text-[11px] font-bold uppercase tracking-[0.45em] text-[#b49761]">
            {heroHighlights.join(" | ")}
          </p>
          <h2 className="mt-8 font-serif text-[2.9rem] italic leading-[1] text-[#1f3322] sm:text-[5rem]">
            Subscribe &amp; Save
          </h2>
          <p className="mx-auto mt-6 max-w-3xl text-lg leading-8 text-[#6d7266] sm:text-xl sm:leading-9">
            Fresh paste delivered to your door on your schedule. Cancel anytime, no
            contracts, just real flavour and dependable local service.
          </p>
        </div>
      </div>
    </section>
  );
}

function KitchenWisdomSection() {
  return (
    <section className="px-4 pb-24 pt-10 sm:px-6 lg:px-8">
      <div className="mx-auto max-w-7xl rounded-[2rem] border border-[#ece2d0] bg-white px-6 py-16 shadow-[0_16px_50px_rgba(43,30,16,0.05)] sm:px-10">
        <div className="mx-auto max-w-4xl text-center">
          <h2 className="font-serif text-[3rem] italic leading-none text-[#1f3322] sm:text-[5rem]">
            Kitchen Wisdom
          </h2>
          <p className="mt-6 text-lg leading-8 text-[#6d7266] sm:text-xl sm:leading-9">
            Tips, recipes, and artisan techniques from our Warangal kitchen to yours.
          </p>
        </div>

        <div className="mt-16 grid gap-12 md:grid-cols-2">
          {journalEntries.map((entry) => (
            <article key={entry.title} className="space-y-6">
              <div className="overflow-hidden bg-[#f6f1e7] shadow-[0_18px_35px_rgba(47,35,18,0.08)]">
                <img
                  src={entry.image}
                  alt={entry.title}
                  className="h-64 w-full object-cover transition duration-700 hover:scale-[1.03]"
                />
              </div>
              <p className="text-[11px] font-bold uppercase tracking-[0.4em] text-[#b49761]">
                {entry.date}
              </p>
              <h3 className="max-w-lg font-serif text-4xl italic leading-tight text-[#1f3322] sm:text-5xl">
                {entry.title}
              </h3>
              <p className="max-w-xl text-lg leading-8 text-[#6d7266] sm:leading-9">{entry.excerpt}</p>
              <Link
                href={entry.href}
                className="inline-flex border-b border-[#1f3322] pb-2 text-[11px] font-bold uppercase tracking-[0.34em] text-[#1f3322]"
              >
                Read Post
              </Link>
            </article>
          ))}
        </div>
      </div>
    </section>
  );
}

function imageForProduct(name: string) {
  if (/garlic/i.test(name) && !/ginger/i.test(name)) {
    return "https://images.unsplash.com/photo-1509358273864-bdbaa9bf939f?auto=format&fit=crop&q=80&w=1200";
  }

  if (/green chilli/i.test(name)) {
    return "https://images.unsplash.com/photo-1604329760661-e71c0c144ce1?auto=format&fit=crop&q=80&w=1200";
  }

  return "https://images.unsplash.com/photo-1515003197210-e0cd71810b5f?auto=format&fit=crop&q=80&w=1200";
}

function parseDisplayPrice(price: string) {
  const parsed = Number(price.replace(/[^\d.]/g, ""));
  return Number.isFinite(parsed) ? parsed : 0;
}
