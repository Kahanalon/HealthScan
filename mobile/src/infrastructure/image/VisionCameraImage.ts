import { Camera, PhotoFile } from 'react-native-vision-camera';
import { IImageService, CapturedImage, ImageCaptureOptions } from '../../core/interfaces/IImageService';

export class VisionCameraImage implements IImageService {
  private cameraRef: React.RefObject<Camera> | null = null;

  setCameraRef(ref: React.RefObject<Camera>): void {
    this.cameraRef = ref;
  }

  async captureImage(options?: ImageCaptureOptions): Promise<CapturedImage> {
    if (!this.cameraRef?.current) {
      throw new Error('Camera not initialized');
    }

    try {
      const photo: PhotoFile = await this.cameraRef.current.takePhoto({
        flash: options?.flash ?? 'off',
        enableShutterSound: false,
      });

      const base64 = await this.readFileAsBase64(photo.path);

      return {
        base64,
        width: photo.width,
        height: photo.height,
        uri: `file://${photo.path}`,
      };
    } catch (error) {
      const message = error instanceof Error ? error.message : 'Unknown error';
      throw new Error(`Failed to capture image: ${message}`);
    }
  }

  async pickFromGallery(): Promise<CapturedImage | null> {
    return null;
  }

  async resizeImage(
    image: CapturedImage,
    maxWidth: number,
    maxHeight: number
  ): Promise<CapturedImage> {
    return image;
  }

  private async readFileAsBase64(path: string): Promise<string> {
    const RNFS = require('react-native-fs');
    return await RNFS.readFile(path, 'base64');
  }
}

export const visionCameraImage = new VisionCameraImage();
