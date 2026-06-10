"use client";

import React, { createContext, useContext, useState, useEffect } from 'react';

interface CartItem {
  id: string;
  name: string;
  price: number;
  quantity: number;
  image: string;
}

interface CartContextType {
  cart: CartItem[];
  addToCart: (item: { id: string; name: string; price: number | string; image?: string; imagePath?: string }) => void;
  removeFromCart: (id: string) => void;
  clearCart: () => void;
  total: number;
  count: number;
}

const CartContext = createContext<CartContextType | undefined>(undefined);

export const CartProvider = ({ children }: { children: React.ReactNode }) => {
  const [cart, setCart] = useState<CartItem[]>([]);
  const [hydrated, setHydrated] = useState(false);

  // Persistent Hydration
  useEffect(() => {
    const saved = localStorage.getItem('safa_cart');
    if (saved) {
      setCart(JSON.parse(saved));
    }
    setHydrated(true);
  }, []);

  useEffect(() => {
    if (hydrated) {
      localStorage.setItem('safa_cart', JSON.stringify(cart));
    }
  }, [cart, hydrated]);

  const addToCart = (product: { id: string; name: string; price: number | string; image?: string; imagePath?: string }) => {
    setCart(prev => {
      const existing = prev.find(i => i.id === product.id);
      if (existing) {
        return prev.map(i => i.id === product.id ? { ...i, quantity: i.quantity + 1 } : i);
      }
      return [...prev, { id: product.id, name: product.name, price: Number(product.price), quantity: 1, image: product.imagePath || product.image || '' }];
    });
  };

  const removeFromCart = (id: string) => {
    setCart(prev => prev.filter(i => i.id !== id));
  };

  const clearCart = () => setCart([]);

  const total = cart.reduce((acc, curr) => acc + (curr.price * curr.quantity), 0);
  const count = cart.reduce((acc, curr) => acc + curr.quantity, 0);

  return (
    <CartContext.Provider value={{ cart, addToCart, removeFromCart, clearCart, total, count }}>
      {children}
    </CartContext.Provider>
  );
};

export const useCart = () => {
  const context = useContext(CartContext);
  if (!context) throw new Error('useCart must be used within a CartProvider');
  return context;
};
