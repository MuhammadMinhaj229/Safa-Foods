import { Clock3, Instagram, Facebook, MapPin, MessageCircle, Phone, Truck } from "lucide-react";

import { Reveal } from "@/components/ui/Reveal";
import { brand, channels } from "@/data/site";

const channelIcons: Record<string, typeof MessageCircle> = {
  WhatsApp: MessageCircle,
  Instagram: Instagram,
  Facebook: Facebook,
};

const quickFacts = [
  {
    icon: MapPin,
    title: "Where we are",
    lines: ["Near Narsampet Road,", "beside Decent Function Hall, Warangal"],
  },
  {
    icon: Truck,
    title: "Where we deliver",
    lines: ["Warangal & Hanamkonda", "Local delivery, transparent slab-based fees"],
  },
  {
    icon: Clock3,
    title: "When we're fresh",
    lines: ["Made fresh every morning", "Pickup usually ready in 2–4 hrs"],
  },
];

export default function ContactPage() {
  return (
    <main className="bg-[#f6f1e7] px-4 pt-32 pb-20 sm:px-6 sm:pt-40 lg:px-8">
      <div className="mx-auto max-w-7xl">
        <div className="grid gap-10 lg:grid-cols-[0.95fr_1.05fr] lg:gap-16">
          {/* Left: intro + WhatsApp CTA */}
          <Reveal>
            <p className="section-kicker">Talk to Us</p>
            <h1 className="section-title mt-5">
              A message away, like reaching out to family.
            </h1>
            <p className="mt-6 max-w-xl text-lg leading-8 text-[#6d7266]">
              The simplest way to reach Safa Foods is WhatsApp. Send us what your kitchen
              needs and we&apos;ll take care of the rest — fresh, preservative-free
              essentials made daily and delivered across Warangal and Hanamkonda.
            </p>

            <a
              href={brand.whatsappHref}
              target="_blank"
              rel="noopener noreferrer"
              className="btn-primary group mt-9 inline-flex items-center justify-center gap-3 bg-[#173d1d] px-8 py-4 text-[11px] font-bold uppercase tracking-[0.3em] text-white shadow-[0_16px_30px_rgba(23,61,29,0.16)]"
            >
              <MessageCircle size={16} className="transition-transform group-hover:scale-110" />
              Order on WhatsApp
            </a>

            <a
              href={brand.whatsappHref}
              target="_blank"
              rel="noopener noreferrer"
              className="mt-6 flex items-center gap-3 text-[#1f3322]"
            >
              <span className="flex h-11 w-11 items-center justify-center rounded-full border border-[#dcc8a0] bg-white text-[#8a7440]">
                <Phone size={18} />
              </span>
              <span className="font-serif text-2xl italic sm:text-3xl">{brand.whatsappNumber}</span>
            </a>

            <div className="mt-8 flex flex-wrap gap-3">
              {channels.map((channel) => {
                const Icon = channelIcons[channel.name] ?? MessageCircle;
                return (
                  <a
                    key={channel.name}
                    href={channel.href}
                    target="_blank"
                    rel="noreferrer"
                    className="inline-flex items-center gap-2 rounded-full border border-[#d7ccb7] bg-white px-5 py-3 text-[11px] font-bold uppercase tracking-[0.22em] text-[#1f3322] transition hover:border-[#b49761] hover:text-[#b49761]"
                  >
                    <Icon size={15} />
                    {channel.name}
                  </a>
                );
              })}
            </div>
          </Reveal>

          {/* Right: framed store card */}
          <Reveal delay={140}>
            <div className="relative">
              <span className="frame-corner left-0 top-0 border-l-2 border-t-2" style={{ animationDelay: "0.2s" }} />
              <span className="frame-corner right-0 top-0 border-r-2 border-t-2" style={{ animationDelay: "0.35s" }} />
              <span className="frame-corner bottom-0 left-0 border-b-2 border-l-2" style={{ animationDelay: "0.5s" }} />
              <span className="frame-corner bottom-0 right-0 border-b-2 border-r-2" style={{ animationDelay: "0.65s" }} />
              <div className="overflow-hidden rounded-[2rem] border-[8px] border-white bg-white shadow-[0_28px_70px_rgba(53,39,18,0.14)] sm:border-[10px]">
                <img
                  src="/images/basket-essentials.png"
                  alt="A basket of fresh Safa Foods kitchen essentials"
                  className="h-64 w-full object-cover sm:h-80"
                />
                <div className="bg-[#173d1d] px-6 py-8 text-[#efe5cf] sm:px-8">
                  <p className="text-[11px] font-bold uppercase tracking-[0.34em] text-[#c1a566]">
                    Store Location
                  </p>
                  <p className="mt-3 font-serif text-2xl italic text-white sm:text-3xl">
                    Near Narsampet Road, beside Decent Function Hall
                  </p>
                  <p className="mt-3 text-base leading-7 text-[#d7d1c1]">
                    Serving Warangal and Hanamkonda households with fresh, local delivery.
                  </p>
                </div>
              </div>
            </div>
          </Reveal>
        </div>

        {/* Quick facts */}
        <div className="mt-14 grid gap-6 md:grid-cols-3">
          {quickFacts.map((fact, index) => {
            const Icon = fact.icon;
            return (
              <Reveal key={fact.title} delay={index * 120}>
                <article className="h-full rounded-[2rem] border border-[#e2d3b6] bg-white px-7 py-8 shadow-[0_16px_40px_rgba(43,30,16,0.05)]">
                  <div className="flex h-12 w-12 items-center justify-center rounded-full bg-[#efe7d6] text-[#8a7440]">
                    <Icon size={22} strokeWidth={1.5} />
                  </div>
                  <h2 className="mt-5 font-serif text-xl italic text-[#1f3322] sm:text-2xl">
                    {fact.title}
                  </h2>
                  <div className="mt-3 space-y-1 text-base leading-7 text-[#6d7266]">
                    {fact.lines.map((line) => (
                      <p key={line}>{line}</p>
                    ))}
                  </div>
                </article>
              </Reveal>
            );
          })}
        </div>
      </div>
    </main>
  );
}
