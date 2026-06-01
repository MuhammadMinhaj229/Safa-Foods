"use client";

import React from 'react';
import { useRole } from '@/lib/context/RoleContext';
import { CustomerHome } from '@/components/home/CustomerHome';
import { AdminDashboardAdapter } from '@/components/home/AdminDashboardAdapter';
import { DeliveryHubAdapter } from '@/components/home/DeliveryHubAdapter';

/**
 * Safa Foods Master Page
 * Adaptively renders the Customer, Admin, or Delivery experience 
 * based on the global RoleContext.
 */
export default function Home() {
  const { role } = useRole();

  return (
    <div className="min-h-screen bg-[#F4F1EA]">
      {role === 'customer' && <CustomerHome />}
      {role === 'admin' && <AdminDashboardAdapter />}
      {role === 'delivery' && <DeliveryHubAdapter />}
    </div>
  );
}
