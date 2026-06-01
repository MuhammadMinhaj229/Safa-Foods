"use client";

import React, { useState, useEffect } from 'react';
import { LayoutDashboard, Package, ClipboardList, BarChart3, LogOut } from 'lucide-react';
import { SafaLogo } from '@/components/shared/SafaLogo';
import { adminService } from '@/lib/services/admin.service';
import { AdminStats } from '@/lib/api/types';

export const AdminDashboardAdapter = () => {
  const [stats, setStats] = useState<AdminStats | null>(null);

  useEffect(() => {
    // Initial data load
    adminService.getDashboardStats().then(setStats);
    
    // Industrial Standard Polling (30s)
    const interval = setInterval(() => {
      adminService.getDashboardStats().then(setStats);
    }, 30000);
    
    return () => clearInterval(interval);
  }, []);
  return (
    <div className="flex bg-[#F9F8F6] min-h-screen transition-opacity duration-500 pt-32">
      <aside className="w-80 bg-[#1E331B] text-[#F4F1EA] p-10 hidden lg:flex flex-col border-r border-[#A68A56]/20 fixed left-0 top-0 h-screen z-[40]">
        <div className="flex items-center gap-4 mb-20 mt-10">
          <SafaLogo className="w-10 h-10" color="#A68A56" />
          <span className="logo-text text-2xl font-black tracking-widest">SAFA OPS</span>
        </div>
        <nav className="flex flex-col gap-8 flex-1">
          <button className="flex items-center gap-5 text-sm font-black tracking-[0.3em] uppercase text-[#A68A56] p-4 bg-white/5 rounded-md text-left"><LayoutDashboard size={20}/> Dashboard</button>
          <button className="flex items-center gap-5 text-sm font-black tracking-[0.3em] uppercase opacity-50 hover:opacity-100 transition-all p-4 hover:bg-white/5 rounded-md text-left"><Package size={20}/> Inventory</button>
          <button className="flex items-center gap-5 text-sm font-black tracking-[0.3em] uppercase opacity-50 hover:opacity-100 transition-all p-4 hover:bg-white/5 rounded-md text-left"><ClipboardList size={20}/> Orders</button>
          <button className="flex items-center gap-5 text-sm font-black tracking-[0.3em] uppercase opacity-50 hover:opacity-100 transition-all p-4 hover:bg-white/5 rounded-md text-left"><BarChart3 size={20}/> Analytics</button>
        </nav>
        <button className="mt-auto flex items-center gap-5 text-xs font-black tracking-widest uppercase opacity-40 mb-10 hover:opacity-100"><LogOut size={20}/> System Exit</button>
      </aside>

      <main className="flex-1 lg:ml-80 p-8 lg:p-16 overflow-y-auto">
        <header className="flex flex-col md:flex-row justify-between items-start md:items-end mb-20 border-b border-[#1E331B]/10 pb-12 animate-fade-up">
          <div>
            <h1 className="font-serif italic text-6xl text-[#1E331B]">Operations Deck</h1>
            <p className="text-[#6B705C] font-black tracking-[0.4em] uppercase text-xs mt-4">Real-time Fulfillment Control</p>
          </div>
          <div className="flex flex-col gap-3 items-end mt-8 md:mt-0">
            <span className="text-[10px] font-black uppercase tracking-[0.4em] text-[#A68A56]">Capacity Tuning</span>
            <div className="flex items-center gap-6 bg-white p-5 shadow-sm border border-[#1E331B]/5 rounded-xl">
              <input type="range" className="accent-[#A68A56] w-56" defaultValue="85" />
              <span className="text-2xl font-black text-[#1E331B]">85%</span>
            </div>
          </div>
        </header>
        
        <div className="grid grid-cols-1 md:grid-cols-3 gap-10 mb-16 animate-fade-up">
          <StatCard label="Live Orders" value="24" color="#A68A56" />
          <StatCard label="Low Stock SKUs" value="03" color="#ef4444" />
          <StatCard label="Daily Revenue" value="₹18k" color="#1E331B" />
        </div>

        {/* Placeholder for real admin content */}
        <div className="bg-white p-12 border border-[#1E331B]/5 rounded-2xl shadow-sm">
          <p className="font-serif italic text-3xl text-[#1E331B]/40 text-center py-20">Monitoring Active Production Batches...</p>
        </div>
      </main>
    </div>
  );
};

const StatCard = ({ label, value, color }: { label: string; value: string; color: string }) => (
  <div className="bg-gradient-to-br from-white to-[#F9F8F6] p-10 border-l-[8px] shadow-[0_15px_40px_rgba(0,0,0,0.05)] rounded-r-2xl" style={{ borderColor: color }}>
    <span className="text-[11px] font-black uppercase tracking-[0.4em] text-[#6B705C]">{label}</span>
    <div className="text-7xl font-serif italic mt-4" style={{ color: color }}>{value}</div>
  </div>
);
