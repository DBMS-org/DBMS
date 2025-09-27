import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormArray, FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { RequestService } from '../services/request.service';
import { ExplosiveRequest, RequestItem } from '../models/explosive-request.model';
import { DispatchForm } from '../models/dispatch.model';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatTableModule } from '@angular/material/table';

@Component({
  selector: 'app-dispatch-request',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatSnackBarModule, MatButtonModule, MatIconModule, MatFormFieldModule, MatSelectModule, MatInputModule, MatDatepickerModule, MatNativeDateModule, MatTableModule],
  templateUrl: './dispatch-request.component.html',
  styleUrls: ['./dispatch-request.component.scss']
})
export class DispatchRequestComponent implements OnInit {
  dispatchForm!: FormGroup;
  request!: ExplosiveRequest | undefined;
  isSubmitting = false;
  // Expose items to template for per-item approval
  viewItems: RequestItem[] = [];
  // Columns for the mat-table in the template
  displayedColumns: string[] = ['itemName', 'requestedQuantity', 'approvedQuantity', 'remarks'];
  // Convenience getter for items FormArray used in template
  get itemsForm(): FormArray {
    return this.dispatchForm.get('items') as FormArray;
  }

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
          } else {
            this.initItemsForm();
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
      additionalNotes: [''],
      items: this.fb.array([])
    });

    // default dispatch date to today
    this.dispatchForm.patchValue({
      dispatchDate: new Date()
    });
  }

  private initItemsForm() {
    const items = this.request?.requestedItems || (this.request ? this.singleAsItem(this.request) : []);
    this.viewItems = items;
    const itemsArray = this.fb.array(
      items.map(item =>
        this.fb.group({
          approvedQuantity: [item.approvedQuantity ?? item.quantity, [Validators.min(0)]],
          remarks: ['']
        })
      )
    );
    this.dispatchForm.setControl('items', itemsArray);
  }

  private singleAsItem(req: ExplosiveRequest): RequestItem[] {
    if (req.explosiveType && req.quantity && req.unit) {
      return [{ explosiveType: req.explosiveType, quantity: req.quantity, unit: req.unit, purpose: req.purpose } as RequestItem];
    }
    return [];
  }

  submit() {
    if (this.dispatchForm.invalid) return;
    if (!this.request) return;
    const payload = { ...this.dispatchForm.value };
    // Map item decisions payload without decision field
    payload.items = (this.dispatchForm.get('items') as FormArray).controls.map((ctrl, idx) => ({
      index: idx,
      approvedQuantity: Number(ctrl.get('approvedQuantity')?.value ?? 0),
      remarks: ctrl.get('remarks')?.value ?? ''
    }));
  
    this.isSubmitting = true;
    this.requestService.dispatchRequest(this.request.id, {
      truckNumber: payload.truckNumber,
      dispatchDate: new Date(payload.dispatchDate),
      driverName: payload.driverName,
      routeInformation: payload.routeInformation,
      dispatchNotes: payload.additionalNotes,
      itemDecisions: payload.items
    }).subscribe({
      next: () => {
        this.isSubmitting = false;
        this.goBack();
      },
      error: () => {
        this.isSubmitting = false;
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