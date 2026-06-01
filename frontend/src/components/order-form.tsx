"use client";

import { useMemo, useState } from "react";

import type { BackendCatalogProduct } from "@/lib/backend-api";
import { apiClient } from "@/lib/api/api-client";

type OrderQuoteResponse = {
  subtotal: number;
  discountTotal: number;
  deliveryFee: number;
  grandTotal: number;
  codAllowed: boolean;
  zoneLabel: string | null;
};

type AddressCreateResponse = {
  addressId: string;
  serviceable: boolean;
  zoneLabel: string | null;
  deliveryFee: number | null;
  distanceKm: number;
};

type OrderCreateResponse = {
  orderId: string;
  status: string;
  subtotal: number;
  discountTotal: number;
  deliveryFee: number;
  grandTotal: number;
  zoneLabel: string | null;
  codAllowed: boolean;
};

type OrderFormProps = {
  product: BackendCatalogProduct;
};

function hasStoredAccessToken() {
  if (typeof window === "undefined") {
    return false;
  }

  const rawSession = window.localStorage.getItem("safa_session");
  if (!rawSession) {
    return false;
  }

  try {
    const session = JSON.parse(rawSession) as { accessToken?: string };
    return Boolean(session.accessToken);
  } catch {
    return false;
  }
}

export function OrderForm({ product }: OrderFormProps) {
  const firstVariant = product.variants[0];
  const [variantId, setVariantId] = useState(firstVariant?.variantId ?? "");
  const [quantity, setQuantity] = useState(1);
  const [distanceKm, setDistanceKm] = useState("3");
  const [paymentMethod, setPaymentMethod] = useState<"Razorpay" | "Cod">("Cod");
  const [fullName, setFullName] = useState("");
  const [phone, setPhone] = useState("");
  const [email, setEmail] = useState("");
  const [addressLine1, setAddressLine1] = useState("");
  const [addressLine2, setAddressLine2] = useState("");
  const [landmark, setLandmark] = useState("");
  const [area, setArea] = useState("");
  const [city, setCity] = useState("Warangal");
  const [pincode, setPincode] = useState("");
  const [quote, setQuote] = useState<OrderQuoteResponse | null>(null);
  const [orderResult, setOrderResult] = useState<OrderCreateResponse | null>(null);
  const [error, setError] = useState<string | null>(null);
  const [isLoading, setIsLoading] = useState(false);

  const selectedVariant = useMemo(
    () => product.variants.find((variant) => variant.variantId === variantId) ?? firstVariant,
    [firstVariant, product.variants, variantId],
  );

  const unitPrice = selectedVariant?.salePrice ?? selectedVariant?.price ?? 0;

  async function handleQuote() {
    setIsLoading(true);
    setError(null);
    setOrderResult(null);

    try {
      const response = await fetch("/api/orders/quote", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          distanceKm: Number(distanceKm),
          paymentMethod,
          items: [
            {
              productName: product.name,
              quantity,
              unitPrice,
            },
          ],
        }),
      });

      const data = (await response.json()) as OrderQuoteResponse | { error?: { message?: string } };
      if (!response.ok) {
        throw new Error((data as { error?: { message?: string } }).error?.message ?? "Quote failed.");
      }

      setQuote(data as OrderQuoteResponse);
    } catch (quoteError) {
      setError(quoteError instanceof Error ? quoteError.message : "Quote failed.");
    } finally {
      setIsLoading(false);
    }
  }

  async function handlePlaceOrder() {
    setIsLoading(true);
    setError(null);
    setOrderResult(null);

    try {
      const isAuthenticated = hasStoredAccessToken();
      const addressEndpoint = isAuthenticated ? "/me/addresses" : "/addresses";
      const createdAddress = await apiClient<AddressCreateResponse>(addressEndpoint, {
        method: "POST",
        body: JSON.stringify({
          fullName,
          phone,
          ...(isAuthenticated ? {} : { email }),
          addressLine1,
          addressLine2: addressLine2 || null,
          landmark: landmark || null,
          area,
          city,
          pincode,
          distanceKm: Number(distanceKm),
          isDefault: true,
        }),
      });

      if (!createdAddress.serviceable) {
        throw new Error("This address is outside the current serviceable delivery area.");
      }

      const orderResponse = await apiClient<OrderCreateResponse>("/orders", {
        method: "POST",
        body: JSON.stringify({
          addressId: createdAddress.addressId,
          paymentMethod,
          items: [
            {
              variantId,
              quantity,
            },
          ],
        }),
      });

      setOrderResult(orderResponse);
      await handleQuote();
    } catch (orderError) {
      setError(orderError instanceof Error ? orderError.message : "Order creation failed.");
    } finally {
      setIsLoading(false);
    }
  }

  return (
    <div className="grid gap-8 lg:grid-cols-[0.9fr_1.1fr]">
      <section className="rounded-[2rem] bg-[var(--color-surface)] p-7 shadow-[0_24px_70px_rgba(16,43,27,0.08)]">
        <p className="text-sm uppercase tracking-[0.16em] text-[var(--color-leaf)]">
          Product configuration
        </p>
        <div className="mt-6 space-y-5">
          <label className="grid gap-2 text-sm text-[var(--color-olive)]">
            Variant
            <select
              value={variantId}
              onChange={(event) => setVariantId(event.target.value)}
              className="rounded-2xl border border-[var(--color-sand-deep)] bg-white px-4 py-3 text-[var(--color-ink)]"
            >
              {product.variants.map((variant) => (
                <option key={variant.variantId} value={variant.variantId}>
                  {variant.label} · Rs. {(variant.salePrice ?? variant.price).toFixed(0)}
                </option>
              ))}
            </select>
          </label>

          <div className="grid gap-5 sm:grid-cols-2">
            <label className="grid gap-2 text-sm text-[var(--color-olive)]">
              Quantity
              <input
                type="number"
                min={1}
                value={quantity}
                onChange={(event) => setQuantity(Number(event.target.value))}
                className="rounded-2xl border border-[var(--color-sand-deep)] bg-white px-4 py-3 text-[var(--color-ink)]"
              />
            </label>
            <label className="grid gap-2 text-sm text-[var(--color-olive)]">
              Approx distance from shop (km)
              <input
                type="number"
                min={0}
                step="0.1"
                value={distanceKm}
                onChange={(event) => setDistanceKm(event.target.value)}
                className="rounded-2xl border border-[var(--color-sand-deep)] bg-white px-4 py-3 text-[var(--color-ink)]"
              />
            </label>
          </div>

          <label className="grid gap-2 text-sm text-[var(--color-olive)]">
            Payment method
            <select
              value={paymentMethod}
              onChange={(event) => setPaymentMethod(event.target.value as "Razorpay" | "Cod")}
              className="rounded-2xl border border-[var(--color-sand-deep)] bg-white px-4 py-3 text-[var(--color-ink)]"
            >
              <option value="Cod">Cash on delivery</option>
              <option value="Razorpay">Razorpay</option>
            </select>
          </label>
        </div>

        <button
          type="button"
          onClick={handleQuote}
          disabled={isLoading}
          className="mt-8 w-full rounded-full bg-[var(--color-leaf)] px-6 py-4 text-sm font-bold uppercase tracking-[0.12em] text-[var(--color-surface)] transition hover:bg-[var(--color-forest-deep)] disabled:cursor-not-allowed disabled:opacity-70"
        >
          {isLoading ? "Calculating..." : "Get live quote"}
        </button>

        {quote ? (
          <div className="mt-6 rounded-[1.5rem] bg-[var(--color-sand)] p-5 text-sm text-[var(--color-olive)]">
            <p className="font-semibold text-[var(--color-ink)]">Live order quote</p>
            <p className="mt-3">Subtotal: Rs. {quote.subtotal.toFixed(2)}</p>
            <p>Delivery fee: Rs. {quote.deliveryFee.toFixed(2)}</p>
            <p>Zone: {quote.zoneLabel ?? "Not serviceable"}</p>
            <p className="mt-2 font-semibold text-[var(--color-ink)]">
              Grand total: Rs. {quote.grandTotal.toFixed(2)}
            </p>
          </div>
        ) : null}
      </section>

      <section className="rounded-[2rem] bg-[var(--color-forest-deep)] p-7 text-[var(--color-surface)]">
        <p className="text-sm uppercase tracking-[0.16em] text-[var(--color-accent)]">
          Delivery details
        </p>
        <div className="mt-6 grid gap-4 sm:grid-cols-2">
          <label className="grid gap-2 text-sm text-[var(--color-muted)]">
            Full name
            <input value={fullName} onChange={(e) => setFullName(e.target.value)} className="rounded-2xl border border-white/10 bg-white/5 px-4 py-3 text-[var(--color-surface)] outline-none" />
          </label>
          <label className="grid gap-2 text-sm text-[var(--color-muted)]">
            Phone
            <input value={phone} onChange={(e) => setPhone(e.target.value)} className="rounded-2xl border border-white/10 bg-white/5 px-4 py-3 text-[var(--color-surface)] outline-none" />
          </label>
          <label className="grid gap-2 text-sm text-[var(--color-muted)] sm:col-span-2">
            Email
            <input value={email} onChange={(e) => setEmail(e.target.value)} className="rounded-2xl border border-white/10 bg-white/5 px-4 py-3 text-[var(--color-surface)] outline-none" />
          </label>
          <label className="grid gap-2 text-sm text-[var(--color-muted)] sm:col-span-2">
            Address line 1
            <input value={addressLine1} onChange={(e) => setAddressLine1(e.target.value)} className="rounded-2xl border border-white/10 bg-white/5 px-4 py-3 text-[var(--color-surface)] outline-none" />
          </label>
          <label className="grid gap-2 text-sm text-[var(--color-muted)] sm:col-span-2">
            Address line 2
            <input value={addressLine2} onChange={(e) => setAddressLine2(e.target.value)} className="rounded-2xl border border-white/10 bg-white/5 px-4 py-3 text-[var(--color-surface)] outline-none" />
          </label>
          <label className="grid gap-2 text-sm text-[var(--color-muted)]">
            Landmark
            <input value={landmark} onChange={(e) => setLandmark(e.target.value)} className="rounded-2xl border border-white/10 bg-white/5 px-4 py-3 text-[var(--color-surface)] outline-none" />
          </label>
          <label className="grid gap-2 text-sm text-[var(--color-muted)]">
            Area
            <input value={area} onChange={(e) => setArea(e.target.value)} className="rounded-2xl border border-white/10 bg-white/5 px-4 py-3 text-[var(--color-surface)] outline-none" />
          </label>
          <label className="grid gap-2 text-sm text-[var(--color-muted)]">
            City
            <input value={city} onChange={(e) => setCity(e.target.value)} className="rounded-2xl border border-white/10 bg-white/5 px-4 py-3 text-[var(--color-surface)] outline-none" />
          </label>
          <label className="grid gap-2 text-sm text-[var(--color-muted)]">
            Pincode
            <input value={pincode} onChange={(e) => setPincode(e.target.value)} className="rounded-2xl border border-white/10 bg-white/5 px-4 py-3 text-[var(--color-surface)] outline-none" />
          </label>
        </div>

        <button
          type="button"
          onClick={handlePlaceOrder}
          disabled={isLoading || !variantId}
          className="mt-8 w-full rounded-full bg-[var(--color-accent)] px-6 py-4 text-sm font-bold uppercase tracking-[0.12em] text-[var(--color-ink)] transition hover:brightness-95 disabled:cursor-not-allowed disabled:opacity-70"
        >
          {isLoading ? "Processing..." : "Place one-time order"}
        </button>

        {error ? (
          <div className="mt-5 rounded-2xl border border-red-400/30 bg-red-500/10 p-4 text-sm text-red-100">
            {error}
          </div>
        ) : null}

        {orderResult ? (
          <div className="mt-5 rounded-2xl border border-emerald-300/30 bg-emerald-500/10 p-5 text-sm text-emerald-50">
            <p className="font-semibold">Order placed successfully.</p>
            <p className="mt-2">Order ID: {orderResult.orderId}</p>
            <p>Status: {orderResult.status}</p>
            <p>Zone: {orderResult.zoneLabel ?? "N/A"}</p>
            <p className="mt-2 font-semibold">
              Total payable: Rs. {orderResult.grandTotal.toFixed(2)}
            </p>
          </div>
        ) : null}
      </section>
    </div>
  );
}
