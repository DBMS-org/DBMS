import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators, FormArray } from '@angular/forms';
import { Subject, takeUntil } from 'rxjs';
import { StockRequestService } from '../../../core/services/stock-request.service';
import { 
  StockRequest, 
  CreateStockRequestRequest, 
  RequestPriority,
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
  recentRequests: StockRequest[] = [];
  
  isLoading = false;
  isSubmitting = false;
  showRecentRequests = true;
  
  successMessage = '';
  errorMessage = '';
  
  ExplosiveType = ExplosiveType;
  RequestPriority = RequestPriority;
  StockRequestStatus = StockRequestStatus;
  
  explosiveTypes = Object.values(ExplosiveType);
  priorities = Object.values(RequestPriority);
  
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
    this.loadRecentRequests();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private initializeForm(): void {
    this.stockRequestForm = this.fb.group({
      requesterStoreId: [this.currentStore.id, Validators.required],
      requestedItems: this.fb.array([this.createRequestedItemGroup()], Validators.required),
      requiredDate: ['', [Validators.required]],
      priority: [RequestPriority.MEDIUM, Validators.required],
      justification: ['', [Validators.required, Validators.minLength(20)]],
      notes: ['']
    });
  }

  private createRequestedItemGroup(): FormGroup {
    return this.fb.group({
      explosiveType: ['', Validators.required],
      requestedQuantity: ['', [Validators.required, Validators.min(1)]],
      unit: ['', Validators.required],
      purpose: ['', [Validators.required, Validators.minLength(10)]],
      specifications: ['']
    });
  }

  get requestedItemsArray(): FormArray {
    return this.stockRequestForm.get('requestedItems') as FormArray;
  }

  addRequestedItem(): void {
    this.requestedItemsArray.push(this.createRequestedItemGroup());
  }

  removeRequestedItem(index: number): void {
    if (this.requestedItemsArray.length > 1) {
      this.requestedItemsArray.removeAt(index);
    }
  }

  onExplosiveTypeChange(index: number): void {
    const item = this.requestedItemsArray.at(index);
    const explosiveType = item.get('explosiveType')?.value as ExplosiveType;
    
    if (explosiveType) {
      const availableUnits = this.stockRequestService.getUnitsForExplosiveType(explosiveType);
      item.get('unit')?.setValue(availableUnits[0] || '');
    }
  }

  getUnitsForExplosiveType(explosiveType: ExplosiveType): string[] {
    return this.stockRequestService.getUnitsForExplosiveType(explosiveType);
  }

  private loadRecentRequests(): void {
    this.isLoading = true;
    this.stockRequestService.getStockRequests()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (requests) => {
          this.recentRequests = requests.slice(0, 5);
          this.isLoading = false;
        },
        error: (error) => {
          this.showError('Failed to load recent requests');
          this.isLoading = false;
        }
      });
  }

  onSubmit(): void {
    if (this.stockRequestForm.valid) {
      this.isSubmitting = true;
      const request: CreateStockRequestRequest = this.stockRequestForm.value;
      
      this.stockRequestService.createStockRequest(request)
        .pipe(takeUntil(this.destroy$))
        .subscribe({
          next: (response) => {
            this.showSuccess('Stock request submitted successfully! Request ID: ' + response.id);
            this.resetForm();
            this.loadRecentRequests();
            this.isSubmitting = false;
          },
          error: (error) => {
            this.showError('Failed to submit stock request. Please try again.');
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

  toggleRecentRequests(): void {
    this.showRecentRequests = !this.showRecentRequests;
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

  isArrayFieldInvalid(arrayIndex: number, fieldName: string): boolean {
    const arrayControl = this.requestedItemsArray.at(arrayIndex) as FormGroup;
    const field = arrayControl.get(fieldName);
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

  getArrayFieldError(arrayIndex: number, fieldName: string): string {
    const arrayControl = this.requestedItemsArray.at(arrayIndex) as FormGroup;
    const field = arrayControl.get(fieldName);
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

  getStatusClass(status: StockRequestStatus): string {
    switch (status) {
      case StockRequestStatus.PENDING:
        return 'bg-warning';
      case StockRequestStatus.APPROVED:
        return 'bg-success';
      case StockRequestStatus.REJECTED:
        return 'bg-danger';
      default:
        return 'bg-secondary';
    }
  }

  getPriorityClass(priority: RequestPriority): string {
    switch (priority) {
      case RequestPriority.LOW:
        return 'text-success';
      case RequestPriority.MEDIUM:
        return 'text-primary';
      case RequestPriority.HIGH:
        return 'text-warning';
      case RequestPriority.URGENT:
        return 'text-danger';
      default:
        return 'text-secondary';
    }
  }

  formatDate(date: Date): string {
    return new Date(date).toLocaleDateString();
  }
} 