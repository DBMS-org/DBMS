import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from '../../../../core/services/user.service';
import { User } from '../../../../core/models/user.model';

@Component({
  selector: 'app-user-details',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './user-details.component.html',
  styleUrls: ['./user-details.component.scss']
})
export class UserDetailsComponent implements OnInit {
  user: User | null = null;
  loading = false;
  error: string | null = null;

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
        console.error('Error loading user:', error);
        
        // Fallback to mock data if API fails
        this.user = {
          id: userId,
          name: 'John Doe',
          email: 'john@example.com',
          role: 'Admin',
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

  goBack() {
    this.router.navigate(['/admin/users']);
  }

  editUser() {
    if (this.user) {
      this.router.navigate(['/admin/users', this.user.id, 'edit']);
    }
  }

  deleteUser() {
    if (this.user && confirm('Are you sure you want to delete this user?')) {
      this.userService.deleteUser(this.user.id).subscribe({
        next: () => {
          console.log('User deleted successfully');
          this.router.navigate(['/admin/users']);
        },
        error: (error) => {
          this.error = error.message;
          console.error('Error deleting user:', error);
        }
      });
    }
  }
}
