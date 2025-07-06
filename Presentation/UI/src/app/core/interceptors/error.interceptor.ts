import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, throwError } from 'rxjs';

import { NotificationService } from '../services/notification.service';
import { AuthService } from '../services/auth.service';

// A global HTTP interceptor that handles API errors in a single place
export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const notificationService = inject(NotificationService);
  const authService = inject(AuthService);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      let message = 'An unexpected error occurred';

      if (error.error instanceof ErrorEvent) {
        // Client-side error or network issue
        message = error.error.message;
      } else {
        // Backend returned an unsuccessful response code
        switch (error.status) {
          case 0: // Network error
            message = 'Unable to reach server. Check your network connection.';
            break;
          case 400:
            message = error.error?.message || 'Bad request.';
            break;
          case 401:
            if (req.url.includes('/logout')) {
              // Don't show error for logout endpoint failures
              message = '';
            } else {
              message = 'Session expired. Please log in again.';
              // Perform immediate logout without confirmation for security
              authService.logout();
            }
            break;
          case 403:
            message = 'You do not have permission to perform this action.';
            break;
          case 404:
            message = 'Requested resource was not found.';
            break;
          case 500:
            message = 'Server error. Please try again later.';
            break;
          default:
            message = error.message || message;
        }
      }

      // Show a user-friendly notification (only if message is not empty)
      if (message) {
        notificationService.showError(message);
      }

      // Pass the error to any further handlers
      return throwError(() => error);
    })
  );
}; 