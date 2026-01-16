import { Camera, CameraDevice, useCameraDevice, useCodeScanner } from 'react-native-vision-camera';
import {
  IBarcodeScanner,
  ScannedBarcode,
  ScannerConfig,
  BarcodeFormat,
} from '../../core/interfaces/IBarcodeScanner';

type VisionCameraCodeType =
  | 'ean-13'
  | 'ean-8'
  | 'upc-a'
  | 'upc-e'
  | 'code-128'
  | 'code-39'
  | 'qr-code';

const formatMap: Record<string, BarcodeFormat> = {
  'ean-13': 'ean-13',
  'ean-8': 'ean-8',
  'upc-a': 'upc-a',
  'upc-e': 'upc-e',
  'code-128': 'code-128',
  'code-39': 'code-39',
  'qr-code': 'qr',
};

export class VisionCameraScanner implements IBarcodeScanner {
  private isScanning: boolean = false;
  private torchEnabled: boolean = false;
  private cameraPosition: 'front' | 'back' = 'back';
  private onScanCallback: ((barcode: ScannedBarcode) => void) | null = null;
  private config: ScannerConfig = {};

  async initialize(config?: ScannerConfig): Promise<void> {
    this.config = config ?? {};
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
    const status = await Camera.requestCameraPermission();
    return status === 'granted';
  }

  async hasPermissions(): Promise<boolean> {
    const status = await Camera.getCameraPermissionStatus();
    return status === 'granted';
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

  handleCodeScanned(codes: { type: string; value: string; corners?: { x: number; y: number }[] }[]): void {
    if (!this.isScanning || !this.onScanCallback || codes.length === 0) {
      return;
    }

    const code = codes[0];
    const format = formatMap[code.type] ?? 'unknown';

    this.onScanCallback({
      value: code.value,
      format,
      cornerPoints: code.corners,
    });
  }

  getSupportedCodeTypes(): VisionCameraCodeType[] {
    const defaultTypes: VisionCameraCodeType[] = ['ean-13', 'ean-8', 'upc-a', 'upc-e'];

    if (!this.config.formats) {
      return defaultTypes;
    }

    return this.config.formats
      .map((f) => {
        if (f === 'qr') return 'qr-code' as VisionCameraCodeType;
        return f as VisionCameraCodeType;
      })
      .filter((f): f is VisionCameraCodeType => f !== 'unknown');
  }
}

export const visionCameraScanner = new VisionCameraScanner();
