export type Product = {
  slug?: string;
  name: string;
  subtitle: string;
  price: string;
  sizes: string[];
  description: string;
  badges: string[];
  subscription: string;
};

export type HomeCollection = {
  name: string;
  description: string;
  href: string;
};

export type DeliveryZone = {
  label: string;
  distance: string;
  fee: string;
  eta: string;
};

export type SubscriptionPlan = {
  code: string;
  name: string;
  summary: string;
  savings: string;
  billing: string;
  price?: string;
  includes?: string;
  featured?: boolean;
};

export type JournalEntry = {
  title: string;
  date: string;
  excerpt: string;
  href: string;
  image: string;
};

export type PromisePoint = {
  title: string;
  description: string;
};

export const brand = {
  name: "Safa Foods",
  tagline: "Premium Organic",
  description:
    "Premium fresh ginger-garlic paste and homemade essentials delivered across Warangal and Hanakonda with transparent local pricing.",
  whatsappHref: "https://wa.me/919999999999",
};

export const heroHighlights = [
  "100% preservative free",
  "Made fresh daily in Warangal",
  "Hyperlocal delivery support",
];

export const products: Product[] = [
  {
    slug: "premium-ginger-garlic-paste",
    name: "Premium Ginger Garlic Paste",
    subtitle: "Hero product for daily cooking",
    price: "Rs. 99",
    sizes: ["200 g", "500 g"],
    description:
      "Freshly prepared ginger-garlic paste with a premium homemade taste, built for repeat weekly household use.",
    badges: ["Fresh daily", "No hidden charges", "Subscription enabled"],
    subscription: "Weekly and monthly plans",
  },
  {
    slug: "fresh-garlic-paste",
    name: "Fresh Garlic Paste",
    subtitle: "Convenience without compromise",
    price: "Rs. 89",
    sizes: ["200 g", "500 g"],
    description:
      "A sharp, smooth garlic paste for curries, marinades, and everyday kitchen prep.",
    badges: ["Fresh batch prep", "Hygienic packing", "Fast local delivery"],
    subscription: "Weekly and monthly plans",
  },
  {
    slug: "green-chilli-paste",
    name: "Green Chilli Paste",
    subtitle: "Fresh heat for home kitchens",
    price: "Rs. 79",
    sizes: ["150 g", "300 g"],
    description:
      "A bright, spicy paste designed for homes that want fast cooking and consistent flavor.",
    badges: ["Locally delivered", "Small-batch quality", "Offer ready"],
    subscription: "Weekly and monthly plans",
  },
];

export const deliveryZones: DeliveryZone[] = [
  {
    label: "Zone A",
    distance: "0 to 3 km",
    fee: "Rs. 10",
    eta: "30 to 45 mins",
  },
  {
    label: "Zone B",
    distance: "3 to 8 km",
    fee: "Rs. 20",
    eta: "45 to 70 mins",
  },
  {
    label: "Zone C",
    distance: "8+ km serviceable",
    fee: "Rs. 40",
    eta: "70 to 110 mins",
  },
];

export const subscriptionPlans: SubscriptionPlan[] = [
  {
    code: "basic_plan",
    name: "Basic Plan",
    summary: "Perfect for couples or light cooks.",
    savings: "Free local delivery included",
    billing: "Fresh every Monday",
    price: "Rs. 270 / month",
    includes: "250g x 4 jars / month",
  },
  {
    code: "family_plan",
    name: "Family Plan",
    summary: "Our most popular fresh daily choice.",
    savings: "Best value for regular homes",
    billing: "Fresh every Monday",
    price: "Rs. 520 / month",
    includes: "500g x 4 jars / month",
    featured: true,
  },
  {
    code: "chef_plan",
    name: "Chef's Plan",
    summary: "Bulk fresh supply for daily creators.",
    savings: "Ideal for heavy household use",
    billing: "Fresh every Monday",
    price: "Rs. 900 / month",
    includes: "1kg x 4 buckets / month",
  },
];

