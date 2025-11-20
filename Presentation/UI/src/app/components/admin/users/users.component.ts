import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AddUserComponent } from './add-user/add-user.component';
import { AssignUserComponent } from './assign-user/assign-user.component';
import { UserService } from '../../../core/services/user.service';
import { User, CreateUserRequest } from '../../../core/models/user.model';
import { NotificationService } from '../../../core/services/notification.service';

// PrimeNG Components (v19+)
import { TableModule } from 'primeng/table';
import { Button } from 'primeng/button';
import { TagModule } from 'primeng/tag';
import { TooltipModule } from 'primeng/tooltip';

@Component({
  selector: 'app-users',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    AddUserComponent,
    AssignUserComponent,
    TableModule,
    Button,
    TagModule,
    TooltipModule
  ],
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss']
})
export class UsersComponent implements OnInit {
  @ViewChild(AddUserComponent) addUserComponent?: AddUserComponent;

  users: User[] = [];
  filteredUsers: User[] = [];
  searchQuery: string = '';
  statusFilter: string = '';
  showAddUserModal = false;
  showAssignUserModal = false;
  loading = false;
  error: string | null = null;
  errorDetails: string[] = [];
  showDeleteModal = false;
  userToDelete: User | null = null;
  isDeleting = false; // Track deletion state

  constructor(
    private router: Router,
    private userService: UserService,
    private notification: NotificationService
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
      }
    });
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
    this.errorDetails = [];
    this.showAddUserModal = true;
  }

  onCloseAddUser() {
    this.showAddUserModal = false;
  }

  openAssignUserModal() {
    this.error = null;
    this.showAssignUserModal = true;
  }

  onCloseAssignUser() {
    this.showAssignUserModal = false;
  }

  onSaveUser(userData: CreateUserRequest) {
    this.error = null; // Clear any previous errors
    this.errorDetails = [];

    this.userService.createUser(userData).subscribe({
      next: (newUser) => {
        this.users.push(newUser);
        this.filteredUsers = [...this.users];
        this.showAddUserModal = false;
        this.error = null;
        this.errorDetails = [];

        // Reset submission state in child component
        if (this.addUserComponent) {
          this.addUserComponent.resetSubmissionState();
        }

        // Show success toast
        this.notification.showSuccess(
          `User "${newUser.name}" created successfully! Credentials have been sent via email.`,
          'User Created'
        );
        console.log('User created successfully:', newUser);
      },
      error: (error) => {
        // Reset submission state in child component to allow retry
        if (this.addUserComponent) {
          this.addUserComponent.resetSubmissionState();
        }

        // Store error for display in modal
        this.error = error.message || 'An error occurred while creating the user';
        this.errorDetails = error.details || [];

        // Also show error toast for immediate feedback
        this.notification.showError(
          error.message || 'Failed to create user. Please check the form and try again.',
          'Creation Failed'
        );
        console.error('Error creating user:', error);
      }
    });
  }

  showUserDetails(user: User) {
    this.router.navigate(['/admin/users', user.id]);
  }

  editUser(userId: number) {
    this.router.navigate(['/admin/users', userId, 'edit']);
  }

  openDeleteModal(user: User) {
    this.userToDelete = user;
    this.showDeleteModal = true;
  }

  closeDeleteModal() {
    this.showDeleteModal = false;
    this.userToDelete = null;
  }

  confirmDelete() {
    if (this.userToDelete && !this.isDeleting) {
      const userName = this.userToDelete.name;
      const userId = this.userToDelete.id;

      this.isDeleting = true; // Disable delete button

      this.userService.deleteUser(userId).subscribe({
        next: (response: any) => {
          // Remove user from lists
          this.users = this.users.filter(user => user.id !== userId);
          this.filteredUsers = this.filteredUsers.filter(user => user.id !== userId);
          this.isDeleting = false;
          this.closeDeleteModal();

          // Show success toast
          this.notification.showSuccess(
            response?.message || `User "${userName}" has been successfully deleted.`,
            'User Deleted'
          );
          console.log('User deleted successfully');
        },
        error: (error) => {
          this.isDeleting = false;
          this.closeDeleteModal();

          // Show error toast
          this.notification.showError(
            error.message || 'Failed to delete user. Please try again.',
            'Deletion Failed'
          );
          console.error('Error deleting user:', error);
        }
      });
    }
  }

  deleteUser(userId: number) {
    const user = this.users.find(u => u.id === userId);
    if (user) {
      this.openDeleteModal(user);
    }
  }

  testConnection() {
    this.userService.testConnection().subscribe({
      next: (result) => {
        this.notification.showSuccess(`Connection successful! Database: ${result.database}, Users: ${result.userCount}`);
      },
      error: (error) => {
        this.notification.showError(`Connection failed: ${error.message}`);
      }
    });
  }
}
