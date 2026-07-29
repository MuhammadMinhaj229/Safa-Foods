"use client";

import React, { useState } from 'react';
import { useRouter } from 'next/navigation';
import { Save, X, ImageIcon, Loader2 } from 'lucide-react';
import type { Product } from '@/lib/types/product';
import Link from 'next/link';

interface ProductFormProps {
  initialData?: Product;
  onSubmit: (data: Partial<Product>) => Promise<void>;
  isSubmitting?: boolean;
}

export function ProductForm({ initialData, onSubmit, isSubmitting = false }: ProductFormProps) {
  const router = useRouter();

  const [formData, setFormData] = useState<Partial<Product>>({
    name: initialData?.name || '',
    description: initialData?.description || '',
    price: initialData?.price || 0,
    category: initialData?.category || 'sweets',
    stockQuantity: initialData?.stockQuantity || 0,
    imagePath: initialData?.imagePath || '',
    isAvailable: initialData?.isAvailable ?? true,
    ...initialData,
  });

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>) => {
    const { name, value, type } = e.target;

    let parsedValue: string | number | boolean = value;

    if (type === 'number') {
      parsedValue = parseFloat(value);
    } else if (type === 'checkbox') {
      parsedValue = (e.target as HTMLInputElement).checked;
    }

    setFormData(prev => ({
      ...prev,
      [name]: parsedValue
    }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    await onSubmit(formData);
  };

  return (
    <form onSubmit={handleSubmit} className="bg-white rounded-2xl shadow-sm border border-gray-100 p-6 md:p-8">
      <div className="grid grid-cols-1 md:grid-cols-2 gap-8">

        {/* Left Column: Main Details */}
        <div className="space-y-6">
          <h3 className="text-lg font-serif text-[#1E331B] border-b border-gray-100 pb-2">Basic Information</h3>

          <div>
            <label htmlFor="name" className="block text-sm font-bold text-gray-700 mb-1">Product Name</label>
            <input
              type="text"
              id="name"
              name="name"
              required
              value={formData.name || ''}
              onChange={handleChange}
              className="w-full px-4 py-2 bg-gray-50 border border-gray-200 rounded-xl focus:ring-2 focus:ring-[#A68A56] focus:bg-white transition-colors text-sm"
              placeholder="e.g. Premium Baklava"
            />
          </div>

          <div>
            <label htmlFor="description" className="block text-sm font-bold text-gray-700 mb-1">Description</label>
            <textarea
              id="description"
              name="description"
              rows={4}
              value={formData.description || ''}
              onChange={handleChange}
              className="w-full px-4 py-2 bg-gray-50 border border-gray-200 rounded-xl focus:ring-2 focus:ring-[#A68A56] focus:bg-white transition-colors text-sm resize-none"
              placeholder="Describe the product..."
            />
          </div>

          <div className="grid grid-cols-2 gap-4">
            <div>
              <label htmlFor="price" className="block text-sm font-bold text-gray-700 mb-1">Price (₹)</label>
              <input
                type="number"
                id="price"
                name="price"
                min="0"
                step="0.01"
                required
                value={formData.price || 0}
                onChange={handleChange}
                className="w-full px-4 py-2 bg-gray-50 border border-gray-200 rounded-xl focus:ring-2 focus:ring-[#A68A56] focus:bg-white transition-colors text-sm"
              />
            </div>
            <div>
              <label htmlFor="stockQuantity" className="block text-sm font-bold text-gray-700 mb-1">Stock</label>
              <input
                type="number"
                id="stockQuantity"
                name="stockQuantity"
                min="0"
                value={formData.stockQuantity || 0}
                onChange={handleChange}
                className="w-full px-4 py-2 bg-gray-50 border border-gray-200 rounded-xl focus:ring-2 focus:ring-[#A68A56] focus:bg-white transition-colors text-sm"
              />
            </div>
          </div>
        </div>

        {/* Right Column: Organization & Media */}
        <div className="space-y-6">
          <h3 className="text-lg font-serif text-[#1E331B] border-b border-gray-100 pb-2">Organization & Media</h3>

          <div>
            <label htmlFor="category" className="block text-sm font-bold text-gray-700 mb-1">Category</label>
            <select
              id="category"
              name="category"
              value={formData.category || 'sweets'}
              onChange={handleChange}
              className="w-full px-4 py-2 bg-gray-50 border border-gray-200 rounded-xl focus:ring-2 focus:ring-[#A68A56] focus:bg-white transition-colors text-sm"
            >
              <option value="sweets">Sweets</option>
              <option value="savouries">Savouries</option>
              <option value="bakery">Bakery</option>
              <option value="dry-fruits">Dry Fruits</option>
            </select>
          </div>

          <div>
            <label htmlFor="imagePath" className="block text-sm font-bold text-gray-700 mb-1">Image URL</label>
            <div className="flex gap-2 mb-4">
              <input
                type="text"
                id="imagePath"
                name="imagePath"
                value={formData.imagePath || ''}
                onChange={handleChange}
                className="w-full px-4 py-2 bg-gray-50 border border-gray-200 rounded-xl focus:ring-2 focus:ring-[#A68A56] focus:bg-white transition-colors text-sm"
                placeholder="https://example.com/image.jpg"
              />
            </div>

            {/* Image Preview */}
            <div className="w-full aspect-video bg-gray-50 rounded-xl border-2 border-dashed border-gray-200 flex flex-col items-center justify-center overflow-hidden relative">
              {formData.imagePath ? (
                // eslint-disable-next-line @next/next/no-img-element
                <img
                  src={formData.imagePath}
                  alt="Product preview"
                  className="w-full h-full object-cover"
                  onError={(e) => {
                    (e.target as HTMLImageElement).src = 'data:image/svg+xml;utf8,<svg xmlns="http://www.w3.org/2000/svg" width="100" height="100" fill="none" stroke="%239ca3af" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" viewBox="0 0 24 24"><rect width="18" height="18" x="3" y="3" rx="2" ry="2"/><circle cx="9" cy="9" r="2"/><path d="m21 15-3.086-3.086a2 2 0 0 0-2.828 0L6 21"/></svg>';
                  }}
                />
              ) : (
                <div className="text-center text-gray-400">
                  <ImageIcon size={32} className="mx-auto mb-2 opacity-50" />
                  <p className="text-xs">No image provided</p>
                </div>
              )}
            </div>
          </div>

          <div className="flex items-center p-4 bg-gray-50 rounded-xl border border-gray-200">
            <input
              type="checkbox"
              id="isAvailable"
              name="isAvailable"
              checked={formData.isAvailable || false}
              onChange={handleChange}
              className="w-4 h-4 text-[#A68A56] border-gray-300 rounded focus:ring-[#A68A56]"
            />
            <label htmlFor="isAvailable" className="ml-3 block text-sm font-medium text-gray-700">
              Product is available for purchase
            </label>
          </div>
        </div>
      </div>

      <div className="mt-8 pt-6 border-t border-gray-100 flex items-center justify-end gap-4">
        <Link
          href="/admin/products"
          className="px-6 py-2.5 text-gray-500 hover:bg-gray-50 rounded-full font-bold uppercase tracking-widest text-xs transition-colors"
        >
          Cancel
        </Link>
        <button
          type="submit"
          disabled={isSubmitting}
          className="inline-flex items-center justify-center gap-2 bg-[#A68A56] hover:bg-[#8A7143] disabled:opacity-50 disabled:cursor-not-allowed text-white px-8 py-2.5 rounded-full font-bold uppercase tracking-widest text-xs transition-colors shadow-lg"
        >
          {isSubmitting ? (
            <Loader2 size={16} className="animate-spin" />
          ) : (
            <Save size={16} />
          )}
          {initialData ? 'Update Product' : 'Save Product'}
        </button>
      </div>
    </form>
  );
}
