import React from 'react';
import { render, screen } from '@testing-library/react-native';
import NutritionCard from '../../../src/presentation/components/NutritionCard';
import { createMockNutritionInfo } from '../../mocks/mockData';

jest.mock('react-i18next', () => ({
  useTranslation: () => ({
    t: (key: string) => {
      const translations: Record<string, string> = {
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

describe('NutritionCard', () => {
  it('should render all nutrition values', () => {
    const nutrition = createMockNutritionInfo();

    render(<NutritionCard nutrition={nutrition} />);

    expect(screen.getByText('Energy')).toBeTruthy();
    expect(screen.getByText('250 kcal')).toBeTruthy();
    expect(screen.getByText('Fat')).toBeTruthy();
    expect(screen.getByText('12g')).toBeTruthy();
    expect(screen.getByText('Saturated Fat')).toBeTruthy();
    expect(screen.getByText('4g')).toBeTruthy();
    expect(screen.getByText('Carbohydrates')).toBeTruthy();
    expect(screen.getByText('30g')).toBeTruthy();
    expect(screen.getByText('Sugars')).toBeTruthy();
    expect(screen.getByText('15g')).toBeTruthy();
    expect(screen.getByText('Fiber')).toBeTruthy();
    expect(screen.getByText('3g')).toBeTruthy();
    expect(screen.getByText('Protein')).toBeTruthy();
    expect(screen.getByText('8g')).toBeTruthy();
    expect(screen.getByText('Salt')).toBeTruthy();
    expect(screen.getByText('1.2g')).toBeTruthy();
  });

  it('should display dash for null values', () => {
    const nutrition = createMockNutritionInfo({
      fiber: null,
      energyKcal: null,
    });

    render(<NutritionCard nutrition={nutrition} />);

    const dashes = screen.getAllByText('-');
    expect(dashes.length).toBeGreaterThanOrEqual(2);
  });

  it('should display zero values correctly', () => {
    const nutrition = createMockNutritionInfo({
      sugars: 0,
      fat: 0,
    });

    render(<NutritionCard nutrition={nutrition} />);

    expect(screen.getAllByText('0g').length).toBeGreaterThanOrEqual(1);
  });

  it('should handle decimal values', () => {
    const nutrition = createMockNutritionInfo({
      salt: 0.5,
      fat: 2.3,
    });

    render(<NutritionCard nutrition={nutrition} />);

    expect(screen.getByText('0.5g')).toBeTruthy();
    expect(screen.getByText('2.3g')).toBeTruthy();
  });
});
