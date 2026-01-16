import i18n from 'i18next';
import { initReactI18next } from 'react-i18next';
import { I18nManager } from 'react-native';
import he from './he.json';
import en from './en.json';

const resources = {
  he: { translation: he },
  en: { translation: en },
};

i18n.use(initReactI18next).init({
  resources,
  lng: 'he',
  fallbackLng: 'en',
  interpolation: {
    escapeValue: false,
  },
  react: {
    useSuspense: false,
  },
});

export function setLanguage(locale: 'he' | 'en'): void {
  i18n.changeLanguage(locale);
  const isRTL = locale === 'he';
  if (I18nManager.isRTL !== isRTL) {
    I18nManager.allowRTL(isRTL);
    I18nManager.forceRTL(isRTL);
  }
}

export function isRTL(): boolean {
  return i18n.language === 'he';
}

export default i18n;
