module.exports = {
  preset: 'react-native',
  setupFilesAfterEnv: ['<rootDir>/__tests__/setup.ts'],
  moduleFileExtensions: ['ts', 'tsx', 'js', 'jsx', 'json', 'node'],
  transformIgnorePatterns: [
    'node_modules/(?!(react-native|@react-native|@react-navigation|react-native-vision-camera|@nozbe/watermelondb|react-native-safe-area-context|react-native-screens|@react-native-async-storage/async-storage|@tanstack/react-query|zustand)/)',
  ],
  moduleNameMapper: {
    '^@core/(.*)$': '<rootDir>/src/core/$1',
    '^@infrastructure/(.*)$': '<rootDir>/src/infrastructure/$1',
    '^@application/(.*)$': '<rootDir>/src/application/$1',
    '^@presentation/(.*)$': '<rootDir>/src/presentation/$1',
    '^@stores/(.*)$': '<rootDir>/src/stores/$1',
    '^@i18n/(.*)$': '<rootDir>/src/i18n/$1',
    '^@di/(.*)$': '<rootDir>/src/di/$1',
  },
  testMatch: ['**/__tests__/**/*.test.ts?(x)'],
  testPathIgnorePatterns: ['/node_modules/'],
  collectCoverageFrom: [
    'src/**/*.{ts,tsx}',
    '!src/**/*.d.ts',
    '!src/**/index.ts',
  ],
  coverageThreshold: {
    global: {
      branches: 60,
      functions: 60,
      lines: 60,
      statements: 60,
    },
  },
  testEnvironment: 'node',
  globals: {
    __DEV__: true,
  },
  fakeTimers: {
    enableGlobally: false,
  },
};
