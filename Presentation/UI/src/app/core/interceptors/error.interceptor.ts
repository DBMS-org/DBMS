import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, throwError } from 'rxjs';

import { NotificationService } from '../services/notification.service';
import { AuthService } from '../services/auth.service';

/**
 * Interface for structured API error responses
 */
interface ApiErrorResponse {
  statusCode: number;
  message?: string;
  errors?: string[];
  errorType?: string;
  details?: any;
}

/**
 * Global HTTP interceptor that provides user-friendly error messages
 * for all API errors across the application
 */
export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const notificationService = inject(NotificationService);
  const authService = inject(AuthService);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      let message = '';
      let showNotification = true;

      if (error.error instanceof ErrorEvent) {
        // Client-side error or network issue
        message = `Network error: ${error.error.message}`;
      } else if (error.status === 0) {
        // Network connectivity issue
        message = 'Unable to connect to the server. Please check your internet connection and try again.';
      } else {
        // Server-side error - extract structured error response
        const errorResponse = error.error as ApiErrorResponse;

        // Use backend's custom message if available, otherwise use default
        const backendMessage = errorResponse?.message;
        const validationErrors = errorResponse?.errors;

        switch (error.status) {
          case 400: // Bad Request (usually validation errors)
            if (validationErrors && validationErrors.length > 0) {
              // Show first validation error prominently
              message = validationErrors[0];

              // If there are multiple errors, show them all
              if (validationErrors.length > 1) {
                const additionalErrors = validationErrors.slice(1);
                setTimeout(() => {
                  additionalErrors.forEach(err => {
                    notificationService.showError(err, 'Validation Error');
                  });
                }, 500);
              }
            } else {
              message = backendMessage || 'Invalid request. Please check your input and try again.';
            }
            break;

          case 401: // Unauthorized
            if (req.url.includes('/logout')) {
              // Don't show error notification for logout endpoint failures
              showNotification = false;
            } else {
              message = backendMessage || 'Your session has expired. Please log in again.';
              // Perform immediate logout for security
              authService.logout();
            }
            break;

          case 403: // Forbidden
            message = backendMessage || 'You don\'t have permission to perform this action. Please contact your administrator if you need access.';
            break;

          case 404: // Not Found
            message = backendMessage || 'The requested resource could not be found. It may have been removed or is temporarily unavailable.';
            break;

          case 409: // Conflict
            message = backendMessage || 'This operation conflicts with existing data. Please refresh the page and try again.';
            break;

          case 422: // Unprocessable Entity
            message = backendMessage || 'The data provided cannot be processed. Please verify your input.';
            break;

          case 500: // Internal Server Error
            message = backendMessage || 'A server error occurred. Our team has been notified. Please try again later.';
            break;

          case 502: // Bad Gateway
            message = 'The server is temporarily unavailable. Please try again in a few moments.';
            break;

          case 503: // Service Unavailable
            message = backendMessage || 'The service is temporarily unavailable due to maintenance. Please try again shortly.';
            break;

          case 504: // Gateway Timeout
            message = 'The request took too long to process. Please try again.';
            break;

          default:
            // For any other error, use backend message or generic fallback
            message = backendMessage || `An unexpected error occurred (Error ${error.status}). Please try again or contact support if the issue persists.`;
        }
      }

      // Show user-friendly notification (only if message is not empty and should be shown)
      if (message && showNotification) {
        notificationService.showError(message, 'Error');
      }

      // Pass the error to component-level handlers if needed
      return throwError(() => error);
    })
  );
}; 