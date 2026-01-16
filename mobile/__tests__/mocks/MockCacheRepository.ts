import { ICacheRepository, CachedProduct } from '../../src/core/interfaces/ICacheRepository';
import { Product } from '../../src/core/entities/Product';
import { ScanHistory } from '../../src/core/entities/ScanHistory';

export class MockCacheRepository implements ICacheRepository {
  private products: Map<string, CachedProduct> = new Map();
  private history: ScanHistory[] = [];

  reset(): void {
    this.products.clear();
    this.history = [];
  }

  setProducts(products: CachedProduct[]): void {
    products.forEach((p) => this.products.set(p.barcode, p));
  }

  setHistory(history: ScanHistory[]): void {
    this.history = [...history];
  }

  async getProduct(barcode: string): Promise<CachedProduct | null> {
    const product = this.products.get(barcode);
    if (!product) return null;

    if (product.expiresAt < new Date()) {
      this.products.delete(barcode);
      return null;
    }

    return product;
  }

  async saveProduct(product: Product, ttlMinutes: number = 1440): Promise<void> {
    const now = new Date();
    const cachedProduct: CachedProduct = {
      ...product,
      cachedAt: now,
      expiresAt: new Date(now.getTime() + ttlMinutes * 60 * 1000),
    };
    this.products.set(product.barcode, cachedProduct);
  }

  async deleteProduct(barcode: string): Promise<void> {
    this.products.delete(barcode);
  }

  async getRecentScans(limit: number): Promise<ScanHistory[]> {
    return this.history
      .sort((a, b) => b.scannedAt.getTime() - a.scannedAt.getTime())
      .slice(0, limit);
  }

  async addScanHistory(scan: ScanHistory): Promise<void> {
    this.history.push(scan);
  }

  async deleteScanHistory(id: string): Promise<void> {
    this.history = this.history.filter((s) => s.id !== id);
  }

  async clearAllHistory(): Promise<void> {
    this.history = [];
  }

  async clearExpiredCache(): Promise<void> {
    const now = new Date();
    for (const [barcode, product] of this.products.entries()) {
      if (product.expiresAt < now) {
        this.products.delete(barcode);
      }
    }
  }

  async clearCache(): Promise<void> {
    this.products.clear();
    this.history = [];
  }

  getProductCount(): number {
    return this.products.size;
  }

  getHistoryCount(): number {
    return this.history.length;
  }
}
