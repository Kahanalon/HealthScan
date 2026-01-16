import { create } from 'zustand';
import { persist, createJSONStorage } from 'zustand/middleware';
import AsyncStorage from '@react-native-async-storage/async-storage';

export type Locale = 'he' | 'en';

interface AppState {
  locale: Locale;
  isOnline: boolean;
  hasSeenOnboarding: boolean;
  setLocale: (locale: Locale) => void;
  setIsOnline: (isOnline: boolean) => void;
  setHasSeenOnboarding: (seen: boolean) => void;
}

export const useAppStore = create<AppState>()(
  persist(
    (set) => ({
      locale: 'he',
      isOnline: true,
      hasSeenOnboarding: false,
      setLocale: (locale) => set({ locale }),
      setIsOnline: (isOnline) => set({ isOnline }),
      setHasSeenOnboarding: (hasSeenOnboarding) => set({ hasSeenOnboarding }),
    }),
    {
      name: 'healthscan-app-storage',
      storage: createJSONStorage(() => AsyncStorage),
      partialize: (state) => ({
        locale: state.locale,
        hasSeenOnboarding: state.hasSeenOnboarding,
      }),
    }
  )
);
