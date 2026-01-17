import { createScanHistoryId } from '../../src/core/entities/ScanHistory';

describe('ScanHistory', () => {
  describe('createScanHistoryId', () => {
    it('should create unique id from barcode and timestamp', () => {
      const barcode = '1234567890123';
      const timestamp = new Date('2025-01-15T10:00:00Z');

      const id = createScanHistoryId(barcode, timestamp);

      expect(id).toBe('1234567890123_1736935200000');
    });

    it('should create different ids for same barcode at different times', () => {
      const barcode = '1234567890123';
      const timestamp1 = new Date('2025-01-15T10:00:00Z');
      const timestamp2 = new Date('2025-01-15T11:00:00Z');

      const id1 = createScanHistoryId(barcode, timestamp1);
      const id2 = createScanHistoryId(barcode, timestamp2);

      expect(id1).not.toBe(id2);
    });

    it('should create different ids for different barcodes at same time', () => {
      const barcode1 = '1234567890123';
      const barcode2 = '9876543210987';
      const timestamp = new Date('2025-01-15T10:00:00Z');

      const id1 = createScanHistoryId(barcode1, timestamp);
      const id2 = createScanHistoryId(barcode2, timestamp);

      expect(id1).not.toBe(id2);
    });

    it('should handle various barcode formats', () => {
      const ean13 = '1234567890123';
      const ean8 = '12345678';
      const upc = '012345678905';
      const timestamp = new Date('2025-01-15T10:00:00Z');

      expect(createScanHistoryId(ean13, timestamp)).toContain(ean13);
      expect(createScanHistoryId(ean8, timestamp)).toContain(ean8);
      expect(createScanHistoryId(upc, timestamp)).toContain(upc);
    });

    it('should create consistent id format', () => {
      const barcode = '1234567890123';
      const timestamp = new Date('2025-01-15T10:00:00Z');

      const id = createScanHistoryId(barcode, timestamp);

      expect(id).toMatch(/^\d+_\d+$/);
      const [barcodepart, timestampPart] = id.split('_');
      expect(barcodepart).toBe(barcode);
      expect(Number(timestampPart)).toBe(timestamp.getTime());
    });
  });
});
