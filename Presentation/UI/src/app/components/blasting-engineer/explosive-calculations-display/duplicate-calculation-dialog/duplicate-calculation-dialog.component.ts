import { Component, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

export interface DuplicateCalculationDialogData {
  projectId: number;
  siteId: number;
  existingCalculationId?: string;
  existingCalculationDate?: string;
}

export interface DuplicateCalculationDialogResult {
  action: 'cancel' | 'replace' | 'view_existing';
}

@Component({
  selector: 'app-duplicate-calculation-dialog',
  standalone: true,
  imports: [CommonModule, MatDialogModule, MatButtonModule, MatIconModule],
  template: `
    <div class="duplicate-dialog-container">
      <div class="dialog-header">
        <mat-icon class="warning-icon">warning</mat-icon>
        <h2 mat-dialog-title>Calculation Already Exists</h2>
      </div>
      
      <mat-dialog-content class="dialog-content">
        <div class="alert-section">
          <div class="alert-message">
            <p class="primary-message">
              A calculation already exists for this site. You can only have one calculation per site.
            </p>
            <div class="existing-calculation-info" *ngIf="data.existingCalculationId">
              <div class="info-item">
                <mat-icon>calculate</mat-icon>
                <span>Existing Calculation ID: <strong>{{ data.existingCalculationId }}</strong></span>
              </div>
              <div class="info-item" *ngIf="data.existingCalculationDate">
                <mat-icon>schedule</mat-icon>
                <span>Created: <strong>{{ data.existingCalculationDate | date:'medium' }}</strong></span>
              </div>
            </div>
          </div>
        </div>
        
        <div class="options-section">
          <h3>What would you like to do?</h3>
          <div class="option-cards">
            <div class="option-card replace-option">
              <div class="option-header">
                <mat-icon>swap_horiz</mat-icon>
                <h4>Replace Existing Calculation</h4>
              </div>
              <p>Delete the existing calculation and save your new one. This action cannot be undone.</p>
              <div class="option-warning">
                <mat-icon>info</mat-icon>
                <span>The previous calculation data will be permanently lost.</span>
              </div>
            </div>
            
            <div class="option-card view-option">
              <div class="option-header">
                <mat-icon>visibility</mat-icon>
                <h4>View Existing Calculation</h4>
              </div>
              <p>Load and review the existing calculation before deciding whether to replace it.</p>
            </div>
          </div>
        </div>
      </mat-dialog-content>
      
      <mat-dialog-actions class="dialog-actions">
        <button mat-button (click)="onCancel()" class="cancel-btn">
          <mat-icon>cancel</mat-icon>
          Cancel
        </button>
        <button mat-button (click)="onViewExisting()" class="view-btn">
          <mat-icon>visibility</mat-icon>
          View Existing
        </button>
        <button mat-raised-button color="warn" (click)="onReplace()" class="replace-btn">
          <mat-icon>swap_horiz</mat-icon>
          Replace Calculation
        </button>
      </mat-dialog-actions>
    </div>
  `,
  styleUrls: ['./duplicate-calculation-dialog.component.scss']
})
export class DuplicateCalculationDialogComponent {
  constructor(
    private dialogRef: MatDialogRef<DuplicateCalculationDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DuplicateCalculationDialogData
  ) {}

  onCancel(): void {
    this.dialogRef.close({ action: 'cancel' } as DuplicateCalculationDialogResult);
  }

  onReplace(): void {
    this.dialogRef.close({ action: 'replace' } as DuplicateCalculationDialogResult);
  }

  onViewExisting(): void {
    this.dialogRef.close({ action: 'view_existing' } as DuplicateCalculationDialogResult);
  }
}