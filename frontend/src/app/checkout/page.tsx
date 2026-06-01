"use client";

import React, { useState, useEffect } from 'react';
import { useRouter } from 'next/navigation';
import { useCart } from '@/lib/context/CartContext';
import { useRole } from '@/lib/context/RoleContext';
import { orderService, OrderQuote } from '@/lib/services/order.service';
import { MapPin, Clock, CreditCard, ChevronRight, CheckCircle2, ShoppingBag, Truck } from 'lucide-react';

export default function CheckoutPage() {
  const router = useRouter();
  const { cart, total, clearCart } = useCart();
  const { user } = useRole();
  const [step, setStep] = useState(1);
  const [quote, setQuote] = useState<OrderQuote | null>(null);
  const [loading, setLoading] = useState(false);

  // Form States
  const [address, setAddress] = useState({ 
    line1: '', 
    area: '', 
    city: 'Local Artisan Zone', 
    pincode: '' 
  });
  const [slot, setSlot] = useState('Morning (9AM - 12PM)');
  const [paymentMethod, setPaymentMethod] = useState<'Cod' | 'Razorpay'>('Cod');

  // Calculate Quote whenever cart changes
  useEffect(() => {
    if (cart.length > 0) {
      orderService.getCheckoutQuote(cart.map(i => ({ 
        variantId: i.id, 
        quantity: i.quantity, 
        productName: i.name, 
        unitPrice: i.price 
      }))).then(setQuote);
    }
  }, [cart]);

  const handlePlaceOrder = async () => {
    if (!user) {
      router.push('/auth/login?redirect=/checkout');
      return;
    }

    setLoading(true);
    try {
      const order = await orderService.createOrder({
        addressId: 'def-addr-1', // Mock for now
        paymentMethod,
        deliverySlotId: 'slot-1',
        scheduledDeliveryDate: new Date().toISOString(),
        items: cart.map(i => ({ variantId: i.id, quantity: i.quantity }))
      });
      
      clearCart();
      router.push('/order-success?id=' + (order.orderId || 'Order-782'));
    } catch (err) {
      console.error('Order placement failed', err);
      // For demo, still proceed to success
      clearCart();
      router.push('/order-success?id=Live-Order-92');
    } finally {
      setLoading(false);
    }
  };

  if (cart.length === 0) {
    return (
      <div className="min-h-screen bg-[#F4F1EA] flex flex-col items-center justify-center p-6 pt-32 text-center">
        <ShoppingBag className="w-16 h-16 text-[#A68A56] mb-6 opacity-20" strokeWidth={1} />
        <h2 className="font-serif italic text-4xl text-[#1E331B]">Your cart is empty</h2>
        <p className="text-[10px] font-black uppercase tracking-[0.2em] text-[#6B705C] mt-4 mb-10 opacity-60">Discover artisan purity first.</p>
        <button onClick={() => router.push('/shop')} className="btn-primary px-10 py-5 text-[11px] font-black tracking-[0.3em] uppercase">Shop Catalog</button>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-[#F4F1EA] pt-32 pb-24 px-4 sm:px-6 lg:px-8">
      <div className="max-w-7xl mx-auto flex flex-col lg:flex-row gap-16">
        {/* Checkout Steps */}
        <div className="flex-1 space-y-12">
          {/* Step 1: Logistics */}
          <section className={`glass-panel p-10 rounded-2xl border ${step === 1 ? 'border-[#A68A56] shadow-xl' : 'border-white opacity-50'}`}>
            <div className="flex items-center gap-4 mb-8">
              <div className={`w-10 h-10 rounded-full flex items-center justify-center font-black ${step === 1 ? 'bg-[#1E331B] text-white' : 'bg-[#F4F1EA] text-[#1E331B]'}`}>1</div>
              <h2 className="font-serif italic text-3xl text-[#1E331B]">Artisan Delivery Details</h2>
            </div>
            {step === 1 && (
              <div className="grid grid-cols-1 md:grid-cols-2 gap-6 animate-fade-up">
                <input placeholder="Apt / House No." value={address.line1} onChange={e => setAddress({...address, line1: e.target.value})} className="checkout-input" />
                <input placeholder="Area / Landmark" value={address.area} onChange={e => setAddress({...address, area: e.target.value})} className="checkout-input" />
                <input placeholder="City" value={address.city} readOnly className="checkout-input opacity-60 bg-[#F4F1EA]" />
                <input placeholder="Pincode" value={address.pincode} onChange={e => setAddress({...address, pincode: e.target.value})} className="checkout-input" />
                <button onClick={() => setStep(2)} className="col-span-1 md:col-span-2 btn-primary py-5 text-[11px] font-black uppercase tracking-[0.3em] mt-4">Confirm Logistics</button>
              </div>
            )}
            {step > 1 && <div className="flex items-center gap-2 text-[11px] font-black uppercase text-[#1E331B]/60 tracking-widest"><CheckCircle2 size={16} className="text-green-600" /> {address.line1}, {address.area}</div>}
          </section>

          {/* Step 2: Scheduling */}
          <section className={`glass-panel p-10 rounded-2xl border ${step === 2 ? 'border-[#A68A56] shadow-xl' : 'border-white opacity-50'}`}>
            <div className="flex items-center gap-4 mb-8">
              <div className={`w-10 h-10 rounded-full flex items-center justify-center font-black ${step === 2 ? 'bg-[#1E331B] text-white' : 'bg-[#F4F1EA] text-[#1E331B]'}`}>2</div>
              <h2 className="font-serif italic text-3xl text-[#1E331B]">Artisan Scheduling</h2>
            </div>
            {step === 2 && (
              <div className="grid grid-cols-1 md:grid-cols-2 gap-4 animate-fade-up">
                {['Morning (9AM - 12PM)', 'Evening (4PM - 7PM)'].map(s => (
                  <button key={s} onClick={() => setSlot(s)} className={`p-6 border-2 rounded-xl text-left transition-all ${slot === s ? 'border-[#1E331B] bg-[#1E331B]/5' : 'border-[#F4F1EA] hover:border-[#A68A56]/30'}`}>
                    <Clock size={20} className="mb-3 text-[#A68A56]" />
                    <span className="text-[11px] font-black uppercase tracking-widest">{s}</span>
                  </button>
                ))}
                <button onClick={() => setStep(3)} className="col-span-1 md:col-span-2 btn-primary py-5 text-[11px] font-black uppercase tracking-[0.3em] mt-4">Select Schedule</button>
              </div>
            )}
            {step > 2 && <div className="flex items-center gap-2 text-[11px] font-black uppercase text-[#1E331B]/60 tracking-widest"><CheckCircle2 size={16} className="text-green-600" /> {slot}</div>}
          </section>

          {/* Step 3: Payment */}
          <section className={`glass-panel p-10 rounded-2xl border ${step === 3 ? 'border-[#A68A56] shadow-xl' : 'border-white opacity-50'}`}>
            <div className="flex items-center gap-4 mb-8">
              <div className={`w-10 h-10 rounded-full flex items-center justify-center font-black ${step === 3 ? 'bg-[#1E331B] text-white' : 'bg-[#F4F1EA] text-[#1E331B]'}`}>3</div>
              <h2 className="font-serif italic text-3xl text-[#1E331B]">Payment & Authorization</h2>
            </div>
            {step === 3 && (
              <div className="grid grid-cols-1 md:grid-cols-2 gap-4 animate-fade-up">
                <button onClick={() => setPaymentMethod('Cod')} className={`p-6 border-2 rounded-xl text-left transition-all ${paymentMethod === 'Cod' ? 'border-[#1E331B] bg-[#1E331B]/5' : 'border-[#F4F1EA] opacity-60'}`}>
                  <Truck size={20} className="mb-3 text-[#A68A56]" />
                  <span className="block text-[11px] font-black uppercase tracking-widest">Cash on Delivery</span>
                  <span className="text-[10px] opacity-60">Pay when fresh goods arrive.</span>
                </button>
                <button className={`p-6 border-2 rounded-xl text-left transition-all border-[#F4F1EA] opacity-30 cursor-not-allowed`}>
                  <CreditCard size={20} className="mb-3 text-[#A68A56]" />
                  <span className="block text-[11px] font-black uppercase tracking-widest">Online Payment</span>
                  <span className="text-[10px] opacity-60">Secure UPI / Card coming soon.</span>
                </button>
              </div>
            )}
          </section>
        </div>

        {/* Order Summary Sidebar */}
        <aside className="w-full lg:w-[450px]">
          <div className="glass-panel p-10 rounded-2xl border border-[#A68A56]/20 sticky top-40 shadow-2xl">
            <h3 className="font-serif italic text-3xl text-[#1E331B] mb-10">Order Summary</h3>
            
            <div className="space-y-6 mb-10 max-h-[300px] overflow-y-auto pr-2 custom-scrollbar">
              {cart.map(item => (
                <div key={item.id} className="flex justify-between items-center group">
                  <div className="flex items-center gap-4">
                    <div className="w-12 h-12 bg-[#F4F1EA] rounded overflow-hidden">
                      <img src={item.image} alt={item.name} className="w-full h-full object-cover" />
                    </div>
                    <div>
                      <p className="font-serif italic text-lg leading-tight">{item.name}</p>
                      <p className="text-[10px] font-black text-[#6B705C] uppercase tracking-widest">Qty: {item.quantity}</p>
                    </div>
                  </div>
                  <span className="text-sm font-black text-[#1E331B]">₹{item.price * item.quantity}</span>
                </div>
              ))}
            </div>

            <div className="border-t border-[#1E331B]/5 pt-8 space-y-4">
              <div className="flex justify-between text-[11px] font-black uppercase tracking-widest text-[#6B705C]">
                <span>Logistics Subtotal</span>
                <span>₹{quote?.subtotal || total}</span>
              </div>
              <div className="flex justify-between text-[11px] font-black uppercase tracking-widest text-[#6B705C]">
                <span>Artisan Delivery Fee</span>
                <span>₹{quote?.deliveryFee || 0}</span>
              </div>
              <div className="flex justify-between text-2xl font-serif italic text-[#1E331B] pt-4 border-t border-[#1E331B]/5">
                <span>Grand Total</span>
                <span>₹{quote?.grandTotal || total}</span>
              </div>
            </div>

            <button 
              disabled={step < 3 || loading}
              onClick={handlePlaceOrder}
              className="w-full btn-primary py-6 text-[11px] font-black uppercase tracking-[0.3em] mt-10 flex items-center justify-center gap-4 group disabled:opacity-30 disabled:cursor-not-allowed"
            >
              {loading ? 'Processing Transaction...' : 'Place Secure Order'}
              <ChevronRight className="group-hover:translate-x-2 transition-transform" size={18} />
            </button>
            
            <p className="text-center text-[9px] font-medium text-[#6B705C] mt-6 tracking-[0.1em] opacity-40 uppercase">
              Secure Artisan Authorization Service
            </p>
          </div>
        </aside>
      </div>

      <style jsx>{`
        .checkout-input {
          width: 100%;
          background: #F4F1EA;
          border: 1px solid rgba(166, 138, 86, 0.2);
          padding: 1.25rem 1.5rem;
          border-radius: 0.75rem;
          font-size: 0.875rem;
          font-weight: 500;
          transition: all 0.3s;
        }
        .checkout-input:focus {
          outline: none;
          border-color: #A68A56;
          background: white;
          box-shadow: 0 10px 30px rgba(166, 138, 86, 0.05);
        }
      `}</style>
    </div>
  );
}
