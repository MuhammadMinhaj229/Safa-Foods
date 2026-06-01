export type DeliveryQuote = {
  serviceable: boolean;
  zoneLabel: "Zone A" | "Zone B" | "Zone C" | null;
  fee: number | null;
  etaMinutes: [number, number] | null;
  distanceKm: number;
};

type DeliverySlab = {
  label: DeliveryQuote["zoneLabel"];
  minKm: number;
  maxKm: number | null;
  fee: number;
  etaMinutes: [number, number];
};

export const DELIVERY_SLABS: DeliverySlab[] = [
  { label: "Zone A", minKm: 0, maxKm: 3, fee: 10, etaMinutes: [30, 45] },
  { label: "Zone B", minKm: 3, maxKm: 8, fee: 20, etaMinutes: [45, 70] },
  { label: "Zone C", minKm: 8, maxKm: 12, fee: 40, etaMinutes: [70, 110] },
];

export function getDeliveryQuote(distanceKm: number): DeliveryQuote {
  const roundedDistance = Number(distanceKm.toFixed(2));
  const slab = DELIVERY_SLABS.find((candidate) => {
    const withinMin = roundedDistance >= candidate.minKm;
    const withinMax =
      candidate.maxKm === null ? true : roundedDistance <= candidate.maxKm;

    return withinMin && withinMax;
  });

  if (!slab) {
    return {
      serviceable: false,
      zoneLabel: null,
      fee: null,
      etaMinutes: null,
      distanceKm: roundedDistance,
    };
  }

  return {
    serviceable: true,
    zoneLabel: slab.label,
    fee: slab.fee,
    etaMinutes: slab.etaMinutes,
    distanceKm: roundedDistance,
  };
}
