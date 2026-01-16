import React, { ReactElement } from 'react';
import { render, RenderOptions } from '@testing-library/react-native';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { DIProvider, DIContainer } from '../../src/di/container';
import { MockApiClient } from '../mocks/MockApiClient';
import { MockCacheRepository } from '../mocks/MockCacheRepository';
import { MockBarcodeScanner } from '../mocks/MockBarcodeScanner';
import { ProductService } from '../../src/application/services/ProductService';
import { noOpAnalytics } from '../../src/infrastructure/analytics/NoOpAnalytics';

export interface TestContainerOptions {
  apiClient?: MockApiClient;
  cacheRepository?: MockCacheRepository;
  barcodeScanner?: MockBarcodeScanner;
}

export function createTestContainer(options: TestContainerOptions = {}): DIContainer {
  const apiClient = options.apiClient ?? new MockApiClient();
  const cacheRepository = options.cacheRepository ?? new MockCacheRepository();
  const barcodeScanner = options.barcodeScanner ?? new MockBarcodeScanner();

  const productService = new ProductService(apiClient, cacheRepository);

  return {
    apiClient,
    cacheRepository,
    barcodeScanner,
    analyticsService: noOpAnalytics,
    imageService: {
      captureImage: jest.fn(),
      pickFromGallery: jest.fn(),
      resizeImage: jest.fn(),
    },
    productService,
  };
}

export function createTestQueryClient(): QueryClient {
  return new QueryClient({
    defaultOptions: {
      queries: {
        retry: false,
        gcTime: 0,
      },
      mutations: {
        retry: false,
      },
    },
  });
}

interface AllProvidersProps {
  children: React.ReactNode;
  container?: DIContainer;
  queryClient?: QueryClient;
}

function AllProviders({ children, container, queryClient }: AllProvidersProps) {
  const testContainer = container ?? createTestContainer();
  const testQueryClient = queryClient ?? createTestQueryClient();

  return (
    <QueryClientProvider client={testQueryClient}>
      <DIProvider container={testContainer}>{children}</DIProvider>
    </QueryClientProvider>
  );
}

interface CustomRenderOptions extends Omit<RenderOptions, 'wrapper'> {
  container?: DIContainer;
  queryClient?: QueryClient;
}

export function renderWithProviders(
  ui: ReactElement,
  options: CustomRenderOptions = {}
): ReturnType<typeof render> & { container: DIContainer; queryClient: QueryClient } {
  const { container, queryClient, ...renderOptions } = options;
  const testContainer = container ?? createTestContainer();
  const testQueryClient = queryClient ?? createTestQueryClient();

  const Wrapper = ({ children }: { children: React.ReactNode }) => (
    <AllProviders container={testContainer} queryClient={testQueryClient}>
      {children}
    </AllProviders>
  );

  return {
    ...render(ui, { wrapper: Wrapper, ...renderOptions }),
    container: testContainer,
    queryClient: testQueryClient,
  };
}

export function flushPromises(): Promise<void> {
  return new Promise((resolve) => setImmediate(resolve));
}

export async function waitForAsync(): Promise<void> {
  await flushPromises();
  await new Promise((resolve) => setTimeout(resolve, 0));
}
