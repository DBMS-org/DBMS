import { ApplicationConfig, provideZoneChangeDetection, ErrorHandler } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { provideAnimations } from '@angular/platform-browser/animations';

import { routes } from './app.routes';
import { authInterceptor } from './core/interceptors/auth.interceptor';
import { errorInterceptor } from './core/interceptors/error.interceptor';
import { dataInterceptor } from './core/interceptors/data.interceptor';
import { GlobalErrorHandler } from './core/handlers/global-error.handler';
import { MessageService, ConfirmationService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';

// Main application configuration with providers for routing, HTTP, and services
export const appConfig: ApplicationConfig = {
  providers: [
    // Enable zone change detection for performance
    provideZoneChangeDetection({ eventCoalescing: true }),

    // Configure routing and animations
    provideRouter(routes),
    provideAnimations(),

    // Provide PrimeNG services for UI components
    MessageService,
    ConfirmationService,
    DialogService,

    // Setup HTTP client with auth, error, and data interceptors
    provideHttpClient(withInterceptors([authInterceptor, errorInterceptor, dataInterceptor])),

    // Global error handling
    { provide: ErrorHandler, useClass: GlobalErrorHandler }
  ]
};
