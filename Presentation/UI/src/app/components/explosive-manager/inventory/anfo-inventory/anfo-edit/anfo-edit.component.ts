import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-anfo-edit',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './anfo-edit.component.html',
  styleUrl: './anfo-edit.component.scss'
})
export class AnfoEditComponent implements OnInit {
  anfoForm!: FormGroup;
  isSubmitting = false;
  itemId: string | null = null;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.itemId = this.route.snapshot.paramMap.get('id');
    this.initializeForm();
    this.loadItemData();
  }

  private initializeForm(): void {
    this.anfoForm = this.fb.group({
      batchId: ['', [Validators.required]],
      manufacturingDate: ['', [Validators.required]],
      expiryDate: ['', [Validators.required]],
      location: ['', [Validators.required]],
      quantity: ['', [Validators.required, Validators.min(0.001)]],
      density: ['', [Validators.required, Validators.min(0.1), Validators.max(2.0)]],
      grade: [''],
      notes: [''],
      status: ['Available']
    });
  }

  private loadItemData(): void {
    // Simulate loading existing data
    // In a real application, this would fetch data from an API
    if (this.itemId) {
      const mockData = {
        batchId: 'ANF-2024-001',
        manufacturingDate: '2024-01-15',
        expiryDate: '2025-01-15',
        location: 'Store A - Section 1',
        quantity: 0.5,
        density: 0.85,
        grade: 'Standard',
        notes: 'High quality ANFO for mining operations',
        status: 'Available'
      };
      
      this.anfoForm.patchValue(mockData);
    }
  }

  onSubmit(): void {
    if (this.anfoForm.valid) {
      this.isSubmitting = true;
      
      // Simulate API call
      const formData = this.anfoForm.value;
      console.log('Updating ANFO stock:', formData);
      
      // Simulate async operation
      setTimeout(() => {
        this.isSubmitting = false;
        // Navigate back to inventory list
        this.router.navigate(['/explosive-manager/inventory/anfo']);
      }, 1500);
    } else {
      // Mark all fields as touched to show validation errors
      Object.keys(this.anfoForm.controls).forEach(key => {
        this.anfoForm.get(key)?.markAsTouched();
      });
    }
  }

  onCancel(): void {
    this.router.navigate(['/explosive-manager/inventory/anfo']);
  }

  getCurrentDateTime(): string {
    return new Date().toLocaleString();
  }
}
