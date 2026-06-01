import Link from "next/link";

import { brand } from "@/data/site";

const navItems = [
  { href: "/", label: "Home" },
  { href: "/shop", label: "Shop" },
  { href: "/subscription", label: "Subscription" },
  { href: "/about", label: "About" },
  { href: "/contact", label: "Contact" },
];

export function SiteHeader() {
  return (
    <header className="sticky top-0 z-50 border-b border-white/10 bg-[rgba(11,31,20,0.88)] backdrop-blur-xl">
      <div className="mx-auto flex w-full max-w-7xl items-center justify-between gap-6 px-5 py-4 sm:px-8">
        <Link href="/" className="flex items-center gap-3">
          <div className="flex h-11 w-11 items-center justify-center rounded-2xl bg-[var(--color-accent)] text-sm font-bold text-[var(--color-ink)]">
            SF
          </div>
          <div>
            <p className="font-[family:var(--font-display)] text-xl tracking-[0.16em] text-[var(--color-surface)] uppercase">
              {brand.name}
            </p>
            <p className="text-xs text-[var(--color-muted)]">{brand.tagline}</p>
          </div>
        </Link>

        <nav className="hidden items-center gap-6 text-sm text-[var(--color-surface)] md:flex">
          {navItems.map((item) => (
            <Link
              key={item.href}
              href={item.href}
              className="transition hover:text-[var(--color-accent)]"
            >
              {item.label}
            </Link>
          ))}
        </nav>

        <a
          href={brand.whatsappHref}
          target="_blank"
          rel="noreferrer"
          className="rounded-full border border-[var(--color-accent)] px-4 py-2 text-sm font-semibold text-[var(--color-accent)] transition hover:bg-[var(--color-accent)] hover:text-[var(--color-ink)]"
        >
          WhatsApp
        </a>
      </div>
    </header>
  );
}
