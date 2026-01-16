import React from 'react';
import { render, screen } from '@testing-library/react-native';
import FlagsList from '../../../src/presentation/components/FlagsList';
import { createMockFlag } from '../../mocks/mockData';

jest.mock('react-i18next', () => ({
  useTranslation: () => ({
    t: (key: string) => {
      const translations: Record<string, string> = {
        'flags.high': 'High',
        'flags.moderate': 'Moderate',
        'flags.low': 'Low',
      };
      return translations[key] || key;
    },
  }),
}));

describe('FlagsList', () => {
  it('should render nothing when flags array is empty', () => {
    const { toJSON } = render(<FlagsList flags={[]} />);

    expect(toJSON()).toBeNull();
  });

  it('should render single flag correctly', () => {
    const flags = [createMockFlag({ nutrient: 'Sugars', level: 'high' })];

    render(<FlagsList flags={flags} />);

    expect(screen.getByText('Sugars')).toBeTruthy();
    expect(screen.getByText('High')).toBeTruthy();
  });

  it('should render multiple flags', () => {
    const flags = [
      createMockFlag({ nutrient: 'Sugars', level: 'high' }),
      createMockFlag({ nutrient: 'Saturated Fat', level: 'moderate' }),
      createMockFlag({ nutrient: 'Fiber', level: 'low' }),
    ];

    render(<FlagsList flags={flags} />);

    expect(screen.getByText('Sugars')).toBeTruthy();
    expect(screen.getByText('Saturated Fat')).toBeTruthy();
    expect(screen.getByText('Fiber')).toBeTruthy();
    expect(screen.getByText('High')).toBeTruthy();
    expect(screen.getByText('Moderate')).toBeTruthy();
    expect(screen.getByText('Low')).toBeTruthy();
  });

  it('should render flag description when provided', () => {
    const flags = [
      createMockFlag({
        nutrient: 'Sugars',
        level: 'high',
        description: 'Contains high sugar content',
      }),
    ];

    render(<FlagsList flags={flags} />);

    expect(screen.getByText('Contains high sugar content')).toBeTruthy();
  });

  it('should handle flags without description', () => {
    const flags = [
      createMockFlag({
        nutrient: 'Sugars',
        level: 'high',
        description: '',
      }),
    ];

    render(<FlagsList flags={flags} />);

    expect(screen.getByText('Sugars')).toBeTruthy();
    expect(screen.queryByText('')).toBeNull();
  });

  it('should render all level types', () => {
    const levels = ['high', 'moderate', 'low'] as const;

    levels.forEach((level) => {
      const flags = [createMockFlag({ level })];
      const { unmount } = render(<FlagsList flags={flags} />);

      const expectedText = level.charAt(0).toUpperCase() + level.slice(1);
      expect(screen.getByText(expectedText)).toBeTruthy();

      unmount();
    });
  });
});
