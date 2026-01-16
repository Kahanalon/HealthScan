import React from 'react';
import { render, screen, fireEvent } from '@testing-library/react-native';
import LanguageToggle from '../../../src/presentation/components/LanguageToggle';
import { useAppStore } from '../../../src/stores/useAppStore';
import { setLanguage } from '../../../src/i18n';

jest.mock('../../../src/stores/useAppStore');
jest.mock('../../../src/i18n', () => ({
  setLanguage: jest.fn(),
  isRTL: jest.fn(() => false),
  default: {},
}));

const mockUseAppStore = useAppStore as jest.MockedFunction<typeof useAppStore>;

describe('LanguageToggle', () => {
  const mockSetLocale = jest.fn();

  beforeEach(() => {
    jest.clearAllMocks();
  });

  it('should display EN when current locale is Hebrew', () => {
    mockUseAppStore.mockImplementation((selector) => {
      const state = { locale: 'he' as const, setLocale: mockSetLocale };
      return selector(state as any);
    });

    render(<LanguageToggle />);

    expect(screen.getByText('EN')).toBeTruthy();
  });

  it('should display Hebrew text when current locale is English', () => {
    mockUseAppStore.mockImplementation((selector) => {
      const state = { locale: 'en' as const, setLocale: mockSetLocale };
      return selector(state as any);
    });

    render(<LanguageToggle />);

    expect(screen.getByText('עב')).toBeTruthy();
  });

  it('should toggle language when pressed', () => {
    mockUseAppStore.mockImplementation((selector) => {
      const state = { locale: 'he' as const, setLocale: mockSetLocale };
      return selector(state as any);
    });

    render(<LanguageToggle />);

    fireEvent.press(screen.getByText('EN'));

    expect(mockSetLocale).toHaveBeenCalledWith('en');
  });

  it('should toggle from English to Hebrew when pressed', () => {
    mockUseAppStore.mockImplementation((selector) => {
      const state = { locale: 'en' as const, setLocale: mockSetLocale };
      return selector(state as any);
    });

    render(<LanguageToggle />);

    fireEvent.press(screen.getByText('עב'));

    expect(mockSetLocale).toHaveBeenCalledWith('he');
  });
});
