import {
  IBarcodeScanner,
  ScannedBarcode,
  ScannerConfig,
} from '../../src/core/interfaces/IBarcodeScanner';

export class MockBarcodeScanner implements IBarcodeScanner {
  private isScanning: boolean = false;
  private torchEnabled: boolean = false;
  private cameraPosition: 'front' | 'back' = 'back';
  private hasPermission: boolean = true;
  private onScanCallback: ((barcode: ScannedBarcode) => void) | null = null;

  setHasPermission(hasPermission: boolean): void {
    this.hasPermission = hasPermission;
  }

  simulateScan(barcode: string): void {
    if (this.isScanning && this.onScanCallback) {
      this.onScanCallback({
        value: barcode,
        format: 'ean-13',
      });
    }
  }

  reset(): void {
    this.isScanning = false;
    this.torchEnabled = false;
    this.cameraPosition = 'back';
    this.hasPermission = true;
    this.onScanCallback = null;
  }

  async initialize(config?: ScannerConfig): Promise<void> {
    this.torchEnabled = config?.enableTorch ?? false;
    this.cameraPosition = config?.cameraPosition ?? 'back';
  }

  startScanning(onScan: (barcode: ScannedBarcode) => void): void {
    this.isScanning = true;
    this.onScanCallback = onScan;
  }

  stopScanning(): void {
    this.isScanning = false;
    this.onScanCallback = null;
  }

  async toggleTorch(enabled: boolean): Promise<void> {
    this.torchEnabled = enabled;
  }

  async switchCamera(): Promise<void> {
    this.cameraPosition = this.cameraPosition === 'back' ? 'front' : 'back';
  }

  async requestPermissions(): Promise<boolean> {
    return this.hasPermission;
  }

  async hasPermissions(): Promise<boolean> {
    return this.hasPermission;
  }

  dispose(): void {
    this.stopScanning();
  }

  getIsScanning(): boolean {
    return this.isScanning;
  }

  getTorchEnabled(): boolean {
    return this.torchEnabled;
  }

  getCameraPosition(): 'front' | 'back' {
    return this.cameraPosition;
  }
}
