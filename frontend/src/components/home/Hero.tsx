"use client";
import Link from "next/link";
import { MessageCircle, ShieldCheck } from "lucide-react";

import { brand } from "@/data/site";

export const Hero = () => {
  return (
    <section className="relative overflow-hidden bg-[#f4efe5] px-4 pb-20 pt-36 sm:px-6 sm:pt-40 lg:px-8 lg:pt-44">
      <div className="absolute inset-0 bg-[radial-gradient(circle_at_20%_20%,rgba(255,255,255,0.82),transparent_35%),radial-gradient(circle_at_80%_18%,rgba(255,255,255,0.65),transparent_28%),radial-gradient(circle_at_68%_68%,rgba(255,255,255,0.45),transparent_30%)]" />
      <div className="absolute inset-y-0 right-[14%] hidden w-[16%] skew-x-[-10deg] bg-white/25 lg:block" />

      <div className="relative mx-auto grid max-w-7xl gap-12 lg:grid-cols-[0.98fr_1.02fr] lg:items-center xl:gap-16">
        <div className="relative max-w-xl py-6 sm:py-10">
          <div className="inline-flex items-center gap-2 border border-[#bf9e64] px-4 py-2 text-[9px] font-bold uppercase tracking-[0.34em] text-[#ae8f59] sm:text-[10px]">
            <ShieldCheck size={12} />
            Warangal &amp; Hanamkonda | Made Fresh Daily
          </div>

          <h1 className="mt-10 font-serif text-[3.1rem] italic leading-[0.92] tracking-[-0.045em] text-[#1b3821] min-[380px]:text-[3.6rem] sm:text-[5rem] lg:text-[5.6rem]">
            Everything your
            <span className="block">home needs.</span>
            <span className="block text-[#c2a267]">Fresh. Trusted.</span>
            <span className="block">Delivered.</span>
          </h1>

          <p className="mt-8 max-w-lg text-[1.05rem] leading-8 text-[#6a7067] sm:text-[1.15rem]">
            We help families keep a happy kitchen. Fresh, preservative-free
            essentials made daily in our Warangal kitchen and brought to your door
            with one simple WhatsApp message &mdash; so daily life feels lighter.
          </p>

          <div className="mt-10 flex flex-col gap-4 sm:flex-row">
            <a
              href={brand.whatsappHref}
              target="_blank"
              rel="noopener noreferrer"
              className="btn-primary group inline-flex min-w-[13rem] items-center justify-center gap-3 bg-[#173d1d] px-8 py-4 text-[11px] font-bold uppercase tracking-[0.3em] text-white shadow-[0_16px_30px_rgba(23,61,29,0.16)]"
            >
              <MessageCircle size={16} className="transition-transform group-hover:scale-110" />
              Order on WhatsApp
            </a>
            <Link
              href="/shop"
              className="inline-flex min-w-[11rem] items-center justify-center border border-[#1b3821]/45 bg-transparent px-8 py-4 text-[11px] font-bold uppercase tracking-[0.3em] text-[#1b3821] transition hover:bg-white/45"
            >
              View Catalogue
            </Link>
          </div>

          <p className="mt-8 text-[11px] font-bold uppercase tracking-[0.34em] text-[#9a8a63]">
            Origin Certified &middot; 100% Preservative Free &middot; Local Delivery
          </p>
        </div>

        <div className="relative flex items-center justify-center pt-4 lg:justify-end">
          <div className="absolute left-0 top-4 z-20 sm:left-6 lg:left-auto lg:right-[58%] lg:top-2">
            <div className="flex h-24 w-24 items-center justify-center rounded-full border-[6px] border-white bg-[#b49761] shadow-[0_18px_30px_rgba(44,32,16,0.16)] sm:h-28 sm:w-28 sm:border-[8px] animate-spin-slow">
              <div
                className="flex h-full w-full flex-col items-center justify-center text-center text-[#18311d]"
                style={{ animation: "spin-slow 20s linear infinite reverse" }}
              >
                <ShieldCheck size={18} className="mb-1 sm:h-5 sm:w-5" />
                <span className="text-[8px] font-black uppercase tracking-[0.18em] sm:text-[9px]">
                  100%
                  <br />
                  Natural
                </span>
              </div>
            </div>
          </div>

          {/* Floating ingredient accents */}
          <img
            src="/images/product-turmeric.png"
            alt=""
            aria-hidden="true"
            className="animate-float absolute -left-2 bottom-6 z-20 h-20 w-20 rounded-full border-4 border-white object-cover shadow-[0_16px_30px_rgba(44,32,16,0.18)] sm:h-24 sm:w-24 lg:-left-6"
            style={{ animationDelay: "1.2s" }}
          />
          <img
            src="/images/product-red-chilli.png"
            alt=""
            aria-hidden="true"
            className="animate-float absolute -right-1 top-8 z-20 hidden h-20 w-20 rounded-full border-4 border-white object-cover shadow-[0_16px_30px_rgba(44,32,16,0.18)] sm:block sm:h-24 sm:w-24"
            style={{ animationDelay: "0.4s" }}
          />

          <div className="relative w-full max-w-[20rem] sm:max-w-[24rem] lg:max-w-[27rem]">
            <div className="overflow-hidden rounded-t-[8rem] rounded-b-[0.5rem] border-[8px] border-white bg-white p-3 shadow-[0_28px_70px_rgba(53,39,18,0.14)] sm:rounded-t-[10rem] sm:border-[10px] sm:p-4">
              <div className="overflow-hidden rounded-t-[7rem] sm:rounded-t-[9rem]">
                <img
                  src="/images/hero-ingredients.png"
                  alt="Fresh ginger, garlic and curry leaves from the Safa Foods kitchen"
                  className="h-[23rem] w-full object-cover sm:h-[29rem] lg:h-[32rem]"
                />
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>
  );
};
