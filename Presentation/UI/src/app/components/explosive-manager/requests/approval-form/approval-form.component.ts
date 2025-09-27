import { Component, OnInit } from '@angular/core';
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
import { Location } from '@angular/common';
import { RequestService } from '../services/request.service';
import { ExplosiveRequest } from '../models/explosive-request.model';

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
export class ApprovalFormComponent implements OnInit {
  approvalForm: FormGroup;
  request!: ExplosiveRequest;

  constructor(
    private fb: FormBuilder,
    private requestService: RequestService,
    private snackBar: MatSnackBar,
    private router: Router,
    private route: ActivatedRoute,
    private location: Location
  ) {
    this.approvalForm = this.fb.group({
      decision: ['', Validators.required],
      approvedQuantity: [{ value: '', disabled: true }, [Validators.min(1)]],
      departureDate: [{ value: '', disabled: true }],
      expectedReceiptDate: [{ value: '', disabled: true }],
      rejectionReason: [{ value: '', disabled: true }, [Validators.minLength(10)]],
      comments: ['', [Validators.required, Validators.minLength(5), Validators.maxLength(500)]]
    });
  }

  ngOnInit(): void {
    this.initializeForm();

    const requestId = this.route.snapshot.paramMap.get('id');
    if (requestId) {
      this.loadRequest(requestId);
    } else {
      this.snackBar.open('No request ID provided', 'Close', {
        duration: 3000,
        panelClass: ['error-snackbar']
      });
    }
  }

  private loadRequest(requestId: string): void {
    this.requestService.getRequestById(requestId).subscribe({
      next: (request) => {
        if (request) {
          this.request = request;
          this.initializeForm();
        } else {
          this.snackBar.open('Request not found', 'Close', {
            duration: 5000,
            panelClass: ['error-snackbar']
          });
          this.goBack();
        }
      },
      error: (error) => {
        this.snackBar.open('Error loading request: ' + error.message, 'Close', {
          duration: 5000,
          panelClass: ['error-snackbar']
        });
        this.goBack();
      }
    });
  }

  private initializeForm(): void {
    this.approvalForm = this.fb.group({
      decision: ['', Validators.required],
      approvedQuantity: [{ value: '', disabled: true }, [Validators.required, Validators.min(0.1), Validators.max(this.request?.quantity || 999)]],
      departureDate: [{ value: '', disabled: true }, [Validators.required]],
      expectedReceiptDate: [{ value: '', disabled: true }, [Validators.required]],
      rejectionReason: [{ value: '', disabled: true }, [Validators.required, Validators.minLength(10)]],
      approvalComments: ['', [Validators.required, Validators.minLength(5), Validators.maxLength(500)]]
    });

    // Subscribe to decision changes to enable/disable conditional fields
    this.approvalForm.get('decision')?.valueChanges.subscribe(decision => {
      this.handleDecisionChange(decision);
    });

    // Add date validation
    this.approvalForm.get('expectedReceiptDate')?.valueChanges.subscribe(() => {
      this.validateDates();
    });
  }

  private handleDecisionChange(decision: string): void {
    const approvedQuantityControl = this.approvalForm.get('approvedQuantity');
    const departureDateControl = this.approvalForm.get('departureDate');
    const expectedReceiptDateControl = this.approvalForm.get('expectedReceiptDate');
    const rejectionReasonControl = this.approvalForm.get('rejectionReason');
    const approvalCommentsControl = this.approvalForm.get('approvalComments');

    // Reset and disable all conditional fields first
    approvedQuantityControl?.disable();
    departureDateControl?.disable();
    expectedReceiptDateControl?.disable();
    rejectionReasonControl?.disable();

    // Clear validators for all fields
    approvedQuantityControl?.clearValidators();
    departureDateControl?.clearValidators();
    expectedReceiptDateControl?.clearValidators();
    rejectionReasonControl?.clearValidators();
    approvalCommentsControl?.clearValidators();

    // Enable relevant fields based on decision
    switch (decision) {
      case 'approve':
        // No additional fields required for approve - simplified approval
        break;
      case 'reject':
        rejectionReasonControl?.enable();
        rejectionReasonControl?.setValidators([Validators.required, Validators.minLength(10)]);
        break;
      case 'pending':
        // Approval comments are required for pending
        approvalCommentsControl?.setValidators([Validators.required, Validators.minLength(5), Validators.maxLength(500)]);
        break;
    }

    // Update validators
    approvedQuantityControl?.updateValueAndValidity();
    departureDateControl?.updateValueAndValidity();
    expectedReceiptDateControl?.updateValueAndValidity();
    rejectionReasonControl?.updateValueAndValidity();
    approvalCommentsControl?.updateValueAndValidity();
  }

  onSubmit(): void {
    if (this.approvalForm.valid) {
      const formValue = this.approvalForm.value;
      
      switch (formValue.decision) {
        case 'approve':
          if (!this.validateDates()) {
            this.snackBar.open('Expected receipt date must be after departure date', 'Close', {
              duration: 5000,
              panelClass: ['error-snackbar']
            });
            return;
          }
          
          this.requestService.approveRequest(this.request.id, {
            approvedQuantity: formValue.approvedQuantity,
            departureDate: formValue.departureDate,
            expectedReceiptDate: formValue.expectedReceiptDate,
            approvalComments: formValue.approvalComments
          }).subscribe({
            next: () => {
              this.snackBar.open('Request approved successfully', 'Close', {
                duration: 3000,
                panelClass: ['success-snackbar']
              });
              this.goBack();
            },
            error: (error) => {
              this.snackBar.open('Error approving request: ' + error.message, 'Close', {
                duration: 5000,
                panelClass: ['error-snackbar']
              });
            }
          });
          break;

        case 'reject':
          this.requestService.rejectRequest(this.request.id, {
            rejectionReason: formValue.rejectionReason,
            approvalComments: formValue.approvalComments
          }).subscribe({
            next: () => {
              this.snackBar.open('Request rejected successfully', 'Close', {
                duration: 3000,
                panelClass: ['success-snackbar']
              });
              this.goBack();
            },
            error: (error) => {
              this.snackBar.open('Error rejecting request: ' + error.message, 'Close', {
                duration: 5000,
                panelClass: ['error-snackbar']
              });
            }
          });
          break;

        case 'pending':
          this.requestService.setPending(this.request.id, {
            approvalComments: formValue.approvalComments
          }).subscribe({
            next: () => {
              this.snackBar.open('Request set to pending successfully', 'Close', {
                duration: 3000,
                panelClass: ['success-snackbar']
              });
              this.goBack();
            },
            error: (error) => {
              this.snackBar.open('Error setting request to pending: ' + error.message, 'Close', {
                duration: 5000,
                panelClass: ['error-snackbar']
              });
            }
          });
          break;
      }
    }
  }

  validateDates(): boolean {
    const departureDate = this.approvalForm.get('departureDate')?.value;
    const expectedReceiptDate = this.approvalForm.get('expectedReceiptDate')?.value;
    
    if (departureDate && expectedReceiptDate) {
      return new Date(expectedReceiptDate) > new Date(departureDate);
    }
    return true;
  }

  goBack(): void {
    this.location.back();
  }
}