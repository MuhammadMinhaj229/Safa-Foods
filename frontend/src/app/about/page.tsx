import Link from "next/link";
import { HeartHandshake, Leaf, MapPin, MessageCircle, ShieldCheck, Sprout } from "lucide-react";

import { Reveal } from "@/components/ui/Reveal";
import { brand } from "@/data/site";

const values = [
  {
    icon: Leaf,
    title: "Made Fresh Daily",
    description:
      "Every batch is prepared the same morning in our Warangal kitchen — never mass-produced, never sitting on a shelf.",
  },
  {
    icon: ShieldCheck,
    title: "Nothing to Hide",
    description:
      "Origin certified, 100% preservative free, and priced clearly before you order. No hidden charges, no compromise.",
  },
  {
    icon: HeartHandshake,
    title: "Care Like Family",
    description:
      "We look after your kitchen the way your own family would — quietly keeping the essentials flowing so you never run out.",
  },
];

const stats = [
  { value: "100%", label: "Preservative Free" },
  { value: "Daily", label: "Made Fresh" },
  { value: "2", label: "Local Pickup Hubs" },
  { value: "1 msg", label: "To Order on WhatsApp" },
];

export default function AboutPage() {
  return (
    <main className="bg-[#f6f1e7] px-4 pt-32 pb-20 sm:px-6 sm:pt-40 lg:px-8">
      <div className="mx-auto max-w-7xl">
        {/* Framed hero */}
        <section className="grid items-center gap-10 lg:grid-cols-[1.05fr_0.95fr] lg:gap-16">
          <Reveal>
            <p className="section-kicker">Our Story</p>
            <h1 className="section-title mt-5">
              We help families keep a happy kitchen.
            </h1>
            <p className="mt-6 max-w-xl text-lg leading-8 text-[#6d7266]">
              Safa Foods began with a simple belief — that no family in Warangal or
              Hanamkonda should have to choose between a busy life and food they can
              trust. We are not just another paste brand. We protect the daily rhythm
              of your home by making fresh, honest kitchen essentials effortless to get.
            </p>
            <p className="mt-5 max-w-xl text-lg leading-8 text-[#6d7266]">
              Whether you are running between work and home, caring for elders and
              little ones, or watching over the family while someone works far away in
              the Gulf — we quietly keep the essentials flowing, so your kitchen always
              feels like home.
            </p>
            <div className="mt-8 flex flex-wrap gap-3">
              {["Origin Certified", "Made Fresh Daily", "100% Preservative Free"].map((chip) => (
                <span
                  key={chip}
                  className="inline-flex items-center gap-2 rounded-full border border-[#dcc8a0] bg-white px-4 py-2 text-[11px] font-bold uppercase tracking-[0.22em] text-[#8a7440]"
                >
                  <Leaf size={13} />
                  {chip}
                </span>
              ))}
            </div>
          </Reveal>

          <Reveal delay={140}>
            <div className="relative">
              <span className="frame-corner left-0 top-0 border-l-2 border-t-2" style={{ animationDelay: "0.2s" }} />
              <span className="frame-corner right-0 top-0 border-r-2 border-t-2" style={{ animationDelay: "0.35s" }} />
              <span className="frame-corner bottom-0 left-0 border-b-2 border-l-2" style={{ animationDelay: "0.5s" }} />
              <span className="frame-corner bottom-0 right-0 border-b-2 border-r-2" style={{ animationDelay: "0.65s" }} />
              <div className="overflow-hidden rounded-[2rem] border-[8px] border-white bg-white shadow-[0_28px_70px_rgba(53,39,18,0.14)] sm:border-[10px]">
                <img
                  src="/images/family-cooking.png"
                  alt="A Warangal family cooking together at home with Safa Foods essentials"
                  className="h-[22rem] w-full object-cover transition duration-700 hover:scale-[1.03] sm:h-[30rem]"
                />
              </div>
            </div>
          </Reveal>
        </section>

        {/* Stats strip */}
        <Reveal className="mt-16 sm:mt-20">
          <div className="grid grid-cols-2 gap-px overflow-hidden rounded-[2rem] border border-[#173d1d]/12 bg-[#173d1d]/12 sm:grid-cols-4">
            {stats.map((stat) => (
              <div key={stat.label} className="bg-[#173d1d] px-5 py-8 text-center text-[#efe5cf]">
                <p className="font-serif text-3xl italic text-white sm:text-4xl">{stat.value}</p>
                <p className="mt-2 text-[10px] font-bold uppercase tracking-[0.24em] text-[#c1a566]">
                  {stat.label}
                </p>
              </div>
            ))}
          </div>
        </Reveal>

        {/* Values */}
        <section className="mt-16 sm:mt-24">
          <Reveal className="mx-auto max-w-3xl text-center">
            <p className="section-kicker">What We Stand For</p>
            <h2 className="section-title mt-5">Honest food, built on trust</h2>
          </Reveal>
          <div className="mt-12 grid gap-6 md:grid-cols-3">
            {values.map((value, index) => {
              const Icon = value.icon;
              return (
                <Reveal key={value.title} delay={index * 120}>
                  <article className="group h-full rounded-[2rem] border border-[#e2d3b6] bg-white px-7 py-9 shadow-[0_16px_40px_rgba(43,30,16,0.05)] transition-transform duration-300 hover:-translate-y-2">
                    <div className="flex h-14 w-14 items-center justify-center rounded-full bg-[#173d1d] text-[#c1a566]">
                      <Icon size={24} strokeWidth={1.5} />
                    </div>
                    <h3 className="mt-6 font-serif text-2xl italic text-[#1f3322] sm:text-3xl">
                      {value.title}
                    </h3>
                    <p className="mt-3 text-base leading-7 text-[#6d7266]">{value.description}</p>
                  </article>
                </Reveal>
              );
            })}
          </div>
        </section>

        {/* Vision + CTA */}
        <Reveal className="mt-16 sm:mt-24">
          <section className="relative overflow-hidden rounded-[2.5rem] bg-[#173d1d] px-6 py-14 text-center text-[#efe5cf] shadow-[0_28px_70px_rgba(23,61,29,0.2)] sm:px-12 sm:py-20">
            <div className="mx-auto flex max-w-3xl flex-col items-center">
              <div className="flex h-16 w-16 items-center justify-center rounded-full border border-[#c1a566]/40 text-[#c1a566]">
                <Sprout size={28} strokeWidth={1.5} />
              </div>
              <h2 className="mt-8 font-serif text-[2.2rem] italic leading-[1.05] text-white sm:text-[3.2rem]">
                Building a better local community
              </h2>
              <p className="mt-6 text-lg leading-8 text-[#d7d1c1]">
                Every order supports a small, local kitchen and the families who make it
                run. As we grow, so does our promise: to take care of your everyday
                essentials so you can focus on the people who matter most.
              </p>
              <div className="mt-9 flex w-full flex-col gap-4 sm:w-auto sm:flex-row">
                <a
                  href={brand.whatsappHref}
                  target="_blank"
                  rel="noopener noreferrer"
                  className="inline-flex items-center justify-center gap-3 bg-[#c1a566] px-8 py-4 text-[11px] font-bold uppercase tracking-[0.3em] text-[#17311d] transition hover:bg-[#d0b878]"
                >
                  <MessageCircle size={16} />
                  Order on WhatsApp
                </a>
                <Link
                  href="/shop"
                  className="inline-flex items-center justify-center gap-2 border border-[#5b6d4f] px-8 py-4 text-[11px] font-bold uppercase tracking-[0.3em] text-[#e9d59b] transition hover:bg-white/5"
                >
                  <MapPin size={15} />
                  Explore the Catalogue
                </Link>
              </div>
            </div>
          </section>
        </Reveal>
      </div>
    </main>
  );
}
