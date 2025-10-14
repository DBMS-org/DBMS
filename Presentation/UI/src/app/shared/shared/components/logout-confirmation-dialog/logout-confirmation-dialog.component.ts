import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { ButtonModule } from 'primeng/button';
import { RippleModule } from 'primeng/ripple';

export interface LogoutDialogData {
  userName?: string;
}

@Component({
  selector: 'app-logout-confirmation-dialog',
  standalone: true,
  imports: [CommonModule, ButtonModule, RippleModule],
  template: `
    <div class="p-6 animate-fade-in">
      <!-- Icon and Message -->
      <div class="flex items-start gap-4 mb-6">
        <div class="flex-shrink-0">
          <div class="w-16 h-16 rounded-full bg-gradient-to-br from-orange-100 to-red-100 flex items-center justify-center animate-pulse-scale shadow-lg">
            <i class="pi pi-sign-out text-orange-600 text-3xl"></i>
          </div>
        </div>
        <div class="flex-1 pt-2">
          <h3 class="text-lg font-bold text-gray-800 mb-2">
            Confirm Logout
          </h3>
          <p class="text-gray-700 text-base leading-relaxed" *ngIf="data.userName; else genericMessage">
            Are you sure you want to logout, <strong class="text-purple-600">{{data.userName}}</strong>?
          </p>
          <ng-template #genericMessage>
            <p class="text-gray-700 text-base leading-relaxed">
              Are you sure you want to logout?
            </p>
          </ng-template>
          <div class="mt-3 flex items-center gap-2 text-sm text-gray-500 bg-gray-50 rounded-lg px-3 py-2">
            <i class="pi pi-info-circle text-blue-500"></i>
            <span>You will need to login again to continue using the application.</span>
          </div>
        </div>
      </div>

      <!-- Action Buttons -->
      <div class="flex justify-end gap-3 pt-4 border-t border-gray-200">
        <button
          pButton
          pRipple
          type="button"
          label="Cancel"
          icon="pi pi-times"
          (click)="onCancel()"
          class="p-button-outlined p-button-secondary"
        ></button>
        <button
          pButton
          pRipple
          type="button"
          label="Logout"
          icon="pi pi-sign-out"
          (click)="onConfirm()"
          class="p-button-danger"
        ></button>
      </div>
    </div>
  `,
  styles: [`
    @keyframes fade-in {
      from {
        opacity: 0;
        transform: translateY(-10px);
      }
      to {
        opacity: 1;
        transform: translateY(0);
      }
    }

    @keyframes pulse-scale {
      0%, 100% {
        transform: scale(1);
      }
      50% {
        transform: scale(1.05);
      }
    }

    .animate-fade-in {
      animation: fade-in 0.3s ease-out;
    }

    .animate-pulse-scale {
      animation: pulse-scale 2s ease-in-out infinite;
    }

    /* Ensure buttons have proper styling */
    button {
      min-width: 100px;
      padding: 0.75rem 1.5rem;
      font-size: 1rem;
      font-weight: 500;
    }

    .p-button-outlined {
      background-color: transparent !important;
      border: 2px solid #6c757d !important;
      color: #6c757d !important;
    }

    .p-button-outlined:hover {
      background-color: #f8f9fa !important;
      border-color: #5a6268 !important;
      color: #5a6268 !important;
    }

    .p-button-danger {
      background-color: #dc3545 !important;
      border: 2px solid #dc3545 !important;
      color: white !important;
    }

    .p-button-danger:hover {
      background-color: #c82333 !important;
      border-color: #bd2130 !important;
    }
  `]
})
export class LogoutConfirmationDialogComponent {
  data: LogoutDialogData;

  constructor(
    private dialogRef: DynamicDialogRef,
    private config: DynamicDialogConfig
  ) {
    this.data = this.config.data as LogoutDialogData;
  }

  onCancel(): void {
    this.dialogRef.close(false);
  }

  onConfirm(): void {
    this.dialogRef.close(true);
  }
} 