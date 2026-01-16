import React from 'react';
import { renderHook, waitFor } from '@testing-library/react-native';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { DIProvider } from '../../src/di/container';
import { useProduct } from '../../src/application/hooks/useProduct';
import { useScanHistory, useClearHistory } from '../../src/application/hooks/useScanHistory';
import { useContribute } from '../../src/application/hooks/useContribute';
import { MockApiClient } from '../mocks/MockApiClient';
import { MockCacheRepository } from '../mocks/MockCacheRepository';
import { createTestContainer, createTestQueryClient } from '../utils/testUtils';
import { createMockProduct, MOCK_SCAN_HISTORY } from '../mocks/mockData';

function createWrapper(queryClient: QueryClient, container: ReturnType<typeof createTestContainer>) {
  return function Wrapper({ children }: { children: React.ReactNode }) {
    return (
      <QueryClientProvider client={queryClient}>
        <DIProvider container={container}>{children}</DIProvider>
      </QueryClientProvider>
    );
  };
}

describe('useProduct', () => {
  let queryClient: QueryClient;
  let mockApiClient: MockApiClient;
  let mockCacheRepository: MockCacheRepository;
  let container: ReturnType<typeof createTestContainer>;

  beforeEach(() => {
    queryClient = createTestQueryClient();
    mockApiClient = new MockApiClient();
    mockCacheRepository = new MockCacheRepository();
    container = createTestContainer({
      apiClient: mockApiClient,
      cacheRepository: mockCacheRepository,
    });
  });

  afterEach(() => {
    queryClient.clear();
    mockApiClient.reset();
    mockCacheRepository.reset();
  });

  it('should return null when barcode is null', async () => {
    const { result } = renderHook(() => useProduct(null), {
      wrapper: createWrapper(queryClient, container),
    });

    await waitFor(() => {
      expect(result.current.isLoading).toBe(false);
    });

    expect(result.current.data).toBeUndefined();
  });

  it('should fetch product when barcode is provided', async () => {
    const product = createMockProduct({ barcode: '1234567890123' });
    mockApiClient.setProduct('1234567890123', product);

    const { result } = renderHook(() => useProduct('1234567890123'), {
      wrapper: createWrapper(queryClient, container),
    });

    await waitFor(() => {
      expect(result.current.isSuccess).toBe(true);
    });

    expect(result.current.data?.barcode).toBe('1234567890123');
    expect(result.current.data?.productName).toBe('Test Product');
  });

  it('should return null when product not found', async () => {
    const { result } = renderHook(() => useProduct('nonexistent'), {
      wrapper: createWrapper(queryClient, container),
    });

    await waitFor(() => {
      expect(result.current.isSuccess).toBe(true);
    });

    expect(result.current.data).toBeNull();
  });

  it('should return cached product first', async () => {
    const cachedProduct = {
      ...createMockProduct({ barcode: '1234567890123', name: 'Cached Product' }),
      cachedAt: new Date(),
      expiresAt: new Date(Date.now() + 3600000),
    };
    mockCacheRepository.setProducts([cachedProduct]);

    const { result } = renderHook(() => useProduct('1234567890123'), {
      wrapper: createWrapper(queryClient, container),
    });

    await waitFor(() => {
      expect(result.current.isSuccess).toBe(true);
    });

    expect(result.current.data?.isFromCache).toBe(true);
  });
});

describe('useScanHistory', () => {
  let queryClient: QueryClient;
  let mockCacheRepository: MockCacheRepository;
  let container: ReturnType<typeof createTestContainer>;

  beforeEach(() => {
    queryClient = createTestQueryClient();
    mockCacheRepository = new MockCacheRepository();
    container = createTestContainer({
      cacheRepository: mockCacheRepository,
    });
  });

  afterEach(() => {
    queryClient.clear();
    mockCacheRepository.reset();
  });

  it('should return empty array when no history', async () => {
    const { result } = renderHook(() => useScanHistory(), {
      wrapper: createWrapper(queryClient, container),
    });

    await waitFor(() => {
      expect(result.current.isSuccess).toBe(true);
    });

    expect(result.current.data).toEqual([]);
  });

  it('should return scan history', async () => {
    mockCacheRepository.setHistory(MOCK_SCAN_HISTORY);

    const { result } = renderHook(() => useScanHistory(), {
      wrapper: createWrapper(queryClient, container),
    });

    await waitFor(() => {
      expect(result.current.isSuccess).toBe(true);
    });

    expect(result.current.data?.length).toBe(3);
  });

  it('should respect limit parameter', async () => {
    mockCacheRepository.setHistory(MOCK_SCAN_HISTORY);

    const { result } = renderHook(() => useScanHistory(2), {
      wrapper: createWrapper(queryClient, container),
    });

    await waitFor(() => {
      expect(result.current.isSuccess).toBe(true);
    });

    expect(result.current.data?.length).toBe(2);
  });
});

describe('useClearHistory', () => {
  let queryClient: QueryClient;
  let mockCacheRepository: MockCacheRepository;
  let container: ReturnType<typeof createTestContainer>;

  beforeEach(() => {
    queryClient = createTestQueryClient();
    mockCacheRepository = new MockCacheRepository();
    mockCacheRepository.setHistory(MOCK_SCAN_HISTORY);
    container = createTestContainer({
      cacheRepository: mockCacheRepository,
    });
  });

  afterEach(() => {
    queryClient.clear();
    mockCacheRepository.reset();
  });

  it('should clear all history', async () => {
    const { result } = renderHook(() => useClearHistory(), {
      wrapper: createWrapper(queryClient, container),
    });

    expect(mockCacheRepository.getHistoryCount()).toBe(3);

    await result.current.mutateAsync();

    expect(mockCacheRepository.getHistoryCount()).toBe(0);
  });
});

describe('useContribute', () => {
  let queryClient: QueryClient;
  let mockApiClient: MockApiClient;
  let container: ReturnType<typeof createTestContainer>;

  beforeEach(() => {
    queryClient = createTestQueryClient();
    mockApiClient = new MockApiClient();
    container = createTestContainer({
      apiClient: mockApiClient,
    });
  });

  afterEach(() => {
    queryClient.clear();
    mockApiClient.reset();
  });

  it('should contribute product successfully', async () => {
    const { result } = renderHook(() => useContribute(), {
      wrapper: createWrapper(queryClient, container),
    });

    const contributionResult = await result.current.mutateAsync({
      barcode: '1234567890123',
      data: { nutritionImageBase64: 'base64data' },
    });

    expect(contributionResult).toBe(true);
  });

  it('should handle contribution failure', async () => {
    mockApiClient.setShouldFail(true);

    const { result } = renderHook(() => useContribute(), {
      wrapper: createWrapper(queryClient, container),
    });

    const contributionResult = await result.current.mutateAsync({
      barcode: '1234567890123',
      data: { nutritionImageBase64: 'base64data' },
    });

    expect(contributionResult).toBe(false);
  });
});
