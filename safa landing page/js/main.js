(function () {
  const phoneNumber = '919999999999';
  const defaultMessage = 'Hi SAFA, I want to order fresh essentials.';

  function whatsappUrl(message) {
    return `https://wa.me/${phoneNumber}?text=${encodeURIComponent(message || defaultMessage)}`;
  }

  document.querySelectorAll('[data-whatsapp-link]').forEach((link) => {
    link.setAttribute('href', whatsappUrl(defaultMessage));
    link.addEventListener('click', () => {
      window.dispatchEvent(new CustomEvent('safa:whatsapp-click'));
    });
  });

  const navToggle = document.querySelector('[data-nav-toggle]');
  const nav = document.querySelector('[data-nav]');

  function closeNav() {
    if (!nav || !navToggle) return;
    nav.classList.remove('open');
    navToggle.setAttribute('aria-expanded', 'false');
  }

  if (navToggle && nav) {
    navToggle.addEventListener('click', () => {
      const isOpen = nav.classList.toggle('open');
      navToggle.setAttribute('aria-expanded', String(isOpen));
    });

    nav.addEventListener('click', (event) => {
      if (event.target.matches('a')) closeNav();
    });

    document.addEventListener('click', (event) => {
      if (!nav.contains(event.target) && !navToggle.contains(event.target)) closeNav();
    });

    document.addEventListener('keydown', (event) => {
      if (event.key === 'Escape') closeNav();
    });
  }

  const form = document.querySelector('[data-order-form]');
  if (form) {
    form.addEventListener('submit', (event) => {
      event.preventDefault();
      const data = new FormData(form);
      const name = String(data.get('name') || '').trim();
      const phone = String(data.get('phone') || '').trim();
      const area = String(data.get('area') || '').trim();
      const items = String(data.get('items') || '').trim();

      const message = [
        'Hi SAFA, I want to place a quick order.',
        `Name: ${name || 'Not provided'}`,
        `Phone: ${phone || 'Not provided'}`,
        `Area: ${area || 'Not provided'}`,
        `Items: ${items || 'Please help me with today fresh essentials'}`,
      ].join('\n');

      window.location.href = whatsappUrl(message);
    });
  }

  const sections = Array.from(document.querySelectorAll('main section[id]'));
  const navLinks = Array.from(document.querySelectorAll('.primary-nav a'));

  if ('IntersectionObserver' in window) {
    const observer = new IntersectionObserver(
      (entries) => {
        entries.forEach((entry) => {
          if (!entry.isIntersecting) return;
          navLinks.forEach((link) => {
            link.classList.toggle('active', link.getAttribute('href') === `#${entry.target.id}`);
          });
        });
      },
      { rootMargin: '-36% 0px -56% 0px', threshold: 0.01 },
    );

    sections.forEach((section) => observer.observe(section));
  }
})();
