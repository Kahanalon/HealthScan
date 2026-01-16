import { MockCacheRepository } from './MockCacheRepository';
import { createMockProduct, createMockScanHistory } from './mockData';
import { CachedProduct } from '../../src/core/interfaces/ICacheRepository';

describe('MockCacheRepository', () => {
  let repository: MockCacheRepository;

  beforeEach(() => {
    repository = new MockCacheRepository();
  });

  afterEach(() => {
    repository.reset();
  });

  describe('getProduct', () => {
    it('should return null when no product cached', async () => {
      const result = await repository.getProduct('nonexistent');

      expect(result).toBeNull();
    });

    it('should return cached product when available', async () => {
      const cachedProduct: CachedProduct = {
        ...createMockProduct({ barcode: '123' }),
        cachedAt: new Date(),
        expiresAt: new Date(Date.now() + 3600000),
      };
      repository.setProducts([cachedProduct]);

      const result = await repository.getProduct('123');

      expect(result).not.toBeNull();
      expect(result?.barcode).toBe('123');
    });

    it('should return null for expired product', async () => {
      const cachedProduct: CachedProduct = {
        ...createMockProduct({ barcode: '123' }),
        cachedAt: new Date(Date.now() - 7200000),
        expiresAt: new Date(Date.now() - 3600000),
      };
      repository.setProducts([cachedProduct]);

      const result = await repository.getProduct('123');

      expect(result).toBeNull();
    });
  });

  describe('saveProduct', () => {
    it('should save product to cache', async () => {
      const product = createMockProduct({ barcode: '123' });

      await repository.saveProduct(product);

      const cached = await repository.getProduct('123');
      expect(cached).not.toBeNull();
    });

    it('should update existing product', async () => {
      const product1 = createMockProduct({ barcode: '123', name: 'Original' });
      const product2 = createMockProduct({ barcode: '123', name: 'Updated' });

      await repository.saveProduct(product1);
      await repository.saveProduct(product2);

      const cached = await repository.getProduct('123');
      expect(cached?.name).toBe('Updated');
      expect(repository.getProductCount()).toBe(1);
    });
  });

  describe('deleteProduct', () => {
    it('should remove product from cache', async () => {
      await repository.saveProduct(createMockProduct({ barcode: '123' }));

      await repository.deleteProduct('123');

      const cached = await repository.getProduct('123');
      expect(cached).toBeNull();
    });
  });

  describe('scan history', () => {
    it('should return empty array when no history', async () => {
      const history = await repository.getRecentScans(10);

      expect(history).toEqual([]);
    });

    it('should return scans sorted by date descending', async () => {
      const older = createMockScanHistory({
        id: '1',
        scannedAt: new Date('2025-01-14'),
      });
      const newer = createMockScanHistory({
        id: '2',
        scannedAt: new Date('2025-01-15'),
      });
      repository.setHistory([older, newer]);

      const history = await repository.getRecentScans(10);

      expect(history[0].id).toBe('2');
      expect(history[1].id).toBe('1');
    });

    it('should respect limit', async () => {
      repository.setHistory([
        createMockScanHistory({ id: '1' }),
        createMockScanHistory({ id: '2' }),
        createMockScanHistory({ id: '3' }),
      ]);

      const history = await repository.getRecentScans(2);

      expect(history.length).toBe(2);
    });

    it('should add scan to history', async () => {
      const scan = createMockScanHistory({ id: 'new' });

      await repository.addScanHistory(scan);

      expect(repository.getHistoryCount()).toBe(1);
    });

    it('should delete scan from history', async () => {
      repository.setHistory([createMockScanHistory({ id: '1' })]);

      await repository.deleteScanHistory('1');

      expect(repository.getHistoryCount()).toBe(0);
    });

    it('should clear all history', async () => {
      repository.setHistory([
        createMockScanHistory({ id: '1' }),
        createMockScanHistory({ id: '2' }),
      ]);

      await repository.clearAllHistory();

      expect(repository.getHistoryCount()).toBe(0);
    });
  });

  describe('clearCache', () => {
    it('should clear both products and history', async () => {
      await repository.saveProduct(createMockProduct({ barcode: '123' }));
      await repository.addScanHistory(createMockScanHistory({ id: '1' }));

      await repository.clearCache();

      expect(repository.getProductCount()).toBe(0);
      expect(repository.getHistoryCount()).toBe(0);
    });
  });
});