export const homeCollections: HomeCollection[] = [
  {
    name: "All Pastes",
    description: "Fresh ginger garlic, garlic-only, chilli and future combos.",
    href: "/shop",
  },
  {
    name: "Subscriptions",
    description: "Weekly fresh delivery plans for homes that cook every day.",
    href: "/subscription",
  },
  {
    name: "Kitchen Wisdom",
    description: "Recipes, storage tips, and local cooking inspiration.",
    href: "/mission",
  },
];

export const promisePoints: PromisePoint[] = [
  {
    title: "All Natural",
    description: "Fresh ingredients. No hidden preservatives. No shortcuts.",
  },
  {
    title: "Hygienic",
    description: "Prepared in clean controlled batches for daily household use.",
  },
  {
    title: "Social Impact",
    description: "A local Warangal brand designed to support community-led growth.",
  },
  {
    title: "Daily Fresh",
    description: "Made in small runs and delivered fast across serviceable areas.",
  },
];

export const journalEntries: JournalEntry[] = [
  {
    title: "5 Ways to Use Fresh Paste (Besides Curry)",
    date: "April 05, 2026",
    excerpt:
      "From artisan marinades to flavored butter, discover how fresh ginger garlic paste transforms everyday cooking.",
    href: "/mission",
    image:
      "https://images.unsplash.com/photo-1515003197210-e0cd71810b5f?auto=format&fit=crop&q=80&w=1200",
  },
  {
    title: "Why Preservative-Free Tastes Better",
    date: "March 28, 2026",
    excerpt:
      "The science behind why real ingredients deliver deeper flavour and a cleaner aroma in home kitchens.",
    href: "/mission",
    image:
      "https://images.unsplash.com/photo-1596040033229-a9821ebd058d?auto=format&fit=crop&q=80&w=1200",
  },
  {
    title: "The Perfect Biryani Base",
    date: "March 15, 2026",
    excerpt:
      "An artisan guide to building a rich aromatic base for traditional biryani without relying on packaged shortcuts.",
    href: "/mission",
    image:
      "https://images.unsplash.com/photo-1509358273864-bdbaa9bf939f?auto=format&fit=crop&q=80&w=1200",
  },
  {
    title: "Storing Fresh Ginger Garlic Paste",
    date: "March 02, 2026",
    excerpt:
      "How to maintain peak freshness and potency in your kitchen for maximum shelf life and flavour integrity.",
    href: "/mission",
    image:
      "https://images.unsplash.com/photo-1604329760661-e71c0c144ce1?auto=format&fit=crop&q=80&w=1200",
  },
];

export const trustPoints = [
  {
    title: "Local trust",
    description:
      "Built for Warangal and Hanakonda first, with serviceability and pricing that match real local operations.",
  },
  {
    title: "Operational clarity",
    description:
      "One-time orders, prepaid subscriptions, and delivery slabs are all visible before checkout.",
  },
  {
    title: "Content velocity",
    description:
      "Offers, banners, FAQs, and product stories can be updated regularly without rebuilding the whole site.",
  },
];

export const orderStatuses = [
  "Placed",
  "Confirmed",
  "Preparing",
  "Out for delivery",
  "Delivered",
];

export const channels = [
  { name: "WhatsApp", href: brand.whatsappHref },
  { name: "Instagram", href: "https://instagram.com" },
  { name: "Facebook", href: "https://facebook.com" },
];

export function formatCatalogPrice(value: number) {
  return `Rs. ${value.toFixed(0)}`;
}

export function productSubtitleFromName(name: string) {
  if (/ginger garlic/i.test(name)) {
    return "Hero product for daily cooking";
  }

  if (/garlic/i.test(name) && !/ginger/i.test(name)) {
    return "Convenience without compromise";
  }

  if (/green chilli/i.test(name)) {
    return "Fresh heat for home kitchens";
  }

  return "Fresh kitchen essential";
}
