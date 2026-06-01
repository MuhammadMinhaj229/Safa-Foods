"use client";

import React, { useState, useEffect } from 'react';
import { HeartHandshake } from 'lucide-react';
import { SafaLogo } from '@/components/shared/SafaLogo';

export default function MissionPage() {
  const [fade, setFade] = useState(false);

  useEffect(() => {
    setFade(true);
    const timer = setTimeout(() => setFade(false), 300);
    return () => clearTimeout(timer);
  }, []);

  return (
    <div className={`transition-opacity duration-700 ${fade ? 'opacity-0' : 'opacity-100'}`}>
      {/* Dynamic Header Section */}
      <div className="bg-[#1E331B] text-[#F4F1EA] py-40 relative overflow-hidden">
        {/* Decorative Watermark */}
        <div className="absolute -right-40 -top-20 opacity-5 rotate-12 scale-[2] pointer-events-none">
          <SafaLogo className="w-64 h-64" color="#F4F1EA" />
        </div>
        
        <div className="absolute inset-0 bg-[url('data:image/svg+xml,%3Csvg viewBox=%220 0 200 200%22 xmlns=%22http://www.w3.org/2000/svg%22%3E%3Cfilter id=%22noiseFilter%22%3E%3CfeTurbulence type=%22fractalNoise%22 baseFrequency=%220.85%22 numOctaves=%223%22 stitchTiles=%22stitch%22/%3E%3C/filter%3E%3Crect width=%22100%25%22 height=%22100%25%22 filter=%22url(%23noiseFilter)%22 opacity=%220.05%22/%3E%3C/svg%3E')] opacity-50 mix-blend-overlay"></div>
        
        <div className="max-w-5xl mx-auto px-4 text-center animate-fade-up relative z-10">
          <HeartHandshake size={72} className="text-[#A68A56] mx-auto mb-10" strokeWidth={1} />
          <h1 className="font-serif italic text-6xl sm:text-[7.5rem] mb-10 leading-[1] text-gradient-gold">Beyond Charity</h1>
          <p className="text-2xl text-[#F4F1EA]/80 max-w-3xl mx-auto leading-relaxed font-medium">
            Safa Foods is a Zakat-aligned livelihood model. We empower local women in Warangal by transforming beneficiaries into skilled artisan producers.
          </p>
        </div>
      </div>

      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-32">
        <div className="grid grid-cols-1 lg:grid-cols-2 gap-24 items-center">
          <div className="space-y-10">
            <div className="space-y-3">
              <span className="text-[11px] font-black uppercase tracking-[0.5em] text-[#A68A56]">Our Values</span>
              <h2 className="font-serif italic text-6xl text-[#1E331B]">Dignified Work</h2>
            </div>
            <p className="text-xl text-[#6B705C] leading-[1.8] font-medium">
              We provide a state-of-the-art hygienic environment where local women craft condiments daily. Proceeds directly support community reinvestment, ensuring economic independence for families in Warangal.
            </p>
            <div className="grid grid-cols-2 gap-12 py-10 border-y border-[#1E331B]/10">
              <div className="space-y-2">
                <span className="block text-6xl font-serif italic text-[#1E331B]">10+</span>
                <span className="text-[10px] font-black uppercase tracking-[0.3em] text-[#A68A56]">Artisans Employed</span>
              </div>
              <div className="space-y-2">
                <span className="block text-6xl font-serif italic text-[#1E331B]">100%</span>
                <span className="text-[10px] font-black uppercase tracking-[0.3em] text-[#A68A56]">Profit Reinvested</span>
              </div>
            </div>
          </div>
          <div className="relative animate-float">
             <div className="aspect-square bg-white p-6 shadow-[0_30px_60px_rgba(0,0,0,0.1)] rounded-full relative z-10 overflow-hidden border border-[#A68A56]/20">
                <img 
                  src="https://images.unsplash.com/photo-1587049352847-4d4b12b14185?auto=format&fit=crop&q=80&w=1200" 
                  alt="Artisan production" 
                  className="w-full h-full object-cover rounded-full grayscale-[20%] hover:grayscale-0 transition-all duration-1000" 
                />
             </div>
             <div className="absolute -bottom-6 -left-10 bg-[#A68A56] p-10 text-[#1E331B] max-w-[280px] shadow-2xl z-20 backdrop-blur-md">
               <span className="font-serif italic text-3xl leading-snug">"Community is the strongest ingredient."</span>
             </div>
          </div>
        </div>
      </div>
    </div>
  );
}
