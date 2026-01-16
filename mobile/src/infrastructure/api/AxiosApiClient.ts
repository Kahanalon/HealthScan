import axios, { AxiosInstance, AxiosError } from 'axios';
import {
  IApiClient,
  ProductResponse,
  SearchResponse,
  ContributionResponse,
  OcrResponse,
} from '../../core/interfaces/IApiClient';
import { ContributionData, Product, NutritionInfo } from '../../core/entities/Product';

export interface ApiClientConfig {
  baseUrl: string;
  timeout?: number;
  enableLogging?: boolean;
}

export class AxiosApiClient implements IApiClient {
  private client: AxiosInstance;
  private enableLogging: boolean;

  constructor(config: ApiClientConfig) {
    this.enableLogging = config.enableLogging ?? __DEV__;

    this.client = axios.create({
      baseURL: config.baseUrl,
      timeout: config.timeout ?? 30000,
      headers: {
        'Content-Type': 'application/json',
      },
    });

    this.setupInterceptors();
  }

  private setupInterceptors(): void {
    this.client.interceptors.request.use(
      (config) => {
        if (this.enableLogging) {
          console.log(`[API] ${config.method?.toUpperCase()} ${config.url}`);
        }
        return config;
      },
      (error) => {
        if (this.enableLogging) {
          console.error('[API] Request error:', error);
        }
        return Promise.reject(error);
      }
    );

    this.client.interceptors.response.use(
      (response) => {
        if (this.enableLogging) {
          console.log(`[API] Response ${response.status} from ${response.config.url}`);
        }
        return response;
      },
      (error: AxiosError) => {
        if (this.enableLogging) {
          console.error('[API] Response error:', error.message);
        }
        return Promise.reject(this.normalizeError(error));
      }
    );
  }

  private normalizeError(error: AxiosError): Error {
    if (error.response) {
      const status = error.response.status;
      const data = error.response.data as { message?: string };
      return new Error(data?.message ?? `HTTP ${status} error`);
    }
    if (error.request) {
      return new Error('Network error - please check your connection');
    }
    return new Error(error.message);
  }

  async getProduct(barcode: string): Promise<ProductResponse> {
    try {
      const response = await this.client.get(`/api/v1/products/${barcode}`);
      return {
        success: true,
        data: this.mapProductResponse(response.data),
      };
    } catch (error) {
      return {
        success: false,
        data: null,
        errorCode: 'FETCH_ERROR',
        message: error instanceof Error ? error.message : 'Unknown error',
      };
    }
  }

  async searchProducts(query: string, page: number, pageSize: number = 20): Promise<SearchResponse> {
    try {
      const response = await this.client.get('/api/v1/products/search', {
        params: { q: query, page, pageSize },
      });
      const results = response.data.results ?? [];
      return {
        success: true,
        data: {
          items: results.map(this.mapSearchResult),
          totalCount: response.data.totalCount ?? 0,
          page: response.data.page ?? page,
          pageSize: response.data.pageSize ?? pageSize,
          hasMore: results.length === pageSize,
        },
      };
    } catch (error) {
      return {
        success: false,
        data: { items: [], totalCount: 0, page, pageSize, hasMore: false },
        errorCode: 'SEARCH_ERROR',
        message: error instanceof Error ? error.message : 'Unknown error',
      };
    }
  }

  async contributeProduct(barcode: string, data: ContributionData): Promise<ContributionResponse> {
    try {
      const backendRequest = {
        fieldName: data.nutritionImageBase64 ? 'nutrition' : 'ingredients',
        imageBase64: data.nutritionImageBase64 ?? data.ingredientsImageBase64,
        deviceId: undefined,
      };
      const response = await this.client.post(`/api/v1/products/${barcode}/contribute`, backendRequest);
      return {
        success: true,
        data: {
          contributionId: response.data.contributionId,
          status: response.data.status ?? 'pending',
        },
      };
    } catch (error) {
      return {
        success: false,
        data: null,
        errorCode: 'CONTRIBUTE_ERROR',
        message: error instanceof Error ? error.message : 'Unknown error',
      };
    }
  }

  async processNutritionOcr(imageBase64: string, barcode: string): Promise<OcrResponse> {
    try {
      const response = await this.client.post('/api/v1/ocr/nutrition', {
        imageBase64,
        barcode,
      });
      return {
        success: true,
        data: {
          nutritionInfo: this.mapNutritionData(response.data.extractedData),
          confidence: response.data.confidence ?? 0,
          rawText: response.data.rawText ?? '',
        },
      };
    } catch (error) {
      return {
        success: false,
        data: null,
        errorCode: 'OCR_ERROR',
        message: error instanceof Error ? error.message : 'Unknown error',
      };
    }
  }

  async processIngredientsOcr(imageBase64: string, barcode: string): Promise<OcrResponse> {
    try {
      const response = await this.client.post('/api/v1/ocr/ingredients', {
        imageBase64,
        barcode,
      });
      return {
        success: true,
        data: {
          ingredients: response.data.extractedText,
          confidence: response.data.confidence ?? 0,
          rawText: response.data.extractedText ?? '',
        },
      };
    } catch (error) {
      return {
        success: false,
        data: null,
        errorCode: 'OCR_ERROR',
        message: error instanceof Error ? error.message : 'Unknown error',
      };
    }
  }

  private mapProductResponse(data: any): Product | null {
    if (!data) return null;
    return {
      barcode: data.barcode,
      name: data.name ?? data.nameEn ?? '',
      brand: data.brand ?? null,
      imageUrl: null,
      nutriScoreGrade: data.score?.grade ?? 'Unknown',
      nutriScoreScore: data.score?.value ?? null,
      nutritionPer100g: this.mapNutritionData(data.nutrition?.per100),
      ingredients: null,
      allergens: [],
      categories: [],
      flags: (data.flags ?? []).map((f: any) => ({
        nutrient: f.type,
        level: 'moderate' as const,
        description: f.description,
      })),
      dataSource: 'cached',
      lastUpdated: data.lastUpdated ?? new Date().toISOString(),
    };
  }

  private mapSearchResult = (data: any): Product => {
    return {
      barcode: data.barcode,
      name: data.name ?? '',
      brand: data.brand ?? null,
      imageUrl: null,
      nutriScoreGrade: data.grade ?? 'Unknown',
      nutriScoreScore: data.score ?? null,
      nutritionPer100g: {
        energyKcal: null,
        fat: null,
        saturatedFat: null,
        carbohydrates: null,
        sugars: null,
        fiber: null,
        protein: null,
        salt: null,
        sodium: null,
      },
      ingredients: null,
      allergens: [],
      categories: [],
      flags: [],
      dataSource: 'cached',
      lastUpdated: new Date().toISOString(),
    };
  };

  private mapNutritionData(data: any): NutritionInfo {
    if (!data) {
      return {
        energyKcal: null,
        fat: null,
        saturatedFat: null,
        carbohydrates: null,
        sugars: null,
        fiber: null,
        protein: null,
        salt: null,
        sodium: null,
      };
    }
    return {
      energyKcal: data.energy ?? null,
      fat: data.fat ?? null,
      saturatedFat: data.saturatedFat ?? null,
      carbohydrates: data.carbohydrates ?? null,
      sugars: data.sugars ?? null,
      fiber: data.fiber ?? null,
      protein: data.protein ?? null,
      salt: data.sodium ? data.sodium * 2.5 : null,
      sodium: data.sodium ?? null,
    };
  }
}
