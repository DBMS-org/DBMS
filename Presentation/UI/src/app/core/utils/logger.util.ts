import { environment } from '../../../environments/environment';

/**
 * Logger utility to conditionally log based on environment
 * Only logs in development mode, strips all logs in production
 */
export class Logger {
  static log(...args: any[]): void {
    if (!environment.production) {
      console.log(...args);
    }
  }

  static warn(...args: any[]): void {
    if (!environment.production) {
      console.warn(...args);
    }
  }

  static error(...args: any[]): void {
    // Always log errors, even in production
    console.error(...args);
  }

  static debug(...args: any[]): void {
    if (!environment.production) {
      console.debug(...args);
    }
  }

  static info(...args: any[]): void {
    if (!environment.production) {
      console.info(...args);
    }
  }
}
