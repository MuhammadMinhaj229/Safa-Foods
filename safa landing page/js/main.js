// Main logic for Safa landing page
document.addEventListener("DOMContentLoaded", () => {
  // Initialize Feather icons
  if (typeof feather !== 'undefined') {
    feather.replace();
  }

  // Navbar background change on scroll
  const header = document.querySelector('header');
  if (header) {
    window.addEventListener('scroll', () => {
      if (window.scrollY > 20) {
        header.classList.add('shadow-sm');
        header.classList.replace('bg-white/80', 'bg-white');
      } else {
        header.classList.remove('shadow-sm');
        header.classList.replace('bg-white', 'bg-white/80');
      }
    });
  }
});
