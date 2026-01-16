import React, { useEffect } from 'react';
import { StatusBar, LogBox } from 'react-native';
import { SafeAreaProvider } from 'react-native-safe-area-context';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { DIProvider } from './di/container';
import { useAppStore } from './stores/useAppStore';
import { setLanguage } from './i18n';
import AppNavigator from './presentation/navigation/AppNavigator';

LogBox.ignoreLogs(['Require cycle:']);

const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      retry: 2,
      staleTime: 5 * 60 * 1000,
      gcTime: 30 * 60 * 1000,
    },
    mutations: {
      retry: 1,
    },
  },
});

function AppContent() {
  const locale = useAppStore((state) => state.locale);

  useEffect(() => {
    setLanguage(locale);
  }, [locale]);

  return (
    <>
      <StatusBar barStyle="dark-content" backgroundColor="#FFFFFF" />
      <AppNavigator />
    </>
  );
}

export default function App() {
  return (
    <SafeAreaProvider>
      <QueryClientProvider client={queryClient}>
        <DIProvider>
          <AppContent />
        </DIProvider>
      </QueryClientProvider>
    </SafeAreaProvider>
  );
}
