jest.mock('../../../src/i18n', () => ({
  setLanguage: jest.fn(),
  isRTL: jest.fn(() => false),
  default: {},
}));

import React from 'react';
import { screen, fireEvent, waitFor } from '@testing-library/react-native';
import HomeScreen from '../../../src/presentation/screens/HomeScreen';
import { renderWithProviders, createTestContainer } from '../../utils/testUtils';
import { MockCacheRepository } from '../../mocks/MockCacheRepository';
import { MOCK_SCAN_HISTORY } from '../../mocks/mockData';

const mockNavigate = jest.fn();
jest.mock('@react-navigation/native', () => ({
  ...jest.requireActual('@react-navigation/native'),
  useNavigation: () => ({
    navigate: mockNavigate,
    goBack: jest.fn(),
  }),
}));

jest.mock('react-i18next', () => ({
  useTranslation: () => ({
    t: (key: string) => {
      const translations: Record<string, string> = {
        'home.title': 'HealthScan',
        'home.subtitle': 'Scan food products to check nutritional values',
        'home.scanButton': 'Scan Barcode',
        'home.searchPlaceholder': 'Search for a product...',
        'home.recentScans': 'Recent Scans',
        'home.noRecentScans': 'No recent scans',
        'home.viewAll': 'View All',
        'common.loading': 'Loading...',
      };
      return translations[key] || key;
    },
  }),
}));

describe('HomeScreen', () => {
  let mockCacheRepository: MockCacheRepository;

  beforeEach(() => {
    jest.clearAllMocks();
    mockCacheRepository = new MockCacheRepository();
  });

  afterEach(() => {
    mockCacheRepository.reset();
  });

  it('should render title and subtitle', async () => {
    const container = createTestContainer({ cacheRepository: mockCacheRepository });
    renderWithProviders(<HomeScreen />, { container });

    expect(screen.getByText('HealthScan')).toBeTruthy();
    expect(screen.getByText('Scan food products to check nutritional values')).toBeTruthy();
  });

  it('should render scan button', async () => {
    const container = createTestContainer({ cacheRepository: mockCacheRepository });
    renderWithProviders(<HomeScreen />, { container });

    expect(screen.getByText('Scan Barcode')).toBeTruthy();
  });

  it('should navigate to Scanner when scan button pressed', async () => {
    const container = createTestContainer({ cacheRepository: mockCacheRepository });
    renderWithProviders(<HomeScreen />, { container });

    fireEvent.press(screen.getByText('Scan Barcode'));

    expect(mockNavigate).toHaveBeenCalledWith('Scanner');
  });

  it('should render search input', async () => {
    const container = createTestContainer({ cacheRepository: mockCacheRepository });
    renderWithProviders(<HomeScreen />, { container });

    expect(screen.getByPlaceholderText('Search for a product...')).toBeTruthy();
  });

  it('should navigate to Search when search submitted', async () => {
    const container = createTestContainer({ cacheRepository: mockCacheRepository });
    renderWithProviders(<HomeScreen />, { container });

    const searchInput = screen.getByPlaceholderText('Search for a product...');
    fireEvent.changeText(searchInput, 'test product');
    fireEvent(searchInput, 'submitEditing');

    expect(mockNavigate).toHaveBeenCalledWith('Search', { query: 'test product' });
  });

  it('should not navigate to Search for short queries', async () => {
    const container = createTestContainer({ cacheRepository: mockCacheRepository });
    renderWithProviders(<HomeScreen />, { container });

    const searchInput = screen.getByPlaceholderText('Search for a product...');
    fireEvent.changeText(searchInput, 'a');
    fireEvent(searchInput, 'submitEditing');

    expect(mockNavigate).not.toHaveBeenCalledWith('Search', expect.anything());
  });

  it('should display recent scans section', async () => {
    const container = createTestContainer({ cacheRepository: mockCacheRepository });
    renderWithProviders(<HomeScreen />, { container });

    expect(screen.getByText('Recent Scans')).toBeTruthy();
  });

  it('should display no recent scans message when history is empty', async () => {
    const container = createTestContainer({ cacheRepository: mockCacheRepository });
    renderWithProviders(<HomeScreen />, { container });

    await waitFor(() => {
      expect(screen.getByText('No recent scans')).toBeTruthy();
    });
  });

  it('should display recent scans when history exists', async () => {
    mockCacheRepository.setHistory(MOCK_SCAN_HISTORY);
    const container = createTestContainer({ cacheRepository: mockCacheRepository });
    renderWithProviders(<HomeScreen />, { container });

    await waitFor(() => {
      expect(screen.getByText('Organic Salad')).toBeTruthy();
    });
  });

  it('should navigate to History when View All pressed', async () => {
    mockCacheRepository.setHistory(MOCK_SCAN_HISTORY);
    const container = createTestContainer({ cacheRepository: mockCacheRepository });
    renderWithProviders(<HomeScreen />, { container });

    await waitFor(() => {
      expect(screen.getByText('View All')).toBeTruthy();
    });

    fireEvent.press(screen.getByText('View All'));

    expect(mockNavigate).toHaveBeenCalledWith('History');
  });

  it('should navigate to Result when product card pressed', async () => {
    mockCacheRepository.setHistory(MOCK_SCAN_HISTORY);
    const container = createTestContainer({ cacheRepository: mockCacheRepository });
    renderWithProviders(<HomeScreen />, { container });

    await waitFor(() => {
      expect(screen.getByText('Organic Salad')).toBeTruthy();
    });

    fireEvent.press(screen.getByText('Organic Salad'));

    expect(mockNavigate).toHaveBeenCalledWith('Result', { barcode: '1111111111111' });
  });
});
