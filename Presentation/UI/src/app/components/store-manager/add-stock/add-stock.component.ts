import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators, FormArray } from '@angular/forms';
import { Subject, takeUntil } from 'rxjs';
import { StockRequestService } from '../../../core/services/stock-request.service';
import { 
  StockRequest, 
  CreateStockRequestRequest, 
  StockRequestStatus
} from '../../../core/models/stock-request.model';
import { ExplosiveType } from '../../../core/models/store.model';

@Component({
  selector: 'app-add-stock',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './add-stock.component.html',
  styleUrls: ['./add-stock.component.scss']
})
export class AddStockComponent implements OnInit, OnDestroy {
  private destroy$ = new Subject<void>();

  stockRequestForm!: FormGroup;
  
  isSubmitting = false;
  
  successMessage = '';
  errorMessage = '';
  
  ExplosiveType = ExplosiveType;

  StockRequestStatus = StockRequestStatus;
  
  explosiveTypes = Object.values(ExplosiveType);

  // User and store information (would typically come from auth service)
  currentUser = {
    name: 'John Smith',
    role: 'Store Manager'
  };
  
  currentStore = {
    id: 'store1',
    name: 'Muscat Field Storage',
    manager: 'Ahmed Al-Rashid'
  };

  constructor(
    private stockRequestService: StockRequestService,
    private fb: FormBuilder
  ) {
    this.initializeForm();
  }

  ngOnInit(): void {
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private initializeForm(): void {
    this.stockRequestForm = this.fb.group({
      requiredDate: ['', Validators.required],
      notes: [''],
      items: this.fb.array([this.buildItemGroup()])
    });
  }

  // Getter for items FormArray
  get items(): FormArray {
    return this.stockRequestForm.get('items') as FormArray;
  }

  // Helper to get a specific item FormGroup
  getItemGroup(index: number): FormGroup {
    return this.items.at(index) as FormGroup;
  }

  // Build a single item form group
  private buildItemGroup(): FormGroup {
    return this.fb.group({
      explosiveType: ['', Validators.required],
      requestedQuantity: ['', [Validators.required, Validators.min(0.1)]],
      unit: ['', Validators.required],
      purpose: ['', [Validators.required, Validators.minLength(5)]],
      specifications: ['']
    });
  }

  // Add a new item row
  addItem(): void {
    this.items.push(this.buildItemGroup());
  }

  // Remove an item row by index
  removeItem(index: number): void {
    if (this.items.length > 1) {
      this.items.removeAt(index);
    }
  }

  // When explosive type changes, reset the unit so user selects a valid one for that type
  onExplosiveTypeChange(index: number): void {
    const group = this.getItemGroup(index);
    group.get('unit')?.reset('');
  }

  // Get units list for an item based on its explosive type
  getUnitsForItem(index: number): string[] {
    const type = this.getItemGroup(index).get('explosiveType')?.value as ExplosiveType | undefined;
    return type ? this.getUnitsForExplosiveType(type) : [];
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

  getUnitsForExplosiveType(explosiveType: ExplosiveType): string[] {
    return this.stockRequestService.getUnitsForExplosiveType(explosiveType);
  }



  onSubmit(): void {
    if (this.stockRequestForm.valid) {
      this.isSubmitting = true;

      const requestedItems = this.items.controls.map(ctrl => {
        const v = (ctrl as FormGroup).value as {
          explosiveType: ExplosiveType;
          requestedQuantity: number | string;
          unit: string;
          purpose: string;
          specifications?: string;
        };
        return {
          explosiveType: v.explosiveType,
          requestedQuantity: typeof v.requestedQuantity === 'string' ? parseFloat(v.requestedQuantity) : v.requestedQuantity,
          unit: v.unit,
          purpose: v.purpose,
          specifications: v.specifications || ''
        };
      });

      const justification = requestedItems
        .map(it => it.purpose)
        .filter(p => !!p && p.trim().length > 0)
        .join('; ');

      // Create the request in the expected format
      const request: CreateStockRequestRequest = {
        requesterStoreId: this.currentStore.id,
        requestedItems,
        requiredDate: this.stockRequestForm.value.requiredDate,
        justification: justification || 'Multiple items request',
        notes: this.stockRequestForm.value.notes || ''
      };

      this.stockRequestService.createStockRequest(request)
        .pipe(takeUntil(this.destroy$))
        .subscribe({
          next: (response) => {
            this.showSuccess('Explosive request created successfully! Request ID: ' + response.id);
            this.resetForm();

            this.isSubmitting = false;
          },
          error: (error) => {
            this.showError('Error creating explosive request. Please try again.');
            this.isSubmitting = false;
          }
        });
    } else {
      this.markFormGroupTouched(this.stockRequestForm);
      this.showError('Please fill in all required fields correctly.');
    }
  }

  resetForm(): void {
    this.stockRequestForm.reset();
    this.initializeForm();
    this.clearMessages();
  }



  private markFormGroupTouched(formGroup: FormGroup): void {
    Object.keys(formGroup.controls).forEach(key => {
      const control = formGroup.get(key);
      control?.markAsTouched();
      
      if (control instanceof FormGroup) {
        this.markFormGroupTouched(control);
      } else if (control instanceof FormArray) {
        control.controls.forEach(arrayControl => {
          if (arrayControl instanceof FormGroup) {
            this.markFormGroupTouched(arrayControl);
          }
        });
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
      requestedQuantity: 'Quantity',
      unit: 'Unit',
      purpose: 'Purpose',
      specifications: 'Specifications',
      requiredDate: 'Required Date',
      priority: 'Priority',
      justification: 'Justification',
      notes: 'Notes'
    };
    return displayNames[fieldName] || fieldName;
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