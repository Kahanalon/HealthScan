import { MockApiClient } from './MockApiClient';
import { createMockProduct } from './mockData';

describe('MockApiClient', () => {
  let apiClient: MockApiClient;

  beforeEach(() => {
    apiClient = new MockApiClient();
  });

  afterEach(() => {
    apiClient.reset();
  });

  describe('getProduct', () => {
    it('should return product when set', async () => {
      const product = createMockProduct({ barcode: '1234567890123' });
      apiClient.setProduct('1234567890123', product);

      const result = await apiClient.getProduct('1234567890123');

      expect(result.success).toBe(true);
      expect(result.data?.barcode).toBe('1234567890123');
    });

    it('should return not found when product not set', async () => {
      const result = await apiClient.getProduct('nonexistent');

      expect(result.success).toBe(false);
      expect(result.data).toBeNull();
      expect(result.errorCode).toBe('NOT_FOUND');
    });

    it('should return error when shouldFail is true', async () => {
      apiClient.setProduct('1234567890123', createMockProduct());
      apiClient.setShouldFail(true, 'Custom error');

      const result = await apiClient.getProduct('1234567890123');

      expect(result.success).toBe(false);
      expect(result.message).toBe('Custom error');
    });
  });

  describe('searchProducts', () => {
    it('should search by name', async () => {
      const searchClient = new MockApiClient();
      const p1 = createMockProduct({ barcode: '1', name: 'Apple Juice' });
      const p2 = createMockProduct({ barcode: '2', name: 'Orange Juice' });
      const p3 = createMockProduct({ barcode: '3', name: 'Apple Pie' });
      searchClient.setProduct('1', p1);
      searchClient.setProduct('2', p2);
      searchClient.setProduct('3', p3);

      const allResult = await searchClient.searchProducts('', 1, 100);
      expect(allResult.data.items.length).toBe(3);

      const result = await searchClient.searchProducts('Apple', 1, 100);
      expect(result.success).toBe(true);
      expect(result.data.items.length).toBe(2);
    });

    it('should support pagination', async () => {
      const searchClient = new MockApiClient();
      searchClient.setProduct('1', createMockProduct({ barcode: '1', name: 'Apple Juice' }));
      searchClient.setProduct('2', createMockProduct({ barcode: '2', name: 'Orange Juice' }));
      searchClient.setProduct('3', createMockProduct({ barcode: '3', name: 'Apple Pie' }));

      const page1 = await searchClient.searchProducts('', 1, 2);
      const page2 = await searchClient.searchProducts('', 2, 2);

      expect(page1.data.items.length).toBe(2);
      expect(page1.data.hasMore).toBe(true);
      expect(page2.data.items.length).toBe(1);
      expect(page2.data.hasMore).toBe(false);
    });
  });

  describe('contributeProduct', () => {
    it('should return success by default', async () => {
      const result = await apiClient.contributeProduct('123', {});

      expect(result.success).toBe(true);
      expect(result.data?.status).toBe('pending');
    });
  });

  describe('reset', () => {
    it('should clear all products and failure state', async () => {
      apiClient.setProduct('123', createMockProduct());
      apiClient.setShouldFail(true);
      apiClient.reset();

      const result = await apiClient.getProduct('123');
      expect(result.success).toBe(false);
      expect(result.errorCode).toBe('NOT_FOUND');
    });
  });
});
