import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { ProjectService } from '../../../core/services/project.service';
import { AuthService } from '../../../core/services/auth.service';
import { Project } from '../../../core/models/project.model';
import { ViewProjectComponent } from './view-project/view-project.component';

@Component({
  selector: 'app-project-management',
  imports: [CommonModule, FormsModule, ViewProjectComponent],
  templateUrl: './project-management.component.html',
  styleUrl: './project-management.component.scss'
})
export class ProjectManagementComponent implements OnInit {
  projects: Project[] = [];
  filteredProjects: Project[] = [];
  searchQuery: string = '';
  statusFilter: string = '';
  loading = false;
  error: string | null = null;

  // Modal properties
  selectedProject: Project | null = null;
  isViewModalVisible = false;

  statusOptions = ['', 'Active', 'Inactive', 'Completed', 'On Hold', 'Cancelled'];

  constructor(
    private router: Router,
    private projectService: ProjectService,
    private authService: AuthService
  ) {}

  ngOnInit() {
    this.loadProjects();
  }

  loadProjects() {
    this.loading = true;
    this.error = null;
    
    // Use role-based project filtering
    this.projectService.getProjectsForCurrentUser().subscribe({
      next: (projects) => {
        this.projects = projects;
        this.filteredProjects = [...this.projects];
        this.loading = false;
      },
      error: (error) => {
        this.error = error.message;
        this.loading = false;
        console.error('Error loading projects:', error);
        
        // Fallback to mock data if API fails
        this.loadMockData();
      }
    });
  }

  private loadMockData() {
    // Mock data as fallback - filter based on user role and region
    const allMockProjects = [
      {
        id: 1,
        name: 'Project Alpha - Muttrah Construction',
        region: 'Muscat',
        status: 'Active',
        description: 'Main construction project in Muttrah area',
        startDate: new Date('2024-01-15'),
        endDate: new Date('2024-12-31'),
        assignedUserId: 1,
        createdAt: new Date('2024-01-01'),
        updatedAt: new Date()
      },
      {
        id: 2,
        name: 'Project Beta - Salalah Infrastructure',
        region: 'Dhofar',
        status: 'Active',
        description: 'Infrastructure development project',
        startDate: new Date('2024-02-01'),
        endDate: new Date('2025-01-31'),
        assignedUserId: 2,
        createdAt: new Date('2024-01-15'),
        updatedAt: new Date()
      },
      {
        id: 3,
        name: 'Project Gamma - Sohar Industrial Zone',
        region: 'Al Batinah North',
        status: 'Completed',
        description: 'Industrial zone construction project',
        startDate: new Date('2023-06-01'),
        endDate: new Date('2023-12-31'),
        assignedUserId: 3,
        createdAt: new Date('2023-06-01'),
        updatedAt: new Date()
      }
    ];

    // Apply the same role-based filtering to mock data
    const currentUser = this.authService.getCurrentUser();
    
    if (this.authService.isAdmin()) {
      this.projects = allMockProjects;
    } else if (this.authService.isBlastingEngineer() && currentUser?.region) {
      this.projects = allMockProjects.filter(project => 
        project.region.toLowerCase() === currentUser.region.toLowerCase()
      );
    } else {
      this.projects = [];
    }
    
    this.filteredProjects = [...this.projects];
  }

  onSearch() {
    this.applyFilters();
  }

  onFilter() {
    this.applyFilters();
  }

  private applyFilters() {
    let filtered = [...this.projects];

    // Apply search filter
    if (this.searchQuery.trim()) {
      const query = this.searchQuery.toLowerCase().trim();
      filtered = filtered.filter(project => 
        project.name?.toLowerCase().includes(query) ||
        project.region.toLowerCase().includes(query) ||
        project.id.toString().includes(query)
      );
    }

    // Apply status filter
    if (this.statusFilter) {
      filtered = filtered.filter(project => project.status === this.statusFilter);
    }

    this.filteredProjects = filtered;
  }

  viewProjectDetails(project: Project) {
    this.selectedProject = project;
    this.isViewModalVisible = true;
  }

  closeViewModal() {
    this.isViewModalVisible = false;
    this.selectedProject = null;
  }

  viewProjectSites(project: Project) {
    this.router.navigate(['/blasting-engineer/project-management', project.id, 'sites']);
  }

  getStatusClass(status: string): string {
    switch (status.toLowerCase()) {
      case 'active':
        return 'status-active';
      case 'inactive':
        return 'status-inactive';
      case 'completed':
        return 'status-completed';
      case 'on hold':
        return 'status-hold';
      case 'cancelled':
        return 'status-cancelled';
      default:
        return 'status-default';
    }
  }
}
