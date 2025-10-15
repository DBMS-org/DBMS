import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatRadioModule } from '@angular/material/radio';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { Router, ActivatedRoute } from '@angular/router';
import { Subject, takeUntil } from 'rxjs';
import { InventoryTransferService } from '../../../../core/services/inventory-transfer.service';
import {
  InventoryTransferRequest,
  ApproveTransferRequest,
  RejectTransferRequest
} from '../../../../core/models/inventory-transfer.model';

@Component({
  selector: 'app-approval-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatRadioModule,
    MatIconModule,
    MatChipsModule,
    MatSnackBarModule
  ],
  templateUrl: './approval-form.component.html',
  styleUrls: ['./approval-form.component.scss']
})
export class ApprovalFormComponent implements OnInit, OnDestroy {
  private destroy$ = new Subject<void>();

  approvalForm: FormGroup;
  request!: InventoryTransferRequest;
  loading = false;

  constructor(
    private fb: FormBuilder,
    private transferService: InventoryTransferService,
    private snackBar: MatSnackBar,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.approvalForm = this.fb.group({
      decision: ['', Validators.required],
      approvedQuantity: [{ value: '', disabled: true }, [Validators.min(0.1)]],
      rejectionReason: [{ value: '', disabled: true }, [Validators.minLength(10)]],
      notes: ['', [Validators.maxLength(500)]]
    });
  }

  ngOnInit(): void {
    const requestId = this.route.snapshot.paramMap.get('id');
    if (requestId) {
      this.loadRequest(parseInt(requestId));
    } else {
      this.snackBar.open('No request ID provided', 'Close', {
        duration: 3000
      });
      this.goBack();
    }
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private loadRequest(requestId: number): void {
    this.loading = true;
    this.transferService.getTransferRequestById(requestId)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (request) => {
          this.request = request;
          this.initializeForm();
          this.loading = false;
        },
        error: (error) => {
          this.snackBar.open('Error loading request: ' + error.message, 'Close', {
            duration: 5000
          });
          this.loading = false;
          this.goBack();
        }
      });
  }

  private initializeForm(): void {
    // Set default approved quantity to requested quantity
    this.approvalForm.patchValue({
      approvedQuantity: this.request.requestedQuantity
    });

    // Subscribe to decision changes
    this.approvalForm.get('decision')?.valueChanges
      .pipe(takeUntil(this.destroy$))
      .subscribe(decision => {
        this.handleDecisionChange(decision);
      });
  }

  private handleDecisionChange(decision: string): void {
    const approvedQuantityControl = this.approvalForm.get('approvedQuantity');
    const rejectionReasonControl = this.approvalForm.get('rejectionReason');
    const notesControl = this.approvalForm.get('notes');

    // Reset and disable all conditional fields first
    approvedQuantityControl?.disable();
    rejectionReasonControl?.disable();

    // Clear validators
    approvedQuantityControl?.clearValidators();
    rejectionReasonControl?.clearValidators();

    // Enable relevant fields based on decision
    if (decision === 'approve') {
      approvedQuantityControl?.enable();
      approvedQuantityControl?.setValidators([
        Validators.required,
        Validators.min(0.1),
        Validators.max(this.request?.requestedQuantity || 999999)
      ]);
      notesControl?.setValidators([Validators.maxLength(500)]);
    } else if (decision === 'reject') {
      rejectionReasonControl?.enable();
      rejectionReasonControl?.setValidators([Validators.required, Validators.minLength(10)]);
    }

    // Update validators
    approvedQuantityControl?.updateValueAndValidity();
    rejectionReasonControl?.updateValueAndValidity();
    notesControl?.updateValueAndValidity();
  }

  onSubmit(): void {
    if (!this.approvalForm.valid) {
      this.markFormAsTouched();
      return;
    }

    const formValue = this.approvalForm.value;
    this.loading = true;

    if (formValue.decision === 'approve') {
      const approveData: ApproveTransferRequest = {
        approvedQuantity: formValue.approvedQuantity,
        approvalNotes: formValue.notes || undefined
      };

      this.transferService.approveTransferRequest(this.request.id, approveData)
        .pipe(takeUntil(this.destroy$))
        .subscribe({
          next: () => {
            this.snackBar.open('Request approved successfully', 'Close', {
              duration: 3000
            });
            this.goBack();
          },
          error: (error) => {
            this.loading = false;
            this.snackBar.open('Error approving request: ' + error.message, 'Close', {
              duration: 5000
            });
          }
        });
    } else if (formValue.decision === 'reject') {
      const rejectData: RejectTransferRequest = {
        reason: formValue.rejectionReason
      };

      this.transferService.rejectTransferRequest(this.request.id, rejectData)
        .pipe(takeUntil(this.destroy$))
        .subscribe({
          next: () => {
            this.snackBar.open('Request rejected successfully', 'Close', {
              duration: 3000
            });
            this.goBack();
          },
          error: (error) => {
            this.loading = false;
            this.snackBar.open('Error rejecting request: ' + error.message, 'Close', {
              duration: 5000
            });
          }
        });
    }
  }

  private markFormAsTouched(): void {
    Object.keys(this.approvalForm.controls).forEach(key => {
      this.approvalForm.get(key)?.markAsTouched();
    });
  }

  goBack(): void {
    this.router.navigate(['/explosive-manager/requests']);
  }
}
