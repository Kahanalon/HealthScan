import { useScanStore } from '../../src/stores/useScanStore';
import { createMockScoreResult } from '../mocks/mockData';

describe('useScanStore', () => {
  beforeEach(() => {
    useScanStore.setState({
      currentScan: {
        barcode: null,
        result: null,
        state: 'idle',
        errorMessage: null,
      },
      torchEnabled: false,
    });
  });

  describe('initial state', () => {
    it('should have idle state initially', () => {
      const state = useScanStore.getState();

      expect(state.currentScan.state).toBe('idle');
      expect(state.currentScan.barcode).toBeNull();
      expect(state.currentScan.result).toBeNull();
      expect(state.currentScan.errorMessage).toBeNull();
    });

    it('should have torch disabled initially', () => {
      const state = useScanStore.getState();

      expect(state.torchEnabled).toBe(false);
    });
  });

  describe('scanBarcode', () => {
    it('should set barcode and loading state', () => {
      useScanStore.getState().scanBarcode('1234567890123');

      const state = useScanStore.getState();
      expect(state.currentScan.barcode).toBe('1234567890123');
      expect(state.currentScan.state).toBe('loading');
    });

    it('should clear previous result and error', () => {
      useScanStore.getState().setScanError('Previous error');
      useScanStore.getState().scanBarcode('1234567890123');

      const state = useScanStore.getState();
      expect(state.currentScan.result).toBeNull();
      expect(state.currentScan.errorMessage).toBeNull();
    });
  });

  describe('setScanResult', () => {
    it('should set result and success state', () => {
      const result = createMockScoreResult();
      useScanStore.getState().scanBarcode('1234567890123');
      useScanStore.getState().setScanResult(result);

      const state = useScanStore.getState();
      expect(state.currentScan.result).toEqual(result);
      expect(state.currentScan.state).toBe('success');
    });

    it('should clear error message', () => {
      useScanStore.getState().setScanError('Some error');
      useScanStore.getState().setScanResult(createMockScoreResult());

      const state = useScanStore.getState();
      expect(state.currentScan.errorMessage).toBeNull();
    });
  });

  describe('setScanError', () => {
    it('should set error message and error state', () => {
      useScanStore.getState().scanBarcode('1234567890123');
      useScanStore.getState().setScanError('Network error');

      const state = useScanStore.getState();
      expect(state.currentScan.errorMessage).toBe('Network error');
      expect(state.currentScan.state).toBe('error');
    });
  });

  describe('setScanNotFound', () => {
    it('should set not_found state', () => {
      useScanStore.getState().scanBarcode('1234567890123');
      useScanStore.getState().setScanNotFound();

      const state = useScanStore.getState();
      expect(state.currentScan.state).toBe('not_found');
    });
  });

  describe('setScanning', () => {
    it('should set scanning state and reset current scan', () => {
      useScanStore.getState().setScanResult(createMockScoreResult());
      useScanStore.getState().setScanning();

      const state = useScanStore.getState();
      expect(state.currentScan.state).toBe('scanning');
      expect(state.currentScan.barcode).toBeNull();
      expect(state.currentScan.result).toBeNull();
    });
  });

  describe('setLoading', () => {
    it('should set loading state', () => {
      useScanStore.getState().setLoading();

      const state = useScanStore.getState();
      expect(state.currentScan.state).toBe('loading');
    });
  });

  describe('resetScan', () => {
    it('should reset to initial state', () => {
      useScanStore.getState().scanBarcode('1234567890123');
      useScanStore.getState().setScanResult(createMockScoreResult());
      useScanStore.getState().resetScan();

      const state = useScanStore.getState();
      expect(state.currentScan.state).toBe('idle');
      expect(state.currentScan.barcode).toBeNull();
      expect(state.currentScan.result).toBeNull();
      expect(state.currentScan.errorMessage).toBeNull();
    });
  });

  describe('toggleTorch', () => {
    it('should toggle torch on', () => {
      useScanStore.getState().toggleTorch();

      expect(useScanStore.getState().torchEnabled).toBe(true);
    });

    it('should toggle torch off', () => {
      useScanStore.getState().toggleTorch();
      useScanStore.getState().toggleTorch();

      expect(useScanStore.getState().torchEnabled).toBe(false);
    });
  });
});
