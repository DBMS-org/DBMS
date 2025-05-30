import { Component, EventEmitter, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

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

  user = {
    name: '',
    email: '',
    password: '',
    confirmPassword: '',
    role: 'User',
    area: '',
    region: '',
    country: '',
    omanPhone: '',
    countryPhone: '',
    status: 'Active'
  };

  roles = ['Admin', 'User'];
  areas = ['North', 'South', 'East', 'West'];
  regions = ['Muscat', 'Salalah', 'Sohar', 'Nizwa', 'Sur'];
  countries = ['Oman', 'UAE', 'Saudi Arabia', 'Kuwait', 'Qatar', 'Bahrain'];
  statuses = ['Active', 'Inactive'];

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
