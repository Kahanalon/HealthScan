import { Product, ContributionData, OcrResult } from '../entities/Product';

export interface ProductResponse {
  success: boolean;
  data: Product | null;
  errorCode?: string;
  message?: string;
}

export interface SearchResponse {
  success: boolean;
  data: {
    items: Product[];
    totalCount: number;
    page: number;
    pageSize: number;
    hasMore: boolean;
  };
  errorCode?: string;
  message?: string;
}

export interface ContributionResponse {
  success: boolean;
  data: {
    contributionId: string;
    status: 'pending' | 'accepted' | 'rejected';
  } | null;
  errorCode?: string;
  message?: string;
}

export interface OcrResponse {
  success: boolean;
  data: OcrResult | null;
  errorCode?: string;
  message?: string;
}

export interface IApiClient {
  getProduct(barcode: string): Promise<ProductResponse>;
  searchProducts(query: string, page: number, pageSize?: number): Promise<SearchResponse>;
  contributeProduct(barcode: string, data: ContributionData): Promise<ContributionResponse>;
  processNutritionOcr(imageBase64: string, barcode: string): Promise<OcrResponse>;
  processIngredientsOcr(imageBase64: string, barcode: string): Promise<OcrResponse>;
}
