"use client";

import Link from "next/link";
import { useEffect, useRef, useState } from "react";
import {
  Baby,
  Briefcase,
  Globe2,
  Heart,
  Leaf,
  MapPin,
  MessageCircle,
  PackageCheck,
  ShieldCheck,
  Sparkles,
  Truck,
  Users,
} from "lucide-react";

import { Hero } from "@/components/home/Hero";
import { ServiceGrid } from "@/components/home/ServiceGrid";
import { brand } from "@/data/site";

export const CustomerHome = () => {
  return (
    <div className="bg-[#f6f1e7] text-[#1f3322]">
      <Hero />
      <ServiceGrid />
      <HowItWorks />
      <WhyTrustSafa />
      <FutureVision />
      <FinalCTA />
    </div>
  );
};

/* ------------------------------------------------------------------ */
/* Scroll reveal helper                                                */
/* ------------------------------------------------------------------ */

function Reveal({
  children,
  className = "",
  delay = 0,
}: {
  children: React.ReactNode;
  className?: string;
  delay?: number;
}) {
  const ref = useRef<HTMLDivElement>(null);
  const [visible, setVisible] = useState(false);

  useEffect(() => {
    const node = ref.current;
    if (!node) return;

    const observer = new IntersectionObserver(
      ([entry]) => {
        if (entry.isIntersecting) {
          setVisible(true);
          observer.disconnect();
        }
      },
      { threshold: 0.15 },
    );

    observer.observe(node);
    return () => observer.disconnect();
  }, []);

  return (
    <div
      ref={ref}
      style={{ transitionDelay: `${delay}ms` }}
      className={`transition-all duration-1000 ease-[cubic-bezier(0.16,1,0.3,1)] ${
        visible ? "translate-y-0 opacity-100" : "translate-y-10 opacity-0"
      } ${className}`}
    >
      {children}
    </div>
  );
}

function SectionEyebrow({ children }: { children: React.ReactNode }) {
  return (
    <p className="text-[11px] font-bold uppercase tracking-[0.42em] text-[#b49761]">
      {children}
    </p>
  );
}

/* ------------------------------------------------------------------ */
/* 2. Problem                                                          */
/* ------------------------------------------------------------------ */


/* ------------------------------------------------------------------ */
/* 3. Introduce Safa                                                   */
/* ------------------------------------------------------------------ */


/* ------------------------------------------------------------------ */
/* 4. For Every Home                                                   */
/* ------------------------------------------------------------------ */

const homeAudiences = [
  {
    icon: Briefcase,
    title: "For Busy Professionals",
    description:
      "No time for daily market runs? Keep your kitchen stocked with one message and cook a real meal even on your busiest days.",
  },
  {
    icon: Users,
    title: "For Growing Families",
    description:
      "When someone should look after your home the way your own family would, Safa quietly keeps the essentials flowing.",
  },
  {
    icon: Baby,
    title: "For Parents & Elders",
    description:
      "Fresh, clean, preservative-free food you can trust for the people who raised you and the little ones you are raising.",
  },
];


/* ------------------------------------------------------------------ */
/* 5. Homemade Products                                                */
/* ------------------------------------------------------------------ */

const liveProducts = [
  {
    slug: "premium-ginger-garlic-paste",
    name: "Ginger Garlic Paste",
    note: "Our hero. Balanced, kitchen-ready flavour for curry, biryani and marinades.",
    price: "Rs. 99",
    image: "/images/product-ginger-garlic.png",
  },
  {
    slug: "turmeric-powder",
    name: "Turmeric",
    note: "Pure, vibrant and aromatic turmeric ground for everyday home cooking.",
    price: "Rs. 89",
    image: "/images/product-turmeric.png",
  },
  {
    slug: "red-chilli-powder",
    name: "Red Chilli",
    note: "Deep colour and honest heat, milled fresh for authentic Telangana flavour.",
    price: "Rs. 79",
    image: "/images/product-red-chilli.png",
  },
];

const comingSoon = [
  { name: "Garlic Paste", phase: "Phase 2" },
  { name: "Green Chilli Paste", phase: "Phase 2" },
  { name: "Festival Combo Packs", phase: "Phase 3" },
];


/* ------------------------------------------------------------------ */
/* 6. For Families Abroad                                              */
/* ------------------------------------------------------------------ */


/* ------------------------------------------------------------------ */
/* 7. How It Works                                                     */
/* ------------------------------------------------------------------ */

const steps = [
  {
    icon: MessageCircle,
    title: "Message Us",
    description:
      "Send your order on WhatsApp. No apps, no forms &mdash; just a simple message like you would to family.",
  },
  {
    icon: PackageCheck,
    title: "We Make It Fresh",
    description:
      "Your essentials are prepared in small, hygienic batches the same day and packed with care.",
  },
  {
    icon: Truck,
    title: "Delivered To You",
    description:
      "We deliver locally across Warangal and Hanamkonda, with clear pricing and quick, reliable service.",
  },
];

