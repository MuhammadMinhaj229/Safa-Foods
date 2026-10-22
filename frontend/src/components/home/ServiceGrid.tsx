import Link from "next/link";
import { Leaf, Package, Plane, UtensilsCrossed, Settings, Users } from "lucide-react";

const services = [
  {
    id: "fresh",
    title: "Fresh",
    description: "Weekly household shopping. Vegetables, fruits, dairy & essentials.",
    icon: Leaf,
    href: "/services/fresh",
    color: "bg-[#e8f3eb]",
    iconColor: "text-[#2d5a3c]"
  },
  {
    id: "foods",
    title: "Foods",
    description: "Homemade premium products, signature ginger garlic paste.",
    icon: UtensilsCrossed,
    href: "/services/foods",
    color: "bg-[#fdf4e6]",
    iconColor: "text-[#c29845]"
  },
  {
    id: "travel",
    title: "Travel",
    description: "NRI food packs, cab booking, and household travel assistance.",
    icon: Plane,
    href: "/services/travel",
    color: "bg-[#eef2f9]",
    iconColor: "text-[#4a6b9c]"
  },
  {
    id: "send",
    title: "Send",
    description: "Parcel management, gift packing, and local courier booking.",
    icon: Package,
    href: "/services/send",
    color: "bg-[#f8ede9]",
    iconColor: "text-[#b26046]"
  },
  {
    id: "services",
    title: "Services",
    description: "Home maintenance: plumbing, cleaning, AC service, and repairs.",
    icon: Settings,
    href: "/services/home-services",
    color: "bg-[#ececed]",
    iconColor: "text-[#54575b]"
  },
  {
    id: "community",
    title: "Community",
    description: "Support local producers, farmers, and women entrepreneurs.",
    icon: Users,
    href: "/services/community",
    color: "bg-[#f4efe8]",
    iconColor: "text-[#8b7355]"
  }
];

export const ServiceGrid = () => {
  return (
    <section className="px-4 py-16 sm:px-6 lg:px-8 bg-[#faf6ee]">
      <div className="mx-auto max-w-7xl">
        <div className="text-center max-w-3xl mx-auto mb-16">
          <p className="text-[11px] font-bold uppercase tracking-[0.3em] text-[#a48449] mb-4">
            Household Operating System
          </p>
          <h2 className="font-serif text-[2.5rem] italic leading-tight text-[#1f3322] sm:text-[3.5rem]">
            Everything your home needs
          </h2>
          <p className="mt-4 text-base text-[#6d7266]">
            Select a service below to get started. One trusted platform for all your daily requirements.
          </p>
        </div>

        <div className="grid gap-6 sm:grid-cols-2 lg:grid-cols-3">
          {services.map((service) => {
            const Icon = service.icon;
            return (
              <Link
                key={service.id}
                href={service.href}
                className="group relative overflow-hidden rounded-[2rem] border border-[#e8dfc8] bg-white p-8 transition-all hover:-translate-y-1 hover:shadow-[0_20px_40px_rgba(43,30,16,0.08)]"
              >
                <div className={`mb-6 flex h-16 w-16 items-center justify-center rounded-2xl ${service.color} transition-transform group-hover:scale-110`}>
                  <Icon className={service.iconColor} size={28} strokeWidth={1.5} />
                </div>
                <h3 className="font-serif text-2xl italic text-[#1f3322] mb-3 group-hover:text-[#b49761] transition-colors">
                  {service.title}
                </h3>
                <p className="text-sm leading-relaxed text-[#6d7266]">
                  {service.description}
                </p>
                <div className="absolute bottom-8 right-8 opacity-0 transition-all group-hover:opacity-100 group-hover:translate-x-1">
                  <span className="text-xs font-bold uppercase tracking-wider text-[#b49761]">Explore →</span>
                </div>
              </Link>
            );
          })}
        </div>
      </div>
    </section>
  );
};
