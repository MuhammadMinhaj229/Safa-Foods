"use client";

import Link from "next/link";
import { ShoppingCart, Star } from "lucide-react";

import { useCart } from "@/lib/context/CartContext";

interface Product {
  id: string;
  name: string;
  sizes: string[];
  price: number | string;
  tag?: string;
  imagePath: string;
  specs?: string;
}

interface ProductCardProps {
  product: Product;
}

export const ProductCard = ({ product }: ProductCardProps) => {
  const { addToCart } = useCart();
  const productSlug = slugify(product.id || product.name);

  return (
    <article className="group">
      <Link href={`/shop/${productSlug}`} className="block">
        <div className="relative overflow-hidden border border-[#ece3d2] bg-white shadow-[0_18px_45px_rgba(40,29,16,0.08)] transition duration-500 group-hover:-translate-y-1 group-hover:shadow-[0_24px_60px_rgba(40,29,16,0.12)]">
          {product.tag ? (
            <div className="absolute left-0 top-6 z-10 bg-[#173d1d] px-5 py-3 text-[10px] font-bold uppercase tracking-[0.32em] text-white shadow-[0_10px_24px_rgba(23,61,29,0.18)]">
              {product.tag}
            </div>
          ) : null}
          <img
            src={product.imagePath}
            alt={product.name}
            className="aspect-[4/4.75] w-full object-cover transition duration-700 group-hover:scale-[1.03]"
          />
        </div>
      </Link>

      <div className="px-2 py-8 text-center">
        <p className="text-[11px] font-bold uppercase tracking-[0.38em] text-[#b49761]">
          {product.sizes[0] ?? product.specs ?? "Fresh Jar"}
        </p>
        <h3 className="mt-4 font-serif text-[2.5rem] italic leading-tight text-[#1f3322]">
          <Link href={`/shop/${productSlug}`}>{product.name}</Link>
        </h3>
        {product.specs ? (
          <p className="mx-auto mt-4 max-w-xs text-base leading-7 text-[#6b6f67]">
            {product.specs}
          </p>
        ) : null}
        <div className="mt-4 flex items-center justify-center gap-1 text-[#b49761]">
          {Array.from({ length: 5 }).map((_, index) => (
            <Star key={index} size={14} fill="currentColor" strokeWidth={1.5} />
          ))}
          <span className="ml-2 text-sm font-semibold text-[#6b6f67]">(89)</span>
        </div>
        <p className="mt-4 text-5xl font-bold tracking-[-0.04em] text-[#17311d]">
          {formatPrice(product.price)}
        </p>
        <div className="mt-6 flex flex-col gap-3 sm:flex-row sm:justify-center">
          <Link
            href={`/shop/${productSlug}`}
            className="inline-flex items-center justify-center border border-[#d7ccb7] px-6 py-4 text-[11px] font-bold uppercase tracking-[0.34em] text-[#1f3322] transition hover:bg-[#faf5ea]"
          >
            View Product
          </Link>
          <button
            onClick={() => addToCart(product)}
            className="inline-flex items-center justify-center gap-3 bg-[#173d1d] px-6 py-4 text-[11px] font-bold uppercase tracking-[0.34em] text-white transition hover:bg-[#214c28]"
          >
            <ShoppingCart size={16} />
            Add to Cart
          </button>
        </div>
      </div>
    </article>
  );
};

function formatPrice(price: number | string) {
  if (typeof price === "number") {
    return `Rs.${price}`;
  }

  return price.replace("Rs. ", "Rs.");
}

function slugify(value: string) {
  return value
    .toLowerCase()
    .trim()
    .replace(/[^a-z0-9]+/g, "-")
    .replace(/^-+|-+$/g, "");
}
