import { getCatalogProducts } from "@/lib/backend-api";
import {
  formatCatalogPrice,
  productSubtitleFromName,
  products as fallbackProducts,
  type Product,
} from "@/data/site";

export async function getDisplayProducts(): Promise<Product[]> {
  try {
    const backendProducts = await getCatalogProducts();

    return backendProducts.map((product) => {
      const sortedVariants = [...product.variants].sort(
        (left, right) => (left.salePrice ?? left.price) - (right.salePrice ?? right.price),
      );
      const leadVariant = sortedVariants[0];

      return {
        slug: product.slug,
        name: product.name,
        subtitle: productSubtitleFromName(product.name),
        price: formatCatalogPrice(leadVariant?.salePrice ?? leadVariant?.price ?? 0),
        sizes: sortedVariants.map((variant) => variant.weight),
        description:
          fallbackProducts.find((item) => item.slug === product.slug)?.description ??
          "Freshly prepared local product designed for repeat household use.",
        badges: [
          "Local delivery",
          product.isSubscriptionEnabled ? "Subscription enabled" : "One-time order",
          product.category,
        ],
        subscription: product.isSubscriptionEnabled
          ? "Weekly and monthly plans"
          : "One-time purchase only",
      };
    });
  } catch {
    return fallbackProducts;
  }
}