function HowItWorks() {
  return (
    <section className="px-4 py-20 sm:px-6 lg:px-8">
      <div className="mx-auto max-w-7xl">
        <Reveal className="mx-auto max-w-3xl text-center">
          <SectionEyebrow>How It Works</SectionEyebrow>
          <h2 className="mt-6 font-serif text-[2.6rem] italic leading-[1.02] text-[#1f3322] sm:text-[3.6rem]">
            Effortless, from message to mealtime
          </h2>
        </Reveal>

        <div className="relative mt-16 grid gap-8 md:grid-cols-3">
          {steps.map((step, index) => {
            const Icon = step.icon;
            return (
              <Reveal key={step.title} delay={index * 140}>
                <article className="relative h-full rounded-[2rem] border border-[#ece3d2] bg-white px-8 py-10 text-center shadow-[0_16px_40px_rgba(43,30,16,0.05)]">
                  <span className="absolute right-7 top-7 font-serif text-6xl italic text-[#efe2c4]">
                    {index + 1}
                  </span>
                  <div className="mx-auto flex h-16 w-16 items-center justify-center rounded-full bg-[#173d1d] text-[#c1a566]">
                    <Icon size={26} strokeWidth={1.5} />
                  </div>
                  <h3 className="mt-7 font-serif text-3xl italic text-[#1f3322]">
                    {step.title}
                  </h3>
                  <p className="mt-4 text-base leading-7 text-[#6d7266]">
                    {step.description}
                  </p>
                </article>
              </Reveal>
            );
          })}
        </div>
      </div>
    </section>
  );
}

/* ------------------------------------------------------------------ */
/* 8. Why Families Trust Safa                                          */
/* ------------------------------------------------------------------ */

const trustPillars = [
  {
    icon: Leaf,
    title: "All Natural",
    description: "Fresh ingredients. No hidden preservatives. No shortcuts, ever.",
  },
  {
    icon: ShieldCheck,
    title: "Hygienic Batches",
    description: "Prepared in clean, controlled runs made for daily household use.",
  },
  {
    icon: Heart,
    title: "Treated Like Family",
    description: "We look after your kitchen the way we look after our own.",
  },
  {
    icon: MapPin,
    title: "Truly Local",
    description: "Built for Warangal and Hanamkonda, with transparent local pricing.",
  },
];

const stats = [
  { value: 100, suffix: "%", label: "Preservative Free" },
  { value: 45, suffix: " min", label: "Local Delivery" },
  { value: 3, suffix: "", label: "Essentials Live Today" },
  { value: 1, suffix: " msg", label: "To Order on WhatsApp" },
];

function useCountUp(target: number, active: boolean, duration = 1600) {
  const [value, setValue] = useState(0);

  useEffect(() => {
    if (!active) return;
    let frame: number;
    const start = performance.now();

    const tick = (now: number) => {
      const progress = Math.min((now - start) / duration, 1);
      const eased = 1 - Math.pow(1 - progress, 3);
      setValue(Math.round(eased * target));
      if (progress < 1) frame = requestAnimationFrame(tick);
    };

    frame = requestAnimationFrame(tick);
    return () => cancelAnimationFrame(frame);
  }, [target, active, duration]);

  return value;
}

function StatItem({
  value,
  suffix,
  label,
  active,
}: {
  value: number;
  suffix: string;
  label: string;
  active: boolean;
}) {
  const count = useCountUp(value, active);
  return (
    <div className="text-center">
      <p className="font-serif text-5xl italic text-[#c1a566] sm:text-6xl">
        {count}
        {suffix}
      </p>
      <p className="mt-3 text-[11px] font-bold uppercase tracking-[0.28em] text-[#d7d1c1]">
        {label}
      </p>
    </div>
  );
}

