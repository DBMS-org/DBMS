import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-anfo-add',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './anfo-add.component.html',
  styleUrl: './anfo-add.component.scss'
})
export class AnfoAddComponent implements OnInit {
  anfoForm!: FormGroup;
  isSubmitting = false;

  constructor(
    private fb: FormBuilder,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.initializeForm();
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
      notes: ['']
    });
  }

  onSubmit(): void {
    if (this.anfoForm.valid) {
      this.isSubmitting = true;
      
      // Simulate API call
      const formData = this.anfoForm.value;
      console.log('Adding ANFO stock:', formData);
      
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
}
