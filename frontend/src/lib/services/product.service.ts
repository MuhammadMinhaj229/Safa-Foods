import { apiClient } from '../api/api-client';
import type { Product } from '../types/product';

export const productService = {
  getProducts: async (): Promise<Product[]> => {
    return await apiClient<Product[]>('/products');
  },

  getProduct: async (id: string): Promise<Product> => {
    return await apiClient<Product>(`/products/${id}`);
  },

  createProduct: async (data: Product): Promise<Product> => {
    return await apiClient<Product>('/admin/products', {
      method: 'POST',
      body: JSON.stringify(data),
    });
  },

  updateProduct: async (id: string, data: Partial<Product>): Promise<Product> => {
    return await apiClient<Product>(`/admin/products/${id}`, {
      method: 'PUT',
      body: JSON.stringify(data),
    });
  },

  deleteProduct: async (id: string): Promise<void> => {
    return await apiClient<void>(`/admin/products/${id}`, {
      method: 'DELETE',
    });
  }
};
