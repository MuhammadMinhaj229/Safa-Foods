"use client";

import React, { useState, useEffect } from 'react';
import { TrendingUp, ArrowRight } from 'lucide-react';
import { ProductCard } from '@/components/shop/ProductCard';
import Link from 'next/link';
import { catalogService } from '@/lib/services/catalog.service';
import { Product } from '@/lib/api/types';

export const TrendingFeed = () => {
  const [products, setProducts] = useState<Product[]>([]);

  useEffect(() => {
    catalogService.getProducts().then(setProducts);
  }, []);
  return (
    <section className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-24">
      <div className="flex items-center justify-between mb-16">
        <div className="flex items-center gap-6">
          <TrendingUp size={40} className="text-[#A68A56]" strokeWidth={1.5} />
          <h2 className="font-serif italic text-5xl lg:text-6xl text-[#1E331B]">Trending Now</h2>
        </div>
        <Link 
          href="/shop" 
          className="hidden sm:flex items-center gap-3 text-[11px] font-black tracking-[0.3em] uppercase text-[#6B705C] hover:text-[#A68A56] transition-all group"
        >
          View Collection <ArrowRight size={16} className="group-hover:translate-x-2 transition-transform" />
        </Link>
      </div>
      <div className="grid grid-cols-1 md:grid-cols-3 gap-16 lg:gap-20">
        {products.map(p => (
          <ProductCard key={p.id} product={p} />
        ))}
      </div>
    </section>
  );
};
