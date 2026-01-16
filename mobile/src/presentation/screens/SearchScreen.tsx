import React, { useState, useEffect } from 'react';
import {
  View,
  Text,
  StyleSheet,
  TextInput,
  FlatList,
  TouchableOpacity,
  ActivityIndicator,
  SafeAreaView,
} from 'react-native';
import { useNavigation, useRoute, RouteProp } from '@react-navigation/native';
import { NativeStackNavigationProp } from '@react-navigation/native-stack';
import { useTranslation } from 'react-i18next';
import { useSearch } from '../../application/hooks/useSearch';
import { Product } from '../../core/entities/Product';
import { RootStackParamList } from '../navigation/AppNavigator';
import ProductCard from '../components/ProductCard';

type SearchScreenNavProp = NativeStackNavigationProp<RootStackParamList, 'Search'>;
type SearchScreenRouteProp = RouteProp<RootStackParamList, 'Search'>;

export default function SearchScreen() {
  const { t } = useTranslation();
  const navigation = useNavigation<SearchScreenNavProp>();
  const route = useRoute<SearchScreenRouteProp>();
  const initialQuery = route.params?.query ?? '';

  const [searchQuery, setSearchQuery] = useState(initialQuery);
  const [debouncedQuery, setDebouncedQuery] = useState(initialQuery);

  const {
    data,
    isLoading,
    isFetchingNextPage,
    hasNextPage,
    fetchNextPage,
  } = useSearch(debouncedQuery);

  useEffect(() => {
    const timer = setTimeout(() => {
      setDebouncedQuery(searchQuery);
    }, 300);
    return () => clearTimeout(timer);
  }, [searchQuery]);

  const allProducts = data?.pages.flatMap((page) => page.items) ?? [];

  function handleProductPress(product: Product) {
    navigation.navigate('Result', { barcode: product.barcode });
  }

  function handleLoadMore() {
    if (hasNextPage && !isFetchingNextPage) {
      fetchNextPage();
    }
  }

  function renderProductItem({ item }: { item: Product }) {
    return (
      <ProductCard
        barcode={item.barcode}
        name={item.name}
        brand={item.brand}
        imageUrl={item.imageUrl}
        grade={item.nutriScoreGrade}
        onPress={() => handleProductPress(item)}
        horizontal
      />
    );
  }

  function renderFooter() {
    if (!isFetchingNextPage) return null;
    return (
      <View style={styles.footerLoader}>
        <ActivityIndicator size="small" color="#007AFF" />
      </View>
    );
  }

  return (
    <SafeAreaView style={styles.container}>
      <View style={styles.header}>
        <TouchableOpacity onPress={() => navigation.goBack()}>
          <Text style={styles.backButton}>←</Text>
        </TouchableOpacity>
        <View style={styles.searchInputContainer}>
          <TextInput
            style={styles.searchInput}
            placeholder={t('search.placeholder')}
            value={searchQuery}
            onChangeText={setSearchQuery}
            autoFocus
            returnKeyType="search"
          />
        </View>
      </View>

      {isLoading && debouncedQuery.length >= 2 ? (
        <View style={styles.loadingContainer}>
          <ActivityIndicator size="large" color="#007AFF" />
          <Text style={styles.loadingText}>{t('search.searching')}</Text>
        </View>
      ) : allProducts.length > 0 ? (
        <FlatList
          data={allProducts}
          renderItem={renderProductItem}
          keyExtractor={(item) => item.barcode}
          contentContainerStyle={styles.listContent}
          ItemSeparatorComponent={() => <View style={styles.separator} />}
          onEndReached={handleLoadMore}
          onEndReachedThreshold={0.5}
          ListFooterComponent={renderFooter}
        />
      ) : debouncedQuery.length >= 2 ? (
        <View style={styles.emptyContainer}>
          <Text style={styles.emptyText}>{t('search.noResults')}</Text>
        </View>
      ) : (
        <View style={styles.emptyContainer}>
          <Text style={styles.hintText}>{t('search.placeholder')}</Text>
        </View>
      )}
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
    alignItems: 'center',
    paddingHorizontal: 20,
    paddingVertical: 12,
    borderBottomWidth: 1,
    borderBottomColor: '#E5E5E5',
  },
  backButton: {
    fontSize: 28,
    color: '#007AFF',
    marginRight: 12,
  },
  searchInputContainer: {
    flex: 1,
  },
  searchInput: {
    backgroundColor: '#F5F5F5',
    borderRadius: 8,
    paddingHorizontal: 12,
    paddingVertical: 10,
    fontSize: 16,
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
  listContent: {
    padding: 20,
  },
  separator: {
    height: 12,
  },
  emptyContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    padding: 20,
  },
  emptyText: {
    fontSize: 16,
    color: '#999999',
  },
  hintText: {
    fontSize: 16,
    color: '#CCCCCC',
  },
  footerLoader: {
    paddingVertical: 20,
    alignItems: 'center',
  },
});
