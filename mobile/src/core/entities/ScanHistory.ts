import { NutriScoreGrade } from './Product';

export interface ScanHistory {
  id: string;
  barcode: string;
  productName: string;
  brand: string | null;
  imageUrl: string | null;
  grade: NutriScoreGrade;
  scannedAt: Date;
}

export function createScanHistoryId(barcode: string, timestamp: Date): string {
  return `${barcode}_${timestamp.getTime()}`;
}
