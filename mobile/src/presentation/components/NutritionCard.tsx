import React from 'react';
import { View, Text, StyleSheet } from 'react-native';
import { useTranslation } from 'react-i18next';
import { NutritionInfo } from '../../core/entities/Product';

interface NutritionCardProps {
  nutrition: NutritionInfo;
}

interface NutrientRowProps {
  label: string;
  value: number | null;
  unit: string;
}

function NutrientRow({ label, value, unit }: NutrientRowProps) {
  return (
    <View style={styles.row}>
      <Text style={styles.rowLabel}>{label}</Text>
      <Text style={styles.rowValue}>
        {value !== null ? `${value}${unit}` : '-'}
      </Text>
    </View>
  );
}

export default function NutritionCard({ nutrition }: NutritionCardProps) {
  const { t } = useTranslation();

  return (
    <View style={styles.container}>
      <NutrientRow label={t('result.energy')} value={nutrition.energyKcal} unit=" kcal" />
      <NutrientRow label={t('result.fat')} value={nutrition.fat} unit="g" />
      <NutrientRow label={t('result.saturatedFat')} value={nutrition.saturatedFat} unit="g" />
      <NutrientRow label={t('result.carbohydrates')} value={nutrition.carbohydrates} unit="g" />
      <NutrientRow label={t('result.sugars')} value={nutrition.sugars} unit="g" />
      <NutrientRow label={t('result.fiber')} value={nutrition.fiber} unit="g" />
      <NutrientRow label={t('result.protein')} value={nutrition.protein} unit="g" />
      <NutrientRow label={t('result.salt')} value={nutrition.salt} unit="g" />
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    backgroundColor: '#F9F9F9',
    borderRadius: 12,
    padding: 16,
  },
  row: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    paddingVertical: 10,
    borderBottomWidth: 1,
    borderBottomColor: '#E5E5E5',
  },
  rowLabel: {
    fontSize: 15,
    color: '#1A1A1A',
  },
  rowValue: {
    fontSize: 15,
    fontWeight: '500',
    color: '#1A1A1A',
  },
});
