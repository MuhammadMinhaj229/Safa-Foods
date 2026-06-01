"use client";

import React from 'react';
import { Bike, Navigation, Phone, Camera, MapPin } from 'lucide-react';

export const DeliveryHubAdapter = () => {
  return (
    <div className="bg-[#0f0f0f] min-h-screen text-white p-6 sm:p-10 transition-opacity duration-500">
      <header className="flex justify-between items-center mb-16 border-b border-white/10 pb-8 animate-fade-up">
        <div className="flex items-center gap-5">
          <Bike className="text-[#A68A56]" size={36} />
          <div>
            <h1 className="text-2xl sm:text-3xl font-black tracking-[0.3em] uppercase">Partner Hub</h1>
            <span className="text-[10px] sm:text-[12px] font-black text-[#A68A56] uppercase tracking-[0.5em]">Warangal Central</span>
          </div>
        </div>
        <div className="relative flex h-14 w-14">
          <span className="animate-ping absolute inline-flex h-full w-full rounded-full bg-green-400 opacity-20"></span>
          <div className="relative inline-flex rounded-full h-14 w-14 bg-green-500 border-4 border-[#222] shadow-[0_0_20px_rgba(34,197,94,0.4)]"></div>
        </div>
      </header>

      <div className="max-w-2xl mx-auto space-y-10 pb-32 animate-fade-up" style={{ animationDelay: '0.1s' }}>
        <div className="bg-[#1A1A1A] border-l-[12px] border-[#A68A56] p-8 sm:p-12 rounded-xl shadow-[0_30px_60px_rgba(0,0,0,0.6)] relative overflow-hidden">
          <div className="absolute top-0 right-0 bg-gradient-to-r from-[#A68A56] to-[#C3A970] text-[#1E331B] px-8 py-2.5 text-[11px] font-black uppercase tracking-widest rounded-bl-xl shadow-lg">Active Task</div>
          <span className="text-[11px] font-black tracking-[0.4em] text-[#6B705C] uppercase block mb-8">#SF-8919 • ASAP DELIVERY</span>
          
          <div className="flex gap-8 mb-16">
             <div className="flex flex-col items-center gap-3 mt-1">
                <div className="w-5 h-5 rounded-full bg-[#A68A56] shadow-[0_0_15px_rgba(166,138,86,0.6)]"></div>
                <div className="w-1 flex-1 bg-white/10 border-dashed border-l"></div>
                <div className="relative">
                  <MapPin className="text-red-500 relative z-10" size={28} />
                  <div className="absolute inset-0 bg-red-500/40 rounded-full animate-ping z-0 scale-150"></div>
                </div>
             </div>
             <div className="flex flex-col gap-14">
                <div><h3 className="text-[10px] font-black uppercase text-white/40 tracking-widest mb-1">Pickup Origin</h3><p className="text-2xl sm:text-3xl font-serif italic text-white/90">Safa Hub - Warangal Main</p></div>
                <div><h3 className="text-[10px] font-black uppercase text-white/40 tracking-widest mb-1">Destination</h3><p className="text-2xl sm:text-3xl font-serif italic leading-relaxed text-white/90">H.No 45-B, Sector 4, Subedari</p></div>
             </div>
          </div>

          <div className="grid grid-cols-2 gap-6">
             <button className="bg-[#252525] flex items-center justify-center gap-3 py-6 rounded-lg active:scale-95 transition-all hover:bg-[#333] border border-white/5">
                <Navigation size={24} className="text-[#A68A56]"/>
                <span className="text-[12px] font-black uppercase tracking-widest">Map</span>
             </button>
             <button className="bg-[#252525] flex items-center justify-center gap-3 py-6 rounded-lg active:scale-95 transition-all hover:bg-[#333] border border-white/5">
                <Phone size={24} className="text-[#A68A56]"/>
                <span className="text-[12px] font-black uppercase tracking-widest">Call</span>
             </button>
             <button className="col-span-2 bg-gradient-to-r from-[#A68A56] to-[#C3A970] text-[#1E331B] flex items-center justify-center gap-4 py-8 rounded-lg font-black uppercase tracking-[0.4em] shadow-[0_10px_30px_rgba(166,138,86,0.3)] active:scale-95 transition-all text-sm mt-2">
                <Camera size={28} /> Drop-off Photo Proof
             </button>
          </div>
        </div>
      </div>
    </div>
  );
};
