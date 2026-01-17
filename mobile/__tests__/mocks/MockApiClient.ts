import {
  IApiClient,
  ProductResponse,
  SearchResponse,
  ContributionResponse,
  OcrResponse,
} from '../../src/core/interfaces/IApiClient';
import { ContributionData, Product } from '../../src/core/entities/Product';

export class MockApiClient implements IApiClient {
  private products: Map<string, Product> = new Map();
  private shouldFail: boolean = false;
  private failureMessage: string = 'Mock API error';

  setProduct(barcode: string, product: Product): void {
    this.products.set(barcode, product);
  }

  setShouldFail(shouldFail: boolean, message?: string): void {
    this.shouldFail = shouldFail;
    if (message) {
      this.failureMessage = message;
    }
  }

  reset(): void {
    this.products.clear();
    this.shouldFail = false;
    this.failureMessage = 'Mock API error';
  }

  async getProduct(barcode: string): Promise<ProductResponse> {
    if (this.shouldFail) {
      return {
        success: false,
        data: null,
        errorCode: 'MOCK_ERROR',
        message: this.failureMessage,
      };
    }

    const product = this.products.get(barcode);
    if (!product) {
      return {
        success: false,
        data: null,
        errorCode: 'NOT_FOUND',
        message: 'Product not found',
      };
    }

    return {
      success: true,
      data: product,
    };
  }

  async searchProducts(query: string, page: number, pageSize: number = 20): Promise<SearchResponse> {
    if (this.shouldFail) {
      return {
        success: false,
        data: { items: [], totalCount: 0, page, pageSize, hasMore: false },
        errorCode: 'MOCK_ERROR',
        message: this.failureMessage,
      };
    }

    const allProducts = Array.from(this.products.values());
    const filtered = allProducts.filter(
      (p) =>
        p.name.toLowerCase().includes(query.toLowerCase()) ||
        (p.brand && p.brand.toLowerCase().includes(query.toLowerCase()))
    );

    const start = (page - 1) * pageSize;
    const items = filtered.slice(start, start + pageSize);

    return {
      success: true,
      data: {
        items,
        totalCount: filtered.length,
        page,
        pageSize,
        hasMore: start + pageSize < filtered.length,
      },
    };
  }

  async contributeProduct(barcode: string, _data: ContributionData): Promise<ContributionResponse> {
    if (this.shouldFail) {
      return {
        success: false,
        data: null,
        errorCode: 'MOCK_ERROR',
        message: this.failureMessage,
      };
    }

    return {
      success: true,
      data: {
        contributionId: `contrib_${barcode}_${Date.now()}`,
        status: 'pending',
      },
    };
  }

  async processNutritionOcr(_imageBase64: string, _barcode: string): Promise<OcrResponse> {
    if (this.shouldFail) {
      return {
        success: false,
        data: null,
        errorCode: 'MOCK_ERROR',
        message: this.failureMessage,
      };
    }

    return {
      success: true,
      data: {
        nutritionInfo: {
          energyKcal: 200,
          fat: 10,
          sugars: 5,
        },
        confidence: 0.95,
        rawText: 'Mock OCR text',
      },
    };
  }

  async processIngredientsOcr(_imageBase64: string, _barcode: string): Promise<OcrResponse> {
    if (this.shouldFail) {
      return {
        success: false,
        data: null,
        errorCode: 'MOCK_ERROR',
        message: this.failureMessage,
      };
    }

    return {
      success: true,
      data: {
        ingredients: 'Water, Sugar, Salt',
        confidence: 0.90,
        rawText: 'Mock ingredients OCR',
      },
    };
  }
}
