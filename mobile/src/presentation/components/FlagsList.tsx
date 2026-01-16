import React from 'react';
import { View, Text, StyleSheet } from 'react-native';
import { useTranslation } from 'react-i18next';
import { NutrientFlag } from '../../core/entities/Product';

interface FlagsListProps {
  flags: NutrientFlag[];
}

function getLevelColor(level: NutrientFlag['level']): string {
  switch (level) {
    case 'high':
      return '#E63E11';
    case 'moderate':
      return '#EE8100';
    case 'low':
      return '#038141';
    default:
      return '#666666';
  }
}

function getLevelBackgroundColor(level: NutrientFlag['level']): string {
  switch (level) {
    case 'high':
      return '#FFEBE6';
    case 'moderate':
      return '#FFF4E6';
    case 'low':
      return '#E6F7ED';
    default:
      return '#F5F5F5';
  }
}

export default function FlagsList({ flags }: FlagsListProps) {
  const { t } = useTranslation();

  if (flags.length === 0) {
    return null;
  }

  return (
    <View style={styles.container}>
      {flags.map((flag, index) => {
        const textColor = getLevelColor(flag.level);
        const backgroundColor = getLevelBackgroundColor(flag.level);

        return (
          <View key={index} style={[styles.flagItem, { backgroundColor }]}>
            <View style={styles.flagHeader}>
              <Text style={[styles.nutrientName, { color: textColor }]}>
                {flag.nutrient}
              </Text>
              <View style={[styles.levelBadge, { backgroundColor: textColor }]}>
                <Text style={styles.levelText}>{t(`flags.${flag.level}`)}</Text>
              </View>
            </View>
            {flag.description && (
              <Text style={styles.description}>{flag.description}</Text>
            )}
          </View>
        );
      })}
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    gap: 8,
  },
  flagItem: {
    borderRadius: 10,
    padding: 12,
  },
  flagHeader: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
  },
  nutrientName: {
    fontSize: 15,
    fontWeight: '600',
  },
  levelBadge: {
    paddingHorizontal: 8,
    paddingVertical: 4,
    borderRadius: 4,
  },
  levelText: {
    fontSize: 12,
    fontWeight: '600',
    color: '#FFFFFF',
  },
  description: {
    fontSize: 13,
    color: '#666666',
    marginTop: 6,
  },
});
