export default function AboutPage() {
  return (
    <main className="mx-auto w-full max-w-5xl px-5 py-16 sm:px-8">
      <div className="space-y-6">
        <p className="section-kicker">About</p>
        <h1 className="section-title">A local food brand with real operating discipline.</h1>
        <p className="max-w-3xl text-lg leading-8 text-[var(--color-olive)]">
          Safa Foods is positioned around freshness, hygiene, and dependable local delivery. The brand story should connect premium homemade-style quality with a clear, repeatable service model for Warangal and Hanakonda households.
        </p>
        <div className="rounded-[2rem] bg-[var(--color-surface)] p-8">
          <p className="leading-8 text-[var(--color-ink)]">
            This storefront is being built as a serious local commerce product, not a brochure site. That means the business model, delivery rules, content workflow, and subscription engine all shape the customer experience.
          </p>
        </div>
      </div>
    </main>
  );
}
