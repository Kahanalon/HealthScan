import React, { createContext, useContext, useMemo } from 'react';
import { IApiClient } from '../core/interfaces/IApiClient';
import { ICacheRepository } from '../core/interfaces/ICacheRepository';
import { IBarcodeScanner } from '../core/interfaces/IBarcodeScanner';
import { IAnalyticsService } from '../core/interfaces/IAnalyticsService';
import { IImageService } from '../core/interfaces/IImageService';
import { AxiosApiClient } from '../infrastructure/api/AxiosApiClient';
import { WatermelonRepository } from '../infrastructure/cache/WatermelonRepository';
import { getDatabase } from '../infrastructure/cache/database';
import { visionCameraScanner } from '../infrastructure/scanner/VisionCameraScanner';
import { noOpAnalytics } from '../infrastructure/analytics/NoOpAnalytics';
import { visionCameraImage } from '../infrastructure/image/VisionCameraImage';
import { ProductService } from '../application/services/ProductService';

export interface DIContainer {
  apiClient: IApiClient;
  cacheRepository: ICacheRepository;
  barcodeScanner: IBarcodeScanner;
  analyticsService: IAnalyticsService;
  imageService: IImageService;
  productService: ProductService;
}

const API_BASE_URL = __DEV__
  ? 'http://localhost:5000'
  : 'https://api.healthscan.co.il';

function createContainer(): DIContainer {
  const apiClient = new AxiosApiClient({
    baseUrl: API_BASE_URL,
    enableLogging: __DEV__,
  });

  const database = getDatabase();
  const cacheRepository = new WatermelonRepository(database);

  const productService = new ProductService(apiClient, cacheRepository);

  return {
    apiClient,
    cacheRepository,
    barcodeScanner: visionCameraScanner,
    analyticsService: noOpAnalytics,
    imageService: visionCameraImage,
    productService,
  };
}

const DIContext = createContext<DIContainer | null>(null);

interface DIProviderProps {
  children: React.ReactNode;
  container?: DIContainer;
}

export function DIProvider({ children, container }: DIProviderProps) {
  const value = useMemo(() => container ?? createContainer(), [container]);

  return React.createElement(DIContext.Provider, { value }, children);
}

export function useDI(): DIContainer {
  const context = useContext(DIContext);
  if (!context) {
    throw new Error('useDI must be used within a DIProvider');
  }
  return context;
}

export function createTestContainer(overrides: Partial<DIContainer> = {}): DIContainer {
  const defaultContainer = createContainer();
  return {
    ...defaultContainer,
    ...overrides,
  };
}
