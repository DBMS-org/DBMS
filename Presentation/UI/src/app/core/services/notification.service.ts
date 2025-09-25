import { Injectable } from '@angular/core';
import { MatSnackBar, MatSnackBarConfig } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private readonly defaultConfig: MatSnackBarConfig = {
    duration: 3000,
    horizontalPosition: 'right',
    verticalPosition: 'top'
  };

  constructor(private snackBar: MatSnackBar) {}

  showSuccess(message: string, action: string = 'OK', config?: MatSnackBarConfig): void {
    this.snackBar.open(message, action, {
      ...this.defaultConfig,
      panelClass: ['snackbar-success'],
      ...config
    });
  }

  showError(message: string, action: string = 'Dismiss', config?: MatSnackBarConfig): void {
    this.snackBar.open(message, action, {
      ...this.defaultConfig,
      panelClass: ['snackbar-error'],
      ...config
    });
  }

  showWarning(message: string, action: string = 'OK', config?: MatSnackBarConfig): void {
    this.snackBar.open(message, action, {
      ...this.defaultConfig,
      panelClass: ['snackbar-warning'],
      ...config
    });
  }
} 