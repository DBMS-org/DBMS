import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject, takeUntil } from 'rxjs';
import { InventoryTransferService } from '../../../../core/services/inventory-transfer.service';
import {
  InventoryTransferRequest,
  DispatchTransferRequest
} from '../../../../core/models/inventory-transfer.model';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatCardModule } from '@angular/material/card';

@Component({
  selector: 'app-dispatch-request',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatSnackBarModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatProgressSpinnerModule,
    MatCardModule
  ],
  templateUrl: './dispatch-request.component.html',
  styleUrl: './dispatch-request.component.scss'
})
export class DispatchRequestComponent implements OnInit, OnDestroy {
  private destroy$ = new Subject<void>();

  dispatchForm!: FormGroup;
  request!: InventoryTransferRequest;
  isSubmitting = false;
  isLoading = true;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private snackBar: MatSnackBar,
    private transferService: InventoryTransferService
  ) {}

  ngOnInit(): void {
    this.initForm();

    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.loadRequest(parseInt(id));
    } else {
      this.snackBar.open('No request ID provided', 'Close', { duration: 3000 });
      this.goBack();
    }
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private initForm(): void {
    this.dispatchForm = this.fb.group({
      truckNumber: ['', [Validators.required, Validators.maxLength(50)]],
      driverName: ['', [Validators.required, Validators.maxLength(200)]],
      driverContactNumber: ['', Validators.maxLength(20)],
      dispatchNotes: ['', Validators.maxLength(1000)]
    });
  }

  private loadRequest(requestId: number): void {
    this.isLoading = true;
    this.transferService.getTransferRequestById(requestId)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (request) => {
          this.request = request;
          this.isLoading = false;

          // Validate that the request can be dispatched
          if (request.status !== 'Approved') {
            this.snackBar.open('Only approved requests can be dispatched', 'Close', { duration: 5000 });
            this.goBack();
          }
        },
        error: (error) => {
          this.isLoading = false;
          this.snackBar.open('Error loading request: ' + error.message, 'Close', { duration: 5000 });
          this.goBack();
        }
      });
  }

  submit(): void {
    if (this.dispatchForm.invalid) {
      this.markFormAsTouched();
      return;
    }

    if (!this.request) return;

    const dispatchData: DispatchTransferRequest = {
      truckNumber: this.dispatchForm.value.truckNumber,
      driverName: this.dispatchForm.value.driverName,
      driverContactNumber: this.dispatchForm.value.driverContactNumber || undefined,
      dispatchNotes: this.dispatchForm.value.dispatchNotes || undefined
    };

    this.isSubmitting = true;
    this.transferService.dispatchTransferRequest(this.request.id, dispatchData)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: () => {
          this.isSubmitting = false;
          this.snackBar.open('Request dispatched successfully!', 'Close', {
            duration: 3000
          });
          this.goBack();
        },
        error: (error) => {
          this.isSubmitting = false;
          this.snackBar.open('Error dispatching request: ' + error.message, 'Close', {
            duration: 5000
          });
        }
      });
  }

  private markFormAsTouched(): void {
    Object.keys(this.dispatchForm.controls).forEach(key => {
      this.dispatchForm.get(key)?.markAsTouched();
    });
  }

  getError(fieldName: string): string | null {
    const field = this.dispatchForm.get(fieldName);
    if (!field || !field.touched || !field.errors) return null;

    if (field.errors['required']) return `${this.displayName(fieldName)} is required`;
    if (field.errors['maxlength']) return `Maximum ${field.errors['maxlength'].requiredLength} characters allowed`;

    return null;
  }

  displayName(fieldName: string): string {
    const map: Record<string, string> = {
      truckNumber: 'Truck Number',
      driverName: 'Driver Name',
      driverContactNumber: 'Driver Contact Number',
      dispatchNotes: 'Dispatch Notes'
    };
    return map[fieldName] || fieldName;
  }

  hasError(fieldName: string): boolean {
    const field = this.dispatchForm.get(fieldName);
    return !!(field && field.touched && field.errors);
  }

  goBack(): void {
    this.router.navigate(['/explosive-manager/requests']);
  }
}
