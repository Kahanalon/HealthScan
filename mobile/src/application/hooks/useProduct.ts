import { useQuery } from '@tanstack/react-query';
import { ScoreResult } from '../../core/entities/ScoreResult';
import { useDI } from '../../di/container';
import { useAppStore } from '../../stores/useAppStore';

export function useProduct(barcode: string | null) {
  const { productService } = useDI();
  const locale = useAppStore((state) => state.locale);

  return useQuery<ScoreResult | null, Error>({
    queryKey: ['product', barcode, locale],
    queryFn: async () => {
      if (!barcode) {
        return null;
      }
      return productService.getProduct(barcode, locale);
    },
    enabled: !!barcode,
    staleTime: 5 * 60 * 1000,
    gcTime: 30 * 60 * 1000,
    retry: 2,
    retryDelay: (attemptIndex) => Math.min(1000 * 2 ** attemptIndex, 10000),
  });
}

export function useProductPrefetch() {
  const { productService } = useDI();
  const locale = useAppStore((state) => state.locale);

  return async (barcode: string) => {
    return productService.getProduct(barcode, locale);
  };
}
