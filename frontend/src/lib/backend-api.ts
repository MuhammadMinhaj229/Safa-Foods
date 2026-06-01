export type BackendCatalogVariant = {
  variantId: string;
  label: string;
  weight: string;
  price: number;
  salePrice: number | null;
  sku: string;
};

export type BackendCatalogProduct = {
  productId: string;
  slug: string;
  name: string;
  category: string;
  isSubscriptionEnabled: boolean;
  variants: BackendCatalogVariant[];
};

export function getBackendApiBaseUrl() {
  return process.env.BACKEND_API_BASE_URL?.replace(/\/+$/, "");
}

export async function getCatalogProducts(): Promise<BackendCatalogProduct[]> {
  const baseUrl = getBackendApiBaseUrl();
  if (!baseUrl) {
    throw new Error("BACKEND_API_BASE_URL is not configured.");
  }

  const response = await fetch(`${baseUrl}/api/catalog/products`, {
    method: "GET",
    next: { revalidate: 60 },
    headers: {
      Accept: "application/json",
    },
  });

  if (!response.ok) {
    throw new Error(`Catalog request failed with status ${response.status}.`);
  }

  return (await response.json()) as BackendCatalogProduct[];
}

export async function getCatalogProductBySlug(
  slug: string,
): Promise<BackendCatalogProduct | null> {
  const products = await getCatalogProducts();
  return products.find((product) => product.slug === slug) ?? null;
}
