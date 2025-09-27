import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { RequestService } from '../services/request.service';
import { ExplosiveRequest } from '../models/explosive-request.model';
import { DispatchForm } from '../models/dispatch.model';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-dispatch-request',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatSnackBarModule, MatButtonModule, MatIconModule],
  templateUrl: './dispatch-request.component.html',
  styleUrls: ['./dispatch-request.component.scss']
})
export class DispatchRequestComponent implements OnInit {
  dispatchForm!: FormGroup;
  request!: ExplosiveRequest | undefined;
  isSubmitting = false;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private location: Location,
    private snackBar: MatSnackBar,
    private requestService: RequestService
  ) {}

  ngOnInit(): void {
    this.initForm();

    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.requestService.getRequestById(id).subscribe({
        next: (req) => {
          this.request = req;
          if (!this.request) {
            this.snackBar.open('Request not found', 'Close', { duration: 3000, panelClass: ['error-snackbar'] });
            this.goBack();
          }
        },
        error: (err) => {
          this.snackBar.open('Error loading request: ' + err.message, 'Close', { duration: 3000, panelClass: ['error-snackbar'] });
          this.goBack();
        }
      });
    } else {
      this.snackBar.open('No request ID provided', 'Close', { duration: 3000, panelClass: ['error-snackbar'] });
      this.goBack();
    }
  }

  private initForm(): void {
    this.dispatchForm = this.fb.group({
      truckNumber: ['', [Validators.required]],
      dispatchDate: ['', Validators.required],
      driverName: [''],
      routeInformation: [''],
      additionalNotes: ['']
    });

    // default dispatch date to today
    this.dispatchForm.patchValue({
      dispatchDate: new Date().toISOString().split('T')[0]
    });
  }

  submit(): void {
    if (!this.request) return;
    if (this.dispatchForm.invalid || this.isSubmitting) {
      this.dispatchForm.markAllAsTouched();
      return;
    }

    this.isSubmitting = true;
    const payload: DispatchForm = {
      truckNumber: this.dispatchForm.value.truckNumber,
      dispatchDate: new Date(this.dispatchForm.value.dispatchDate),
      driverName: this.dispatchForm.value.driverName || undefined,
      routeInformation: this.dispatchForm.value.routeInformation || undefined,
      dispatchNotes: this.dispatchForm.value.additionalNotes || undefined
    };

    this.requestService.dispatchRequest(this.request.id, payload).subscribe({
      next: () => {
        this.isSubmitting = false;
        this.snackBar.open('Request dispatched successfully', 'Close', { duration: 3000, panelClass: ['success-snackbar'] });
        this.goBack();
      },
      error: (err) => {
        this.isSubmitting = false;
        this.snackBar.open('Error dispatching request: ' + err.message, 'Close', { duration: 4000, panelClass: ['error-snackbar'] });
      }
    });
  }

  getError(fieldName: string): string | null {
    const field = this.dispatchForm.get(fieldName);
    if (!field) return null;
    if (field.touched && field.errors) {
      if (field.errors['required']) return `${this.displayName(fieldName)} is required`;
      // Removed strict pattern requirement message for truck number
    }
    return null;
  }

  displayName(fieldName: string): string {
    const map: Record<string, string> = {
      truckNumber: 'Truck Number',
      dispatchDate: 'Dispatch Date',
      driverName: 'Driver Name',
      routeInformation: 'Route Information',
      additionalNotes: 'Additional Notes'
    };
    return map[fieldName] || fieldName;
  }

  hasError(fieldName: string): boolean {
    const field = this.dispatchForm.get(fieldName);
    return !!(field && field.touched && field.errors);
  }

  goBack(): void {
    this.location.back();
  }
}