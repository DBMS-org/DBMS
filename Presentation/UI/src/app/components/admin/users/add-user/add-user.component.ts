import { Component, EventEmitter, Output, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { REGIONS } from '../../../../core/constants/regions';
import { USER_ROLES_ARRAY } from '../../../../core/constants/roles';
import { Router } from '@angular/router';
import { NotificationService } from '../../../../core/services/notification.service';

@Component({
  selector: 'app-add-user',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './add-user.component.html',
  styleUrls: ['./add-user.component.scss']
})
export class AddUserComponent {
  @Output() close = new EventEmitter<void>();
  @Output() save = new EventEmitter<any>();
  @Input() error: string | null = null;
  @Input() errorDetails: string[] = [];

  user = {
    name: '',
    email: '',
    password: '',
    confirmPassword: '',
    role: 'Operator',
    region: '',
    country: '',
    omanPhone: '',
    countryPhone: ''
  };

  roles = USER_ROLES_ARRAY;
  regions = REGIONS;
  countries = ['Oman', 'UAE', 'Saudi Arabia', 'Kuwait', 'Qatar', 'Bahrain'];
  isSubmitting = false; // Prevent duplicate submissions

  constructor(
    private router: Router,
    private notification: NotificationService
  ) {}

  onSubmit() {
    // Prevent duplicate submissions
    if (this.isSubmitting) {
      return;
    }

    if (this.validatePassword()) {
      this.isSubmitting = true;
      const { confirmPassword, ...userToSave } = this.user;
      this.save.emit(userToSave);
    }
  }

  // Method to reset submission state (called by parent component)
  resetSubmissionState() {
    this.isSubmitting = false;
  }

  validatePassword(): boolean {
    const password = this.user.password;
    const confirmPassword = this.user.confirmPassword;

    // Check if passwords match
    if (password !== confirmPassword) {
      this.notification.showError('Passwords do not match. Please re-enter matching passwords.', 'Password Mismatch');
      return false;
    }

    // Check minimum length
    if (password.length < 8) {
      this.notification.showError('Password must be at least 8 characters long for security.', 'Password Too Short');
      return false;
    }

    // Check for uppercase letter
    if (!/[A-Z]/.test(password)) {
      this.notification.showError('Password must contain at least one uppercase letter (A-Z).', 'Password Requirement');
      return false;
    }

    // Check for lowercase letter
    if (!/[a-z]/.test(password)) {
      this.notification.showError('Password must contain at least one lowercase letter (a-z).', 'Password Requirement');
      return false;
    }

    // Check for digit
    if (!/\d/.test(password)) {
      this.notification.showError('Password must contain at least one digit (0-9).', 'Password Requirement');
      return false;
    }

    // Check for special character
    if (!/[^\w\s]/.test(password)) {
      this.notification.showError('Password must contain at least one special character (e.g., @, #, $, %).', 'Password Requirement');
      return false;
    }

    return true;
  }

  onClose() {
    this.close.emit();
  }
}
