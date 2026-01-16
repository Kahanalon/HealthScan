import React from 'react';
import { render, screen, fireEvent } from '@testing-library/react-native';
import ProductCard from '../../../src/presentation/components/ProductCard';

describe('ProductCard', () => {
  const defaultProps = {
    barcode: '1234567890123',
    name: 'Test Product',
    brand: 'Test Brand',
    imageUrl: 'https://example.com/image.jpg',
    grade: 'B' as const,
    onPress: jest.fn(),
  };

  beforeEach(() => {
    jest.clearAllMocks();
  });

  it('should render product name', () => {
    render(<ProductCard {...defaultProps} />);

    expect(screen.getByText('Test Product')).toBeTruthy();
  });

  it('should render brand when provided', () => {
    render(<ProductCard {...defaultProps} />);

    expect(screen.getByText('Test Brand')).toBeTruthy();
  });

  it('should not crash when brand is null', () => {
    render(<ProductCard {...defaultProps} brand={null} />);

    expect(screen.getByText('Test Product')).toBeTruthy();
    expect(screen.queryByText('Test Brand')).toBeNull();
  });

  it('should render grade badge', () => {
    render(<ProductCard {...defaultProps} />);

    expect(screen.getByText('B')).toBeTruthy();
  });

  it('should call onPress when pressed', () => {
    render(<ProductCard {...defaultProps} />);

    fireEvent.press(screen.getByText('Test Product'));

    expect(defaultProps.onPress).toHaveBeenCalledTimes(1);
  });

  it('should render horizontal layout when specified', () => {
    render(<ProductCard {...defaultProps} horizontal />);

    expect(screen.getByText('Test Product')).toBeTruthy();
  });

  it('should render placeholder when no image', () => {
    render(<ProductCard {...defaultProps} imageUrl={null} />);

    expect(screen.getByText('📦')).toBeTruthy();
  });

  it('should render all grades correctly', () => {
    const grades = ['A', 'B', 'C', 'D', 'E', 'Unknown'] as const;

    grades.forEach((grade) => {
      const { unmount } = render(<ProductCard {...defaultProps} grade={grade} />);
      expect(screen.getByText(grade)).toBeTruthy();
      unmount();
    });
  });
});
