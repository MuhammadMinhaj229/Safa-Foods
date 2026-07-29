"use client";

import React, { useEffect, useState } from "react";
import Link from "next/link";
import { ArrowLeft, Plus, Edit, Trash2, Search, ImageIcon } from "lucide-react";
import { productService } from "@/lib/services/product.service";
import type { Product } from "@/lib/types/product";
import { useRole } from "@/lib/context/RoleContext";
import { useRouter } from "next/navigation";
import Image from "next/image";

export default function AdminProducts() {
  const [products, setProducts] = useState<Product[]>([]);
  const [loading, setLoading] = useState(true);
  const [search, setSearch] = useState("");
  const { role } = useRole();
  const router = useRouter();

  useEffect(() => {
    if (role !== 'admin') {
      router.push('/auth/login');
      return;
    }

    fetchProducts();
  }, [role, router]);

  const fetchProducts = async () => {
    setLoading(true);
    try {
      const data = await productService.getProducts();
      setProducts(data);
    } catch (err) {
      console.error("Failed to fetch products", err);
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (id: string) => {
    if (!window.confirm("Are you sure you want to delete this product?")) return;

    try {
      await productService.deleteProduct(id);
      setProducts(products.filter(p => p.id !== id));
    } catch (err) {
      console.error("Failed to delete product", err);
      alert("Error deleting product");
    }
  };

  const filteredProducts = products.filter(p =>
    p.name.toLowerCase().includes(search.toLowerCase()) ||
    p.description?.toLowerCase().includes(search.toLowerCase())
  );

  if (role !== 'admin') {
    return <div className="min-h-screen flex items-center justify-center">Loading...</div>;
  }

  return (
    <div className="min-h-screen bg-[#F4F1EA] p-8 mt-20">
      <div className="max-w-7xl mx-auto space-y-8 animate-fade-up">

        {/* Header */}
        <header className="flex flex-col sm:flex-row justify-between items-start sm:items-center bg-white p-6 rounded-2xl shadow-sm gap-4">
          <div>
            <Link href="/admin" className="inline-flex items-center text-gray-500 hover:text-[#A68A56] mb-2 text-sm font-medium transition-colors">
              <ArrowLeft size={16} className="mr-1" />
              Back to Dashboard
            </Link>
            <h1 className="text-3xl font-serif text-[#1E331B]">Manage Products</h1>
          </div>
          <Link href="/admin/products/new" className="inline-flex items-center justify-center gap-2 bg-[#A68A56] hover:bg-[#8A7143] text-white px-6 py-3 rounded-full font-bold uppercase tracking-widest text-xs transition-colors shadow-lg">
            <Plus size={16} />
            Add Product
          </Link>
        </header>

        {/* Toolbar */}
        <div className="bg-white p-4 rounded-2xl shadow-sm border border-gray-100 flex items-center gap-4">
          <div className="relative flex-1 max-w-md">
            <Search className="absolute left-3 top-1/2 -translate-y-1/2 text-gray-400" size={20} />
            <input
              type="text"
              placeholder="Search products..."
              value={search}
              onChange={(e) => setSearch(e.target.value)}
              className="w-full pl-10 pr-4 py-2 bg-gray-50 border-none rounded-xl focus:ring-2 focus:ring-[#A68A56] text-sm"
            />
          </div>
        </div>

        {/* Products Table */}
        <div className="bg-white rounded-2xl shadow-sm border border-gray-100 overflow-hidden">
          <div className="overflow-x-auto">
            <table className="w-full text-left border-collapse">
              <thead>
                <tr className="bg-gray-50/50 border-b border-gray-100">
                  <th className="px-6 py-4 text-xs font-black uppercase tracking-widest text-gray-500">Product</th>
                  <th className="px-6 py-4 text-xs font-black uppercase tracking-widest text-gray-500">Price</th>
                  <th className="px-6 py-4 text-xs font-black uppercase tracking-widest text-gray-500">Stock</th>
                  <th className="px-6 py-4 text-xs font-black uppercase tracking-widest text-gray-500">Category</th>
                  <th className="px-6 py-4 text-xs font-black uppercase tracking-widest text-gray-500 text-right">Actions</th>
                </tr>
              </thead>
              <tbody className="divide-y divide-gray-100">
                {loading ? (
                  <tr>
                    <td colSpan={5} className="px-6 py-12 text-center text-gray-500">
                      Loading products...
                    </td>
                  </tr>
                ) : filteredProducts.length === 0 ? (
                  <tr>
                    <td colSpan={5} className="px-6 py-12 text-center text-gray-500">
                      No products found.
                    </td>
                  </tr>
                ) : (
                  filteredProducts.map((product) => (
                    <tr key={product.id} className="hover:bg-gray-50/50 transition-colors">
                      <td className="px-6 py-4">
                        <div className="flex items-center gap-4">
                          <div className="w-12 h-12 bg-gray-100 rounded-lg overflow-hidden flex items-center justify-center shrink-0 border border-gray-200">
                            {product.imagePath ? (
                              <Image
                                src={product.imagePath}
                                alt={product.name}
                                width={48}
                                height={48}
                                className="w-full h-full object-cover"
                              />
                            ) : (
                              <ImageIcon className="text-gray-400" size={20} />
                            )}
                          </div>
                          <div>
                            <p className="font-bold text-[#1E331B]">{product.name}</p>
                            <p className="text-sm text-gray-500 line-clamp-1 max-w-xs">{product.description}</p>
                          </div>
                        </div>
                      </td>
                      <td className="px-6 py-4 font-medium text-[#1E331B]">₹{product.price}</td>
                      <td className="px-6 py-4">
                        <span className={`inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-bold ${
                          (product.stockQuantity || 0) > 10 ? 'bg-green-100 text-green-800' :
                          (product.stockQuantity || 0) > 0 ? 'bg-yellow-100 text-yellow-800' : 'bg-red-100 text-red-800'
                        }`}>
                          {product.stockQuantity || 0} in stock
                        </span>
                      </td>
                      <td className="px-6 py-4 text-sm text-gray-600 capitalize">
                        {product.category || '-'}
                      </td>
                      <td className="px-6 py-4">
                        <div className="flex justify-end gap-2">
                          <Link
                            href={`/admin/products/${product.id}/edit`}
                            className="p-2 text-gray-400 hover:text-[#A68A56] hover:bg-[#A68A56]/10 rounded-lg transition-colors"
                          >
                            <Edit size={18} />
                          </Link>
                          <button
                            onClick={() => handleDelete(product.id)}
                            className="p-2 text-gray-400 hover:text-red-500 hover:bg-red-50 rounded-lg transition-colors"
                          >
                            <Trash2 size={18} />
                          </button>
                        </div>
                      </td>
                    </tr>
                  ))
                )}
              </tbody>
            </table>
          </div>
        </div>

      </div>
    </div>
  );
}
