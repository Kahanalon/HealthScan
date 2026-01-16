import React from 'react';
import { render, screen } from '@testing-library/react-native';
import ScoreDisplay from '../../../src/presentation/components/ScoreDisplay';

jest.mock('react-i18next', () => ({
  useTranslation: () => ({
    t: (key: string) => {
      const translations: Record<string, string> = {
        'grades.A': 'Excellent nutritional quality',
        'grades.B': 'Good nutritional quality',
        'grades.C': 'Average nutritional quality',
        'grades.D': 'Poor nutritional quality',
        'grades.E': 'Bad nutritional quality',
        'grades.Unknown': 'Unknown',
        'result.nutriScore': 'Nutri-Score',
      };
      return translations[key] || key;
    },
  }),
}));

describe('ScoreDisplay', () => {
  it('should render grade A correctly', () => {
    render(<ScoreDisplay grade="A" score={-5} />);

    expect(screen.getByText('A')).toBeTruthy();
    expect(screen.getByText('Excellent nutritional quality')).toBeTruthy();
  });

  it('should render grade B correctly', () => {
    render(<ScoreDisplay grade="B" score={1} />);

    expect(screen.getByText('B')).toBeTruthy();
    expect(screen.getByText('Good nutritional quality')).toBeTruthy();
  });

  it('should render grade C correctly', () => {
    render(<ScoreDisplay grade="C" score={8} />);

    expect(screen.getByText('C')).toBeTruthy();
    expect(screen.getByText('Average nutritional quality')).toBeTruthy();
  });

  it('should render grade D correctly', () => {
    render(<ScoreDisplay grade="D" score={15} />);

    expect(screen.getByText('D')).toBeTruthy();
    expect(screen.getByText('Poor nutritional quality')).toBeTruthy();
  });

  it('should render grade E correctly', () => {
    render(<ScoreDisplay grade="E" score={25} />);

    expect(screen.getByText('E')).toBeTruthy();
    expect(screen.getByText('Bad nutritional quality')).toBeTruthy();
  });

  it('should render Unknown grade correctly', () => {
    render(<ScoreDisplay grade="Unknown" score={null} />);

    expect(screen.getAllByText('Unknown').length).toBeGreaterThanOrEqual(1);
  });

  it('should display score when provided', () => {
    render(<ScoreDisplay grade="B" score={3} />);

    expect(screen.getByText('Nutri-Score: 3')).toBeTruthy();
  });

  it('should not display score when null', () => {
    render(<ScoreDisplay grade="B" score={null} />);

    expect(screen.queryByText(/Nutri-Score/)).toBeNull();
  });
});
