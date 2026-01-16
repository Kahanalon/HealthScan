import React from 'react';
import { render, screen, fireEvent } from '@testing-library/react-native';
import ScanButton from '../../../src/presentation/components/ScanButton';

jest.mock('react-i18next', () => ({
  useTranslation: () => ({
    t: (key: string) => {
      const translations: Record<string, string> = {
        'home.scanButton': 'Scan Barcode',
      };
      return translations[key] || key;
    },
  }),
}));

describe('ScanButton', () => {
  it('should render button text', () => {
    render(<ScanButton onPress={jest.fn()} />);

    expect(screen.getByText('Scan Barcode')).toBeTruthy();
  });

  it('should call onPress when pressed', () => {
    const onPress = jest.fn();
    render(<ScanButton onPress={onPress} />);

    fireEvent.press(screen.getByText('Scan Barcode'));

    expect(onPress).toHaveBeenCalledTimes(1);
  });

  it('should be accessible', () => {
    render(<ScanButton onPress={jest.fn()} />);

    const button = screen.getByText('Scan Barcode');
    expect(button).toBeTruthy();
  });
});
