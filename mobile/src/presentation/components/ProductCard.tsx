import React from 'react';
import { View, Text, StyleSheet, TouchableOpacity, Image } from 'react-native';
import { NutriScoreGrade, getGradeColor } from '../../core/entities/ScoreResult';

interface ProductCardProps {
  barcode: string;
  name: string;
  brand: string | null;
  imageUrl: string | null;
  grade: NutriScoreGrade;
  onPress: () => void;
  horizontal?: boolean;
}

export default function ProductCard({
  name,
  brand,
  imageUrl,
  grade,
  onPress,
  horizontal = false,
}: ProductCardProps) {
  const gradeColor = getGradeColor(grade);

  if (horizontal) {
    return (
      <TouchableOpacity style={styles.horizontalContainer} onPress={onPress}>
        {imageUrl ? (
          <Image source={{ uri: imageUrl }} style={styles.horizontalImage} />
        ) : (
          <View style={[styles.horizontalImage, styles.placeholderImage]}>
            <Text style={styles.placeholderText}>📦</Text>
          </View>
        )}
        <View style={styles.horizontalContent}>
          <Text style={styles.horizontalName} numberOfLines={2}>
            {name}
          </Text>
          {brand && (
            <Text style={styles.horizontalBrand} numberOfLines={1}>
              {brand}
            </Text>
          )}
        </View>
        <View style={[styles.gradeBadge, { backgroundColor: gradeColor }]}>
          <Text style={styles.gradeBadgeText}>{grade}</Text>
        </View>
      </TouchableOpacity>
    );
  }

  return (
    <TouchableOpacity style={styles.container} onPress={onPress}>
      {imageUrl ? (
        <Image source={{ uri: imageUrl }} style={styles.image} />
      ) : (
        <View style={[styles.image, styles.placeholderImage]}>
          <Text style={styles.placeholderText}>📦</Text>
        </View>
      )}
      <View style={[styles.gradeBadgeSmall, { backgroundColor: gradeColor }]}>
        <Text style={styles.gradeBadgeTextSmall}>{grade}</Text>
      </View>
      <Text style={styles.name} numberOfLines={2}>
        {name}
      </Text>
      {brand && (
        <Text style={styles.brand} numberOfLines={1}>
          {brand}
        </Text>
      )}
    </TouchableOpacity>
  );
}

const styles = StyleSheet.create({
  container: {
    width: 140,
    marginRight: 12,
  },
  image: {
    width: 140,
    height: 100,
    borderRadius: 10,
    backgroundColor: '#F5F5F5',
  },
  placeholderImage: {
    justifyContent: 'center',
    alignItems: 'center',
  },
  placeholderText: {
    fontSize: 32,
  },
  gradeBadgeSmall: {
    position: 'absolute',
    top: 8,
    right: 8,
    width: 28,
    height: 28,
    borderRadius: 14,
    justifyContent: 'center',
    alignItems: 'center',
  },
  gradeBadgeTextSmall: {
    fontSize: 14,
    fontWeight: 'bold',
    color: '#FFFFFF',
  },
  name: {
    fontSize: 14,
    fontWeight: '500',
    color: '#1A1A1A',
    marginTop: 8,
  },
  brand: {
    fontSize: 12,
    color: '#666666',
    marginTop: 2,
  },
  horizontalContainer: {
    flexDirection: 'row',
    alignItems: 'center',
    backgroundColor: '#FFFFFF',
    borderRadius: 12,
    padding: 12,
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 1 },
    shadowOpacity: 0.05,
    shadowRadius: 4,
    elevation: 2,
    borderWidth: 1,
    borderColor: '#F0F0F0',
  },
  horizontalImage: {
    width: 60,
    height: 60,
    borderRadius: 8,
    backgroundColor: '#F5F5F5',
  },
  horizontalContent: {
    flex: 1,
    marginLeft: 12,
  },
  horizontalName: {
    fontSize: 15,
    fontWeight: '500',
    color: '#1A1A1A',
  },
  horizontalBrand: {
    fontSize: 13,
    color: '#666666',
    marginTop: 2,
  },
  gradeBadge: {
    width: 36,
    height: 36,
    borderRadius: 18,
    justifyContent: 'center',
    alignItems: 'center',
    marginLeft: 8,
  },
  gradeBadgeText: {
    fontSize: 18,
    fontWeight: 'bold',
    color: '#FFFFFF',
  },
});
