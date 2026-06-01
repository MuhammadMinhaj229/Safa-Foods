"use client";

import React from 'react';
import { User, LayoutDashboard, Bike } from 'lucide-react';
import { useRole } from '@/lib/context/RoleContext';

export const RoleSwitcher = () => {
  const { role, setRole, isDev } = useRole();

  if (!isDev) return null;

  return (
    <div className="fixed bottom-24 lg:bottom-12 right-6 z-[100] flex flex-col gap-2 scale-90 sm:scale-100 p-2 bg-white/50 backdrop-blur-md rounded-full border border-white/40 shadow-2xl animate-fade-up">
      <button 
        onClick={() => setRole('customer')} 
        className={`p-3 rounded-full transition-all duration-300 ${role === 'customer' ? 'bg-gradient-to-r from-[#A68A56] to-[#C3A970] text-white shadow-lg scale-110' : 'bg-transparent text-[#1E331B] hover:bg-white/60'}`} 
        title="Customer View"
      >
        <User size={20}/>
      </button>
      <button 
        onClick={() => setRole('admin')} 
        className={`p-3 rounded-full transition-all duration-300 ${role === 'admin' ? 'bg-[#1E331B] text-white shadow-lg scale-110' : 'bg-transparent text-[#1E331B] hover:bg-white/60'}`} 
        title="Admin Dashboard"
      >
        <LayoutDashboard size={20}/>
      </button>
      <button 
        onClick={() => setRole('delivery')} 
        className={`p-3 rounded-full transition-all duration-300 ${role === 'delivery' ? 'bg-[#111] text-[#A68A56] shadow-lg scale-110' : 'bg-transparent text-[#1E331B] hover:bg-white/60'}`} 
        title="Delivery Hub"
      >
        <Bike size={20}/>
      </button>
    </div>
  );
};
