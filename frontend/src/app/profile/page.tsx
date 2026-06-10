"use client";

import React, { useState, useEffect } from 'react';
import { useRole } from '@/lib/context/RoleContext';
import { orderService } from '@/lib/services/order.service';
import { SafaLogo } from '@/components/shared/SafaLogo';
import { 
  User, Package, MapPin, Settings, ShoppingBag, 
  ChevronRight, LogOut, ShieldCheck, TrendingUp, History 
} from 'lucide-react';
import { useRouter } from 'next/navigation';

export default function ProfilePage() {
  const { user, logout } = useRole();
  const router = useRouter();
  const [orders, setOrders] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    if (user) {
      orderService.getCustomerOrders()
        .then(setOrders)
        .catch(console.error)
        .finally(() => setLoading(false));
    }
  }, [user]);

  const handleLogout = () => {
    logout();
    router.push('/');
  };

  if (!user) {
    return (
      <div className="min-h-screen bg-[#F4F1EA] flex flex-col items-center justify-center p-6 pt-32 text-center">
        <ShieldCheck className="w-16 h-16 text-[#A68A56] mb-6 opacity-20" strokeWidth={1} />
        <h2 className="font-serif italic text-4xl text-[#1E331B]">Identity Required</h2>
        <p className="text-[10px] font-black uppercase tracking-[0.2em] text-[#6B705C] mt-4 mb-10 opacity-60">Please verify your artisan access first.</p>
        <button onClick={() => router.push('/auth/login')} className="btn-primary px-10 py-5 text-[11px] font-black tracking-[0.3em] uppercase">Authorize Access</button>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-[#F4F1EA] pt-40 pb-24 px-4 sm:px-6 lg:px-8">
      <div className="max-w-7xl mx-auto flex flex-col lg:flex-row gap-16">
        
        {/* Sidebar Nav */}
        <aside className="w-full lg:w-80 space-y-8">
          <div className="glass-panel p-10 rounded-2xl border border-white text-center shadow-xl">
            <div className="w-24 h-24 bg-[#1E331B] rounded-full flex items-center justify-center mx-auto mb-6 shadow-2xl relative overflow-hidden group">
               <User className="text-white w-10 h-10 relative z-10" />
               <div className="absolute inset-0 bg-[#A68A56] opacity-0 group-hover:opacity-20 transition-opacity" />
            </div>
            <h2 className="font-serif italic text-3xl text-[#1E331B] mb-2">
              {'identifier' in user ? user.identifier?.split('@')[0] : user.email?.split('@')[0] || 'Artisan Guest'}
            </h2>
            <div className="inline-block px-3 py-1 bg-[#1E331B]/10 rounded-full text-[9px] font-black uppercase tracking-widest text-[#1E331B] mb-8">
               Purity Tier: Gold
            </div>
            
            <nav className="space-y-4 pt-8 border-t border-[#1E331B]/5 text-left">
               <button className="flex items-center gap-4 w-full p-4 rounded-xl bg-[#1E331B] text-white shadow-xl text-[10px] font-black uppercase tracking-widest transition-all">
                  <Package size={16} /> <span>Live Orders</span> <ChevronRight size={14} className="ml-auto opacity-40" />
               </button>
               <button className="flex items-center gap-4 w-full p-4 rounded-xl hover:bg-white transition-all text-[10px] font-black uppercase tracking-widest text-[#6B705C]">
                  <History size={16} /> <span>Legacy History</span> <ChevronRight size={14} className="ml-auto opacity-40" />
               </button>
               <button className="flex items-center gap-4 w-full p-4 rounded-xl hover:bg-white transition-all text-[10px] font-black uppercase tracking-widest text-[#6B705C]">
                  <MapPin size={16} /> <span>Stored Addresses</span> <ChevronRight size={14} className="ml-auto opacity-40" />
               </button>
               <button className="flex items-center gap-4 w-full p-4 rounded-xl hover:bg-white transition-all text-[10px] font-black uppercase tracking-widest text-[#6B705C]">
                  <Settings size={16} /> <span>Security Key</span> <ChevronRight size={14} className="ml-auto opacity-40" />
               </button>
               <button onClick={handleLogout} className="flex items-center gap-4 w-full p-4 rounded-xl hover:bg-red-50 transition-all text-[10px] font-black uppercase tracking-widest text-red-600/60 mt-10">
                  <LogOut size={16} /> <span>Revoke Session</span>
               </button>
            </nav>
          </div>
        </aside>

        {/* Main Content */}
        <main className="flex-1 space-y-12">
          {/* Dashboard Hero */}
          <div className="glass-panel p-12 rounded-3xl border border-white bg-white/40 shadow-2xl relative overflow-hidden animate-fade-up">
             <div className="absolute top-[-20%] right-[-10%] opacity-5 scale-[2] pointer-events-none">
                <SafaLogo className="w-96 h-96" color="#1E331B" />
             </div>
             <div className="relative z-10 flex flex-col md:flex-row justify-between items-end gap-10">
                <div>
                   <h1 className="font-serif italic text-6xl text-[#1E331B] mb-4">Pure Operations</h1>
                   <p className="text-[12px] font-black uppercase tracking-[0.4em] text-[#A68A56] uppercase">
                     Identity: {'identifier' in user ? user.identifier : user.email}
                   </p>
                </div>
                <div className="flex gap-4">
                   <div className="bg-white p-6 rounded-2xl shadow-lg text-center border border-[#1E331B]/5">
                      <p className="text-[9px] font-black text-[#6B705C] uppercase tracking-widest mb-1">Total Artisan Batch</p>
                      <p className="font-serif italic text-3xl">₹14,290</p>
                   </div>
                   <div className="bg-[#1E331B] text-white p-6 rounded-2xl shadow-xl text-center">
                      <p className="text-[9px] font-black text-[#A68A56] uppercase tracking-widest mb-1">Current Active</p>
                      <p className="font-serif italic text-3xl">01</p>
                   </div>
                </div>
             </div>
          </div>

          {/* Orders / Live Feed */}
          <div className="animate-fade-up" style={{ animationDelay: '0.2s' }}>
             <div className="flex items-center justify-between mb-10 px-4">
                <h3 className="font-serif italic text-3xl text-[#1E331B]">Live Artisan Tracking</h3>
                <Link href="/shop" className="text-[10px] font-black text-[#A68A56] uppercase tracking-[0.2em] border-b border-[#A68A56]/20">Explore Fresh Catalog</Link>
             </div>

             {loading ? (
                <div className="space-y-6">
                   {[1,2].map(i => <div key={i} className="h-40 bg-white/50 rounded-2xl animate-pulse" />)}
                </div>
             ) : orders.length > 0 ? (
                <div className="space-y-6">
                   {orders.map(order => (
                      <div key={order.id} className="glass-panel p-8 rounded-2xl border border-white hover:border-[#A68A56]/30 transition-all group">
                         <div className="flex justify-between items-center">
                            <div className="flex items-center gap-6">
                               <div className="w-16 h-16 bg-[#F4F1EA] rounded-full flex items-center justify-center">
                                  <ShoppingBag className="text-[#A68A56]" />
                               </div>
                               <div>
                                  <p className="text-[10px] font-black text-[#6B705C] uppercase tracking-widest mb-1">{new Date(order.placedAt).toLocaleDateString()} • Batch #{order.id.slice(0,8)}</p>
                                  <p className="font-serif italic text-2xl text-[#1E331B]">{order.itemCount} Items Delivered</p>
                               </div>
                            </div>
                            <div className="text-right">
                               <p className="text-xl font-serif italic mb-1">₹{order.grandTotal}</p>
                               <span className="px-3 py-1 bg-green-50 text-green-600 text-[9px] font-black uppercase tracking-widest rounded-full">{order.status}</span>
                            </div>
                         </div>
                      </div>
                   ))}
                </div>
             ) : (
                <div className="glass-panel p-20 rounded-3xl border border-white bg-white/20 text-center">
                   <Package className="w-12 h-12 text-[#A68A56] mx-auto mb-6 opacity-20" />
                   <p className="text-[12px] font-black uppercase tracking-[0.3em] text-[#6B705C] opacity-60">No Live Hand-Crafted Batches Processed</p>
                   <button onClick={() => router.push('/shop')} className="btn-primary px-10 py-5 mt-10 text-[10px] font-black uppercase tracking-widest">Start First Artisan Order</button>
                </div>
             )}
          </div>
        </main>

      </div>
    </div>
  );
}

// Minimal Link replacement if not imported
function Link({ href, children, className }: any) {
   return <a href={href} className={className}>{children}</a>
}
