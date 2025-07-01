import { CANVAS_CONSTANTS } from '../constants/canvas.constants';

export class Logger {
  private static isEnabled: boolean = CANVAS_CONSTANTS.DEBUG.ENABLED;

  static info(message: string, data?: any): void {
    if (this.isEnabled) {
      console.log(`[DrillingPattern] ${message}`, data || '');
    }
  }

  static warn(message: string, data?: any): void {
    if (this.isEnabled) {
      console.warn(`[DrillingPattern] ${message}`, data || '');
    }
  }

  static error(message: string, error?: any): void {
    // Always log errors, even in production
    console.error(`[DrillingPattern] ${message}`, error || '');
  }

  static debug(message: string, data?: any): void {
    if (this.isEnabled) {
      console.debug(`[DrillingPattern] ${message}`, data || '');
    }
  }

  static setEnabled(enabled: boolean): void {
    this.isEnabled = enabled;
  }
} 