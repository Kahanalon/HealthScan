import { create } from 'zustand';
import { ScoreResult } from '../core/entities/ScoreResult';

type ScanState = 'idle' | 'scanning' | 'loading' | 'success' | 'error' | 'not_found';

interface CurrentScan {
  barcode: string | null;
  result: ScoreResult | null;
  state: ScanState;
  errorMessage: string | null;
}

interface ScanStoreState {
  currentScan: CurrentScan;
  torchEnabled: boolean;
  scanBarcode: (barcode: string) => void;
  setScanResult: (result: ScoreResult) => void;
  setScanError: (message: string) => void;
  setScanNotFound: () => void;
  setScanning: () => void;
  setLoading: () => void;
  resetScan: () => void;
  toggleTorch: () => void;
}

const initialScan: CurrentScan = {
  barcode: null,
  result: null,
  state: 'idle',
  errorMessage: null,
};

export const useScanStore = create<ScanStoreState>((set) => ({
  currentScan: initialScan,
  torchEnabled: false,

  scanBarcode: (barcode) =>
    set({
      currentScan: {
        barcode,
        result: null,
        state: 'loading',
        errorMessage: null,
      },
    }),

  setScanResult: (result) =>
    set((state) => ({
      currentScan: {
        ...state.currentScan,
        result,
        state: 'success',
        errorMessage: null,
      },
    })),

  setScanError: (message) =>
    set((state) => ({
      currentScan: {
        ...state.currentScan,
        state: 'error',
        errorMessage: message,
      },
    })),

  setScanNotFound: () =>
    set((state) => ({
      currentScan: {
        ...state.currentScan,
        state: 'not_found',
        errorMessage: null,
      },
    })),

  setScanning: () =>
    set({
      currentScan: {
        ...initialScan,
        state: 'scanning',
      },
    }),

  setLoading: () =>
    set((state) => ({
      currentScan: {
        ...state.currentScan,
        state: 'loading',
      },
    })),

  resetScan: () => set({ currentScan: initialScan }),

  toggleTorch: () => set((state) => ({ torchEnabled: !state.torchEnabled })),
}));