function WhyTrustSafa() {
  const statsRef = useRef<HTMLDivElement>(null);
  const [statsActive, setStatsActive] = useState(false);

  useEffect(() => {
    const node = statsRef.current;
    if (!node) return;
    const observer = new IntersectionObserver(
      ([entry]) => {
        if (entry.isIntersecting) {
          setStatsActive(true);
          observer.disconnect();
        }
      },
      { threshold: 0.3 },
    );
    observer.observe(node);
    return () => observer.disconnect();
  }, []);

  return (
    <section className="bg-[#efe7d6] px-4 py-20 sm:px-6 lg:px-8">
      <div className="mx-auto max-w-7xl">
        <Reveal className="mx-auto max-w-3xl text-center">
          <SectionEyebrow>Why Families Trust Safa</SectionEyebrow>
          <h2 className="mt-6 font-serif text-[2.6rem] italic leading-[1.02] text-[#1f3322] sm:text-[3.6rem]">
            Peace of mind, packed into every jar
          </h2>
        </Reveal>

        <div className="mt-16 grid gap-6 sm:grid-cols-2 lg:grid-cols-4">
          {trustPillars.map((pillar, index) => {
            const Icon = pillar.icon;
            return (
              <Reveal key={pillar.title} delay={index * 100}>
                <article className="h-full rounded-[1.75rem] border border-[#e2d3b6] bg-[#faf6ee] px-7 py-9 transition-transform duration-300 hover:-translate-y-2">
                  <div className="flex h-14 w-14 items-center justify-center rounded-full border border-[#d3c19c] text-[#b49761]">
                    <Icon size={24} strokeWidth={1.5} />
                  </div>
                  <h3 className="mt-6 text-[13px] font-bold uppercase tracking-[0.24em] text-[#1f3322]">
                    {pillar.title}
                  </h3>
                  <p className="mt-3 text-base leading-7 text-[#6d7266]">
                    {pillar.description}
                  </p>
                </article>
              </Reveal>
            );
          })}
        </div>

        <div
          ref={statsRef}
          className="mt-16 grid gap-10 rounded-[2rem] bg-[#173d1d] px-8 py-14 sm:grid-cols-2 lg:grid-cols-4"
        >
          {stats.map((stat) => (
            <StatItem key={stat.label} {...stat} active={statsActive} />
          ))}
        </div>
      </div>
    </section>
  );
}

/* ------------------------------------------------------------------ */
/* 9. Future Vision                                                    */
/* ------------------------------------------------------------------ */

function FutureVision() {
  return (
    <section className="px-4 py-20 sm:px-6 lg:px-8">
      <div className="mx-auto max-w-5xl rounded-[2.5rem] border border-[#ece2d0] bg-white px-8 py-16 text-center shadow-[0_16px_50px_rgba(43,30,16,0.05)] sm:px-14">
        <Reveal>
          <SectionEyebrow>Our Future Vision</SectionEyebrow>
          <h2 className="mt-6 font-serif text-[2.6rem] italic leading-[1.02] text-[#1f3322] sm:text-[3.8rem]">
            Building a Better Local Community
          </h2>
          <p className="mx-auto mt-6 max-w-2xl text-lg leading-8 text-[#6d7266]">
            Safa Foods is a Warangal brand at heart. As we grow, we want to create
            local jobs, support neighbourhood kitchens, and make dependable, honest
            food part of everyday life here &mdash; one home, one family, one happy
            kitchen at a time.
          </p>
          <div className="mt-9 inline-flex items-center gap-3 rounded-full bg-[#f6f1e7] px-6 py-3 text-[12px] font-bold uppercase tracking-[0.24em] text-[#8a7440]">
            <Heart size={15} className="text-[#b49761]" />
            We help families keep a happy kitchen
          </div>
        </Reveal>
      </div>
    </section>
  );
}

/* ------------------------------------------------------------------ */
/* 10. Final CTA                                                       */
/* ------------------------------------------------------------------ */

function FinalCTA() {
  return (
    <section className="px-4 pb-24 pt-4 sm:px-6 lg:px-8">
      <div className="mx-auto max-w-7xl overflow-hidden rounded-[2.5rem] bg-[#173d1d] px-8 py-20 text-center text-[#efe5cf] shadow-[0_24px_60px_rgba(23,61,29,0.2)] sm:px-16">
        <Reveal>
          <p className="text-[11px] font-bold uppercase tracking-[0.42em] text-[#c1a566]">
            Let&apos;s Get Started
          </p>
          <h2 className="mx-auto mt-6 max-w-3xl font-serif text-[2.8rem] italic leading-[1.02] text-white sm:text-[4.2rem]">
            Let us take care of your everyday essentials
          </h2>
          <p className="mx-auto mt-6 max-w-2xl text-lg leading-8 text-[#d7d1c1]">
            Fresh. Trusted. Delivered. Send one message and let your kitchen stay
            happy &mdash; we&apos;ll handle the rest, like family would.
          </p>
          <div className="mt-10 flex flex-col items-center justify-center gap-4 sm:flex-row">
            <a
              href={brand.whatsappHref}
              target="_blank"
              rel="noopener noreferrer"
              className="btn-primary group inline-flex min-w-[14rem] items-center justify-center gap-3 bg-[#b49761] px-9 py-5 text-[12px] font-bold uppercase tracking-[0.3em] text-[#17311d]"
            >
              <MessageCircle size={17} className="transition-transform group-hover:scale-110" />
              Order on WhatsApp
            </a>
            <Link
              href="/shop"
              className="inline-flex min-w-[12rem] items-center justify-center border border-[#5b6d4f] px-9 py-5 text-[12px] font-bold uppercase tracking-[0.3em] text-[#e9d59b] transition hover:bg-white/5"
            >
              Browse Catalogue
            </Link>
          </div>
        </Reveal>
      </div>
    </section>
  );
}
