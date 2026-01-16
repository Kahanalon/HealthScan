import React from 'react';
import {
  View,
  Text,
  StyleSheet,
  TouchableOpacity,
  FlatList,
  TextInput,
  SafeAreaView,
} from 'react-native';
import { useNavigation } from '@react-navigation/native';
import { NativeStackNavigationProp } from '@react-navigation/native-stack';
import { useTranslation } from 'react-i18next';
import { useScanHistory } from '../../application/hooks/useScanHistory';
import { ScanHistory } from '../../core/entities/ScanHistory';
import { RootStackParamList } from '../navigation/AppNavigator';
import ProductCard from '../components/ProductCard';
import ScanButton from '../components/ScanButton';
import LanguageToggle from '../components/LanguageToggle';

type HomeScreenNavProp = NativeStackNavigationProp<RootStackParamList, 'Home'>;

export default function HomeScreen() {
  const { t } = useTranslation();
  const navigation = useNavigation<HomeScreenNavProp>();
  const { data: recentScans, isLoading } = useScanHistory(5);
  const [searchQuery, setSearchQuery] = React.useState('');

  function handleScanPress() {
    navigation.navigate('Scanner');
  }

  function handleSearchSubmit() {
    if (searchQuery.trim().length >= 2) {
      navigation.navigate('Search', { query: searchQuery.trim() });
    }
  }

  function handleProductPress(scan: ScanHistory) {
    navigation.navigate('Result', { barcode: scan.barcode });
  }

  function handleViewAllHistory() {
    navigation.navigate('History');
  }

  function renderRecentScan({ item }: { item: ScanHistory }) {
    return (
      <ProductCard
        barcode={item.barcode}
        name={item.productName}
        brand={item.brand}
        imageUrl={item.imageUrl}
        grade={item.grade}
        onPress={() => handleProductPress(item)}
      />
    );
  }

  return (
    <SafeAreaView style={styles.container}>
      <View style={styles.header}>
        <View>
          <Text style={styles.title}>{t('home.title')}</Text>
          <Text style={styles.subtitle}>{t('home.subtitle')}</Text>
        </View>
        <LanguageToggle />
      </View>

      <View style={styles.searchContainer}>
        <TextInput
          style={styles.searchInput}
          placeholder={t('home.searchPlaceholder')}
          value={searchQuery}
          onChangeText={setSearchQuery}
          onSubmitEditing={handleSearchSubmit}
          returnKeyType="search"
        />
      </View>

      <ScanButton onPress={handleScanPress} />

      <View style={styles.recentSection}>
        <View style={styles.recentHeader}>
          <Text style={styles.recentTitle}>{t('home.recentScans')}</Text>
          {recentScans && recentScans.length > 0 && (
            <TouchableOpacity onPress={handleViewAllHistory}>
              <Text style={styles.viewAllText}>{t('home.viewAll')}</Text>
            </TouchableOpacity>
          )}
        </View>

        {isLoading ? (
          <Text style={styles.emptyText}>{t('common.loading')}</Text>
        ) : recentScans && recentScans.length > 0 ? (
          <FlatList
            data={recentScans}
            renderItem={renderRecentScan}
            keyExtractor={(item) => item.id}
            horizontal
            showsHorizontalScrollIndicator={false}
            contentContainerStyle={styles.recentList}
          />
        ) : (
          <Text style={styles.emptyText}>{t('home.noRecentScans')}</Text>
        )}
      </View>
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
    alignItems: 'flex-start',
    paddingHorizontal: 20,
    paddingTop: 20,
    paddingBottom: 10,
  },
  title: {
    fontSize: 28,
    fontWeight: 'bold',
    color: '#1A1A1A',
  },
  subtitle: {
    fontSize: 14,
    color: '#666666',
    marginTop: 4,
  },
  searchContainer: {
    paddingHorizontal: 20,
    marginBottom: 20,
  },
  searchInput: {
    backgroundColor: '#F5F5F5',
    borderRadius: 12,
    paddingHorizontal: 16,
    paddingVertical: 12,
    fontSize: 16,
  },
  recentSection: {
    flex: 1,
    paddingTop: 20,
  },
  recentHeader: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    paddingHorizontal: 20,
    marginBottom: 12,
  },
  recentTitle: {
    fontSize: 18,
    fontWeight: '600',
    color: '#1A1A1A',
  },
  viewAllText: {
    fontSize: 14,
    color: '#007AFF',
  },
  recentList: {
    paddingHorizontal: 20,
  },
  emptyText: {
    fontSize: 14,
    color: '#999999',
    textAlign: 'center',
    marginTop: 40,
  },
});
