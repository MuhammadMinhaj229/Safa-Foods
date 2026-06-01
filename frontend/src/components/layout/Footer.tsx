"use client";

import Link from "next/link";
import { Globe, Play, Share2 } from "lucide-react";

import { SafaLogo } from "@/components/shared/SafaLogo";
import { channels } from "@/data/site";
import { useRole } from "@/lib/context/RoleContext";

export const Footer = () => {
  const { role } = useRole();

  if (role === "delivery") {
    return null;
  }

  return (
    <footer className="mt-16 border-t border-[#e6dfcf] bg-[#f1ece1] px-4 pt-16 pb-10 sm:px-6 lg:px-8">
      <div className="mx-auto max-w-7xl">
        <div className="grid gap-14 lg:grid-cols-4">
          <div className="space-y-6">
            <div className="flex items-center gap-4">
              <SafaLogo className="h-12 w-12" />
              <div className="text-[2rem] font-black uppercase tracking-[0.24em] text-[#1f3322]">
                Safa Foods
              </div>
            </div>
            <p className="max-w-sm text-lg leading-9 text-[#666d63]">
              Artisan condiments crafted fresh every morning in Warangal. Pure ingredients,
              zero preservatives, social mission.
            </p>
            <div className="flex items-center gap-5 text-[#1f3322]">
              <Link href={channels[1].href} aria-label="Instagram" className="footer-social">
                <Globe size={20} />
              </Link>
              <Link href={channels[2].href} aria-label="Facebook" className="footer-social">
                <Share2 size={20} />
              </Link>
              <Link href="#" aria-label="YouTube" className="footer-social">
                <Play size={20} />
              </Link>
            </div>
          </div>

          <FooterColumn
            title="Explore"
            links={[
              { href: "/shop", label: "Shop Pastes" },
              { href: "/subscription", label: "Subscriptions" },
              { href: "/shop", label: "Gifting Combos" },
              { href: "/contact", label: "Bulk Orders" },
            ]}
          />

          <FooterColumn
            title="Brand"
            links={[
              { href: "/mission", label: "Our Story" },
              { href: "/mission", label: "Impact Report" },
              { href: "/mission", label: "Recipes" },
              { href: "/contact", label: "FAQ" },
            ]}
          />

          <div className="space-y-8">
            <p className="text-[11px] font-bold uppercase tracking-[0.45em] text-[#1f3322]">
              Join Us
            </p>
            <p className="text-lg leading-9 text-[#666d63]">
              Subscribe for artisan cooking tips and restock notifications.
            </p>
            <form className="space-y-4">
              <input
                type="email"
                placeholder="Email Address"
                className="w-full border-b border-[#cfc6b3] bg-transparent px-0 py-4 text-base text-[#1f3322] outline-none placeholder:text-[#b5b0a6]"
              />
              <button
                type="button"
                className="text-[11px] font-bold uppercase tracking-[0.36em] text-[#1f3322]"
              >
                Join
              </button>
            </form>
          </div>
        </div>

        <div className="mt-16 border-t border-[#e4dccb] pt-8 text-[11px] font-bold uppercase tracking-[0.32em] text-[#6c7067]">
          <div className="flex flex-col gap-6 md:flex-row md:items-center md:justify-between">
            <p>© 2026 Safa Foods. Made with heart in Warangal.</p>
            <div className="flex flex-wrap items-center gap-8">
              <Link href="/contact">Privacy</Link>
              <Link href="/contact">Terms</Link>
              <Link href="/contact">Shipping</Link>
            </div>
          </div>
        </div>
      </div>
    </footer>
  );
};

function FooterColumn({
  title,
  links,
}: {
  title: string;
  links: { href: string; label: string }[];
}) {
  return (
    <div className="space-y-8">
      <p className="text-[11px] font-bold uppercase tracking-[0.45em] text-[#1f3322]">{title}</p>
      <div className="space-y-5">
        {links.map((link) => (
          <Link
            key={link.label}
            href={link.href}
            className="block text-[14px] font-semibold uppercase tracking-[0.28em] text-[#555f55] transition hover:text-[#b49761]"
          >
            {link.label}
          </Link>
        ))}
      </div>
    </div>
  );
}
