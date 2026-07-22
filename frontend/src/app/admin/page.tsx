"use client";

import React, { useEffect, useState } from "react";
import Link from "next/link";
import { Package, Users, ShoppingCart, Activity } from "lucide-react";
import { adminService } from "@/lib/services/admin.service";
import type { AdminStats } from "@/lib/api/types";
import { useRole } from "@/lib/context/RoleContext";
import { useRouter } from "next/navigation";

export default function AdminDashboard() {
  const [stats, setStats] = useState<AdminStats | null>(null);
  const { role, user } = useRole();
  const router = useRouter();

  useEffect(() => {
    // If not authenticated or not an admin, proxy.ts handles redirecting, but let's be safe client-side too
    if (role !== 'admin') {
      router.push('/auth/login');
      return;
    }

    const fetchStats = async () => {
      const data = await adminService.getDashboardStats();
      setStats(data);
    };

    fetchStats();
  }, [role, router]);

  if (role !== 'admin') {
    return <div className="min-h-screen flex items-center justify-center">Loading...</div>;
  }

  return (
    <div className="min-h-screen bg-[#F4F1EA] p-8 mt-20">
      <div className="max-w-7xl mx-auto space-y-8 animate-fade-up">

        <header className="flex justify-between items-center bg-white p-6 rounded-2xl shadow-sm">
          <div>
            <h1 className="text-3xl font-serif text-[#1E331B]">Admin Dashboard</h1>
            <p className="text-gray-500 mt-1">Welcome back, {(user as { email?: string })?.email || 'Admin'}</p>
          </div>
        </header>

        {/* Stats Grid */}
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
          <div className="bg-white p-6 rounded-2xl shadow-sm border border-gray-100 flex items-center space-x-4">
             <div className="p-3 bg-blue-50 text-blue-600 rounded-lg">
               <Activity size={24} />
             </div>
             <div>
               <p className="text-sm text-gray-500 font-medium">Live Orders</p>
               <h3 className="text-2xl font-bold text-[#1E331B]">{stats?.liveOrdersCount || 0}</h3>
             </div>
          </div>

          <div className="bg-white p-6 rounded-2xl shadow-sm border border-gray-100 flex items-center space-x-4">
             <div className="p-3 bg-green-50 text-green-600 rounded-lg">
               <Package size={24} />
             </div>
             <div>
               <p className="text-sm text-gray-500 font-medium">Low Stock SKUs</p>
               <h3 className="text-2xl font-bold text-[#1E331B]">{stats?.lowStockSkus || 0}</h3>
             </div>
          </div>

          <div className="bg-white p-6 rounded-2xl shadow-sm border border-gray-100 flex items-center space-x-4">
             <div className="p-3 bg-[#A68A56]/10 text-[#A68A56] rounded-lg">
               <ShoppingCart size={24} />
             </div>
             <div>
               <p className="text-sm text-gray-500 font-medium">Daily Revenue</p>
               <h3 className="text-2xl font-bold text-[#1E331B]">₹{stats?.dailyRevenue?.toLocaleString() || 0}</h3>
             </div>
          </div>

          <div className="bg-white p-6 rounded-2xl shadow-sm border border-gray-100 flex items-center space-x-4">
             <div className="p-3 bg-purple-50 text-purple-600 rounded-lg">
               <Users size={24} />
             </div>
             <div>
               <p className="text-sm text-gray-500 font-medium">Active Drivers</p>
               <h3 className="text-2xl font-bold text-[#1E331B]">{stats?.activeDrivers || 0}</h3>
             </div>
          </div>
        </div>

        {/* Quick Links / Navigation */}
        <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
          <Link href="/admin/products" className="group bg-white p-8 rounded-2xl shadow-sm border border-transparent hover:border-[#A68A56] transition-all">
            <Package size={32} className="text-[#A68A56] mb-4 group-hover:scale-110 transition-transform" />
            <h2 className="text-xl font-bold text-[#1E331B] mb-2">Manage Products</h2>
            <p className="text-gray-500 text-sm">Add, update, or remove products. Adjust pricing, stock, and descriptions.</p>
          </Link>

          <Link href="/admin/orders" className="group bg-white p-8 rounded-2xl shadow-sm border border-transparent hover:border-[#A68A56] transition-all">
            <ShoppingCart size={32} className="text-[#A68A56] mb-4 group-hover:scale-110 transition-transform" />
            <h2 className="text-xl font-bold text-[#1E331B] mb-2">Manage Orders</h2>
            <p className="text-gray-500 text-sm">View active orders, update status, and manage fulfillments.</p>
          </Link>

          <Link href="/admin/customers" className="group bg-white p-8 rounded-2xl shadow-sm border border-transparent hover:border-[#A68A56] transition-all">
            <Users size={32} className="text-[#A68A56] mb-4 group-hover:scale-110 transition-transform" />
            <h2 className="text-xl font-bold text-[#1E331B] mb-2">Manage Customers</h2>
            <p className="text-gray-500 text-sm">View customer accounts, order history, and support inquiries.</p>
          </Link>
        </div>
      </div>
    </div>
  );
}
