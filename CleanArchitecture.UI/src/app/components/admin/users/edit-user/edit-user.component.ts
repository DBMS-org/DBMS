import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

interface User {
  id: number;
  name: string;
  email: string;
  role: string;
  area: string;
  region: string;
  country: string;
  omanPhone: string;
  countryPhone: string;
  status: string;
}

@Component({
  selector: 'app-edit-user',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './edit-user.component.html',
  styleUrls: ['./edit-user.component.scss']
})
export class EditUserComponent implements OnInit {
  user: User | null = null;

  roles = ['Admin', 'User'];
  areas = ['North', 'South', 'East', 'West'];
  regions = ['Muscat', 'Salalah', 'Sohar', 'Nizwa', 'Sur'];
  countries = ['Oman', 'UAE', 'Saudi Arabia', 'Kuwait', 'Qatar', 'Bahrain'];
  statuses = ['Active', 'Inactive'];

  constructor(
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit() {
    // Get user ID from route parameters
    this.route.params.subscribe(params => {
      const userId = +params['id'];
      // TODO: Replace with actual API call
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
        status: 'Active'
      };
    });
  }

  onSubmit() {
    // TODO: Replace with actual API call
    console.log('Saving user:', this.user);
    // After successful save, navigate back to user details
    this.router.navigate(['/admin/users', this.user?.id]);
  }

  goBack() {
    this.router.navigate(['/admin/users', this.user?.id]);
  }
}
