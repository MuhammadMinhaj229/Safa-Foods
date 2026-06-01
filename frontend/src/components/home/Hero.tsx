"use client";
import Link from "next/link";
import { ShieldCheck, Star } from "lucide-react";

export const Hero = () => {
  return (
    <section className="relative overflow-hidden bg-[#f4efe5] px-4 pb-20 pt-36 sm:px-6 sm:pt-40 lg:px-8 lg:pt-44">
      <div className="absolute inset-0 bg-[radial-gradient(circle_at_20%_20%,rgba(255,255,255,0.82),transparent_35%),radial-gradient(circle_at_80%_18%,rgba(255,255,255,0.65),transparent_28%),radial-gradient(circle_at_68%_68%,rgba(255,255,255,0.45),transparent_30%)]" />
      <div className="absolute inset-y-0 right-[14%] hidden w-[16%] skew-x-[-10deg] bg-white/25 lg:block" />

      <div className="relative mx-auto grid max-w-7xl gap-12 lg:grid-cols-[0.92fr_1.08fr] lg:items-center xl:gap-16">
        <div className="relative max-w-xl py-6 sm:py-10">
          <div className="inline-flex items-center gap-2 border border-[#bf9e64] px-4 py-2 text-[9px] font-bold uppercase tracking-[0.34em] text-[#ae8f59] sm:text-[10px]">
            <Star size={12} fill="currentColor" />
            Delivered in 45 mins | Warangal
          </div>

          <h1 className="mt-10 font-serif text-[3.5rem] italic leading-[0.9] tracking-[-0.045em] text-[#1b3821] min-[380px]:text-[4rem] sm:text-[5.6rem] lg:text-[6.5rem]">
            Experience the
            
            <span className="block text-[#c2a267]">Fresh Kitchen</span>
            <span className="block">Goodness</span>
          </h1>

          <p className="mt-8 max-w-lg text-[1.05rem] leading-8 text-[#6a7067] sm:text-[1.15rem]">
            100% Fresh. No Preservatives. Made Daily. Hand-ground condiments from our
            Warangal kitchen, delivered to your door.
          </p>

          <div className="mt-10 flex flex-col gap-4 sm:flex-row">
            <Link
              href="/shop"
              className="inline-flex min-w-[12rem] items-center justify-center bg-[#173d1d] px-8 py-4 text-[11px] font-bold uppercase tracking-[0.3em] text-white shadow-[0_16px_30px_rgba(23,61,29,0.16)] transition hover:bg-[#204926]"
            >
              Shop Authentic Flavors
            </Link>
            <Link
              href="/mission"
              className="inline-flex min-w-[11rem] items-center justify-center border border-[#1b3821]/45 bg-transparent px-8 py-4 text-[11px] font-bold uppercase tracking-[0.3em] text-[#1b3821] transition hover:bg-white/45"
            >
              Our Mission
            </Link>
          </div>
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

          <div className="relative w-full max-w-[20rem] sm:max-w-[24rem] lg:max-w-[27rem]">
            <div className="overflow-hidden rounded-t-[8rem] rounded-b-[0.5rem] border-[8px] border-white bg-white p-3 shadow-[0_28px_70px_rgba(53,39,18,0.14)] sm:rounded-t-[10rem] sm:border-[10px] sm:p-4">
              <div className="overflow-hidden rounded-t-[7rem] sm:rounded-t-[9rem]">
                <img
                  src="https://images.unsplash.com/photo-1515003197210-e0cd71810b5f?auto=format&fit=crop&q=80&w=1400"
                  alt="Safa Foods ingredients"
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
