import { notFound } from "next/navigation";

import { OrderForm } from "@/components/order-form";
import { getCatalogProductBySlug } from "@/lib/backend-api";
import { getDisplayProducts } from "@/lib/catalog";

type ProductPageProps = {
  params: Promise<{
    slug: string;
  }>;
};

export default async function ProductPage({ params }: ProductPageProps) {
  const { slug } = await params;
  const [backendProduct, displayProducts] = await Promise.all([
    getCatalogProductBySlug(slug).catch(() => null),
    getDisplayProducts(),
  ]);

  const displayProduct = displayProducts.find((product) => product.slug === slug);

  if (!backendProduct && !displayProduct) {
    notFound();
  }

  return (
    <main className="mx-auto w-full max-w-7xl px-5 py-16 sm:px-8">
      <div className="grid gap-10 lg:grid-cols-[0.88fr_1.12fr]">
        <section className="space-y-6">
          <p className="section-kicker">Product detail</p>
          <h1 className="section-title">
            {backendProduct?.name ?? displayProduct?.name ?? "Product"}
          </h1>
          <p className="max-w-2xl text-lg leading-8 text-[var(--color-olive)]">
            {displayProduct?.description ??
              "Freshly prepared local product built for repeat household demand."}
          </p>

          <div className="grid gap-4 sm:grid-cols-2">
            <div className="rounded-[2rem] bg-[var(--color-surface)] p-6">
              <p className="text-sm uppercase tracking-[0.16em] text-[var(--color-leaf)]">
                Category
              </p>
              <p className="mt-3 text-2xl font-bold text-[var(--color-ink)]">
                {backendProduct?.category ?? "Fresh Pastes"}
              </p>
            </div>
            <div className="rounded-[2rem] bg-[var(--color-surface)] p-6">
              <p className="text-sm uppercase tracking-[0.16em] text-[var(--color-leaf)]">
                Subscription
              </p>
              <p className="mt-3 text-2xl font-bold text-[var(--color-ink)]">
                {backendProduct
                  ? backendProduct.isSubscriptionEnabled
                    ? "Enabled"
                    : "Unavailable"
                  : displayProduct?.subscription ?? "Unavailable"}
              </p>
            </div>
          </div>

          <div className="rounded-[2rem] bg-[var(--color-sand)] p-6">
            <p className="text-sm uppercase tracking-[0.16em] text-[var(--color-leaf)]">
              Available sizes
            </p>
            <div className="mt-4 flex flex-wrap gap-3">
              {backendProduct
                ? backendProduct.variants.map((variant) => (
                    <span
                      key={variant.variantId}
                      className="rounded-full border border-[var(--color-sand-deep)] px-4 py-2 text-sm font-semibold text-[var(--color-ink)]"
                    >
                      {variant.weight} · Rs. {(variant.salePrice ?? variant.price).toFixed(0)}
                    </span>
                  ))
                : (displayProduct?.sizes ?? []).map((size) => (
                    <span
                      key={size}
                      className="rounded-full border border-[var(--color-sand-deep)] px-4 py-2 text-sm font-semibold text-[var(--color-ink)]"
                    >
                      {size} · {displayProduct?.price}
                    </span>
                  ))}
            </div>
          </div>

          {!backendProduct ? (
            <div className="rounded-[2rem] border border-amber-300/50 bg-amber-50 p-6 text-[var(--color-ink)]">
              <p className="font-semibold">Backend catalog is currently unavailable.</p>
              <p className="mt-2 leading-7 text-[var(--color-olive)]">
                The interface is visible, but the database-backed order form is disabled until the PostgreSQL connection is configured correctly.
              </p>
            </div>
          ) : null}
        </section>

        {backendProduct ? (
          <OrderForm product={backendProduct} />
        ) : (
          <section className="rounded-[2rem] bg-[var(--color-forest-deep)] p-7 text-[var(--color-surface)]">
            <p className="text-sm uppercase tracking-[0.16em] text-[var(--color-accent)]">
              Order flow
            </p>
            <h2 className="mt-4 font-[family:var(--font-display)] text-4xl">
              UI ready, backend checkout blocked by database auth.
            </h2>
            <p className="mt-5 leading-8 text-[var(--color-muted)]">
              Once the PostgreSQL credentials are corrected, this panel will switch to the live delivery quote and one-time order form automatically.
            </p>
          </section>
        )}
      </div>
    </main>
  );
}
