import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { AppComponent } from './app/app.component';

// Global error handler to suppress browser extension interference
window.addEventListener('error', (event) => {
  // Suppress content script errors that don't affect our app
  if (event.filename && event.filename.includes('content.js') && 
      event.message && event.message.includes('is not valid JSON')) {
    console.warn('Suppressed browser extension error:', event.message);
    event.preventDefault();
    return false;
  }
  return true; // Let other errors bubble up normally
});

// Handle unhandled promise rejections from extensions
window.addEventListener('unhandledrejection', (event) => {
  if (event.reason && typeof event.reason === 'object' && 
      event.reason.message && event.reason.message.includes('is not valid JSON')) {
    console.warn('Suppressed browser extension promise rejection:', event.reason.message);
    event.preventDefault();
  }
});

bootstrapApplication(AppComponent, appConfig)
  .catch((err) => console.error(err));
