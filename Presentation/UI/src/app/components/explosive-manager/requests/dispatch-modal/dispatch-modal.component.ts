import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { Subject, takeUntil } from 'rxjs';
import { InventoryTransferService } from '../../../../core/services/inventory-transfer.service';
import {
  InventoryTransferRequest,
  DispatchTransferRequest
} from '../../../../core/models/inventory-transfer.model';

@Component({
  selector: 'app-dispatch-modal',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    MatNativeDateModule
  ],
  template: `
    <div class="modal-overlay" (click)="onClose()">
      <div class="modal-content" (click)="$event.stopPropagation()">
        <div class="modal-header">
          <h2>Dispatch Transfer Request</h2>
          <button class="close-btn" (click)="onClose()">
            <span>&times;</span>
          </button>
        </div>

        <div class="modal-body">
          <div class="request-info">
            <h3>Request Information</h3>
            <div class="info-row">
              <span class="label">Request Number:</span>
              <span class="value">{{ request?.requestNumber }}</span>
            </div>
            <div class="info-row">
              <span class="label">Explosive Type:</span>
              <span class="value">{{ request?.explosiveTypeName }}</span>
            </div>
            <div class="info-row">
              <span class="label">Quantity:</span>
              <span class="value">{{ request?.finalQuantity }} {{ request?.unit }}</span>
            </div>
            <div class="info-row">
              <span class="label">Destination:</span>
              <span class="value">{{ request?.destinationStoreName }}</span>
            </div>
          </div>

          <form [formGroup]="dispatchForm" class="dispatch-form">
            <div class="form-row">
              <mat-form-field appearance="outline" class="full-width">
                <mat-label>Truck Number</mat-label>
                <input matInput formControlName="truckNumber" placeholder="e.g., TRK-001" required>
                <mat-error *ngIf="dispatchForm.get('truckNumber')?.hasError('required')">
                  Truck number is required
                </mat-error>
                <mat-error *ngIf="dispatchForm.get('truckNumber')?.hasError('maxlength')">
                  Maximum 50 characters allowed
                </mat-error>
              </mat-form-field>
            </div>

            <div class="form-row">
              <mat-form-field appearance="outline" class="full-width">
                <mat-label>Driver Name</mat-label>
                <input matInput formControlName="driverName" placeholder="Full name" required>
                <mat-error *ngIf="dispatchForm.get('driverName')?.hasError('required')">
                  Driver name is required
                </mat-error>
                <mat-error *ngIf="dispatchForm.get('driverName')?.hasError('maxlength')">
                  Maximum 200 characters allowed
                </mat-error>
              </mat-form-field>
            </div>

            <div class="form-row">
              <mat-form-field appearance="outline" class="full-width">
                <mat-label>Driver Contact Number</mat-label>
                <input matInput formControlName="driverContactNumber" placeholder="Phone number (optional)">
                <mat-error *ngIf="dispatchForm.get('driverContactNumber')?.hasError('maxlength')">
                  Maximum 20 characters allowed
                </mat-error>
              </mat-form-field>
            </div>

            <div class="form-row">
              <mat-form-field appearance="outline" class="full-width">
                <mat-label>Dispatch Notes</mat-label>
                <textarea
                  matInput
                  formControlName="dispatchNotes"
                  rows="4"
                  placeholder="Any special instructions or notes (optional)">
                </textarea>
                <mat-error *ngIf="dispatchForm.get('dispatchNotes')?.hasError('maxlength')">
                  Maximum 1000 characters allowed
                </mat-error>
              </mat-form-field>
            </div>

            <div class="error-message" *ngIf="errorMessage">
              {{ errorMessage }}
            </div>
          </form>
        </div>

        <div class="modal-footer">
          <button mat-button (click)="onClose()" [disabled]="loading">Cancel</button>
          <button
            mat-raised-button
            color="primary"
            (click)="onSubmit()"
            [disabled]="!dispatchForm.valid || loading">
            <span *ngIf="!loading">Dispatch</span>
            <span *ngIf="loading">Dispatching...</span>
          </button>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .modal-overlay {
      position: fixed;
      top: 0;
      left: 0;
      width: 100%;
      height: 100%;
      background: rgba(0, 0, 0, 0.5);
      display: flex;
      align-items: center;
      justify-content: center;
      z-index: 1000;
      animation: fadeIn 0.2s ease;
    }

    @keyframes fadeIn {
      from { opacity: 0; }
      to { opacity: 1; }
    }

    .modal-content {
      background: white;
      border-radius: 12px;
      max-width: 600px;
      width: 90%;
      max-height: 90vh;
      overflow: auto;
      box-shadow: 0 10px 40px rgba(0, 0, 0, 0.2);
      animation: slideUp 0.3s ease;
    }

    @keyframes slideUp {
      from {
        transform: translateY(20px);
        opacity: 0;
      }
      to {
        transform: translateY(0);
        opacity: 1;
      }
    }

    .modal-header {
      padding: 24px;
      border-bottom: 1px solid #e0e0e0;
      display: flex;
      justify-content: space-between;
      align-items: center;
    }

    .modal-header h2 {
      margin: 0;
      font-size: 24px;
      font-weight: 600;
      color: #1a237e;
    }

    .close-btn {
      background: none;
      border: none;
      font-size: 32px;
      color: #666;
      cursor: pointer;
      padding: 0;
      width: 32px;
      height: 32px;
      display: flex;
      align-items: center;
      justify-content: center;
      border-radius: 50%;
      transition: all 0.2s;
    }

    .close-btn:hover {
      background: #f5f5f5;
      color: #333;
    }

    .modal-body {
      padding: 24px;
    }

    .request-info {
      background: #f5f7fa;
      padding: 16px;
      border-radius: 8px;
      margin-bottom: 24px;
    }

    .request-info h3 {
      margin: 0 0 12px 0;
      font-size: 16px;
      font-weight: 600;
      color: #333;
    }

    .info-row {
      display: flex;
      margin-bottom: 8px;
    }

    .info-row:last-child {
      margin-bottom: 0;
    }

    .info-row .label {
      font-weight: 500;
      color: #666;
      min-width: 150px;
    }

    .info-row .value {
      color: #333;
      font-weight: 500;
    }

    .dispatch-form {
      margin-top: 20px;
    }

    .form-row {
      margin-bottom: 16px;
    }

    .full-width {
      width: 100%;
    }

    .error-message {
      background: #ffebee;
      color: #c62828;
      padding: 12px;
      border-radius: 4px;
      margin-top: 16px;
      font-size: 14px;
    }

    .modal-footer {
      padding: 16px 24px;
      border-top: 1px solid #e0e0e0;
      display: flex;
      justify-content: flex-end;
      gap: 12px;
    }
  `]
})
export class DispatchModalComponent implements OnInit {
  @Input() request: InventoryTransferRequest | null = null;
  @Output() close = new EventEmitter<void>();
  @Output() dispatchComplete = new EventEmitter<void>();

