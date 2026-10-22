import type { NextConfig } from "next";

const nextConfig: NextConfig = {
  /* config options here */
  async redirects() {
    return [
      {
        source: '/shop',
        destination: '/services/foods',
        permanent: true,
      },
    ]
  },
};

export default nextConfig;
