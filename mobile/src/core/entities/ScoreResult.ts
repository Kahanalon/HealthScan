import { NutriScoreGrade, NutrientFlag, NutritionInfo } from './Product';

export type { NutriScoreGrade };

export interface ScoreResult {
  barcode: string;
  productName: string;
  brand: string | null;
  imageUrl: string | null;
  grade: NutriScoreGrade;
  score: number | null;
  flags: NutrientFlag[];
  nutritionPer100g: NutritionInfo;
  isFromCache: boolean;
  disclaimer: string;
}

export function getGradeColor(grade: NutriScoreGrade): string {
  const colors: Record<NutriScoreGrade, string> = {
    A: '#038141',
    B: '#85BB2F',
    C: '#FECB02',
    D: '#EE8100',
    E: '#E63E11',
    Unknown: '#808080',
  };
  return colors[grade];
}

export function getGradeDescription(grade: NutriScoreGrade, locale: 'he' | 'en'): string {
  const descriptions: Record<NutriScoreGrade, { he: string; en: string }> = {
    A: { he: 'איכות תזונתית מעולה', en: 'Excellent nutritional quality' },
    B: { he: 'איכות תזונתית טובה', en: 'Good nutritional quality' },
    C: { he: 'איכות תזונתית בינונית', en: 'Average nutritional quality' },
    D: { he: 'איכות תזונתית נמוכה', en: 'Poor nutritional quality' },
    E: { he: 'איכות תזונתית נמוכה מאוד', en: 'Bad nutritional quality' },
    Unknown: { he: 'לא ידוע', en: 'Unknown' },
  };
  return descriptions[grade][locale];
}
