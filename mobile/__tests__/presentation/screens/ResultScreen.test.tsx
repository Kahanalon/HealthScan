import React from 'react';
import { render, screen, fireEvent, waitFor } from '@testing-library/react-native';
import ResultScreen from '../../../src/presentation/screens/ResultScreen';
import { renderWithProviders, createTestContainer } from '../../utils/testUtils';
import { MockApiClient } from '../../mocks/MockApiClient';
import { MockCacheRepository } from '../../mocks/MockCacheRepository';
import { createMockProduct } from '../../mocks/mockData';

const mockNavigate = jest.fn();
const mockGoBack = jest.fn();

jest.mock('@react-navigation/native', () => ({
  ...jest.requireActual('@react-navigation/native'),
  useNavigation: () => ({
    navigate: mockNavigate,
    goBack: mockGoBack,
  }),
  useRoute: () => ({
    params: { barcode: '1234567890123' },
  }),
}));

jest.mock('react-i18next', () => ({
  useTranslation: () => ({
    t: (key: string) => {
      const translations: Record<string, string> = {
        'result.title': 'Results',
        'result.nutriScore': 'Nutri-Score',
        'result.nutritionFacts': 'Nutrition Facts per 100g',
        'result.flags': 'Warnings',
        'result.notFound': 'Product not found',
        'result.notFoundDescription': 'Would you like to contribute information about this product?',
        'result.contribute': 'Contribute Info',
        'result.cachedData': 'Cached data',
        'common.loading': 'Loading...',
        'common.error': 'Error',
        'common.retry': 'Retry',
        'grades.A': 'Excellent nutritional quality',
        'grades.B': 'Good nutritional quality',
        'grades.C': 'Average nutritional quality',
        'grades.D': 'Poor nutritional quality',
        'grades.E': 'Bad nutritional quality',
        'flags.high': 'High',
        'flags.moderate': 'Moderate',
        'flags.low': 'Low',
        'result.energy': 'Energy',
        'result.fat': 'Fat',
        'result.saturatedFat': 'Saturated Fat',
        'result.carbohydrates': 'Carbohydrates',
        'result.sugars': 'Sugars',
        'result.fiber': 'Fiber',
        'result.protein': 'Protein',
        'result.salt': 'Salt',
      };
      return translations[key] || key;
    },
  }),
}));

describe('ResultScreen', () => {
  let mockApiClient: MockApiClient;
  let mockCacheRepository: MockCacheRepository;

  beforeEach(() => {
    jest.clearAllMocks();
    mockApiClient = new MockApiClient();
    mockCacheRepository = new MockCacheRepository();
  });

  afterEach(() => {
    mockApiClient.reset();
    mockCacheRepository.reset();
  });

  it('should show loading state initially', async () => {
    const product = createMockProduct({ barcode: '1234567890123' });
    mockApiClient.setProduct('1234567890123', product);

    const container = createTestContainer({
      apiClient: mockApiClient,
      cacheRepository: mockCacheRepository,
    });

    renderWithProviders(<ResultScreen />, { container });

    expect(screen.getByText('Loading...')).toBeTruthy();
  });

  it('should display product information when loaded', async () => {
    const product = createMockProduct({
      barcode: '1234567890123',
      name: 'Test Product',
      brand: 'Test Brand',
    });
    mockApiClient.setProduct('1234567890123', product);

    const container = createTestContainer({
      apiClient: mockApiClient,
      cacheRepository: mockCacheRepository,
    });

    renderWithProviders(<ResultScreen />, { container });

    await waitFor(() => {
      expect(screen.getByText('Test Product')).toBeTruthy();
    });

    expect(screen.getByText('Test Brand')).toBeTruthy();
  });

  it('should display nutri-score grade', async () => {
    const product = createMockProduct({
      barcode: '1234567890123',
      nutriScoreGrade: 'A',
    });
    mockApiClient.setProduct('1234567890123', product);

    const container = createTestContainer({
      apiClient: mockApiClient,
      cacheRepository: mockCacheRepository,
    });

    renderWithProviders(<ResultScreen />, { container });

    await waitFor(() => {
      expect(screen.getByText('A')).toBeTruthy();
    });

    expect(screen.getByText('Excellent nutritional quality')).toBeTruthy();
  });

  it('should display nutrition information', async () => {
    const product = createMockProduct({ barcode: '1234567890123' });
    mockApiClient.setProduct('1234567890123', product);

    const container = createTestContainer({
      apiClient: mockApiClient,
      cacheRepository: mockCacheRepository,
    });

    renderWithProviders(<ResultScreen />, { container });

    await waitFor(() => {
      expect(screen.getByText('Nutrition Facts per 100g')).toBeTruthy();
    });

    expect(screen.getByText('Energy')).toBeTruthy();
    expect(screen.getByText('Fat')).toBeTruthy();
    expect(screen.getByText('Protein')).toBeTruthy();
  });

  it('should display warnings when product has flags', async () => {
    const product = createMockProduct({ barcode: '1234567890123' });
    mockApiClient.setProduct('1234567890123', product);

    const container = createTestContainer({
      apiClient: mockApiClient,
      cacheRepository: mockCacheRepository,
    });

    renderWithProviders(<ResultScreen />, { container });

    await waitFor(() => {
      expect(screen.getByText('Warnings')).toBeTruthy();
    });

    expect(screen.getAllByText('Sugars').length).toBeGreaterThanOrEqual(1);
    expect(screen.getAllByText('High').length).toBeGreaterThanOrEqual(1);
  });

  it('should show not found screen when product not available', async () => {
    const container = createTestContainer({
      apiClient: mockApiClient,
      cacheRepository: mockCacheRepository,
    });

    renderWithProviders(<ResultScreen />, { container });

    await waitFor(() => {
      expect(screen.getByText('Product not found')).toBeTruthy();
    });

    expect(screen.getByText('Would you like to contribute information about this product?')).toBeTruthy();
    expect(screen.getByText('Contribute Info')).toBeTruthy();
  });

  it('should navigate to Contribute screen when contribute button pressed', async () => {
    const container = createTestContainer({
      apiClient: mockApiClient,
      cacheRepository: mockCacheRepository,
    });

    renderWithProviders(<ResultScreen />, { container });

    await waitFor(() => {
      expect(screen.getByText('Contribute Info')).toBeTruthy();
    });

    fireEvent.press(screen.getByText('Contribute Info'));

    expect(mockNavigate).toHaveBeenCalledWith('Contribute', { barcode: '1234567890123' });
  });

  it('should go back when back button pressed', async () => {
    const product = createMockProduct({ barcode: '1234567890123' });
    mockApiClient.setProduct('1234567890123', product);

    const container = createTestContainer({
      apiClient: mockApiClient,
      cacheRepository: mockCacheRepository,
    });

    renderWithProviders(<ResultScreen />, { container });

    await waitFor(() => {
      expect(screen.getByText('Test Product')).toBeTruthy();
    });

    fireEvent.press(screen.getByText('←'));

    expect(mockGoBack).toHaveBeenCalled();
  });

  it('should show cached indicator for cached products', async () => {
    const cachedProduct = {
      ...createMockProduct({ barcode: '1234567890123' }),
      cachedAt: new Date(),
      expiresAt: new Date(Date.now() + 3600000),
    };
    mockCacheRepository.setProducts([cachedProduct]);

    const container = createTestContainer({
      apiClient: mockApiClient,
      cacheRepository: mockCacheRepository,
    });

    renderWithProviders(<ResultScreen />, { container });

    await waitFor(() => {
      expect(screen.getByText('Cached data')).toBeTruthy();
    });
  });
});
