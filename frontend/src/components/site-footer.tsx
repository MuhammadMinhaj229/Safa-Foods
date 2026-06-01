import Link from "next/link";

import { brand, channels, deliveryZones } from "@/data/site";

export function SiteFooter() {
  return (
    <footer className="border-t border-white/10 bg-[var(--color-forest-deep)]">
      <div className="mx-auto grid w-full max-w-7xl gap-10 px-5 py-14 text-sm text-[var(--color-muted)] sm:px-8 lg:grid-cols-[1.2fr_0.8fr_1fr]">
        <div className="space-y-4">
          <p className="font-[family:var(--font-display)] text-2xl uppercase tracking-[0.14em] text-[var(--color-surface)]">
            {brand.name}
          </p>
          <p className="max-w-md leading-7">{brand.description}</p>
          <p className="text-[var(--color-surface)]">
            Near Narsampet Road, beside Decent Function Hall, serving Warangal and Hanakonda.
          </p>
        </div>

        <div className="space-y-3">
          <p className="text-base font-semibold text-[var(--color-surface)]">
            Quick links
          </p>
          <ul className="space-y-2">
            <li>
              <Link href="/shop" className="transition hover:text-[var(--color-accent)]">
                Shop
              </Link>
            </li>
            <li>
              <Link
                href="/subscription"
                className="transition hover:text-[var(--color-accent)]"
              >
                Subscription
              </Link>
            </li>
            <li>
              <Link href="/about" className="transition hover:text-[var(--color-accent)]">
                About
              </Link>
            </li>
            <li>
              <Link href="/contact" className="transition hover:text-[var(--color-accent)]">
                Contact
              </Link>
            </li>
          </ul>
        </div>

        <div className="space-y-4">
          <p className="text-base font-semibold text-[var(--color-surface)]">
            Delivery slabs
          </p>
          <ul className="space-y-3">
            {deliveryZones.map((zone) => (
              <li key={zone.label} className="rounded-2xl bg-white/5 p-4">
                <p className="font-semibold text-[var(--color-surface)]">{zone.label}</p>
                <p>{zone.distance}</p>
                <p>
                  {zone.fee} · {zone.eta}
                </p>
              </li>
            ))}
          </ul>
          <div className="flex gap-4 pt-2">
            {channels.map((channel) => (
              <a
                key={channel.name}
                href={channel.href}
                target="_blank"
                rel="noreferrer"
                className="transition hover:text-[var(--color-accent)]"
              >
                {channel.name}
              </a>
            ))}
          </div>
        </div>
      </div>
    </footer>
  );
}
