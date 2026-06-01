import { apiClient } from '../api/api-client';

export interface OrderQuote {
  subtotal: number;
  discountTotal: number;
  deliveryFee: number;
  grandTotal: number;
  codAllowed: boolean;
  zoneLabel: string;
}

export interface OrderItemInput {
  variantId: string;
  quantity: number;
  productName?: string;
  unitPrice?: number;
}

export interface CreateOrderInput {
  addressId: string;
  paymentMethod: 'Cod' | 'Razorpay' | 'Upi';
  deliverySlotId: string;
  scheduledDeliveryDate: string;
  items: OrderItemInput[];
}

/**
 * High-Scale Order Management Service
 * Connects to OrderEndpoints.cs for Logistics & Fulfillment.
 */
export const orderService = {
  /**
   * Calculates checkout totals from items and delivery distance.
   */
  getCheckoutQuote: async (items: OrderItemInput[], distanceKm: number = 0): Promise<OrderQuote> => {
    try {
      return await apiClient<OrderQuote>('/orders/quote', {
        method: 'POST',
        body: JSON.stringify({ 
          items: items.map(i => ({ productName: i.productName, quantity: i.quantity, unitPrice: i.unitPrice })),
          distanceKm: distanceKm,
          paymentMethod: 'Cod'
        }),
      });
    } catch {
      // Professional Mock Fallback for logistics
      const subtotal = items.reduce((acc, curr) => acc + ((curr.unitPrice || 0) * curr.quantity), 0);
      return {
        subtotal,
        discountTotal: 0,
        deliveryFee: 30, // Mock base delivery
        grandTotal: subtotal + 30,
        codAllowed: true,
        zoneLabel: 'Standard Artisan Zone'
      };
    }
  },

  /**
   * Finalizes the order transaction.
   */
  createOrder: async (input: CreateOrderInput): Promise<{ orderId: string }> => {
    return await apiClient<{ orderId: string }>('/orders', {
      method: 'POST',
      body: JSON.stringify(input),
    });
  },

  /**
   * Retrieves order history for a specific customer.
   */
  getCustomerOrders: async (): Promise<any[]> => {
    return await apiClient<any[]>('/orders');
  }
};
