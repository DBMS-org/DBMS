import { Injectable, inject } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';

export interface MaintenanceError {
  message: string;
  code?: string;
  details?: any;
  retryable?: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class MaintenanceErrorHandlerService {
  
  handleError(error: HttpErrorResponse | Error): Observable<never> {
    const maintenanceError = this.processError(error);
    
    // Log error for debugging
    console.error('Maintenance Error:', error);
    
    // You could integrate with a notification service here
    // this.notificationService.showError(maintenanceError.message);
    
    return throwError(() => maintenanceError);
  }

  private processError(error: HttpErrorResponse | Error): MaintenanceError {
    if (error instanceof HttpErrorResponse) {
      return this.handleHttpError(error);
    } else {
      return this.handleGenericError(error);
    }
  }

  private handleHttpError(error: HttpErrorResponse): MaintenanceError {
    let message = 'An unexpected error occurred';
    let code = 'UNKNOWN_ERROR';
    let retryable = false;

    switch (error.status) {
      case 400:
        message = this.getBadRequestMessage(error);
        code = 'BAD_REQUEST';
        retryable = false;
        break;
        
      case 401:
        message = 'Your session has expired. Please log in again.';
        code = 'UNAUTHORIZED';
        retryable = false;
        break;
        
      case 403:
        message = 'You do not have permission to perform this maintenance operation.';
        code = 'FORBIDDEN';
        retryable = false;
        break;
        
      case 404:
        message = this.getNotFoundMessage(error);
        code = 'NOT_FOUND';
        retryable = false;
        break;
        
      case 409:
        message = this.getConflictMessage(error);
        code = 'CONFLICT';
        retryable = false;
        break;
        
      case 422:
        message = this.getValidationMessage(error);
        code = 'VALIDATION_ERROR';
        retryable = false;
        break;
        
      case 429:
        message = 'Too many requests. Please wait a moment and try again.';
        code = 'RATE_LIMITED';
        retryable = true;
        break;
        
      case 500:
        message = 'Server error occurred while processing maintenance data. Please try again.';
        code = 'SERVER_ERROR';
        retryable = true;
        break;
        
      case 502:
      case 503:
      case 504:
        message = 'Maintenance service is temporarily unavailable. Please try again later.';
        code = 'SERVICE_UNAVAILABLE';
        retryable = true;
        break;
        
      case 0:
        message = 'Network connection error. Please check your internet connection.';
        code = 'NETWORK_ERROR';
        retryable = true;
        break;
        
      default:
        message = `Unexpected error occurred (${error.status}). Please try again.`;
        code = 'HTTP_ERROR';
        retryable = true;
    }

    return {
      message,
      code,
      details: error.error,
      retryable
    };
  }

  private handleGenericError(error: Error): MaintenanceError {
    return {
      message: error.message || 'An unexpected error occurred',
      code: 'GENERIC_ERROR',
      details: error,
      retryable: false
    };
  }

  private getBadRequestMessage(error: HttpErrorResponse): string {
    if (error.error?.message) {
      return error.error.message;
    }
    
    if (error.error?.errors) {
      // Handle validation errors
      const errors = Object.values(error.error.errors).flat();
      return errors.length > 0 ? errors[0] as string : 'Invalid request data';
    }
    
    return 'Invalid maintenance data provided. Please check your input.';
  }

  private getNotFoundMessage(error: HttpErrorResponse): string {
    const url = error.url || '';
    
    if (url.includes('/jobs/')) {
      return 'Maintenance job not found. It may have been deleted or moved.';
    }
    
    if (url.includes('/machines/')) {
      return 'Machine not found. Please verify the machine ID.';
    }
    
    if (url.includes('/alerts/')) {
      return 'Maintenance alert not found.';
    }
    
    if (url.includes('/analytics/')) {
      return 'Analytics data not available.';
    }
    
    return 'Requested maintenance data not found.';
  }

  private getConflictMessage(error: HttpErrorResponse): string {
    if (error.error?.message) {
      return error.error.message;
    }
    
    return 'Maintenance operation conflicts with existing data. Please refresh and try again.';
  }

  private getValidationMessage(error: HttpErrorResponse): string {
    if (error.error?.message) {
      return error.error.message;
    }
    
    if (error.error?.errors) {
      const errors = Object.entries(error.error.errors)
        .map(([field, messages]) => `${field}: ${(messages as string[]).join(', ')}`)
        .join('; ');
      return `Validation failed: ${errors}`;
    }
    
    return 'Maintenance data validation failed. Please check your input.';
  }

  // Utility methods for components
  isRetryableError(error: MaintenanceError): boolean {
    return error.retryable === true;
  }

  isNetworkError(error: MaintenanceError): boolean {
    return error.code === 'NETWORK_ERROR';
  }

  isAuthenticationError(error: MaintenanceError): boolean {
    return error.code === 'UNAUTHORIZED';
  }

  isPermissionError(error: MaintenanceError): boolean {
    return error.code === 'FORBIDDEN';
  }

  isValidationError(error: MaintenanceError): boolean {
    return error.code === 'VALIDATION_ERROR' || error.code === 'BAD_REQUEST';
  }

  getRetryDelay(error: MaintenanceError): number {
    switch (error.code) {
      case 'RATE_LIMITED':
        return 5000; // 5 seconds
      case 'SERVER_ERROR':
      case 'SERVICE_UNAVAILABLE':
        return 3000; // 3 seconds
      case 'NETWORK_ERROR':
        return 2000; // 2 seconds
      default:
        return 1000; // 1 second
    }
  }

  getUserFriendlyMessage(error: MaintenanceError): string {
    // Return a more user-friendly version of technical errors
    switch (error.code) {
      case 'NETWORK_ERROR':
        return 'Connection problem. Please check your internet and try again.';
      case 'SERVER_ERROR':
        return 'Something went wrong on our end. We\'re working to fix it.';
      case 'SERVICE_UNAVAILABLE':
        return 'Maintenance system is temporarily down. Please try again in a few minutes.';
      case 'RATE_LIMITED':
        return 'You\'re doing that too quickly. Please wait a moment.';
      default:
        return error.message;
    }
  }
}