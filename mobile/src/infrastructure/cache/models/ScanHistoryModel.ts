import { Model } from '@nozbe/watermelondb';
import { field, text } from '@nozbe/watermelondb/decorators';
import { ScanHistory } from '../../../core/entities/ScanHistory';
import { NutriScoreGrade } from '../../../core/entities/Product';

export default class ScanHistoryModel extends Model {
  static table = 'scan_history';

  @text('barcode') barcode!: string;
  @text('product_name') productName!: string;
  @text('brand') brand!: string | null;
  @text('image_url') imageUrl!: string | null;
  @text('grade') grade!: string;
  @field('scanned_at') scannedAt!: number;

  toScanHistory(): ScanHistory {
    return {
      id: this.id,
      barcode: this.barcode,
      productName: this.productName,
      brand: this.brand,
      imageUrl: this.imageUrl,
      grade: this.grade as NutriScoreGrade,
      scannedAt: new Date(this.scannedAt),
    };
  }
}
