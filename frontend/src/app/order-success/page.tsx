"use client";

import React, { Suspense } from 'react';
import { useSearchParams, useRouter } from 'next/navigation';
import { SafaLogo } from '@/components/shared/SafaLogo';
import { CheckCircle2, Package, Truck, ArrowRight, ShoppingBag } from 'lucide-react';
import Link from 'next/link';

function SuccessContent() {
  const searchParams = useSearchParams();
  const router = useRouter();
  const orderId = searchParams.get('id');

  return (
    <div className="min-h-screen bg-[#F4F1EA] flex flex-col items-center justify-center p-6 text-center">
      {/* Heritage Illustration Backdrop */}
      <div className="absolute top-1/2 left-1/2 -translate-x-1/2 -translate-y-1/2 opacity-5 pointer-events-none scale-[4]">
        <SafaLogo className="w-96 h-96" color="#1E331B" />
      </div>

      <div className="max-w-2xl relative z-10 animate-fade-up">
        <div className="w-24 h-24 bg-[#1E331B] text-white rounded-full flex items-center justify-center mx-auto mb-10 shadow-2xl scale-125">
          <CheckCircle2 size={48} strokeWidth={1.5} />
        </div>

        <h1 className="font-serif italic text-6xl text-[#1E331B] mb-6">Order Confirmed</h1>
        <p className="text-[12px] font-black uppercase tracking-[0.4em] text-[#A68A56] mb-12">Artisan Purity is on its way</p>

        <div className="glass-panel p-10 rounded-2xl border border-white mb-12 shadow-xl bg-white/40">
          <div className="flex flex-col md:flex-row justify-between gap-10 text-left">
            <div>
              <p className="text-[10px] font-black uppercase tracking-widest text-[#6B705C] mb-2 opacity-60">Order Reference</p>
              <p className="font-serif italic text-2xl text-[#1E331B]">#{orderId || 'SFA-8921-X'}</p>
            </div>
            <div>
              <p className="text-[10px] font-black uppercase tracking-widest text-[#6B705C] mb-2 opacity-60">Estimated Freshness</p>
              <p className="font-serif italic text-2xl text-[#1E331B]">Tomorrow Morning</p>
            </div>
          </div>

          <div className="mt-10 pt-10 border-t border-[#1E331B]/5 grid grid-cols-1 md:grid-cols-3 gap-6">
            <div className="flex flex-col items-center gap-2">
              <Package size={20} className="text-[#A68A56] mb-2" />
              <p className="text-[9px] font-black uppercase tracking-widest">Small Batch Packing</p>
            </div>
            <div className="flex flex-col items-center gap-2 opacity-30">
              <Truck size={20} className="text-[#A68A56] mb-2" />
              <p className="text-[9px] font-black uppercase tracking-widest">Hand-Delivery</p>
            </div>
            <div className="flex flex-col items-center gap-2 opacity-30">
              <ShoppingBag size={20} className="text-[#A68A56] mb-2" />
              <p className="text-[9px] font-black uppercase tracking-widest">Delivered Pure</p>
            </div>
          </div>
        </div>

        <div className="flex flex-col sm:flex-row gap-6 justify-center">
          <Link href="/" className="btn-primary px-10 py-5 text-[11px] font-black tracking-[0.3em] uppercase flex items-center gap-3">
            Back to Home <ArrowRight size={16} />
          </Link>
          <button onClick={() => router.push('/shop')} className="glass-panel border border-[#1E331B]/10 px-10 py-5 text-[11px] font-black tracking-[0.3em] uppercase text-[#1E331B] hover:bg-[#1E331B] hover:text-white transition-all">
            Browse More
          </button>
        </div>

        <p className="mt-16 text-[10px] font-medium text-[#6B705C] tracking-[0.2em] uppercase opacity-60 leading-relaxed">
          You will receive a WhatsApp confirmation <br/> shortly with your live tracker link.
        </p>
      </div>
    </div>
  );
}

export default function OrderSuccessPage() {
  return (
    <Suspense fallback={
      <div className="min-h-screen bg-[#F4F1EA] flex items-center justify-center">
        <SafaLogo className="w-16 h-16 animate-pulse" color="#A68A56" />
      </div>
    }>
      <SuccessContent />
    </Suspense>
  );
}
