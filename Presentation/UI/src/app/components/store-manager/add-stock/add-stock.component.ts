import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Subject, takeUntil } from 'rxjs';
import { InventoryTransferService } from '../../../core/services/inventory-transfer.service';
import { CentralInventoryService } from '../../../core/services/central-inventory.service';
import {
  CreateTransferRequest,
  TransferRequestStatus
} from '../../../core/models/inventory-transfer.model';
import { CentralInventory, ExplosiveType } from '../../../core/models/central-inventory.model';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

@Component({
  selector: 'app-add-stock',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, MatSnackBarModule],
  templateUrl: './add-stock.component.html',
  styleUrls: ['./add-stock.component.scss']
})
export class AddStockComponent implements OnInit, OnDestroy {
  private destroy$ = new Subject<void>();

  stockRequestForm!: FormGroup;

  isSubmitting = false;
  isLoadingInventory = false;

  successMessage = '';
  errorMessage = '';

  ExplosiveType = ExplosiveType;
  TransferRequestStatus = TransferRequestStatus;

  availableInventory: CentralInventory[] = [];
  filteredInventory: CentralInventory[] = [];
  selectedBatch: CentralInventory | null = null;

  // User and store information (would typically come from auth service)
  currentUser = {
    name: 'Store Manager',
    role: 'Store Manager'
  };

  currentStore = {
    id: 1,
    name: 'Field Storage Site',
    manager: 'Store Manager'
  };

  constructor(
    private transferService: InventoryTransferService,
    private inventoryService: CentralInventoryService,
    private fb: FormBuilder,
    private snackBar: MatSnackBar
  ) {
    this.initializeForm();
  }

  ngOnInit(): void {
    this.loadAvailableInventory();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private initializeForm(): void {
    this.stockRequestForm = this.fb.group({
      explosiveType: ['', Validators.required],
      batchId: ['', Validators.required],
      requestedQuantity: ['', [Validators.required, Validators.min(0.1)]],
      unit: ['kg', Validators.required],
      requiredDate: ['', Validators.required],
      notes: ['']
    });
  }

  loadAvailableInventory(): void {
    this.isLoadingInventory = true;
    this.inventoryService.getInventory({ pageSize: 100 })
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (response) => {
          // Filter to only show available inventory (not expired, quarantined, or depleted)
          this.availableInventory = response.items.filter(
            item => item.availableQuantity > 0 && item.status === 'Available'
          );
          this.filteredInventory = this.availableInventory;
          this.isLoadingInventory = false;
        },
        error: (error) => {
          console.error('Error loading inventory:', error);
          this.snackBar.open('Failed to load available inventory', 'Close', { duration: 3000 });
          this.isLoadingInventory = false;
        }
      });
  }

  onExplosiveTypeChange(): void {
    const explosiveType = this.stockRequestForm.get('explosiveType')?.value;
    this.stockRequestForm.get('batchId')?.setValue('');
    this.selectedBatch = null;

    if (explosiveType) {
      this.filteredInventory = this.availableInventory.filter(
        inv => inv.explosiveType === explosiveType
      );
    } else {
      this.filteredInventory = this.availableInventory;
    }
  }

  onBatchChange(): void {
    const batchId = this.stockRequestForm.get('batchId')?.value;
    this.selectedBatch = this.availableInventory.find(inv => inv.id === parseInt(batchId)) || null;

    if (this.selectedBatch) {
      this.stockRequestForm.get('unit')?.setValue(this.selectedBatch.unit);
    }
  }

  getExplosiveTypes(): ExplosiveType[] {
    const types = new Set(this.availableInventory.map(inv => inv.explosiveType));
    return Array.from(types);
  }

  getCurrentDateTime(): string {
    const now = new Date();
    return now.toLocaleString('en-US', {
      year: 'numeric',
      month: 'short',
      day: '2-digit',
      hour: '2-digit',
      minute: '2-digit',
      second: '2-digit',
      hour12: true
    });
  }



  onSubmit(): void {
    if (!this.selectedBatch) {
      this.showError('Please select a batch from central warehouse');
      return;
    }

    if (this.stockRequestForm.valid) {
      this.isSubmitting = true;

      const formValue = this.stockRequestForm.value;

      const request: CreateTransferRequest = {
        centralWarehouseInventoryId: this.selectedBatch.id,
        destinationStoreId: this.currentStore.id,
        requestedQuantity: parseFloat(formValue.requestedQuantity),
        unit: formValue.unit,
        requiredByDate: formValue.requiredDate ? new Date(formValue.requiredDate) : undefined,
        requestNotes: formValue.notes || undefined
      };

      this.transferService.createTransferRequest(request)
        .pipe(takeUntil(this.destroy$))
        .subscribe({
          next: (response) => {
            this.showSuccess(`Transfer request created successfully! Request #${response.requestNumber}`);
            this.snackBar.open(`Request #${response.requestNumber} created successfully`, 'Close', { duration: 5000 });
            this.resetForm();
            this.isSubmitting = false;
          },
          error: (error) => {
            const errorMsg = error.message || 'Error creating transfer request. Please try again.';
            this.showError(errorMsg);
            this.snackBar.open(errorMsg, 'Close', { duration: 5000 });
            this.isSubmitting = false;
          }
        });
    } else {
      this.markFormGroupTouched(this.stockRequestForm);
      this.showError('Please fill in all required fields correctly.');
    }
  }

  resetForm(): void {
    this.stockRequestForm.reset({
      explosiveType: '',
      batchId: '',
      unit: 'kg'
    });
    this.selectedBatch = null;
    this.filteredInventory = this.availableInventory;
    this.clearMessages();
  }

  private markFormGroupTouched(formGroup: FormGroup): void {
    Object.keys(formGroup.controls).forEach(key => {
      const control = formGroup.get(key);
      control?.markAsTouched();

      if (control instanceof FormGroup) {
        this.markFormGroupTouched(control);
      }
    });
  }

  isFieldInvalid(form: FormGroup, fieldName: string): boolean {
    const field = form.get(fieldName);
    return !!(field && field.invalid && (field.dirty || field.touched));
  }



  getFieldError(form: FormGroup, fieldName: string): string {
    const field = form.get(fieldName);
    if (field?.errors) {
      if (field.errors['required']) return `${this.getFieldDisplayName(fieldName)} is required`;
      if (field.errors['minlength']) return `${this.getFieldDisplayName(fieldName)} is too short`;
      if (field.errors['min']) return 'Quantity must be greater than 0';
    }
    return '';
  }



  private getFieldDisplayName(fieldName: string): string {
    const displayNames: { [key: string]: string } = {
      explosiveType: 'Explosive Type',
      batchId: 'Batch',
      requestedQuantity: 'Requested Quantity',
      unit: 'Unit',
      requiredDate: 'Required Date',
      notes: 'Notes'
    };
    return displayNames[fieldName] || fieldName;
  }

  getExplosiveTypeName(type: ExplosiveType): string {
    return type === ExplosiveType.ANFO ? 'ANFO' : 'Emulsion';
  }

  getBatchDisplayName(batch: CentralInventory): string {
    return `${batch.batchId} - ${batch.availableQuantity} ${batch.unit} available`;
  }

  private showSuccess(message: string): void {
    this.successMessage = message;
    this.errorMessage = '';
    setTimeout(() => this.clearMessages(), 5000);
  }

  private showError(message: string): void {
    this.errorMessage = message;
    this.successMessage = '';
    setTimeout(() => this.clearMessages(), 5000);
  }

  clearMessages(): void {
    this.successMessage = '';
    this.errorMessage = '';
  }
}