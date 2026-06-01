import { DELIVERY_SLABS } from "@/server/delivery";
import { SUBSCRIPTION_PLANS } from "@/server/subscriptions";

export default function BackendPage() {
  return (
    <main className="mx-auto w-full max-w-7xl px-5 py-16 sm:px-8">
      <div className="max-w-3xl space-y-5">
        <p className="section-kicker">Backend foundation</p>
        <h1 className="section-title">The app now has a real backend-ready contract.</h1>
        <p className="text-lg leading-8 text-[var(--color-olive)]">
          Environment validation, Prisma schema, delivery slabs, subscription quote logic, and first API route shapes are all in place.
        </p>
      </div>

      <div className="mt-12 grid gap-8 lg:grid-cols-2">
        <section className="rounded-[2rem] bg-[var(--color-surface)] p-8">
          <p className="text-sm uppercase tracking-[0.16em] text-[var(--color-leaf)]">
            Delivery slabs
          </p>
          <div className="mt-6 space-y-4">
            {DELIVERY_SLABS.map((slab) => (
              <div key={slab.label} className="rounded-2xl bg-[var(--color-sand)] p-4">
                <p className="font-semibold text-[var(--color-ink)]">{slab.label}</p>
                <p className="text-[var(--color-olive)]">
                  {slab.minKm} km to {slab.maxKm ?? "service limit"} km
                </p>
                <p className="text-[var(--color-olive)]">
                  Rs. {slab.fee} · {slab.etaMinutes[0]} to {slab.etaMinutes[1]} mins
                </p>
              </div>
            ))}
          </div>
        </section>

        <section className="rounded-[2rem] bg-[var(--color-forest-deep)] p-8 text-[var(--color-surface)]">
          <p className="text-sm uppercase tracking-[0.16em] text-[var(--color-accent)]">
            Subscription plans
          </p>
          <div className="mt-6 space-y-4">
            {Object.values(SUBSCRIPTION_PLANS).map((plan) => (
              <div key={plan.code} className="rounded-2xl bg-white/5 p-4">
                <p className="font-semibold">{plan.name}</p>
                <p className="text-[var(--color-muted)]">
                  {plan.deliveriesInCycle} deliveries per {plan.billingCycle}
                </p>
                <p className="text-[var(--color-muted)]">Discount: {plan.discountPercent}%</p>
              </div>
            ))}
          </div>
        </section>
      </div>
    </main>
  );
}
