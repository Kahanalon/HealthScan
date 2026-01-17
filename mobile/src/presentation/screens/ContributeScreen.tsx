import React, { useState, useRef, useCallback } from 'react';
import {
  View,
  Text,
  StyleSheet,
  TouchableOpacity,
  Image,
  ScrollView,
  ActivityIndicator,
  Alert,
  SafeAreaView,
} from 'react-native';
import { useNavigation, useRoute, RouteProp } from '@react-navigation/native';
import { NativeStackNavigationProp } from '@react-navigation/native-stack';
import { useTranslation } from 'react-i18next';
import { Camera, useCameraDevice } from 'react-native-vision-camera';
import { useContribute } from '../../application/hooks/useContribute';
import { useDI } from '../../di/container';
import { RootStackParamList } from '../navigation/AppNavigator';
import { CapturedImage } from '../../core/interfaces/IImageService';
import { visionCameraImage } from '../../infrastructure/image/VisionCameraImage';

type ContributeScreenNavProp = NativeStackNavigationProp<RootStackParamList, 'Contribute'>;
type ContributeScreenRouteProp = RouteProp<RootStackParamList, 'Contribute'>;

type CaptureStep = 'nutrition' | 'ingredients' | 'preview';

export default function ContributeScreen() {
  const { t } = useTranslation();
  const navigation = useNavigation<ContributeScreenNavProp>();
  const route = useRoute<ContributeScreenRouteProp>();
  const { barcode } = route.params;
  const { barcodeScanner } = useDI();

  const cameraRef = useRef<Camera>(null);
  const device = useCameraDevice('back');

  const [step, setStep] = useState<CaptureStep>('nutrition');
  const [nutritionImage, setNutritionImage] = useState<CapturedImage | null>(null);
  const [ingredientsImage, setIngredientsImage] = useState<CapturedImage | null>(null);
  const [hasPermission, setHasPermission] = useState<boolean | null>(null);

  const contributeMutation = useContribute();

  const checkPermissions = useCallback(async () => {
    const granted = await barcodeScanner.hasPermissions();
    if (granted) {
      setHasPermission(true);
    } else {
      const result = await barcodeScanner.requestPermissions();
      setHasPermission(result);
    }
  }, [barcodeScanner]);

  React.useEffect(() => {
    checkPermissions();
    if (cameraRef.current) {
      visionCameraImage.setCameraRef(cameraRef as React.RefObject<Camera>);
    }
  }, [checkPermissions]);

  async function handleCapture() {
    try {
      const image = await visionCameraImage.captureImage({ quality: 0.8 });
      if (step === 'nutrition') {
        setNutritionImage(image);
        setStep('ingredients');
      } else if (step === 'ingredients') {
        setIngredientsImage(image);
        setStep('preview');
      }
    } catch (error) {
      Alert.alert(t('common.error'), (error as Error).message);
    }
  }

  function handleRetake() {
    if (step === 'preview' && ingredientsImage) {
      setIngredientsImage(null);
      setStep('ingredients');
    } else if (step === 'ingredients' && nutritionImage) {
      setNutritionImage(null);
      setStep('nutrition');
    }
  }

  async function handleSubmit() {
    if (!nutritionImage) {
      return;
    }

    try {
      await contributeMutation.mutateAsync({
        barcode,
        data: {
          nutritionImageBase64: nutritionImage.base64,
          ingredientsImageBase64: ingredientsImage?.base64,
        },
      });
      Alert.alert(t('contribute.success'), '', [
        { text: 'OK', onPress: () => navigation.goBack() },
      ]);
    } catch (error) {
      Alert.alert(t('contribute.error'));
    }
  }

  if (hasPermission === null) {
    return (
      <SafeAreaView style={styles.container}>
        <ActivityIndicator size="large" color="#007AFF" />
      </SafeAreaView>
    );
  }

  if (!hasPermission || !device) {
    return (
      <SafeAreaView style={styles.container}>
        <View style={styles.permissionContainer}>
          <Text style={styles.permissionText}>{t('scanner.permissionDenied')}</Text>
          <TouchableOpacity style={styles.permissionButton} onPress={checkPermissions}>
            <Text style={styles.permissionButtonText}>{t('scanner.requestPermission')}</Text>
          </TouchableOpacity>
        </View>
      </SafeAreaView>
    );
  }

  if (step === 'preview') {
    return (
      <SafeAreaView style={styles.container}>
        <View style={styles.header}>
          <TouchableOpacity onPress={() => navigation.goBack()}>
            <Text style={styles.backButton}>←</Text>
          </TouchableOpacity>
          <Text style={styles.headerTitle}>{t('contribute.title')}</Text>
          <View style={styles.placeholder} />
        </View>

        <ScrollView style={styles.previewScroll}>
          <Text style={styles.previewLabel}>{t('contribute.nutritionLabel')}</Text>
          {nutritionImage && (
            <Image source={{ uri: nutritionImage.uri }} style={styles.previewImage} />
          )}

          {ingredientsImage && (
            <>
              <Text style={styles.previewLabel}>{t('contribute.ingredientsList')}</Text>
              <Image source={{ uri: ingredientsImage.uri }} style={styles.previewImage} />
            </>
          )}
        </ScrollView>

        <View style={styles.previewActions}>
          <TouchableOpacity style={styles.retakeButton} onPress={handleRetake}>
            <Text style={styles.retakeButtonText}>{t('contribute.retake')}</Text>
          </TouchableOpacity>
          <TouchableOpacity
            style={[styles.submitButton, contributeMutation.isPending && styles.buttonDisabled]}
            onPress={handleSubmit}
            disabled={contributeMutation.isPending}
          >
            <Text style={styles.submitButtonText}>
              {contributeMutation.isPending ? t('contribute.submitting') : t('contribute.submit')}
            </Text>
          </TouchableOpacity>
        </View>
      </SafeAreaView>
    );
  }

  return (
    <View style={styles.container}>
      <Camera ref={cameraRef} style={StyleSheet.absoluteFill} device={device} isActive={true} photo={true} />

      <SafeAreaView style={styles.overlay}>
        <View style={styles.header}>
          <TouchableOpacity onPress={() => navigation.goBack()}>
            <Text style={styles.backButtonWhite}>←</Text>
          </TouchableOpacity>
          <Text style={styles.headerTitleWhite}>{t('contribute.title')}</Text>
          <View style={styles.placeholder} />
        </View>

        <View style={styles.instructionContainer}>
          <Text style={styles.instructionText}>
            {step === 'nutrition' ? t('contribute.nutritionLabel') : t('contribute.ingredientsList')}
          </Text>
          <Text style={styles.stepIndicator}>
            {step === 'nutrition' ? '1/2' : '2/2'}
          </Text>
        </View>

        <View style={styles.captureContainer}>
          <TouchableOpacity style={styles.captureButton} onPress={handleCapture}>
            <View style={styles.captureButtonInner} />
          </TouchableOpacity>
        </View>
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
    paddingVertical: 12,
  },
  backButton: {
    fontSize: 28,
    color: '#007AFF',
  },
  backButtonWhite: {
    fontSize: 28,
    color: '#FFFFFF',
  },
  headerTitle: {
    fontSize: 18,
    fontWeight: '600',
    color: '#1A1A1A',
  },
  headerTitleWhite: {
    fontSize: 18,
    fontWeight: '600',
    color: '#FFFFFF',
  },
  placeholder: {
    width: 28,
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
  instructionContainer: {
    alignItems: 'center',
    marginTop: 20,
  },
  instructionText: {
    fontSize: 18,
    color: '#FFFFFF',
    fontWeight: '600',
  },
  stepIndicator: {
    fontSize: 14,
    color: '#CCCCCC',
    marginTop: 8,
  },
  captureContainer: {
    flex: 1,
    justifyContent: 'flex-end',
    alignItems: 'center',
    paddingBottom: 40,
  },
  captureButton: {
    width: 80,
    height: 80,
    borderRadius: 40,
    backgroundColor: 'rgba(255, 255, 255, 0.3)',
    justifyContent: 'center',
    alignItems: 'center',
  },
  captureButtonInner: {
    width: 64,
    height: 64,
    borderRadius: 32,
    backgroundColor: '#FFFFFF',
  },
  previewScroll: {
    flex: 1,
    padding: 20,
  },
  previewLabel: {
    fontSize: 16,
    fontWeight: '600',
    color: '#1A1A1A',
    marginTop: 16,
    marginBottom: 8,
  },
  previewImage: {
    width: '100%',
    height: 200,
    borderRadius: 8,
    backgroundColor: '#F5F5F5',
  },
  previewActions: {
    flexDirection: 'row',
    padding: 20,
    gap: 12,
  },
  retakeButton: {
    flex: 1,
    backgroundColor: '#F5F5F5',
    paddingVertical: 14,
    borderRadius: 8,
    alignItems: 'center',
  },
  retakeButtonText: {
    fontSize: 16,
    color: '#1A1A1A',
    fontWeight: '600',
  },
  submitButton: {
    flex: 1,
    backgroundColor: '#038141',
    paddingVertical: 14,
    borderRadius: 8,
    alignItems: 'center',
  },
  buttonDisabled: {
    opacity: 0.6,
  },
  submitButtonText: {
    fontSize: 16,
    color: '#FFFFFF',
    fontWeight: '600',
  },
});
