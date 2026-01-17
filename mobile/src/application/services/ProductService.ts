import { Product, ContributionData } from '../../core/entities/Product';
import { ScanHistory, createScanHistoryId } from '../../core/entities/ScanHistory';
import { ScoreResult } from '../../core/entities/ScoreResult';
import { IApiClient, SearchResponse } from '../../core/interfaces/IApiClient';
import { ICacheRepository } from '../../core/interfaces/ICacheRepository';

export class ProductService {
  constructor(
    private apiClient: IApiClient,
    private cacheRepository: ICacheRepository
  ) {}

  async getProduct(barcode: string, locale: 'he' | 'en' = 'he'): Promise<ScoreResult | null> {
    const cached = await this.cacheRepository.getProduct(barcode);
    if (cached) {
      return this.toScoreResult(cached, locale, true);
    }

    const response = await this.apiClient.getProduct(barcode);
    if (!response.success || !response.data) {
      return null;
    }

    await this.cacheRepository.saveProduct(response.data);
    await this.addToHistory(response.data);

    return this.toScoreResult(response.data, locale, false);
  }

  async searchProducts(query: string, page: number = 1, pageSize: number = 20): Promise<SearchResponse> {
    return this.apiClient.searchProducts(query, page, pageSize);
  }

  async contributeProduct(barcode: string, data: ContributionData): Promise<boolean> {
    const response = await this.apiClient.contributeProduct(barcode, data);
    return response.success;
  }

  async getRecentScans(limit: number = 20): Promise<ScanHistory[]> {
    return this.cacheRepository.getRecentScans(limit);
  }

  async clearHistory(): Promise<void> {
    await this.cacheRepository.clearAllHistory();
  }

  private async addToHistory(product: Product): Promise<void> {
    const now = new Date();
    const scanHistory: ScanHistory = {
      id: createScanHistoryId(product.barcode, now),
      barcode: product.barcode,
      productName: product.name,
      brand: product.brand,
      imageUrl: product.imageUrl,
      grade: product.nutriScoreGrade,
      scannedAt: now,
    };
    await this.cacheRepository.addScanHistory(scanHistory);
  }

  private toScoreResult(product: Product, locale: 'he' | 'en', isFromCache: boolean): ScoreResult {
    return {
      barcode: product.barcode,
      productName: product.name,
      brand: product.brand,
      imageUrl: product.imageUrl,
      grade: product.nutriScoreGrade,
      score: product.nutriScoreScore,
      flags: product.flags,
      nutritionPer100g: product.nutritionPer100g,
      isFromCache,
      disclaimer: this.getDisclaimer(locale),
    };
  }

  private getDisclaimer(locale: 'he' | 'en'): string {
    return locale === 'he'
      ? 'המידע מבוסס על נתונים ממקורות חיצוניים ועשוי לא להיות מעודכן. בדקו תמיד את התווית לפני הצריכה.'
      : 'Information is based on external data sources and may not be up to date. Always check the label before consumption.';
  }
}
