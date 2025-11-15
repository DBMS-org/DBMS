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

  constructor(
    private router: Router,
    private notification: NotificationService
  ) {}

  onSubmit() {
    if (this.validatePassword()) {
      const { confirmPassword, ...userToSave } = this.user;
      this.save.emit(userToSave);
    }
  }

  validatePassword(): boolean {
    if (this.user.password !== this.user.confirmPassword) {
      this.notification.showError('Passwords do not match!');
      return false;
    }
    if (this.user.password.length < 8) {
      this.notification.showError('Password must be at least 8 characters long!');
      return false;
    }
    return true;
  }

  onClose() {
    this.close.emit();
  }
}
