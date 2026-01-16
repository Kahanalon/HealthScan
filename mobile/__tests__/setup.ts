jest.mock('@react-native-async-storage/async-storage', () => ({
  setItem: jest.fn(() => Promise.resolve()),
  getItem: jest.fn(() => Promise.resolve(null)),
  removeItem: jest.fn(() => Promise.resolve()),
  clear: jest.fn(() => Promise.resolve()),
  getAllKeys: jest.fn(() => Promise.resolve([])),
  multiGet: jest.fn(() => Promise.resolve([])),
  multiSet: jest.fn(() => Promise.resolve()),
  multiRemove: jest.fn(() => Promise.resolve()),
}));

jest.mock('react-native-vision-camera', () => ({
  Camera: jest.fn(() => null),
  useCameraDevice: jest.fn(() => ({ id: 'back' })),
  useCodeScanner: jest.fn(() => ({})),
}));

jest.mock('@nozbe/watermelondb', () => ({
  Database: jest.fn(),
  Model: class MockModel {},
  Q: {
    where: jest.fn(),
    sortBy: jest.fn(),
    desc: 'desc',
    take: jest.fn(),
    gt: jest.fn(),
    lt: jest.fn(),
  },
  appSchema: jest.fn(() => ({})),
  tableSchema: jest.fn(() => ({})),
}));

jest.mock('@nozbe/watermelondb/adapters/sqlite', () => jest.fn());

jest.mock('@nozbe/watermelondb/decorators', () => ({
  field: jest.fn(() => jest.fn()),
  text: jest.fn(() => jest.fn()),
  json: jest.fn(() => jest.fn()),
}));

jest.mock('react-native-safe-area-context', () => ({
  SafeAreaProvider: ({ children }: { children: React.ReactNode }) => children,
  SafeAreaView: ({ children }: { children: React.ReactNode }) => children,
  useSafeAreaInsets: () => ({ top: 0, bottom: 0, left: 0, right: 0 }),
}));

jest.mock('@react-navigation/native', () => {
  const actual = jest.requireActual('@react-navigation/native');
  return {
    ...actual,
    useNavigation: () => ({
      navigate: jest.fn(),
      goBack: jest.fn(),
      replace: jest.fn(),
    }),
    useRoute: () => ({
      params: {},
    }),
    NavigationContainer: ({ children }: { children: React.ReactNode }) => children,
  };
});

jest.mock('@react-navigation/native-stack', () => ({
  createNativeStackNavigator: () => ({
    Navigator: ({ children }: { children: React.ReactNode }) => children,
    Screen: () => null,
  }),
}));

jest.mock('react-i18next', () => ({
  useTranslation: () => ({
    t: (key: string) => key,
    i18n: {
      language: 'en',
      changeLanguage: jest.fn(),
    },
  }),
  initReactI18next: {
    type: '3rdParty',
    init: jest.fn(),
  },
}));

global.__DEV__ = true;
