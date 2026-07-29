"use client";

import React, { useEffect, useState } from "react";
import Link from "next/link";
import { ArrowLeft } from "lucide-react";
import { productService } from "@/lib/services/product.service";
import type { Product } from "@/lib/types/product";
import { useRole } from "@/lib/context/RoleContext";
import { useRouter, useParams } from "next/navigation";
import { ProductForm } from "@/components/admin/ProductForm";

export default function EditProduct() {
  const [product, setProduct] = useState<Product | null>(null);
  const [loading, setLoading] = useState(true);
  const [isSubmitting, setIsSubmitting] = useState(false);

  const { role } = useRole();
  const router = useRouter();
  const params = useParams();
  const id = params.id as string;

  useEffect(() => {
    if (role !== 'admin') {
      router.push('/auth/login');
      return;
    }

    const fetchProduct = async () => {
      try {
        const data = await productService.getProduct(id);
        setProduct(data);
      } catch (err) {
        console.error("Failed to fetch product", err);
      } finally {
        setLoading(false);
      }
    };

    if (id) fetchProduct();
  }, [role, router, id]);

  const handleSubmit = async (data: Partial<Product>) => {
    setIsSubmitting(true);
    try {
      await productService.updateProduct(id, data);
      router.push('/admin/products');
    } catch (err) {
      console.error("Failed to update product", err);
      alert("Error updating product");
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
          <h1 className="text-3xl font-serif text-[#1E331B]">Edit Product</h1>
        </header>

        {loading ? (
          <div className="bg-white p-12 rounded-2xl shadow-sm border border-gray-100 flex items-center justify-center text-gray-500">
            Loading product data...
          </div>
        ) : product ? (
          <ProductForm initialData={product} onSubmit={handleSubmit} isSubmitting={isSubmitting} />
        ) : (
          <div className="bg-white p-12 rounded-2xl shadow-sm border border-gray-100 flex items-center justify-center text-gray-500">
            Product not found.
          </div>
        )}

      </div>
    </div>
  );
}
