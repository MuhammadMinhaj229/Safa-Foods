export interface Product {
  id: string;
  name: string;
  description: string;
  price: number;
  imagePath: string;
  category: string;
  sizes: string[];
  specs?: string;
  tag?: string;
}

export interface Order {
  id: string;
  orderNumber: string;
  customerId: string;
  totalAmount: number;
  status: 'Pending' | 'Confirmed' | 'Dispatched' | 'Delivered' | 'Cancelled';
  createdAt: string;
  items: OrderItem[];
}

export interface OrderItem {
  id: string;
  productId: string;
  productName: string;
  quantity: number;
  unitPrice: number;
}

export interface AdminStats {
  liveOrdersCount: number;
  lowStockSkus: number;
  dailyRevenue: number;
  activeDrivers: number;
}

export interface DeliveryTask {
  orderId: string;
  orderNumber: string;
  pickupAddress: string;
  deliveryAddress: string;
  customerName: string;
  status: 'Assigned' | 'PickedUp' | 'InTransit';
}