  private destroy$ = new Subject<void>();
  dispatchForm: FormGroup;
  loading = false;
  errorMessage: string | null = null;

  constructor(
    private fb: FormBuilder,
    private transferService: InventoryTransferService
  ) {
    this.dispatchForm = this.fb.group({
      truckNumber: ['', [Validators.required, Validators.maxLength(50)]],
      driverName: ['', [Validators.required, Validators.maxLength(200)]],
      driverContactNumber: ['', Validators.maxLength(20)],
      dispatchNotes: ['', Validators.maxLength(1000)]
    });
  }

  ngOnInit(): void {
    // Form is already initialized in constructor
  }

  onClose(): void {
    this.close.emit();
  }

  onSubmit(): void {
    if (this.dispatchForm.valid && this.request) {
      this.loading = true;
      this.errorMessage = null;

      const dispatchData: DispatchTransferRequest = {
        truckNumber: this.dispatchForm.value.truckNumber,
        driverName: this.dispatchForm.value.driverName,
        driverContactNumber: this.dispatchForm.value.driverContactNumber || undefined,
        dispatchNotes: this.dispatchForm.value.dispatchNotes || undefined
      };

      this.transferService.dispatchTransferRequest(this.request.id, dispatchData)
        .pipe(takeUntil(this.destroy$))
        .subscribe({
          next: () => {
            this.loading = false;
            this.dispatchComplete.emit();
          },
          error: (error) => {
            this.loading = false;
            this.errorMessage = error.message || 'Failed to dispatch request';
          }
        });
    }
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
