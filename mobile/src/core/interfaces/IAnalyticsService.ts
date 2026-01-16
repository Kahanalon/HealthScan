export interface AnalyticsEvent {
  name: string;
  properties?: Record<string, string | number | boolean>;
}

export interface IAnalyticsService {
  trackEvent(event: AnalyticsEvent): void;
  trackScreen(screenName: string): void;
  trackError(error: Error, context?: Record<string, string>): void;
  setUserId(userId: string | null): void;
  setUserProperty(name: string, value: string): void;
}
