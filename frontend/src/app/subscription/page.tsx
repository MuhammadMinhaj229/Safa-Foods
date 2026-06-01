import { CheckCircle2, Clock3, Truck } from "lucide-react";

import { subscriptionPlans } from "@/data/site";

const steps = [
  {
    title: "Choose Your Plan",
    description: "Select the size and frequency that fits your kitchen needs.",
    icon: CheckCircle2,
  },
  {
    title: "We Deliver Fresh",
    description: "Made fresh that morning and delivered directly to your doorstep.",
    icon: Truck,
  },
  {
    title: "Flexible Always",
    description: "Pause, skip, or cancel your subscription with one click anytime.",
    icon: Clock3,
  },
];

export default function SubscriptionPage() {
  return (
    <main className="bg-[#f6f1e7] px-4 pt-40 pb-20 sm:px-6 lg:px-8">
      <div className="mx-auto max-w-7xl space-y-12">
        <section className="rounded-[2rem] border border-[#e7dfcf] bg-white px-6 py-16 text-center shadow-[0_16px_45px_rgba(43,30,16,0.05)] sm:px-10">
          <h1 className="font-serif text-[3.7rem] italic leading-none text-[#1f3322] sm:text-[5.2rem]">
            Never Run Out —
            <span className="block text-[#b49761]">Subscribe & Save</span>
          </h1>
          <p className="mx-auto mt-8 max-w-4xl text-xl leading-9 text-[#6d7266]">
            Fresh paste delivered to your door on your schedule. Cancel anytime, no contracts,
            just real flavour.
          </p>
        </section>

        <section className="rounded-[2rem] border border-[#e4dccb] bg-white px-6 py-14 shadow-[0_14px_40px_rgba(43,30,16,0.05)] sm:px-10">
          <div className="grid gap-8 lg:grid-cols-3">
            {subscriptionPlans.map((plan) => (
              <article
                key={plan.code}
                className={`border px-8 py-10 text-center ${
                  plan.featured
                    ? "border-[#173d1d] bg-[#173d1d] text-white"
                    : "border-[#ece3d2] bg-[#fbf9f4] text-[#1f3322]"
                }`}
              >
                <h2 className="font-serif text-5xl italic">{plan.name}</h2>
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
                <button
                  type="button"
                  className={`mt-8 inline-flex items-center justify-center px-8 py-4 text-[11px] font-bold uppercase tracking-[0.34em] ${
                    plan.featured ? "bg-[#b49761] text-[#17311d]" : "bg-[#173d1d] text-white"
                  }`}
                >
                  Select Subscription
                </button>
                <p className={`mt-6 text-[11px] font-bold uppercase tracking-[0.34em] ${plan.featured ? "text-[#d9ddd2]" : "text-[#a19b8f]"}`}>
                  {plan.savings}
                </p>
              </article>
            ))}
          </div>
        </section>

        <section className="rounded-[2rem] border border-[#e4dccb] bg-white px-6 py-14 shadow-[0_14px_40px_rgba(43,30,16,0.05)] sm:px-10">
          <div className="grid gap-10 md:grid-cols-3">
            {steps.map((step) => {
              const Icon = step.icon;

              return (
                <article key={step.title} className="text-center">
                  <div className="mx-auto flex h-20 w-20 items-center justify-center rounded-full border border-[#d4c7ae] text-[#b49761]">
                    <Icon size={32} strokeWidth={1.5} />
                  </div>
                  <h3 className="mt-6 text-[15px] font-bold uppercase tracking-[0.32em] text-[#1f3322]">
                    {step.title}
                  </h3>
                  <p className="mt-5 text-lg leading-9 text-[#6d7266]">{step.description}</p>
                </article>
              );
            })}
          </div>
        </section>
      </div>
    </main>
  );
}
