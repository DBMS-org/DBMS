import { ErrorHandler, Injectable, Injector } from '@angular/core';
import { NotificationService } from '../services/notification.service';

@Injectable()
export class GlobalErrorHandler implements ErrorHandler {
  constructor(private injector: Injector) {}

  handleError(error: any): void {
    // Lazy inject to avoid cyclic dependency issues
    const notificationService = this.injector.get(NotificationService);

    // Log the error (could be replaced by remote logging)
    console.error('Unhandled application error:', error);

    // Show friendly message to user
    notificationService.showError('An unexpected error occurred. Please try again.');
  }
} 