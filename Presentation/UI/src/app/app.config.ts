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

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideAnimations(),
    MessageService,
    ConfirmationService,
    DialogService,
    provideHttpClient(withInterceptors([authInterceptor, errorInterceptor, dataInterceptor])),
    { provide: ErrorHandler, useClass: GlobalErrorHandler }
  ]
};
