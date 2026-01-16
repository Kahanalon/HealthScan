import React from 'react';
import { TouchableOpacity, Text, StyleSheet, View } from 'react-native';
import { useTranslation } from 'react-i18next';

interface ScanButtonProps {
  onPress: () => void;
}

export default function ScanButton({ onPress }: ScanButtonProps) {
  const { t } = useTranslation();

  return (
    <TouchableOpacity style={styles.container} onPress={onPress} activeOpacity={0.8}>
      <View style={styles.iconContainer}>
        <View style={styles.barcodeIcon}>
          <View style={styles.barcodeLine} />
          <View style={[styles.barcodeLine, styles.lineWide]} />
          <View style={styles.barcodeLine} />
          <View style={[styles.barcodeLine, styles.lineWide]} />
          <View style={styles.barcodeLine} />
          <View style={[styles.barcodeLine, styles.lineWide]} />
          <View style={styles.barcodeLine} />
        </View>
      </View>
      <Text style={styles.text}>{t('home.scanButton')}</Text>
    </TouchableOpacity>
  );
}

const styles = StyleSheet.create({
  container: {
    backgroundColor: '#007AFF',
    marginHorizontal: 20,
    borderRadius: 16,
    paddingVertical: 20,
    paddingHorizontal: 24,
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'center',
    shadowColor: '#007AFF',
    shadowOffset: { width: 0, height: 4 },
    shadowOpacity: 0.3,
    shadowRadius: 12,
    elevation: 6,
  },
  iconContainer: {
    marginRight: 12,
  },
  barcodeIcon: {
    flexDirection: 'row',
    alignItems: 'center',
    height: 24,
    gap: 2,
  },
  barcodeLine: {
    width: 2,
    height: 20,
    backgroundColor: '#FFFFFF',
    borderRadius: 1,
  },
  lineWide: {
    width: 4,
    height: 24,
  },
  text: {
    fontSize: 18,
    fontWeight: '600',
    color: '#FFFFFF',
  },
});
