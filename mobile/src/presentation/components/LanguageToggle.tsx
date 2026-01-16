import React from 'react';
import { TouchableOpacity, Text, StyleSheet } from 'react-native';
import { useAppStore, Locale } from '../../stores/useAppStore';
import { setLanguage } from '../../i18n';

export default function LanguageToggle() {
  const locale = useAppStore((state) => state.locale);
  const setLocale = useAppStore((state) => state.setLocale);

  function handleToggle() {
    const newLocale: Locale = locale === 'he' ? 'en' : 'he';
    setLocale(newLocale);
    setLanguage(newLocale);
  }

  return (
    <TouchableOpacity style={styles.container} onPress={handleToggle}>
      <Text style={styles.text}>{locale === 'he' ? 'EN' : 'עב'}</Text>
    </TouchableOpacity>
  );
}

const styles = StyleSheet.create({
  container: {
    backgroundColor: '#F5F5F5',
    paddingHorizontal: 12,
    paddingVertical: 8,
    borderRadius: 8,
  },
  text: {
    fontSize: 14,
    fontWeight: '600',
    color: '#1A1A1A',
  },
});
