export type NutriScoreGrade = 'A' | 'B' | 'C' | 'D' | 'E' | 'Unknown';

export interface NutritionInfo {
  energyKcal: number | null;
  fat: number | null;
  saturatedFat: number | null;
  carbohydrates: number | null;
  sugars: number | null;
  fiber: number | null;
  protein: number | null;
  salt: number | null;
  sodium: number | null;
}

export interface NutrientFlag {
  nutrient: string;
  level: 'low' | 'moderate' | 'high';
  description: string;
}

export interface Product {
  barcode: string;
  name: string;
  brand: string | null;
  imageUrl: string | null;
  nutriScoreGrade: NutriScoreGrade;
  nutriScoreScore: number | null;
  nutritionPer100g: NutritionInfo;
  ingredients: string | null;
  allergens: string[];
  categories: string[];
  flags: NutrientFlag[];
  dataSource: 'openFoodFacts' | 'userContributed' | 'cached';
  lastUpdated: string;
}

export interface ContributionData {
  nutritionImageBase64?: string;
  ingredientsImageBase64?: string;
  productName?: string;
  brand?: string;
}

export interface OcrResult {
  nutritionInfo?: Partial<NutritionInfo>;
  ingredients?: string;
  confidence: number;
  rawText: string;
}
