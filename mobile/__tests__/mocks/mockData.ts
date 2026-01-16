import { Product, NutriScoreGrade, NutritionInfo, NutrientFlag } from '../../src/core/entities/Product';
import { ScanHistory } from '../../src/core/entities/ScanHistory';
import { ScoreResult } from '../../src/core/entities/ScoreResult';

export function createMockNutritionInfo(overrides: Partial<NutritionInfo> = {}): NutritionInfo {
  return {
    energyKcal: 250,
    fat: 12,
    saturatedFat: 4,
    carbohydrates: 30,
    sugars: 15,
    fiber: 3,
    protein: 8,
    salt: 1.2,
    sodium: 480,
    ...overrides,
  };
}

export function createMockFlag(overrides: Partial<NutrientFlag> = {}): NutrientFlag {
  return {
    nutrient: 'Sugars',
    level: 'high',
    description: 'High sugar content',
    ...overrides,
  };
}

export function createMockProduct(overrides: Partial<Product> = {}): Product {
  return {
    barcode: '1234567890123',
    name: 'Test Product',
    brand: 'Test Brand',
    imageUrl: 'https://example.com/image.jpg',
    nutriScoreGrade: 'B' as NutriScoreGrade,
    nutriScoreScore: 3,
    nutritionPer100g: createMockNutritionInfo(),
    ingredients: 'Water, Sugar, Salt, Flour',
    allergens: ['gluten', 'milk'],
    categories: ['snacks', 'biscuits'],
    flags: [createMockFlag()],
    dataSource: 'openFoodFacts',
    lastUpdated: '2025-01-15T10:00:00Z',
    ...overrides,
  };
}

export function createMockScanHistory(overrides: Partial<ScanHistory> = {}): ScanHistory {
  return {
    id: 'scan_123',
    barcode: '1234567890123',
    productName: 'Test Product',
    brand: 'Test Brand',
    imageUrl: 'https://example.com/image.jpg',
    grade: 'B' as NutriScoreGrade,
    scannedAt: new Date('2025-01-15T10:00:00Z'),
    ...overrides,
  };
}

export function createMockScoreResult(overrides: Partial<ScoreResult> = {}): ScoreResult {
  return {
    barcode: '1234567890123',
    productName: 'Test Product',
    brand: 'Test Brand',
    imageUrl: 'https://example.com/image.jpg',
    grade: 'B' as NutriScoreGrade,
    score: 3,
    flags: [createMockFlag()],
    nutritionPer100g: createMockNutritionInfo(),
    isFromCache: false,
    disclaimer: 'Test disclaimer',
    ...overrides,
  };
}

export const MOCK_PRODUCTS = {
  healthy: createMockProduct({
    barcode: '1111111111111',
    name: 'Organic Salad',
    brand: 'Healthy Foods',
    nutriScoreGrade: 'A',
    nutriScoreScore: -5,
    flags: [],
  }),
  moderate: createMockProduct({
    barcode: '2222222222222',
    name: 'Whole Wheat Bread',
    brand: 'Bakery Plus',
    nutriScoreGrade: 'B',
    nutriScoreScore: 1,
  }),
  unhealthy: createMockProduct({
    barcode: '3333333333333',
    name: 'Chocolate Candy Bar',
    brand: 'Sweet Treats',
    nutriScoreGrade: 'E',
    nutriScoreScore: 25,
    flags: [
      createMockFlag({ nutrient: 'Sugars', level: 'high' }),
      createMockFlag({ nutrient: 'Saturated Fat', level: 'high' }),
    ],
  }),
};

export const MOCK_SCAN_HISTORY = [
  createMockScanHistory({
    id: 'scan_1',
    barcode: '1111111111111',
    productName: 'Organic Salad',
    grade: 'A',
    scannedAt: new Date('2025-01-15T10:00:00Z'),
  }),
  createMockScanHistory({
    id: 'scan_2',
    barcode: '2222222222222',
    productName: 'Whole Wheat Bread',
    grade: 'B',
    scannedAt: new Date('2025-01-14T15:30:00Z'),
  }),
  createMockScanHistory({
    id: 'scan_3',
    barcode: '3333333333333',
    productName: 'Chocolate Candy Bar',
    grade: 'E',
    scannedAt: new Date('2025-01-13T09:15:00Z'),
  }),
];
