import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from '../../../../core/services/user.service';
import { User, UpdateUserRequest } from '../../../../core/models/user.model';

@Component({
  selector: 'app-edit-user',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './edit-user.component.html',
  styleUrls: ['./edit-user.component.scss']
})
export class EditUserComponent implements OnInit {
  user: User | null = null;
  loading = false;
  saving = false;
  error: string | null = null;

  roles = ['Admin', 'BlastingEngineer', 'User'];
  areas = ['North', 'South', 'East', 'West', 'Central'];
  regions = ['Muscat', 'Salalah', 'Sohar', 'Nizwa', 'Sur', 'Ibri', 'Rustaq'];
  countries = ['Oman', 'UAE', 'Saudi Arabia', 'Kuwait', 'Qatar', 'Bahrain'];
  statuses = ['Active', 'Inactive'];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private userService: UserService
  ) {}

  ngOnInit() {
    // Get user ID from route parameters
    this.route.params.subscribe(params => {
      const userId = +params['id'];
      if (userId) {
        this.loadUser(userId);
      }
    });
  }

  loadUser(userId: number) {
    this.loading = true;
    this.error = null;

    this.userService.getUser(userId).subscribe({
      next: (user) => {
        this.user = user;
        this.loading = false;
      },
      error: (error) => {
        this.error = error.message;
        this.loading = false;
        console.error('Error loading user for edit:', error);
        
        // Fallback to mock data if API fails
        this.user = {
          id: userId,
          name: 'John Doe',
          email: 'john@example.com',
          role: 'Admin',
          area: 'North',
          region: 'Muscat',
          country: 'Oman',
          omanPhone: '+968 9876 5432',
          countryPhone: '+1 234 567 8900',
          status: 'Active',
          createdAt: new Date(),
          updatedAt: new Date()
        };
      }
    });
  }

  onSubmit() {
    if (!this.user) return;

    this.saving = true;
    this.error = null;

    const updateRequest: UpdateUserRequest = {
      id: this.user.id,
      name: this.user.name,
      email: this.user.email,
      role: this.user.role,
      status: this.user.status,
      area: this.user.area,
      region: this.user.region,
      country: this.user.country,
      omanPhone: this.user.omanPhone,
      countryPhone: this.user.countryPhone
    };

    this.userService.updateUser(this.user.id, updateRequest).subscribe({
      next: () => {
        console.log('User updated successfully');
        this.saving = false;
        // Navigate back to user details
        this.router.navigate(['/admin/users', this.user?.id]);
      },
      error: (error) => {
        this.error = error.message;
        this.saving = false;
        console.error('Error updating user:', error);
      }
    });
  }

  goBack() {
    this.router.navigate(['/admin/users', this.user?.id]);
  }
}
