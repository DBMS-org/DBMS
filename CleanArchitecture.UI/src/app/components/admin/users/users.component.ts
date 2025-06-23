import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AddUserComponent } from './add-user/add-user.component';
import { AssignUserComponent } from './assign-user/assign-user.component';
import { UserService } from '../../../core/services/user.service';
import { User, CreateUserRequest } from '../../../core/models/user.model';

@Component({
  selector: 'app-users',
  standalone: true,
  imports: [CommonModule, FormsModule, AddUserComponent, AssignUserComponent],
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss']
})
export class UsersComponent implements OnInit {
  users: User[] = [];
  filteredUsers: User[] = [];
  searchQuery: string = '';
  statusFilter: string = '';
  showAddUserModal = false;
  showAssignUserModal = false;
  loading = false;
  error: string | null = null;
  showDeleteModal = false;
  userToDelete: User | null = null;

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

  openDeleteModal(user: User) {
    this.userToDelete = user;
    this.showDeleteModal = true;
  }

  closeDeleteModal() {
    this.showDeleteModal = false;
    this.userToDelete = null;
  }

  confirmDelete() {
    if (this.userToDelete) {
      this.userService.deleteUser(this.userToDelete.id).subscribe({
        next: () => {
          this.users = this.users.filter(user => user.id !== this.userToDelete!.id);
          this.filteredUsers = this.filteredUsers.filter(user => user.id !== this.userToDelete!.id);
          this.closeDeleteModal();
          console.log('User deleted successfully');
        },
        error: (error) => {
          this.error = error.message;
          this.closeDeleteModal();
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
