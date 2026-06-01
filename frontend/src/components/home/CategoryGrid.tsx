"use client";

import React from 'react';
import Link from 'next/link';

const categories = [
  { name: 'Pickles', icon: 'https://cdn-icons-png.flaticon.com/512/3014/3014524.png' },
  { name: 'Masalas', icon: 'https://cdn-icons-png.flaticon.com/512/2713/2713919.png' },
  { name: 'Powders', icon: 'https://cdn-icons-png.flaticon.com/512/1041/1041355.png' },
  { name: 'Ready-to-Cook', icon: 'https://cdn-icons-png.flaticon.com/512/3014/3014447.png' },
  { name: 'Profile', icon: 'https://cdn-icons-png.flaticon.com/512/3501/3501170.png' }
];

export const CategoryGrid = () => {
  return (
    <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 -mt-20 relative z-30 pb-20">
      <div className="bg-white/70 backdrop-blur-2xl p-10 lg:p-12 shadow-[0_20px_60px_rgba(0,0,0,0.05)] border border-white rounded-xl">
        <h3 className="text-[10px] font-black tracking-[0.5em] uppercase text-[#A68A56] mb-10 text-center">Curated Selections</h3>
        <div className="flex overflow-x-auto gap-8 lg:gap-12 pb-6 scrollbar-hide justify-between px-4">
          {categories.map((cat, i) => (
            <Link 
              key={i} 
              href={cat.name === 'Profile' ? '/profile' : '/shop'} 
              className="flex flex-col items-center gap-4 group cursor-pointer min-w-[100px]"
            >
              <div className="w-20 h-20 lg:w-24 lg:h-24 rounded-full bg-gradient-to-b from-[#F4F1EA] to-white flex items-center justify-center border-2 border-white group-hover:border-[#A68A56] transition-all duration-500 p-5 shadow-sm group-hover:shadow-[0_15px_30px_rgba(166,138,86,0.15)] group-hover:-translate-y-2">
                <img 
                  src={cat.icon} 
                  alt={cat.name} 
                  className="w-full h-full object-contain grayscale opacity-70 group-hover:opacity-100 group-hover:grayscale-0 transition-all duration-500"
                />
              </div>
              <span className="text-[10px] font-black tracking-widest uppercase text-[#1E331B] group-hover:text-[#A68A56] transition-colors">
                {cat.name}
              </span>
            </Link>
          ))}
        </div>
      </div>
    </div>
  );
};
