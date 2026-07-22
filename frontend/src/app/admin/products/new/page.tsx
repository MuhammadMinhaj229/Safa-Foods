"use client";

import React, { useEffect, useState } from "react";
import Link from "next/link";
import { ArrowLeft } from "lucide-react";
import { productService } from "@/lib/services/product.service";
import type { Product } from "@/lib/types/product";
import { useRole } from "@/lib/context/RoleContext";
import { useRouter } from "next/navigation";
import { ProductForm } from "@/components/admin/ProductForm";

export default function NewProduct() {
  const [isSubmitting, setIsSubmitting] = useState(false);
  const { role } = useRole();
  const router = useRouter();

  useEffect(() => {
    if (role !== 'admin') {
      router.push('/auth/login');
    }
  }, [role, router]);

  const handleSubmit = async (data: Partial<Product>) => {
    setIsSubmitting(true);
    try {
      await productService.createProduct(data as Product);
      router.push('/admin/products');
    } catch (err) {
      console.error("Failed to create product", err);
      alert("Error creating product");
    } finally {
      setIsSubmitting(false);
    }
  };

  if (role !== 'admin') return null;

  return (
    <div className="min-h-screen bg-[#F4F1EA] p-8 mt-20">
      <div className="max-w-4xl mx-auto space-y-8 animate-fade-up">

        <header className="flex flex-col items-start bg-transparent py-2 gap-2">
          <Link href="/admin/products" className="inline-flex items-center text-gray-500 hover:text-[#A68A56] text-sm font-medium transition-colors">
            <ArrowLeft size={16} className="mr-1" />
            Back to Products
          </Link>
          <h1 className="text-3xl font-serif text-[#1E331B]">Add New Product</h1>
        </header>

        <ProductForm onSubmit={handleSubmit} isSubmitting={isSubmitting} />

      </div>
    </div>
  );
}
