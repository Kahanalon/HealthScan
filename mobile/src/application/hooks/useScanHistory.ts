import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { ScanHistory } from '../../core/entities/ScanHistory';
import { useDI } from '../../di/container';

export function useScanHistory(limit: number = 20) {
  const { productService } = useDI();

  return useQuery<ScanHistory[], Error>({
    queryKey: ['scanHistory', limit],
    queryFn: () => productService.getRecentScans(limit),
    staleTime: 0,
  });
}

export function useClearHistory() {
  const { productService } = useDI();
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: () => productService.clearHistory(),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['scanHistory'] });
    },
  });
}
