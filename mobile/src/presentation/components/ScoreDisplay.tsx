import React from 'react';
import { View, Text, StyleSheet } from 'react-native';
import { useTranslation } from 'react-i18next';
import { NutriScoreGrade, getGradeColor } from '../../core/entities/ScoreResult';

interface ScoreDisplayProps {
  grade: NutriScoreGrade;
  score: number | null;
}

export default function ScoreDisplay({ grade, score }: ScoreDisplayProps) {
  const { t } = useTranslation();
  const backgroundColor = getGradeColor(grade);

  return (
    <View style={styles.container}>
      <View style={[styles.gradeCircle, { backgroundColor }]}>
        <Text style={styles.gradeText}>{grade}</Text>
      </View>
      <Text style={styles.description}>{t(`grades.${grade}`)}</Text>
      {score !== null && (
        <Text style={styles.scoreText}>
          {t('result.nutriScore')}: {score}
        </Text>
      )}
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    alignItems: 'center',
    paddingVertical: 20,
  },
  gradeCircle: {
    width: 100,
    height: 100,
    borderRadius: 50,
    justifyContent: 'center',
    alignItems: 'center',
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.15,
    shadowRadius: 8,
    elevation: 4,
  },
  gradeText: {
    fontSize: 48,
    fontWeight: 'bold',
    color: '#FFFFFF',
  },
  description: {
    fontSize: 16,
    color: '#666666',
    marginTop: 12,
    textAlign: 'center',
  },
  scoreText: {
    fontSize: 14,
    color: '#999999',
    marginTop: 4,
  },
});
