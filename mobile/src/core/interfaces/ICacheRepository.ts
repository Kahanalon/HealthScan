import { Product } from '../entities/Product';
import { ScanHistory } from '../entities/ScanHistory';

export interface CachedProduct extends Product {
  cachedAt: Date;
  expiresAt: Date;
}

export interface ICacheRepository {
  getProduct(barcode: string): Promise<CachedProduct | null>;
  saveProduct(product: Product, ttlMinutes?: number): Promise<void>;
  deleteProduct(barcode: string): Promise<void>;
  getRecentScans(limit: number): Promise<ScanHistory[]>;
  addScanHistory(scan: ScanHistory): Promise<void>;
  deleteScanHistory(id: string): Promise<void>;
  clearAllHistory(): Promise<void>;
  clearExpiredCache(): Promise<void>;
  clearCache(): Promise<void>;
}
