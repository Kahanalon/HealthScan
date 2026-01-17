import { Database, Q } from '@nozbe/watermelondb';
import { ICacheRepository, CachedProduct } from '../../core/interfaces/ICacheRepository';
import { Product } from '../../core/entities/Product';
import { ScanHistory } from '../../core/entities/ScanHistory';
import CachedProductModel from './models/CachedProductModel';
import ScanHistoryModel from './models/ScanHistoryModel';

interface CachedProductRaw {
  _raw: {
    nutrition_json: string;
    allergens_json: string;
    categories_json: string;
    flags_json: string;
  };
}

const DEFAULT_TTL_MINUTES = 60 * 24;

export class WatermelonRepository implements ICacheRepository {
  constructor(private database: Database) {
    this.clearExpiredCache().catch(() => {});
  }

  async getProduct(barcode: string): Promise<CachedProduct | null> {
    const collection = this.database.get<CachedProductModel>('cached_products');
    const now = Date.now();

    const results = await collection
      .query(Q.where('barcode', barcode), Q.where('expires_at', Q.gt(now)))
      .fetch();

    if (results.length === 0) {
      return null;
    }

    return results[0].toCachedProduct();
  }

  async saveProduct(product: Product, ttlMinutes: number = DEFAULT_TTL_MINUTES): Promise<void> {
    const collection = this.database.get<CachedProductModel>('cached_products');
    const now = Date.now();
    const expiresAt = now + ttlMinutes * 60 * 1000;

    await this.database.write(async () => {
      const existing = await collection.query(Q.where('barcode', product.barcode)).fetch();

      if (existing.length > 0) {
        await existing[0].update((record) => {
          const rawRecord = record as CachedProductModel & CachedProductRaw;
          record.name = product.name;
          record.brand = product.brand;
          record.imageUrl = product.imageUrl;
          record.nutriScoreGrade = product.nutriScoreGrade;
          record.nutriScoreScore = product.nutriScoreScore;
          rawRecord._raw.nutrition_json = JSON.stringify(product.nutritionPer100g);
          record.ingredients = product.ingredients;
          rawRecord._raw.allergens_json = JSON.stringify(product.allergens);
          rawRecord._raw.categories_json = JSON.stringify(product.categories);
          rawRecord._raw.flags_json = JSON.stringify(product.flags);
          record.dataSource = product.dataSource;
          record.lastUpdated = product.lastUpdated;
          record.cachedAt = now;
          record.expiresAt = expiresAt;
        });
      } else {
        await collection.create((record) => {
          const rawRecord = record as CachedProductModel & CachedProductRaw;
          record.barcode = product.barcode;
          record.name = product.name;
          record.brand = product.brand;
          record.imageUrl = product.imageUrl;
          record.nutriScoreGrade = product.nutriScoreGrade;
          record.nutriScoreScore = product.nutriScoreScore;
          rawRecord._raw.nutrition_json = JSON.stringify(product.nutritionPer100g);
          record.ingredients = product.ingredients;
          rawRecord._raw.allergens_json = JSON.stringify(product.allergens);
          rawRecord._raw.categories_json = JSON.stringify(product.categories);
          rawRecord._raw.flags_json = JSON.stringify(product.flags);
          record.dataSource = product.dataSource;
          record.lastUpdated = product.lastUpdated;
          record.cachedAt = now;
          record.expiresAt = expiresAt;
        });
      }
    });
  }

  async deleteProduct(barcode: string): Promise<void> {
    const collection = this.database.get<CachedProductModel>('cached_products');

    await this.database.write(async () => {
      const products = await collection.query(Q.where('barcode', barcode)).fetch();
      for (const product of products) {
        await product.destroyPermanently();
      }
    });
  }

  async getRecentScans(limit: number): Promise<ScanHistory[]> {
    const collection = this.database.get<ScanHistoryModel>('scan_history');

    const results = await collection
      .query(Q.sortBy('scanned_at', Q.desc), Q.take(limit))
      .fetch();

    return results.map((model) => model.toScanHistory());
  }

  async addScanHistory(scan: ScanHistory): Promise<void> {
    const collection = this.database.get<ScanHistoryModel>('scan_history');

    await this.database.write(async () => {
      await collection.create((record) => {
        record.barcode = scan.barcode;
        record.productName = scan.productName;
        record.brand = scan.brand;
        record.imageUrl = scan.imageUrl;
        record.grade = scan.grade;
        record.scannedAt = scan.scannedAt.getTime();
      });
    });
  }

  async deleteScanHistory(id: string): Promise<void> {
    const collection = this.database.get<ScanHistoryModel>('scan_history');

    await this.database.write(async () => {
      const record = await collection.find(id);
      await record.destroyPermanently();
    });
  }

  async clearAllHistory(): Promise<void> {
    const collection = this.database.get<ScanHistoryModel>('scan_history');

    await this.database.write(async () => {
      const allRecords = await collection.query().fetch();
      for (const record of allRecords) {
        await record.destroyPermanently();
      }
    });
  }

  async clearExpiredCache(): Promise<void> {
    const collection = this.database.get<CachedProductModel>('cached_products');
    const now = Date.now();

    await this.database.write(async () => {
      const expired = await collection.query(Q.where('expires_at', Q.lt(now))).fetch();
      for (const record of expired) {
        await record.destroyPermanently();
      }
    });
  }

  async clearCache(): Promise<void> {
    const productCollection = this.database.get<CachedProductModel>('cached_products');
    const historyCollection = this.database.get<ScanHistoryModel>('scan_history');

    await this.database.write(async () => {
      const allProducts = await productCollection.query().fetch();
      for (const record of allProducts) {
        await record.destroyPermanently();
      }

      const allHistory = await historyCollection.query().fetch();
      for (const record of allHistory) {
        await record.destroyPermanently();
      }
    });
  }
}
