import { apiClient } from '../api/api-client';
import { Product } from '../api/types';

/**
 * Catalog Service
 * Connects to CatalogEndpoints.cs
 */
export const catalogService = {
  getProducts: async (category?: string): Promise<Product[]> => {
    const query = category ? `?category=${category}` : '';
    // MOCK DATA for safety during development
    try {
      return await apiClient<Product[]>(`/catalog${query}`);
    } catch {
      console.log('Using Mock Catalog Data');
      return [
        { id: '1', name: 'Fresh Ginger Garlic Paste', category: 'Masalas', price: 35, imagePath: 'https://images.unsplash.com/photo-1604329760661-e71c0c144ce1?auto=format&fit=crop&q=80&w=600', sizes: ['100g'], specs: 'Starter Pack', tag: 'New', description: 'Hand-ground' },
        { id: '2', name: 'Premium GGP', category: 'Masalas', price: 80, imagePath: 'https://images.unsplash.com/photo-1596040033229-a9821ebd058d?auto=format&fit=crop&q=80&w=600', sizes: ['250g'], tag: 'Trending', description: 'Artisan Batch' },
      ];
    }
  },

  getProductBySlug: async (id: string): Promise<Product> => {
    return await apiClient<Product>(`/catalog/${id}`);
  }
};
