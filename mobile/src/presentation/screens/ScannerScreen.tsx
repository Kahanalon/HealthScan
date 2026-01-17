import React, { useEffect, useCallback, useRef } from 'react';
import {
  View,
  Text,
  StyleSheet,
  TouchableOpacity,
  ActivityIndicator,
  SafeAreaView,
} from 'react-native';
import { useNavigation } from '@react-navigation/native';
import { NativeStackNavigationProp } from '@react-navigation/native-stack';
import { useTranslation } from 'react-i18next';
import { Camera, useCameraDevice, useCodeScanner } from 'react-native-vision-camera';
import { useScanStore } from '../../stores/useScanStore';
import { useDI } from '../../di/container';
import { RootStackParamList } from '../navigation/AppNavigator';

type ScannerScreenNavProp = NativeStackNavigationProp<RootStackParamList, 'Scanner'>;

export default function ScannerScreen() {
  const { t } = useTranslation();
  const navigation = useNavigation<ScannerScreenNavProp>();
  const { barcodeScanner } = useDI();
  const device = useCameraDevice('back');
  const cameraRef = useRef<Camera>(null);

  const { currentScan, torchEnabled, scanBarcode, toggleTorch, setScanning } = useScanStore();
  const [hasPermission, setHasPermission] = React.useState<boolean | null>(null);
  const lastScannedRef = useRef<string | null>(null);

  const checkPermissions = useCallback(async () => {
    const granted = await barcodeScanner.hasPermissions();
    if (granted) {
      setHasPermission(true);
    } else {
      const result = await barcodeScanner.requestPermissions();
      setHasPermission(result);
    }
  }, [barcodeScanner]);

  useEffect(() => {
    checkPermissions();
    setScanning();
    return () => {
      barcodeScanner.stopScanning();
    };
  }, [checkPermissions, setScanning, barcodeScanner]);

  const handleCodeScanned = useCallback(
    (codes: { type: string; value?: string }[]) => {
      if (codes.length === 0 || currentScan.state === 'loading') return;

      const code = codes[0];
      if (!code.value || code.value === lastScannedRef.current) return;

      lastScannedRef.current = code.value;
      scanBarcode(code.value);
      navigation.replace('Result', { barcode: code.value });
    },
    [currentScan.state, navigation, scanBarcode]
  );

  const codeScanner = useCodeScanner({
    codeTypes: ['ean-13', 'ean-8', 'upc-a', 'upc-e'],
    onCodeScanned: handleCodeScanned,
  });

  function handleTorchPress() {
    toggleTorch();
  }

  const handleRequestPermission = useCallback(() => {
    checkPermissions();
  }, [checkPermissions]);

  if (hasPermission === null) {
    return (
      <SafeAreaView style={styles.container}>
        <ActivityIndicator size="large" color="#007AFF" />
      </SafeAreaView>
    );
  }

  if (!hasPermission) {
    return (
      <SafeAreaView style={styles.container}>
        <View style={styles.permissionContainer}>
          <Text style={styles.permissionText}>{t('scanner.permissionDenied')}</Text>
          <TouchableOpacity style={styles.permissionButton} onPress={handleRequestPermission}>
            <Text style={styles.permissionButtonText}>{t('scanner.requestPermission')}</Text>
          </TouchableOpacity>
        </View>
      </SafeAreaView>
    );
  }

  if (!device) {
    return (
      <SafeAreaView style={styles.container}>
        <Text style={styles.errorText}>No camera device available</Text>
      </SafeAreaView>
    );
  }

  return (
    <View style={styles.container}>
      <Camera
        ref={cameraRef}
        style={StyleSheet.absoluteFill}
        device={device}
        isActive={true}
        codeScanner={codeScanner}
        torch={torchEnabled ? 'on' : 'off'}
      />

      <SafeAreaView style={styles.overlay}>
        <View style={styles.header}>
          <TouchableOpacity style={styles.backButton} onPress={() => navigation.goBack()}>
            <Text style={styles.backButtonText}>←</Text>
          </TouchableOpacity>
          <Text style={styles.title}>{t('scanner.title')}</Text>
          <TouchableOpacity style={styles.torchButton} onPress={handleTorchPress}>
            <Text style={styles.torchButtonText}>{torchEnabled ? '🔦' : '💡'}</Text>
          </TouchableOpacity>
        </View>

        <View style={styles.scanArea}>
          <View style={styles.scanFrame}>
            <View style={[styles.corner, styles.topLeft]} />
            <View style={[styles.corner, styles.topRight]} />
            <View style={[styles.corner, styles.bottomLeft]} />
            <View style={[styles.corner, styles.bottomRight]} />
          </View>
          <Text style={styles.instruction}>{t('scanner.instruction')}</Text>
        </View>

        {currentScan.state === 'loading' && (
          <View style={styles.loadingOverlay}>
            <ActivityIndicator size="large" color="#FFFFFF" />
          </View>
        )}
      </SafeAreaView>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#000000',
  },
  overlay: {
    flex: 1,
  },
  header: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    paddingHorizontal: 20,
    paddingTop: 10,
  },
  backButton: {
    width: 44,
    height: 44,
    justifyContent: 'center',
    alignItems: 'center',
  },
  backButtonText: {
    fontSize: 28,
    color: '#FFFFFF',
  },
  title: {
    fontSize: 18,
    fontWeight: '600',
    color: '#FFFFFF',
  },
  torchButton: {
    width: 44,
    height: 44,
    justifyContent: 'center',
    alignItems: 'center',
  },
  torchButtonText: {
    fontSize: 24,
  },
  scanArea: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
  },
  scanFrame: {
    width: 280,
    height: 180,
    position: 'relative',
  },
  corner: {
    position: 'absolute',
    width: 30,
    height: 30,
    borderColor: '#007AFF',
  },
  topLeft: {
    top: 0,
    left: 0,
    borderTopWidth: 4,
    borderLeftWidth: 4,
  },
  topRight: {
    top: 0,
    right: 0,
    borderTopWidth: 4,
    borderRightWidth: 4,
  },
  bottomLeft: {
    bottom: 0,
    left: 0,
    borderBottomWidth: 4,
    borderLeftWidth: 4,
  },
  bottomRight: {
    bottom: 0,
    right: 0,
    borderBottomWidth: 4,
    borderRightWidth: 4,
  },
  instruction: {
    fontSize: 16,
    color: '#FFFFFF',
    marginTop: 24,
    textAlign: 'center',
  },
  permissionContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    padding: 20,
  },
  permissionText: {
    fontSize: 16,
    color: '#FFFFFF',
    textAlign: 'center',
    marginBottom: 20,
  },
  permissionButton: {
    backgroundColor: '#007AFF',
    paddingHorizontal: 24,
    paddingVertical: 12,
    borderRadius: 8,
  },
  permissionButtonText: {
    fontSize: 16,
    color: '#FFFFFF',
    fontWeight: '600',
  },
  errorText: {
    fontSize: 16,
    color: '#FFFFFF',
    textAlign: 'center',
  },
  loadingOverlay: {
    ...StyleSheet.absoluteFillObject,
    backgroundColor: 'rgba(0, 0, 0, 0.5)',
    justifyContent: 'center',
    alignItems: 'center',
  },
});
