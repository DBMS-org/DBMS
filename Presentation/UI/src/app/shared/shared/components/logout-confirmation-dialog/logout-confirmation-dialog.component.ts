import { Component, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

export interface LogoutDialogData {
  userName?: string;
}

@Component({
  selector: 'app-logout-confirmation-dialog',
  standalone: true,
  imports: [CommonModule, MatDialogModule, MatButtonModule, MatIconModule],
  template: `
    <div class="logout-dialog">
      <div class="dialog-header">
        <mat-icon class="warning-icon">warning</mat-icon>
        <h2 mat-dialog-title>Confirm Logout</h2>
      </div>
      
      <div mat-dialog-content class="dialog-content">
        <p *ngIf="data.userName; else genericMessage">
          Are you sure you want to logout, <strong>{{data.userName}}</strong>?
        </p>
        <ng-template #genericMessage>
          <p>Are you sure you want to logout?</p>
        </ng-template>
        <p class="warning-text">
          You will need to login again to continue using the application.
        </p>
      </div>
      
      <div mat-dialog-actions class="dialog-actions">
        <button mat-button (click)="onCancel()" class="cancel-btn">
          Cancel
        </button>
        <button mat-raised-button color="warn" (click)="onConfirm()" class="logout-btn">
          <mat-icon>logout</mat-icon>
          Logout
        </button>
      </div>
    </div>
  `,
  styles: [`
    .logout-dialog {
      min-width: 400px;
      padding: 20px;
    }
    
    .dialog-header {
      display: flex;
      align-items: center;
      gap: 12px;
      margin-bottom: 20px;
    }
    
    .warning-icon {
      color: #ff9800;
      font-size: 32px;
      width: 32px;
      height: 32px;
    }
    
    h2 {
      margin: 0;
      color: #333;
      font-weight: 500;
    }
    
    .dialog-content {
      margin-bottom: 24px;
    }
    
    .dialog-content p {
      margin: 0 0 12px 0;
      color: #666;
      line-height: 1.5;
    }
    
    .warning-text {
      font-size: 0.9em;
      color: #999;
      font-style: italic;
    }
    
    .dialog-actions {
      display: flex;
      justify-content: flex-end;
      gap: 12px;
      margin-top: 24px;
    }
    
    .cancel-btn {
      color: #666;
    }
    
    .logout-btn {
      background-color: #f44336;
      color: white;
    }
    
    .logout-btn:hover {
      background-color: #d32f2f;
    }
    
    .logout-btn mat-icon {
      margin-right: 8px;
      font-size: 18px;
      width: 18px;
      height: 18px;
    }
  `]
})
export class LogoutConfirmationDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<LogoutConfirmationDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: LogoutDialogData
  ) {}

  onCancel(): void {
    this.dialogRef.close(false);
  }

  onConfirm(): void {
    this.dialogRef.close(true);
  }
} 