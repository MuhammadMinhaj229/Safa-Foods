import { channels } from "@/data/site";

export default function ContactPage() {
  return (
    <main className="mx-auto w-full max-w-6xl px-5 py-16 sm:px-8">
      <div className="grid gap-8 lg:grid-cols-[0.9fr_1.1fr]">
        <div className="space-y-5">
          <p className="section-kicker">Contact</p>
          <h1 className="section-title">WhatsApp should be the fastest path to trust.</h1>
          <p className="text-lg leading-8 text-[var(--color-olive)]">
            Customer communication should start with WhatsApp, then expand to Instagram and Facebook campaigns for demand generation.
          </p>
        </div>

        <div className="rounded-[2rem] bg-[var(--color-surface)] p-8 shadow-[0_24px_70px_rgba(16,43,27,0.08)]">
          <p className="text-sm uppercase tracking-[0.16em] text-[var(--color-leaf)]">
            Store location
          </p>
          <p className="mt-4 text-2xl font-bold text-[var(--color-ink)]">
            Near Narsampet Road, beside Decent Function Hall
          </p>
          <p className="mt-3 leading-7 text-[var(--color-olive)]">
            Service coverage is centered on Warangal and Hanakonda with slab-based local delivery fees.
          </p>

          <div className="mt-8 flex flex-wrap gap-3">
            {channels.map((channel) => (
              <a
                key={channel.name}
                href={channel.href}
                target="_blank"
                rel="noreferrer"
                className="rounded-full border border-[var(--color-leaf)] px-5 py-3 text-sm font-semibold text-[var(--color-leaf)] transition hover:bg-[var(--color-leaf)] hover:text-[var(--color-surface)]"
              >
                {channel.name}
              </a>
            ))}
          </div>
        </div>
      </div>
    </main>
  );
}
