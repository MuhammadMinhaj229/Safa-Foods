import React from 'react';
import Image from "next/image";

interface SafaLogoProps {
  className?: string;
  color?: string;
  showText?: boolean;
  textColor?: string;
  taglineColor?: string;
  imageClassName?: string;
}

export const SafaLogo = ({
  className = "w-10 h-10",
  showText = false,
  textColor = "#1E331B",
  taglineColor = "#b49761",
  imageClassName = "",
}: SafaLogoProps) => (
  <div className="group flex cursor-pointer items-center gap-2 sm:gap-5">
    <div className={`${className} relative transition-transform duration-700 group-hover:scale-105`}>
      <Image
        src="/logo.jpg"
        alt="Safa Foods logo"
        fill
        sizes="(max-width: 640px) 40px, 56px"
        className={`object-contain ${imageClassName}`.trim()}
        priority={showText}
      />
    </div>
    {showText && (
      <div className="flex min-w-0 flex-col">
        <span
          className="whitespace-nowrap text-[0.76rem] font-black uppercase leading-none tracking-[0.14em] min-[380px]:text-[0.84rem] min-[380px]:tracking-[0.18em] sm:text-[1.2rem] sm:tracking-[0.34em] lg:text-[1.55rem]"
          style={{ color: textColor }}
        >
          SAFA FOODS
        </span>
        <span className="mt-1 hidden pl-0.5 text-[8px] font-bold uppercase tracking-[0.28em] min-[380px]:block sm:mt-2 sm:pl-1 sm:text-[9px] sm:tracking-[0.45em]" style={{ color: taglineColor }}>
          Premium Organic
        </span>
      </div>
    )}
  </div>
);
