import { useInfiniteQuery } from '@tanstack/react-query';
import { Product } from '../../core/entities/Product';
import { useDI } from '../../di/container';

export interface SearchResult {
  items: Product[];
  totalCount: number;
  hasMore: boolean;
}

export function useSearch(query: string) {
  const { productService } = useDI();

  return useInfiniteQuery<SearchResult, Error>({
    queryKey: ['search', query],
    queryFn: async ({ pageParam }) => {
      const response = await productService.searchProducts(query, pageParam as number);
      if (!response.success) {
        throw new Error(response.message ?? 'Search failed');
      }
      return {
        items: response.data.items,
        totalCount: response.data.totalCount,
        hasMore: response.data.hasMore,
      };
    },
    initialPageParam: 1,
    getNextPageParam: (lastPage, allPages) => {
      return lastPage.hasMore ? allPages.length + 1 : undefined;
    },
    enabled: query.length >= 2,
    staleTime: 2 * 60 * 1000,
  });
}
