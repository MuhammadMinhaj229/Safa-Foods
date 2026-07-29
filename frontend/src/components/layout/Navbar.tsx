"use client";

import Link from "next/link";
import {
  Check,
  ChevronDown,
  Clock3,
  MapPin,
  Menu,
  Search,
  ShoppingCart,
  Store,
  User,
  X,
} from "lucide-react";
import { usePathname } from "next/navigation";
import { useEffect, useState } from "react";

import { SafaLogo } from "@/components/shared/SafaLogo";
import { useCart } from "@/lib/context/CartContext";
import { useRole } from "@/lib/context/RoleContext";

const tickerItems = [
  "Origin Certified",
  "100% Preservative Free",
  "Made Fresh Daily",
  "Warangal Local Delivery",
];

const pickupStores = [
  {
    id: "hanamkonda-hub",
    name: "Safa Foods Hanamkonda Hub",
    subtitle: "Usually ready in 2 to 4 hrs",
    fee: "Free",
    address: "Hanamkonda, Warangal",
  },
  {
    id: "warangal-main",
    name: "Safa Foods Warangal Main",
    subtitle: "Usually ready by next morning",
    fee: "Free",
    address: "Warangal Main Road",
  },
];

export const Navbar = () => {
  const { role } = useRole();
  const { count } = useCart();
  const pathname = usePathname();
  const [scrolled, setScrolled] = useState(false);
  const [isMobileMenuOpen, setIsMobileMenuOpen] = useState(false);
  const [isStoreModalOpen, setIsStoreModalOpen] = useState(false);
  const [selectedStoreId, setSelectedStoreId] = useState(pickupStores[0].id);
  const isMissionPage = pathname === "/mission";
  const useLightTheme = isMissionPage && !scrolled;

  useEffect(() => {
    const handleScroll = () => setScrolled(window.scrollY > 50);
    window.addEventListener("scroll", handleScroll, { passive: true });
    return () => window.removeEventListener("scroll", handleScroll);
  }, []);


  useEffect(() => {
    if (!isStoreModalOpen) {
      document.body.style.overflow = "";
      return;
    }

    const previousOverflow = document.body.style.overflow;
    document.body.style.overflow = "hidden";

    const handleEscape = (event: KeyboardEvent) => {
      if (event.key === "Escape") {
        setIsStoreModalOpen(false);
      }
    };

    window.addEventListener("keydown", handleEscape);

    return () => {
      document.body.style.overflow = previousOverflow;
      window.removeEventListener("keydown", handleEscape);
    };
  }, [isStoreModalOpen]);

  if (role === "delivery") {
    return null;
  }

  return (
    <>
      <div className="fixed inset-x-0 top-0 z-[90] overflow-hidden bg-[#163d1d] py-1.5 sm:py-2">
        <div className="header-marquee text-[8px] font-bold uppercase tracking-[0.18em] text-[#efe2bd] sm:text-[10px] sm:tracking-[0.34em]">
          {[...tickerItems, ...tickerItems].map((item, index) => (
            <span key={`${item}-${index}`} className="mx-4 inline-flex items-center sm:mx-5">
              {item}
              <span className="ml-4 text-[#b49761] sm:ml-5">&bull;</span>
            </span>
          ))}
        </div>
      </div>

      <nav
        className={`fixed w-full z-[80] transition-all duration-700 ${
          scrolled
            ? "top-6 border-b border-[#1E331B]/5 bg-white/80 py-3 shadow-[0_10px_40px_rgba(0,0,0,0.05)] backdrop-blur-2xl sm:top-7 sm:py-4"
            : "top-6 bg-transparent py-3 sm:top-7 sm:py-7 lg:py-10"
        }`}
      >
        <div className="mx-auto max-w-7xl px-3 sm:px-6 lg:px-8">
          <div className="grid grid-cols-[auto_1fr_auto] items-center gap-2 lg:flex lg:justify-between">
            <div className="flex items-center lg:hidden">
              <button
                onClick={() => setIsMobileMenuOpen(!isMobileMenuOpen)}
                className={`rounded-full p-2 transition-colors sm:p-2.5 ${
                  useLightTheme ? "text-[#f3ead5] hover:bg-white/10" : "text-[#1E331B] hover:bg-[#1E331B]/5"
                }`}
                aria-label="Toggle menu"
              >
                {isMobileMenuOpen ? (
                  <X size={26} className="sm:h-8 sm:w-8" />
                ) : (
                  <Menu size={26} className="sm:h-8 sm:w-8" />
                )}
              </button>
            </div>

            <Link
              href="/"
              className="group flex min-w-0 items-center justify-center justify-self-center lg:flex-none lg:justify-start"
            >
              <SafaLogo
                showText
                className="h-9 w-9 transition-transform duration-700 group-hover:scale-105 sm:h-12 sm:w-12 lg:h-14 lg:w-14"
                color={useLightTheme ? "#f3ead5" : "#1E331B"}
                textColor={useLightTheme ? "#f3ead5" : "#1E331B"}
                taglineColor={useLightTheme ? "#c8ad71" : "#b49761"}
                imageClassName={
                  useLightTheme
                    ? "brightness-0 invert sepia-[0.22] saturate-[0.9] hue-rotate-[340deg] brightness-[1.05]"
                    : ""
                }
              />
            </Link>

            <div className="hidden flex-1 items-center justify-center pl-8 text-[12px] font-black uppercase tracking-[0.5em] lg:flex">
              <div className="flex items-center gap-10">
                <NavButton href="/" active={pathname === "/"} light={useLightTheme}>
                  Home
                </NavButton>
                <NavButton href="/shop" active={pathname.startsWith("/shop")} light={useLightTheme}>
                  Catalogue
                </NavButton>
                <NavButton href="/mission" active={pathname === "/mission"} light={useLightTheme}>
                  Mission
                </NavButton>
                <button
                  type="button"
                  onClick={() => setIsStoreModalOpen(true)}
                  className="inline-flex items-center gap-2 rounded-full bg-[#f5ecd0] px-4 py-2 text-[11px] font-semibold normal-case tracking-normal text-[#4d4635] shadow-sm transition-all duration-300 hover:bg-[#efe2bd] hover:text-[#1E331B]"
                >
                  <MapPin size={16} className="text-[#7e6c42]" />
                  <span>My store</span>
                  <ChevronDown size={15} className="text-[#7e6c42]" />
                </button>
              </div>
            </div>

            <div className="flex items-center justify-end gap-2 sm:gap-6 lg:flex-none lg:gap-8">
              <Search
                size={24}
                className={`hidden cursor-pointer transition-colors sm:block ${
                  useLightTheme ? "text-[#f3ead5] hover:text-[#c8ad71]" : "text-[#1E331B] hover:text-[#A68A56]"
                }`}
              />
              <Link
                href="/auth/login"
                className={`hidden transition-colors sm:block ${
                  useLightTheme ? "text-[#f3ead5] hover:text-[#c8ad71]" : "text-[#1E331B] hover:text-[#A68A56]"
                }`}
                aria-label="Login"
              >
                <User size={22} />
              </Link>
              <Link href="/cart" className="group relative cursor-pointer" aria-label="Cart">
                <div className="rounded-full bg-[#1E331B] p-2.5 text-white shadow-[0_10px_24px_rgba(30,51,27,0.18)] transition-all duration-300 group-hover:rotate-12 group-hover:bg-[#A68A56] sm:p-3">
                  <ShoppingCart size={18} className="sm:h-[22px] sm:w-[22px]" />
                </div>
                <span className="absolute -right-1.5 -top-1.5 flex h-5 w-5 items-center justify-center rounded-full border-2 border-[#F4F1EA] bg-gradient-to-tr from-[#A68A56] to-[#D4C395] text-[10px] font-black text-[#1E331B] shadow-lg sm:-right-2 sm:-top-2 sm:h-6 sm:w-6 sm:text-[11px]">
                  {count}
                </span>
              </Link>
            </div>
          </div>
        </div>

        {isMobileMenuOpen ? (
          <div className="mt-3 px-3 sm:mt-4 sm:px-6 lg:hidden">
            <div className="mx-auto max-w-7xl bg-white/95 p-7 shadow-2xl backdrop-blur-3xl animate-fade-up sm:p-10">
              <div className="flex flex-col gap-6 text-center text-[12px] font-black uppercase tracking-[0.34em] sm:gap-8 sm:text-[14px] sm:tracking-[0.5em]">
                <Link href="/" onClick={() => setIsMobileMenuOpen(false)}>
                  Home
                </Link>
                <Link href="/shop" onClick={() => setIsMobileMenuOpen(false)}>
                  Catalog
                </Link>
                <Link href="/mission" onClick={() => setIsMobileMenuOpen(false)}>
                  Mission
                </Link>
                <button
                  type="button"
                  onClick={() => {
                    setIsMobileMenuOpen(false);
                    setIsStoreModalOpen(true);
                  }}
                  className="inline-flex items-center justify-center gap-2"
                >
                  <MapPin size={16} className="text-[#A68A56]" />
                  <span>My store</span>
                  <ChevronDown size={16} />
                </button>
                <hr className="border-[#1E331B]/5" />
                <Link href="/auth/login" onClick={() => setIsMobileMenuOpen(false)}>
                  Login
                </Link>
                <Link href="/profile" onClick={() => setIsMobileMenuOpen(false)}>
                  My Account
                </Link>
              </div>
            </div>
          </div>
        ) : null}
      </nav>

      {isStoreModalOpen ? (
        <div className="fixed inset-0 z-[120] flex items-start justify-center overflow-y-auto bg-[#102312]/55 px-4 py-24 backdrop-blur-md sm:px-6">
          <div
            className="absolute inset-0"
            onClick={() => setIsStoreModalOpen(false)}
            aria-hidden="true"
          />
          <div className="relative z-10 w-full max-w-6xl overflow-hidden rounded-[2rem] border border-[#efe2bd]/60 bg-[#f6f0e5] shadow-[0_40px_120px_rgba(14,24,16,0.25)]">
            <div className="flex items-center justify-between border-b border-[#1E331B]/8 px-6 py-5 sm:px-8">
              <div>
                <p className="text-[11px] font-black uppercase tracking-[0.38em] text-[#b49761]">Store Pickup</p>
                <h2 className="mt-2 text-3xl font-semibold tracking-tight text-[#1E331B] sm:text-5xl">
                  Select Pickup Location
                </h2>
              </div>
              <button
                type="button"
                onClick={() => setIsStoreModalOpen(false)}
                className="flex h-14 w-14 items-center justify-center rounded-full bg-[#eadfcb] text-[#4d4635] transition-colors hover:bg-[#e1d3bb]"
                aria-label="Close pickup location modal"
              >
                <X size={28} />
              </button>
            </div>

            <div className="grid gap-0 lg:grid-cols-[1.45fr_1fr]">
              <div className="relative min-h-[320px] bg-[radial-gradient(circle_at_top,#f8f4ea_0%,#ece5d7_55%,#e5dccb_100%)] p-6 sm:p-8">
                <div className="relative flex h-full min-h-[320px] flex-col justify-between overflow-hidden rounded-[1.5rem] border border-[#1E331B]/8 bg-[#e4dfd5] p-8 shadow-inner">
                  <div className="absolute inset-0 opacity-[0.05] [background-image:linear-gradient(#1E331B_1px,transparent_1px),linear-gradient(90deg,#1E331B_1px,transparent_1px)] [background-size:32px_32px]" />
                  <div className="relative flex h-full flex-col items-center justify-center text-center">
                    <div className="mb-8 flex h-28 w-28 items-center justify-center rounded-full border border-[#1E331B]/10 bg-white/35 text-[#7d7567]">
                      <MapPin size={48} />
                    </div>
                    <h3 className="text-2xl font-semibold text-[#1E331B]">Pickup Available Near You</h3>
                    <p className="mt-4 max-w-md text-base leading-8 text-[#6f6a60]">
                      Choose your preferred Safa Foods pickup point. We will keep your order ready and notify you on WhatsApp once it is packed.
                    </p>
                    <div className="mt-8 inline-flex items-center gap-3 rounded-full bg-white/60 px-5 py-3 text-sm font-semibold text-[#4d4635]">
                      <Store size={18} className="text-[#a48449]" />
                      {pickupStores.find((store) => store.id === selectedStoreId)?.address}
                    </div>
                  </div>
                </div>
              </div>

              <div className="flex flex-col gap-5 bg-[#f8f3ea] p-6 sm:p-8">
                <div className="space-y-4">
                  {pickupStores.map((store) => {
                    const selected = store.id === selectedStoreId;

                    return (
                      <button
                        key={store.id}
                        type="button"
                        onClick={() => setSelectedStoreId(store.id)}
                        className={`w-full rounded-[1.5rem] border p-5 text-left transition-all duration-300 ${
                          selected
                            ? "border-[#d8c18a] bg-[#fbf4dd] shadow-[0_15px_35px_rgba(180,151,97,0.12)]"
                            : "border-[#1E331B]/8 bg-white/65 hover:border-[#d8c18a] hover:bg-[#fcf7eb]"
                        }`}
                      >
                        <div className="flex items-start justify-between gap-4">
                          <div className="flex items-start gap-4">
                            <div
                              className={`mt-1 flex h-7 w-7 items-center justify-center rounded-md border ${
                                selected ? "border-[#b49761] bg-[#1E331B] text-[#f6f0e5]" : "border-[#d8ccb6] bg-white text-transparent"
                              }`}
                            >
                              <Check size={18} />
                            </div>
                            <div>
                              <p className="text-2xl font-semibold tracking-tight text-[#1E331B]">{store.name}</p>
                              <div className="mt-3 flex items-center gap-2 text-[15px] text-[#5f584e]">
                                <Clock3 size={16} className="text-[#a48449]" />
                                <span>{store.subtitle}</span>
                              </div>
                            </div>
                          </div>
                          <div className="text-right">
                            <div className="text-lg font-semibold text-[#1E331B]">{store.fee}</div>
                            <ChevronDown size={20} className="ml-auto mt-3 text-[#7f755f]" />
                          </div>
                        </div>
                      </button>
                    );
                  })}
                </div>

                <button
                  type="button"
                  onClick={() => setIsStoreModalOpen(false)}
                  className="mt-auto rounded-2xl bg-[#1E331B] px-6 py-5 text-center text-sm font-black uppercase tracking-[0.28em] text-[#f6f0e5] transition-all duration-300 hover:bg-[#284924]"
                >
                  Select Pick Up Store
                </button>
              </div>
            </div>
          </div>
        </div>
      ) : null}
    </>
  );
};

function NavButton({
  href,
  active,
  light = false,
  children,
}: {
  href: string;
  active: boolean;
  light?: boolean;
  children: React.ReactNode;
}) {
  return (
    <Link
      href={href}
      className={`relative pb-2 transition-all duration-300 after:absolute after:-bottom-1 after:left-0 after:h-0.5 after:w-full after:bg-[#A68A56] after:transition-transform after:duration-300 ${
        active
          ? `${light ? "text-[#f3ead5]" : "text-[#1E331B]"} after:scale-x-100`
          : `${light ? "text-[#e3d7b6]/80 hover:text-[#f3ead5]" : "text-[#6B705C] hover:text-[#1E331B]"} after:scale-x-0 hover:after:scale-x-100`
      }`}
    >
      {children}
    </Link>
  );
}
