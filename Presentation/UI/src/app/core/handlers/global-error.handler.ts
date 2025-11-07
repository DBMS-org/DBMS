import { ErrorHandler, Injectable, Injector } from '@angular/core';
import { NotificationService } from '../services/notification.service';

@Injectable()
export class GlobalErrorHandler implements ErrorHandler {
  constructor(private injector: Injector) {}

  handleError(error: any): void {
    const notificationService = this.injector.get(NotificationService);
    console.error('Unhandled application error:', error);
    notificationService.showError('An unexpected error occurred. Please try again.');
  }
} 