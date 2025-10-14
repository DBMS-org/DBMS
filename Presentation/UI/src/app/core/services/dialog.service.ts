import { Injectable } from '@angular/core';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { ConfirmDialogComponent, ConfirmDialogData } from '../../shared/shared/components/confirm-dialog/confirm-dialog.component';
import { LogoutConfirmationDialogComponent, LogoutDialogData } from '../../shared/shared/components/logout-confirmation-dialog/logout-confirmation-dialog.component';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AppDialogService {
  constructor(private dialogService: DialogService) {}

  /**
   * Opens a confirmation dialog
   * @param data - Dialog configuration
   * @returns Observable<boolean> - true if confirmed, false if cancelled
   */
  openConfirmDialog(data: ConfirmDialogData): Observable<boolean> {
    const ref: DynamicDialogRef = this.dialogService.open(ConfirmDialogComponent, {
      header: data.title || 'Confirm',
      width: '450px',
      modal: true,
      data: data
    });

    return new Observable(observer => {
      ref.onClose.subscribe((result: boolean) => {
        observer.next(result || false);
        observer.complete();
      });
    });
  }

  /**
   * Opens a logout confirmation dialog
   * @param userName - Optional user name to display
   * @returns Observable<boolean> - true if confirmed, false if cancelled
   */
  openLogoutDialog(userName?: string): Observable<boolean> {
    const ref: DynamicDialogRef = this.dialogService.open(LogoutConfirmationDialogComponent, {
      header: 'Confirm Logout',
      width: '500px',
      modal: true,
      data: { userName } as LogoutDialogData
    });

    return new Observable(observer => {
      ref.onClose.subscribe((result: boolean) => {
        observer.next(result || false);
        observer.complete();
      });
    });
  }

  /**
   * Quick confirmation dialog with simple message
   * @param message - The confirmation message
   * @param title - Optional title (defaults to 'Confirm')
   * @returns Observable<boolean>
   */
  confirm(message: string, title?: string): Observable<boolean> {
    return this.openConfirmDialog({
      message,
      title,
      confirmText: 'OK',
      cancelText: 'Cancel'
    });
  }

  /**
   * Quick delete confirmation dialog
   * @param itemName - Name of the item to delete
   * @returns Observable<boolean>
   */
  confirmDelete(itemName: string): Observable<boolean> {
    return this.openConfirmDialog({
      title: 'Confirm Deletion',
      message: `Are you sure you want to delete "${itemName}"? This action cannot be undone.`,
      confirmText: 'Delete',
      cancelText: 'Cancel'
    });
  }
}
