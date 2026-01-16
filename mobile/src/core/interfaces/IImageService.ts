export interface CapturedImage {
  base64: string;
  width: number;
  height: number;
  uri: string;
}

export interface ImageCaptureOptions {
  quality?: number;
  maxWidth?: number;
  maxHeight?: number;
  flash?: 'on' | 'off' | 'auto';
}

export interface IImageService {
  captureImage(options?: ImageCaptureOptions): Promise<CapturedImage>;
  pickFromGallery(): Promise<CapturedImage | null>;
  resizeImage(image: CapturedImage, maxWidth: number, maxHeight: number): Promise<CapturedImage>;
}
