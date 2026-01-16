import React from 'react';
import { NavigationContainer } from '@react-navigation/native';
import { createNativeStackNavigator } from '@react-navigation/native-stack';
import HomeScreen from '../screens/HomeScreen';
import ScannerScreen from '../screens/ScannerScreen';
import ResultScreen from '../screens/ResultScreen';
import ContributeScreen from '../screens/ContributeScreen';
import HistoryScreen from '../screens/HistoryScreen';
import SearchScreen from '../screens/SearchScreen';

export type RootStackParamList = {
  Home: undefined;
  Scanner: undefined;
  Result: { barcode: string };
  Contribute: { barcode: string };
  History: undefined;
  Search: { query?: string };
};

const Stack = createNativeStackNavigator<RootStackParamList>();

export default function AppNavigator() {
  return (
    <NavigationContainer>
      <Stack.Navigator
        initialRouteName="Home"
        screenOptions={{
          headerShown: false,
          animation: 'slide_from_right',
          contentStyle: { backgroundColor: '#FFFFFF' },
        }}
      >
        <Stack.Screen name="Home" component={HomeScreen} />
        <Stack.Screen
          name="Scanner"
          component={ScannerScreen}
          options={{ animation: 'slide_from_bottom' }}
        />
        <Stack.Screen name="Result" component={ResultScreen} />
        <Stack.Screen
          name="Contribute"
          component={ContributeScreen}
          options={{ animation: 'slide_from_bottom' }}
        />
        <Stack.Screen name="History" component={HistoryScreen} />
        <Stack.Screen name="Search" component={SearchScreen} />
      </Stack.Navigator>
    </NavigationContainer>
  );
}
