import { useMutation, useQueryClient } from '@tanstack/react-query';
import { ContributionData } from '../../core/entities/Product';
import { useDI } from '../../di/container';

interface ContributeVariables {
  barcode: string;
  data: ContributionData;
}

export function useContribute() {
  const { productService } = useDI();
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ barcode, data }: ContributeVariables) =>
      productService.contributeProduct(barcode, data),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({ queryKey: ['product', variables.barcode] });
    },
  });
}
