import { ProductService } from '../../src/application/services/ProductService';
import { MockApiClient } from '../mocks/MockApiClient';
import { MockCacheRepository } from '../mocks/MockCacheRepository';
import { createMockProduct, MOCK_PRODUCTS, MOCK_SCAN_HISTORY } from '../mocks/mockData';
import { CachedProduct } from '../../src/core/interfaces/ICacheRepository';

describe('ProductService', () => {
  let productService: ProductService;
  let mockApiClient: MockApiClient;
  let mockCacheRepository: MockCacheRepository;

  beforeEach(() => {
    mockApiClient = new MockApiClient();
    mockCacheRepository = new MockCacheRepository();
    productService = new ProductService(mockApiClient, mockCacheRepository);
  });

  afterEach(() => {
    mockApiClient.reset();
    mockCacheRepository.reset();
  });

  describe('getProduct', () => {
    it('should return cached product if available and not expired', async () => {
      const cachedProduct: CachedProduct = {
        ...createMockProduct({ barcode: '1234567890123' }),
        cachedAt: new Date(),
        expiresAt: new Date(Date.now() + 3600000),
      };
      mockCacheRepository.setProducts([cachedProduct]);

      const result = await productService.getProduct('1234567890123', 'en');

      expect(result).not.toBeNull();
      expect(result!.barcode).toBe('1234567890123');
      expect(result!.isFromCache).toBe(true);
    });

    it('should fetch from API when product not in cache', async () => {
      const product = createMockProduct({ barcode: '1234567890123' });
      mockApiClient.setProduct('1234567890123', product);

      const result = await productService.getProduct('1234567890123', 'en');

      expect(result).not.toBeNull();
      expect(result!.barcode).toBe('1234567890123');
      expect(result!.isFromCache).toBe(false);
    });

    it('should cache product after fetching from API', async () => {
      const product = createMockProduct({ barcode: '1234567890123' });
      mockApiClient.setProduct('1234567890123', product);

      await productService.getProduct('1234567890123', 'en');

      expect(mockCacheRepository.getProductCount()).toBe(1);
    });

    it('should add scan to history after successful fetch', async () => {
      const product = createMockProduct({ barcode: '1234567890123' });
      mockApiClient.setProduct('1234567890123', product);

      await productService.getProduct('1234567890123', 'en');

      expect(mockCacheRepository.getHistoryCount()).toBe(1);
    });

    it('should return null when product not found', async () => {
      const result = await productService.getProduct('nonexistent', 'en');

      expect(result).toBeNull();
    });

    it('should return null when API fails', async () => {
      mockApiClient.setShouldFail(true, 'Network error');

      const result = await productService.getProduct('1234567890123', 'en');

      expect(result).toBeNull();
    });

    it('should include Hebrew disclaimer for Hebrew locale', async () => {
      const product = createMockProduct({ barcode: '1234567890123' });
      mockApiClient.setProduct('1234567890123', product);

      const result = await productService.getProduct('1234567890123', 'he');

      expect(result!.disclaimer).toContain('המידע מבוסס');
    });

    it('should include English disclaimer for English locale', async () => {
      const product = createMockProduct({ barcode: '1234567890123' });
      mockApiClient.setProduct('1234567890123', product);

      const result = await productService.getProduct('1234567890123', 'en');

      expect(result!.disclaimer).toContain('Information is based');
    });
  });

  describe('searchProducts', () => {
    beforeEach(() => {
      mockApiClient.setProduct('1111111111111', MOCK_PRODUCTS.healthy);
      mockApiClient.setProduct('2222222222222', MOCK_PRODUCTS.moderate);
      mockApiClient.setProduct('3333333333333', MOCK_PRODUCTS.unhealthy);
    });

    it('should return matching products', async () => {
      const result = await productService.searchProducts('Salad');

      expect(result.success).toBe(true);
      expect(result.data.items.length).toBe(1);
      expect(result.data.items[0].name).toBe('Organic Salad');
    });

    it('should return empty results for no matches', async () => {
      const result = await productService.searchProducts('NonexistentProduct');

      expect(result.success).toBe(true);
      expect(result.data.items.length).toBe(0);
    });

    it('should handle search failure gracefully', async () => {
      mockApiClient.setShouldFail(true);

      const result = await productService.searchProducts('test');

      expect(result.success).toBe(false);
    });

    it('should support pagination', async () => {
      const page1 = await productService.searchProducts('', 1, 2);
      const page2 = await productService.searchProducts('', 2, 2);

      expect(page1.data.items.length).toBe(2);
      expect(page2.data.items.length).toBe(1);
    });
  });

  describe('contributeProduct', () => {
    it('should return true on successful contribution', async () => {
      const result = await productService.contributeProduct('1234567890123', {
        nutritionImageBase64: 'base64data',
      });

      expect(result).toBe(true);
    });

    it('should return false when contribution fails', async () => {
      mockApiClient.setShouldFail(true);

      const result = await productService.contributeProduct('1234567890123', {
        nutritionImageBase64: 'base64data',
      });

      expect(result).toBe(false);
    });
  });

  describe('getRecentScans', () => {
    beforeEach(() => {
      mockCacheRepository.setHistory(MOCK_SCAN_HISTORY);
    });

    it('should return recent scans sorted by date', async () => {
      const scans = await productService.getRecentScans(10);

      expect(scans.length).toBe(3);
      expect(scans[0].productName).toBe('Organic Salad');
    });

    it('should limit results to specified count', async () => {
      const scans = await productService.getRecentScans(2);

      expect(scans.length).toBe(2);
    });

    it('should return empty array when no history', async () => {
      mockCacheRepository.reset();

      const scans = await productService.getRecentScans(10);

      expect(scans.length).toBe(0);
    });
  });

  describe('clearHistory', () => {
    it('should clear all scan history', async () => {
      mockCacheRepository.setHistory(MOCK_SCAN_HISTORY);
      expect(mockCacheRepository.getHistoryCount()).toBe(3);

      await productService.clearHistory();

      expect(mockCacheRepository.getHistoryCount()).toBe(0);
    });
  });
});
