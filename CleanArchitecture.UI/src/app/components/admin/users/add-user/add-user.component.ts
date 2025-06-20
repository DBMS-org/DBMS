import { Component, EventEmitter, Output, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { REGIONS } from '../../../../core/constants/regions';

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

  user = {
    name: '',
    email: '',
    password: '',
    confirmPassword: '',
    role: 'User',
    region: '',
    country: '',
    omanPhone: '',
    countryPhone: ''
  };

  roles = ['Admin', 'BlastingEngineer', 'Operator'];
  regions = REGIONS;
  countries = ['Oman', 'UAE', 'Saudi Arabia', 'Kuwait', 'Qatar', 'Bahrain'];

  onSubmit() {
    if (this.validatePassword()) {
      // Remove confirmPassword before sending to backend
      const { confirmPassword, ...userToSave } = this.user;
      this.save.emit(userToSave);
    }
  }

  validatePassword(): boolean {
    if (this.user.password !== this.user.confirmPassword) {
      alert('Passwords do not match!');
      return false;
    }
    if (this.user.password.length < 8) {
      alert('Password must be at least 8 characters long!');
      return false;
    }
    return true;
  }

  onClose() {
    this.close.emit();
  }
}
