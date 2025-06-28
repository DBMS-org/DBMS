import { Component, EventEmitter, OnInit, Output, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Subscription, forkJoin } from 'rxjs';
import { UserService } from '../../../../core/services/user.service';
import { ProjectService } from '../../../../core/services/project.service';
import { User } from '../../../../core/models/user.model';
import { UserProjectsComponent } from './user-projects/user-projects.component';
import { OperatorAssignmentComponent } from './operator-assignment/operator-assignment.component';

interface UserAssignment {
  id: number;
  name: string;
  email: string;
  role: string;
  region: string;
  status: string;
}

@Component({
  selector: 'app-assign-user',
  standalone: true,
  imports: [CommonModule, FormsModule, UserProjectsComponent, OperatorAssignmentComponent],
  templateUrl: './assign-user.component.html',
  styleUrls: ['./assign-user.component.scss']
})
export class AssignUserComponent implements OnInit, OnDestroy {
  @Output() close = new EventEmitter<void>();

  userAssignments: UserAssignment[] = [];
  filteredAssignments: UserAssignment[] = [];
  users: User[] = [];
  projects: any[] = [];
  
  isLoading = false;
  error: string | null = null;
  
  // User Projects Modal
  showUserProjectsModal = false;
  selectedUserForProjects: UserAssignment | null = null;
  showOperatorAssignment = false;
  selectedOperatorForAssignment: UserAssignment | null = null;
  
  // Filters
  selectedRegion = '';
  selectedRole = '';
  searchTerm = '';
  
  // Available regions and roles for filtering
  regions = ['Al Hajar Mountains', 'Muscat Governorate', 'Dhofar Governorate', 'Ad Dakhiliyah Governorate', 'Al Batinah North Governorate'];
  roles = ['Blasting Engineer', 'Machine Manager', 'Mechanical Engineer', 'Operator', 'Store Manager', 'Explosive Manager'];
  
  private subscriptions: Subscription[] = [];

  constructor(
    private userService: UserService,
    private projectService: ProjectService
  ) {}

  ngOnInit(): void {
    this.loadData();
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }

  private loadData(): void {
    this.isLoading = true;
    this.error = null;

    const sub = forkJoin({
      users: this.userService.getUsers(),
      projects: this.projectService.getProjects()
    }).subscribe({
      next: ({ users, projects }) => {
        this.users = users;
        this.projects = projects;
        this.processUserAssignments();
        this.isLoading = false;
      },
      error: (error) => {
        this.error = 'Failed to load data: ' + error.message;
        this.isLoading = false;
        console.error('Error loading data:', error);
      }
    });

    this.subscriptions.push(sub);
  }

  private processUserAssignments(): void {
    this.userAssignments = this.users
      .filter(user => user.role !== 'Admin')
      .map(user => ({
        id: user.id,
        name: user.name,
        email: user.email,
        role: user.role,
        region: user.region,
        status: user.status
      }));

    this.applyFilters();
  }

  applyFilters(): void {
    let filtered = [...this.userAssignments];

    // Filter by region
    if (this.selectedRegion) {
      filtered = filtered.filter(assignment => 
        assignment.region === this.selectedRegion
      );
    }

    // Filter by role
    if (this.selectedRole) {
      filtered = filtered.filter(assignment => 
        assignment.role === this.selectedRole
      );
    }

    // Filter by search term
    if (this.searchTerm.trim()) {
      const term = this.searchTerm.toLowerCase().trim();
      filtered = filtered.filter(assignment =>
        assignment.name.toLowerCase().includes(term) ||
        assignment.email.toLowerCase().includes(term) ||
        assignment.region.toLowerCase().includes(term)
      );
    }

    this.filteredAssignments = filtered;
  }

  onRegionFilterChange(): void {
    this.applyFilters();
  }

  onRoleFilterChange(): void {
    this.applyFilters();
  }

  onSearchChange(): void {
    this.applyFilters();
  }

  clearFilters(): void {
    this.selectedRegion = '';
    this.selectedRole = '';
    this.searchTerm = '';
    this.applyFilters();
  }

  refreshData(): void {
    this.loadData();
  }

  closeModal(): void {
    this.close.emit();
  }

  getStatusClass(status: string): string {
    return status === 'Active' ? 'status-active' : 'status-inactive';
  }

  getRoleClass(role: string): string {
    return `role-${role.toLowerCase().replace(' ', '-')}`;
  }

  getUserInitials(name: string): string {
    const names = name.split(' ');
    if (names.length >= 2) {
      return (names[0][0] + names[1][0]).toUpperCase();
    }
    return names[0].substring(0, 2).toUpperCase();
  }

  getRoleIcon(role: string): string {
    const roleIconMap: { [key: string]: string } = {
      'Admin': 'admin_panel_settings',
      'Blasting Engineer': 'engineering',
      'Machine Manager': 'precision_manufacturing',
      'Mechanical Engineer': 'build',
      'Operator': 'smart_toy',
      'Store Manager': 'inventory',
      'Explosive Manager': 'dangerous'
    };
    return roleIconMap[role] || 'person';
  }

  trackByUserId(index: number, assignment: UserAssignment): number {
    return assignment.id;
  }

  viewUserProjects(assignment: UserAssignment): void {
    this.selectedUserForProjects = assignment;
    this.showUserProjectsModal = true;
  }

  onCloseUserProjects(): void {
    this.showUserProjectsModal = false;
    this.selectedUserForProjects = null;
  }

  viewOperatorAssignment(operator: UserAssignment): void {
    this.selectedOperatorForAssignment = operator;
    this.showOperatorAssignment = true;
  }

  onCloseOperatorAssignment(): void {
    this.showOperatorAssignment = false;
    this.selectedOperatorForAssignment = null;
  }

  isOperator(user: UserAssignment): boolean {
    return user.role.toLowerCase() === 'operator';
  }
}
