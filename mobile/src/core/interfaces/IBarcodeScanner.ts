export type BarcodeFormat =
  | 'ean-13'
  | 'ean-8'
  | 'upc-a'
  | 'upc-e'
  | 'code-128'
  | 'code-39'
  | 'qr'
  | 'unknown';

export interface ScannedBarcode {
  value: string;
  format: BarcodeFormat;
  cornerPoints?: { x: number; y: number }[];
}

export interface ScannerConfig {
  formats?: BarcodeFormat[];
  enableTorch?: boolean;
  cameraPosition?: 'front' | 'back';
}

export interface IBarcodeScanner {
  initialize(config?: ScannerConfig): Promise<void>;
  startScanning(onScan: (barcode: ScannedBarcode) => void): void;
  stopScanning(): void;
  toggleTorch(enabled: boolean): Promise<void>;
  switchCamera(): Promise<void>;
  requestPermissions(): Promise<boolean>;
  hasPermissions(): Promise<boolean>;
  dispose(): void;
}
