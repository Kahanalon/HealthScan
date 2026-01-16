import { IAnalyticsService, AnalyticsEvent } from '../../core/interfaces/IAnalyticsService';

export class NoOpAnalytics implements IAnalyticsService {
  trackEvent(event: AnalyticsEvent): void {
    if (__DEV__) {
      console.log('[Analytics] Event:', event.name, event.properties);
    }
  }

  trackScreen(screenName: string): void {
    if (__DEV__) {
      console.log('[Analytics] Screen:', screenName);
    }
  }

  trackError(error: Error, context?: Record<string, string>): void {
    if (__DEV__) {
      console.log('[Analytics] Error:', error.message, context);
    }
  }

  setUserId(userId: string | null): void {
    if (__DEV__) {
      console.log('[Analytics] User ID:', userId);
    }
  }

  setUserProperty(name: string, value: string): void {
    if (__DEV__) {
      console.log('[Analytics] User Property:', name, value);
    }
  }
}

export const noOpAnalytics = new NoOpAnalytics();
