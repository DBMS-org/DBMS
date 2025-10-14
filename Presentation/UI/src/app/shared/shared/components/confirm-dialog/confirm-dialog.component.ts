import { Component, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { ButtonModule } from 'primeng/button';

export interface ConfirmDialogData {
  title?: string;
  message: string;
  confirmText?: string;
  cancelText?: string;
}

@Component({
  selector: 'app-confirm-dialog',
  standalone: true,
  imports: [CommonModule, ButtonModule],
  template: `
    <div class="p-6 animate-fade-in">
      <!-- Icon and Message -->
      <div class="flex items-start gap-4 mb-6">
        <div class="flex-shrink-0">
          <div class="w-12 h-12 rounded-full bg-blue-100 flex items-center justify-center animate-scale-in">
            <i class="pi pi-question-circle text-blue-600 text-2xl"></i>
          </div>
        </div>
        <div class="flex-1 pt-1">
          <p class="text-gray-800 text-base leading-relaxed font-medium">{{ data.message }}</p>
        </div>
      </div>

      <!-- Action Buttons -->
      <div class="flex justify-end gap-3 pt-4 border-t border-gray-200">
        <button
          pButton
          [label]="data.cancelText || 'Cancel'"
          icon="pi pi-times"
          (click)="onCancel()"
          class="p-button-text p-button-secondary hover:bg-gray-100"
        ></button>
        <button
          pButton
          [label]="data.confirmText || 'OK'"
          icon="pi pi-check"
          (click)="onConfirm()"
          class="p-button-primary shadow-md hover:shadow-lg transition-shadow"
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

    @keyframes scale-in {
      from {
        transform: scale(0.5);
        opacity: 0;
      }
      to {
        transform: scale(1);
        opacity: 1;
      }
    }

    .animate-fade-in {
      animation: fade-in 0.3s ease-out;
    }

    .animate-scale-in {
      animation: scale-in 0.4s ease-out;
    }
  `]
})
export class ConfirmDialogComponent {
  data: ConfirmDialogData;

  constructor(
    private dialogRef: DynamicDialogRef,
    private config: DynamicDialogConfig
  ) {
    this.data = this.config.data as ConfirmDialogData;
  }

  onConfirm(): void {
    this.dialogRef.close(true);
  }

  onCancel(): void {
    this.dialogRef.close(false);
  }
} 