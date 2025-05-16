import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AddUserComponent } from './add-user/add-user.component';

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
  selector: 'app-users',
  standalone: true,
  imports: [CommonModule, FormsModule, AddUserComponent],
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss']
})
export class UsersComponent implements OnInit {
  users: User[] = [];
  filteredUsers: User[] = [];
  searchQuery: string = '';
  statusFilter: string = '';
  showAddUserModal = false;

  constructor(private router: Router) {}

  ngOnInit() {
    // TODO: Replace with actual API call
    this.users = [
      {
        id: 1,
        name: 'John Doe',
        email: 'john@example.com',
        role: 'Admin',
        area: 'North',
        region: 'Muscat',
        country: 'Oman',
        omanPhone: '+968 9876 5432',
        countryPhone: '+1 234 567 8900',
        status: 'Active'
      },
      {
        id: 2,
        name: 'Jane Smith',
        email: 'jane@example.com',
        role: 'User',
        area: 'South',
        region: 'Salalah',
        country: 'UAE',
        omanPhone: '+968 1234 5678',
        countryPhone: '+971 50 123 4567',
        status: 'Active'
      },
      {
        id: 3,
        name: 'Bob Wilson',
        email: 'bob@example.com',
        role: 'User',
        area: 'East',
        region: 'Sohar',
        country: 'Oman',
        omanPhone: '+968 5555 6666',
        countryPhone: '+968 7777 8888',
        status: 'Inactive'
      }
    ];
    this.filteredUsers = [...this.users];
  }

  onSearch() {
    let filtered = [...this.users];

    // Apply search filter
    if (this.searchQuery.trim()) {
      const query = this.searchQuery.toLowerCase().trim();
      filtered = filtered.filter(user => 
        user.name.toLowerCase().includes(query) ||
        user.email.toLowerCase().includes(query) ||
        user.region.toLowerCase().includes(query)
      );
    }

    // Apply status filter
    if (this.statusFilter) {
      filtered = filtered.filter(user => user.status === this.statusFilter);
    }

    this.filteredUsers = filtered;
  }

  addUser() {
    this.showAddUserModal = true;
  }

  onCloseAddUser() {
    this.showAddUserModal = false;
  }

  onSaveUser(user: User) {
    // TODO: Replace with actual API call
    this.users.push(user);
    this.filteredUsers = [...this.users];
    this.showAddUserModal = false;
  }

  showUserDetails(user: User) {
    this.router.navigate(['/admin/users', user.id]);
  }

  editUser(userId: number) {
    this.router.navigate(['/admin/users', userId, 'edit']);
  }

  deleteUser(userId: number) {
    // TODO: Implement delete functionality
    console.log('Delete user:', userId);
  }
}
