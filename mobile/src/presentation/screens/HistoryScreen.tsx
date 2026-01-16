import React from 'react';
import {
  View,
  Text,
  StyleSheet,
  FlatList,
  TouchableOpacity,
  Alert,
  ActivityIndicator,
  SafeAreaView,
} from 'react-native';
import { useNavigation } from '@react-navigation/native';
import { NativeStackNavigationProp } from '@react-navigation/native-stack';
import { useTranslation } from 'react-i18next';
import { useScanHistory, useClearHistory } from '../../application/hooks/useScanHistory';
import { ScanHistory } from '../../core/entities/ScanHistory';
import { RootStackParamList } from '../navigation/AppNavigator';
import ProductCard from '../components/ProductCard';

type HistoryScreenNavProp = NativeStackNavigationProp<RootStackParamList, 'History'>;

export default function HistoryScreen() {
  const { t } = useTranslation();
  const navigation = useNavigation<HistoryScreenNavProp>();
  const { data: scans, isLoading, refetch } = useScanHistory(100);
  const clearHistoryMutation = useClearHistory();

  function handleProductPress(scan: ScanHistory) {
    navigation.navigate('Result', { barcode: scan.barcode });
  }

  function handleClearHistory() {
    Alert.alert(t('history.clearHistory'), t('history.clearConfirm'), [
      { text: t('common.cancel'), style: 'cancel' },
      {
        text: t('common.confirm'),
        style: 'destructive',
        onPress: async () => {
          await clearHistoryMutation.mutateAsync();
          refetch();
        },
      },
    ]);
  }

  function renderScanItem({ item }: { item: ScanHistory }) {
    const formattedDate = item.scannedAt.toLocaleDateString(undefined, {
      day: 'numeric',
      month: 'short',
      hour: '2-digit',
      minute: '2-digit',
    });

    return (
      <View style={styles.itemContainer}>
        <ProductCard
          barcode={item.barcode}
          name={item.productName}
          brand={item.brand}
          imageUrl={item.imageUrl}
          grade={item.grade}
          onPress={() => handleProductPress(item)}
          horizontal
        />
        <Text style={styles.dateText}>{formattedDate}</Text>
      </View>
    );
  }

  return (
    <SafeAreaView style={styles.container}>
      <View style={styles.header}>
        <TouchableOpacity onPress={() => navigation.goBack()}>
          <Text style={styles.backButton}>←</Text>
        </TouchableOpacity>
        <Text style={styles.headerTitle}>{t('history.title')}</Text>
        {scans && scans.length > 0 ? (
          <TouchableOpacity onPress={handleClearHistory}>
            <Text style={styles.clearButton}>{t('history.clearHistory')}</Text>
          </TouchableOpacity>
        ) : (
          <View style={styles.placeholder} />
        )}
      </View>

      {isLoading ? (
        <View style={styles.loadingContainer}>
          <ActivityIndicator size="large" color="#007AFF" />
        </View>
      ) : scans && scans.length > 0 ? (
        <FlatList
          data={scans}
          renderItem={renderScanItem}
          keyExtractor={(item) => item.id}
          contentContainerStyle={styles.listContent}
          ItemSeparatorComponent={() => <View style={styles.separator} />}
        />
      ) : (
        <View style={styles.emptyContainer}>
          <Text style={styles.emptyText}>{t('history.empty')}</Text>
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
  clearButton: {
    fontSize: 14,
    color: '#E63E11',
  },
  placeholder: {
    width: 80,
  },
  loadingContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
  },
  listContent: {
    padding: 20,
  },
  itemContainer: {
    marginBottom: 8,
  },
  dateText: {
    fontSize: 12,
    color: '#999999',
    marginTop: 4,
    textAlign: 'right',
  },
  separator: {
    height: 12,
  },
  emptyContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
  },
  emptyText: {
    fontSize: 16,
    color: '#999999',
  },
});
