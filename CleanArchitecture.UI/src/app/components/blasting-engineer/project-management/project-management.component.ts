import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { ProjectService } from '../../../core/services/project.service';
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
  regionFilter: string = '';
  loading = false;
  error: string | null = null;

  // Modal properties
  selectedProject: Project | null = null;
  isViewModalVisible = false;

  statusOptions = ['', 'Active', 'Inactive', 'Completed', 'On Hold', 'Cancelled'];
  regionOptions = ['', 'Muscat', 'Dhofar', 'Musandam', 'Al Buraimi', 'Al Dakhiliyah', 'Al Dhahirah', 'Al Wusta', 'Al Batinah North', 'Al Batinah South', 'Ash Sharqiyah North', 'Ash Sharqiyah South'];

  constructor(
    private router: Router,
    private projectService: ProjectService
  ) {}

  ngOnInit() {
    this.loadProjects();
  }

  loadProjects() {
    this.loading = true;
    this.error = null;
    
    this.projectService.getProjects().subscribe({
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
    // Mock data as fallback
    this.projects = [
      {
        id: '001',
        name: 'Project Alpha',
        region: 'Muscat',
        project: 'Muttrah Construction',
        status: 'Active',
        description: 'Main construction project in Muttrah area',
        startDate: new Date('2024-01-15'),
        endDate: new Date('2024-12-31'),
        budget: 2500000,
        assignedUserId: 1,
        createdAt: new Date('2024-01-01'),
        updatedAt: new Date()
      },
      {
        id: '002',
        name: 'Project Beta',
        region: 'Dhofar',
        project: 'Salalah Infrastructure',
        status: 'Active',
        description: 'Infrastructure development project',
        startDate: new Date('2024-02-01'),
        endDate: new Date('2025-01-31'),
        budget: 3000000,
        assignedUserId: 2,
        createdAt: new Date('2024-01-15'),
        updatedAt: new Date()
      },
      {
        id: '003',
        name: 'Project Gamma',
        region: 'Al Batinah North',
        project: 'Sohar Industrial Zone',
        status: 'Completed',
        description: 'Industrial zone construction project',
        startDate: new Date('2023-06-01'),
        endDate: new Date('2023-12-31'),
        budget: 1800000,
        assignedUserId: 3,
        createdAt: new Date('2023-06-01'),
        updatedAt: new Date()
      }
    ];
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
        project.project.toLowerCase().includes(query) ||
        project.id.toLowerCase().includes(query)
      );
    }

    // Apply status filter
    if (this.statusFilter) {
      filtered = filtered.filter(project => project.status === this.statusFilter);
    }

    // Apply region filter
    if (this.regionFilter) {
      filtered = filtered.filter(project => project.region === this.regionFilter);
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
