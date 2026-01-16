import React from 'react';
import {
  View,
  Text,
  StyleSheet,
  ScrollView,
  TouchableOpacity,
  Image,
  ActivityIndicator,
  SafeAreaView,
} from 'react-native';
import { useNavigation, useRoute, RouteProp } from '@react-navigation/native';
import { NativeStackNavigationProp } from '@react-navigation/native-stack';
import { useTranslation } from 'react-i18next';
import { useProduct } from '../../application/hooks/useProduct';
import { RootStackParamList } from '../navigation/AppNavigator';
import ScoreDisplay from '../components/ScoreDisplay';
import NutritionCard from '../components/NutritionCard';
import FlagsList from '../components/FlagsList';

type ResultScreenNavProp = NativeStackNavigationProp<RootStackParamList, 'Result'>;
type ResultScreenRouteProp = RouteProp<RootStackParamList, 'Result'>;

export default function ResultScreen() {
  const { t } = useTranslation();
  const navigation = useNavigation<ResultScreenNavProp>();
  const route = useRoute<ResultScreenRouteProp>();
  const { barcode } = route.params;

  const { data: result, isLoading, error, refetch } = useProduct(barcode);

  function handleContributePress() {
    navigation.navigate('Contribute', { barcode });
  }

  function handleRetry() {
    refetch();
  }

  if (isLoading) {
    return (
      <SafeAreaView style={styles.container}>
        <View style={styles.loadingContainer}>
          <ActivityIndicator size="large" color="#007AFF" />
          <Text style={styles.loadingText}>{t('common.loading')}</Text>
        </View>
      </SafeAreaView>
    );
  }

  if (error) {
    return (
      <SafeAreaView style={styles.container}>
        <View style={styles.errorContainer}>
          <Text style={styles.errorText}>{t('common.error')}</Text>
          <Text style={styles.errorMessage}>{error.message}</Text>
          <TouchableOpacity style={styles.retryButton} onPress={handleRetry}>
            <Text style={styles.retryButtonText}>{t('common.retry')}</Text>
          </TouchableOpacity>
        </View>
      </SafeAreaView>
    );
  }

  if (!result) {
    return (
      <SafeAreaView style={styles.container}>
        <View style={styles.header}>
          <TouchableOpacity onPress={() => navigation.goBack()}>
            <Text style={styles.backButton}>←</Text>
          </TouchableOpacity>
          <Text style={styles.headerTitle}>{t('result.title')}</Text>
          <View style={styles.placeholder} />
        </View>
        <View style={styles.notFoundContainer}>
          <Text style={styles.notFoundTitle}>{t('result.notFound')}</Text>
          <Text style={styles.notFoundDescription}>{t('result.notFoundDescription')}</Text>
          <TouchableOpacity style={styles.contributeButton} onPress={handleContributePress}>
            <Text style={styles.contributeButtonText}>{t('result.contribute')}</Text>
          </TouchableOpacity>
        </View>
      </SafeAreaView>
    );
  }

  return (
    <SafeAreaView style={styles.container}>
      <View style={styles.header}>
        <TouchableOpacity onPress={() => navigation.goBack()}>
          <Text style={styles.backButton}>←</Text>
        </TouchableOpacity>
        <Text style={styles.headerTitle}>{t('result.title')}</Text>
        <View style={styles.placeholder} />
      </View>

      <ScrollView style={styles.scrollView} contentContainerStyle={styles.scrollContent}>
        <View style={styles.productInfo}>
          {result.imageUrl && (
            <Image source={{ uri: result.imageUrl }} style={styles.productImage} />
          )}
          <Text style={styles.productName}>{result.productName}</Text>
          {result.brand && <Text style={styles.productBrand}>{result.brand}</Text>}
          {result.isFromCache && (
            <Text style={styles.cachedIndicator}>{t('result.cachedData')}</Text>
          )}
        </View>

        <ScoreDisplay grade={result.grade} score={result.score} />

        {result.flags.length > 0 && (
          <View style={styles.section}>
            <Text style={styles.sectionTitle}>{t('result.flags')}</Text>
            <FlagsList flags={result.flags} />
          </View>
        )}

        <View style={styles.section}>
          <Text style={styles.sectionTitle}>{t('result.nutritionFacts')}</Text>
          <NutritionCard nutrition={result.nutritionPer100g} />
        </View>

        <View style={styles.disclaimerContainer}>
          <Text style={styles.disclaimer}>{result.disclaimer}</Text>
        </View>
      </ScrollView>
    </SafeAreaView>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#FFFFFF',
  },
  header: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    paddingHorizontal: 20,
    paddingVertical: 12,
    borderBottomWidth: 1,
    borderBottomColor: '#E5E5E5',
  },
  backButton: {
    fontSize: 28,
    color: '#007AFF',
  },
  headerTitle: {
    fontSize: 18,
    fontWeight: '600',
    color: '#1A1A1A',
  },
  placeholder: {
    width: 28,
  },
  scrollView: {
    flex: 1,
  },
  scrollContent: {
    padding: 20,
  },
  loadingContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
  },
  loadingText: {
    marginTop: 12,
    fontSize: 16,
    color: '#666666',
  },
  errorContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    padding: 20,
  },
  errorText: {
    fontSize: 18,
    fontWeight: '600',
    color: '#E63E11',
    marginBottom: 8,
  },
  errorMessage: {
    fontSize: 14,
    color: '#666666',
    textAlign: 'center',
    marginBottom: 20,
  },
  retryButton: {
    backgroundColor: '#007AFF',
    paddingHorizontal: 24,
    paddingVertical: 12,
    borderRadius: 8,
  },
  retryButtonText: {
    fontSize: 16,
    color: '#FFFFFF',
    fontWeight: '600',
  },
  notFoundContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    padding: 20,
  },
  notFoundTitle: {
    fontSize: 20,
    fontWeight: '600',
    color: '#1A1A1A',
    marginBottom: 8,
  },
  notFoundDescription: {
    fontSize: 16,
    color: '#666666',
    textAlign: 'center',
    marginBottom: 24,
  },
  contributeButton: {
    backgroundColor: '#038141',
    paddingHorizontal: 24,
    paddingVertical: 12,
    borderRadius: 8,
  },
  contributeButtonText: {
    fontSize: 16,
    color: '#FFFFFF',
    fontWeight: '600',
  },
  productInfo: {
    alignItems: 'center',
    marginBottom: 24,
  },
  productImage: {
    width: 120,
    height: 120,
    borderRadius: 12,
    marginBottom: 12,
  },
  productName: {
    fontSize: 20,
    fontWeight: '600',
    color: '#1A1A1A',
    textAlign: 'center',
  },
  productBrand: {
    fontSize: 16,
    color: '#666666',
    marginTop: 4,
  },
  cachedIndicator: {
    fontSize: 12,
    color: '#999999',
    marginTop: 4,
    fontStyle: 'italic',
  },
  section: {
    marginTop: 24,
  },
  sectionTitle: {
    fontSize: 18,
    fontWeight: '600',
    color: '#1A1A1A',
    marginBottom: 12,
  },
  disclaimerContainer: {
    marginTop: 24,
    padding: 12,
    backgroundColor: '#F5F5F5',
    borderRadius: 8,
  },
  disclaimer: {
    fontSize: 12,
    color: '#666666',
    lineHeight: 18,
  },
});
