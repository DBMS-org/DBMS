import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AddUserComponent } from './add-user/add-user.component';
import { UserService } from '../../../core/services/user.service';
import { User, CreateUserRequest } from '../../../core/models/user.model';

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
  loading = false;
  error: string | null = null;

  constructor(
    private router: Router,
    private userService: UserService
  ) {}

  ngOnInit() {
    this.loadUsers();
  }

  loadUsers() {
    this.loading = true;
    this.error = null;
    
    this.userService.getUsers().subscribe({
      next: (users) => {
        this.users = users;
        this.filteredUsers = [...this.users];
        this.loading = false;
      },
      error: (error) => {
        this.error = error.message;
        this.loading = false;
        console.error('Error loading users:', error);
        
        // Fallback to mock data if API fails
        this.loadMockData();
      }
    });
  }

  private loadMockData() {
    // Keep mock data as fallback
    this.users = [
      {
        id: 1,
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
      },
      {
        id: 2,
        name: 'Jane Smith',
        email: 'jane@example.com',
        role: 'User',
        region: 'Salalah',
        country: 'UAE',
        omanPhone: '+968 1234 5678',
        countryPhone: '+971 50 123 4567',
        status: 'Active',
        createdAt: new Date(),
        updatedAt: new Date()
      },
      {
        id: 3,
        name: 'Bob Wilson',
        email: 'bob@example.com',
        role: 'User',
        region: 'Sohar',
        country: 'Oman',
        omanPhone: '+968 5555 6666',
        countryPhone: '+968 7777 8888',
        status: 'Inactive',
        createdAt: new Date(),
        updatedAt: new Date()
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
    this.error = null; // Clear any previous errors
    this.showAddUserModal = true;
  }

  onCloseAddUser() {
    this.showAddUserModal = false;
  }

  onSaveUser(userData: CreateUserRequest) {
    this.userService.createUser(userData).subscribe({
      next: (newUser) => {
        this.users.push(newUser);
        this.filteredUsers = [...this.users];
        this.showAddUserModal = false;
        this.error = null; // Clear any previous errors
        console.log('User created successfully:', newUser);
      },
      error: (error) => {
        this.error = error.message;
        console.error('Error creating user:', error);
        // Don't close modal or add to local list when there's an error
        // Keep the modal open so user can see the error and try again
      }
    });
  }

  showUserDetails(user: User) {
    this.router.navigate(['/admin/users', user.id]);
  }

  editUser(userId: number) {
    this.router.navigate(['/admin/users', userId, 'edit']);
  }

  deleteUser(userId: number) {
    if (confirm('Are you sure you want to delete this user?')) {
      this.userService.deleteUser(userId).subscribe({
        next: () => {
          this.users = this.users.filter(user => user.id !== userId);
          this.filteredUsers = this.filteredUsers.filter(user => user.id !== userId);
          console.log('User deleted successfully');
        },
        error: (error) => {
          this.error = error.message;
          console.error('Error deleting user:', error);
        }
      });
    }
  }

  // Test database connection
  testConnection() {
    this.userService.testConnection().subscribe({
      next: (result) => {
        alert(`Connection successful! Database: ${result.database}, Users: ${result.userCount}`);
      },
      error: (error) => {
        alert(`Connection failed: ${error.message}`);
      }
    });
  }
}
