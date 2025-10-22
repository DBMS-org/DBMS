import { ApplicationConfig, provideZoneChangeDetection, ErrorHandler } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { provideAnimations } from '@angular/platform-browser/animations';
import { providePrimeNG } from 'primeng/config';
import Aura from '@primeng/themes/aura';
import { definePreset } from '@primeng/themes';

import { routes } from './app.routes';
import { authInterceptor } from './core/interceptors/auth.interceptor';
import { errorInterceptor } from './core/interceptors/error.interceptor';
import { dataInterceptor } from './core/interceptors/data.interceptor';
import { GlobalErrorHandler } from './core/handlers/global-error.handler';
import { MessageService, ConfirmationService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';

// Custom light theme preset based on your design system
const CustomPreset = definePreset(Aura, {
  semantic: {
    primary: {
      50: '{purple.50}',
      100: '{purple.100}',
      200: '{purple.200}',
      300: '{purple.300}',
      400: '{purple.400}',
      500: '{purple.500}',
      600: '{purple.600}',
      700: '{purple.700}',
      800: '{purple.800}',
      900: '{purple.900}',
      950: '{purple.950}'
    },
    colorScheme: {
      light: {
        primary: {
          color: '#667eea',
          contrastColor: '#ffffff',
          hoverColor: '#5a67d8',
          activeColor: '#4c51bf'
        },
        surface: {
          0: '#ffffff',
          50: '#f8f9fc',
          100: '#f3f4f6',
          200: '#e9ecef',
          300: '#dee2e6',
          400: '#ced4da',
          500: '#adb5bd',
          600: '#6c757d',
          700: '#495057',
          800: '#343a40',
          900: '#212529',
          950: '#0a0a0a'
        }
      }
    }
  }
});

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideAnimations(),
    providePrimeNG({
      theme: {
        preset: CustomPreset,
        options: {
          darkModeSelector: false, // Disable dark mode
          cssLayer: {
            name: 'primeng',
            order: 'tailwind-base, primeng, tailwind-utilities'
          }
        }
      }
    }),
    MessageService,
    ConfirmationService,
    DialogService,
    provideHttpClient(withInterceptors([authInterceptor, errorInterceptor, dataInterceptor])),
    { provide: ErrorHandler, useClass: GlobalErrorHandler }
  ]
};
