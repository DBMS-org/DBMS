import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
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
  selector: 'app-user-details',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './user-details.component.html',
  styleUrls: ['./user-details.component.scss']
})
export class UserDetailsComponent implements OnInit {
  user: User | null = null;

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

  goBack() {
    this.router.navigate(['/admin/users']);
  }

  editUser() {
    if (this.user) {
      this.router.navigate(['/admin/users', this.user.id, 'edit']);
    }
  }
}
