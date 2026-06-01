"use client";

import React from 'react';
import { useRouter } from 'next/navigation';
import { useCart } from '@/lib/context/CartContext';
import { ArrowRight, Trash2, ShoppingBag, ChevronRight } from 'lucide-react';
import Link from 'next/link';

export default function CartPage() {
  const router = useRouter();
  const { cart, total, removeFromCart } = useCart();

  if (cart.length === 0) {
    return (
      <div className="min-h-screen bg-[#F4F1EA] flex flex-col items-center justify-center p-6 pt-32 text-center">
        <ShoppingBag className="w-16 h-16 text-[#A68A56] mb-6 opacity-20" strokeWidth={1} />
        <h2 className="font-serif italic text-4xl text-[#1E331B]">Your cart is empty</h2>
        <p className="text-[10px] font-black uppercase tracking-[0.2em] text-[#6B705C] mt-4 mb-10 opacity-60">Discover artisan purity first.</p>
        <button onClick={() => router.push('/shop')} className="btn-primary px-10 py-5 text-[11px] font-black tracking-[0.3em] uppercase">Shop Catalog</button>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-[#F4F1EA] pt-40 pb-24 px-4 sm:px-6 lg:px-8">
      <div className="max-w-4xl mx-auto">
        <div className="mb-16 flex justify-between items-end border-b border-[#1E331B]/5 pb-10">
          <div>
            <h1 className="font-serif italic text-6xl text-[#1E331B]">Shopping Bag</h1>
            <p className="text-[10px] font-black uppercase tracking-[0.4em] text-[#A68A56] mt-4 uppercase">Hand-Picked Selections</p>
          </div>
          <p className="text-[12px] font-black uppercase tracking-widest text-[#6B705C]">{cart.length} Items</p>
        </div>

        <div className="space-y-12 mb-20 animate-fade-up">
          {cart.map((item) => (
            <div key={item.id} className="flex flex-col sm:flex-row items-center gap-10 group bg-white/40 p-10 rounded-2xl glass-panel border border-white hover:border-[#A68A56]/20 transition-all">
              <div className="w-32 h-40 bg-[#F4F1EA] rounded overflow-hidden shadow-lg group-hover:scale-105 transition-transform duration-700">
                <img src={item.image} alt={item.name} className="w-full h-full object-cover" />
              </div>
              <div className="flex-1 text-center sm:text-left">
                <h3 className="font-serif italic text-3xl text-[#1E331B] mb-2">{item.name}</h3>
                <p className="text-[10px] font-black text-[#A68A56] uppercase tracking-[0.2em] mb-4">Artisan Selection</p>
                <div className="flex items-center justify-center sm:justify-start gap-8 mt-4">
                  <div className="px-5 py-2.5 bg-[#F4F1EA] rounded-full text-[12px] font-black text-[#1E331B]">Qty: {item.quantity}</div>
                  <button onClick={() => removeFromCart(item.id)} className="text-red-600/40 hover:text-red-600 transition-colors">
                     <Trash2 size={20} strokeWidth={1.5} />
                  </button>
                </div>
              </div>
              <div className="text-right">
                <p className="text-2xl font-serif italic text-[#1E331B]">₹{item.price * item.quantity}</p>
                <p className="text-[9px] font-black uppercase tracking-widest text-[#6B705C] mt-1 opacity-50">Incl. Taxes</p>
              </div>
            </div>
          ))}
        </div>

        <div className="glass-panel p-12 rounded-3xl border border-white bg-white/70 shadow-2xl flex flex-col md:flex-row justify-between items-center gap-10">
          <div>
            <p className="text-[12px] font-black uppercase tracking-widest text-[#6B705C] mb-2 opacity-60">Basket Subtotal</p>
            <p className="font-serif italic text-5xl text-[#1E331B]">₹{total}</p>
          </div>
          <button 
            onClick={() => router.push('/checkout')}
            className="w-full md:w-auto btn-primary px-16 py-7 text-[12px] font-black tracking-[0.4em] uppercase flex items-center justify-center gap-4 group"
          >
            Fulfillment Options <ChevronRight className="group-hover:translate-x-2 transition-transform" />
          </button>
        </div>
      </div>
    </div>
  );
}
