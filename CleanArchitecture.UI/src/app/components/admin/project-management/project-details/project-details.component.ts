import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ProjectService } from '../../../../core/services/project.service';
import { Project, ProjectSite, UpdateProjectRequest } from '../../../../core/models/project.model';

@Component({
  selector: 'app-project-details',
  imports: [CommonModule, FormsModule],
  templateUrl: './project-details.component.html',
  styleUrl: './project-details.component.scss'
})
export class ProjectDetailsComponent implements OnInit {
  project: Project | null = null;
  projectSites: ProjectSite[] = [];
  loading = false;
  error: string | null = null;
  isEditing = false;
  
  // Form data for editing
  editForm: UpdateProjectRequest = {
    id: '',
    name: '',
    region: '',
    project: '',
    status: '',
    description: '',
    startDate: undefined,
    endDate: undefined,
    budget: undefined,
    assignedUserId: undefined
  };

  statusOptions = ['Active', 'Inactive', 'Completed', 'On Hold', 'Cancelled'];
  regionOptions = ['Muscat', 'Dhofar', 'Musandam', 'Al Buraimi', 'Al Dakhiliyah', 'Al Dhahirah', 'Al Wusta', 'Al Batinah North', 'Al Batinah South', 'Ash Sharqiyah North', 'Ash Sharqiyah South'];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private projectService: ProjectService
  ) {}

  ngOnInit() {
    this.route.params.subscribe(params => {
      const projectId = params['id'];
      if (projectId) {
        this.loadProject(projectId);
        this.loadProjectSites(projectId);
      }
    });
  }

  loadProject(id: string) {
    this.loading = true;
    this.error = null;
    
    this.projectService.getProject(id).subscribe({
      next: (project) => {
        this.project = project;
        this.initializeEditForm();
        this.loading = false;
      },
      error: (error) => {
        this.error = error.message;
        this.loading = false;
        console.error('Error loading project:', error);
        
        // Fallback to mock data if API fails
        this.loadMockProject(id);
      }
    });
  }

  loadProjectSites(projectId: string) {
    this.projectService.getProjectSites(projectId).subscribe({
      next: (sites) => {
        this.projectSites = sites;
      },
      error: (error) => {
        console.error('Error loading project sites:', error);
        // Fallback to mock sites
        this.loadMockSites(projectId);
      }
    });
  }

  private loadMockProject(id: string) {
    // Mock project data as fallback
    this.project = {
      id: id,
      name: 'Project Alpha',
      region: 'Muscat',
      project: 'Muttrah Construction',
      status: 'Active',
      description: 'This is a sample project description for Project Alpha. It includes various engineering and construction activities in the Muttrah area.',
      startDate: new Date('2024-01-15'),
      endDate: new Date('2024-12-31'),
      budget: 2500000,
      assignedUserId: 1,
      createdAt: new Date('2024-01-01'),
      updatedAt: new Date()
    };
    this.initializeEditForm();
  }

  private loadMockSites(projectId: string) {
    // Mock sites data as fallback
    this.projectSites = [
      {
        id: '1',
        projectId: projectId,
        name: 'Main Construction Site',
        location: 'Muscat, Oman',
        coordinates: { latitude: 23.5859, longitude: 58.4059 },
        description: 'Primary construction site for the project',
        createdAt: new Date('2024-01-15'),
        updatedAt: new Date()
      },
      {
        id: '2',
        projectId: projectId,
        name: 'Secondary Site',
        location: 'Muttrah, Oman',
        coordinates: { latitude: 23.6145, longitude: 58.5627 },
        description: 'Secondary site for material storage and processing',
        createdAt: new Date('2024-02-01'),
        updatedAt: new Date()
      }
    ];
  }

  initializeEditForm() {
    if (this.project) {
      this.editForm = {
        id: this.project.id,
        name: this.project.name,
        region: this.project.region,
        project: this.project.project,
        status: this.project.status,
        description: this.project.description || '',
        startDate: this.project.startDate,
        endDate: this.project.endDate,
        budget: this.project.budget,
        assignedUserId: this.project.assignedUserId
      };
    }
  }

  toggleEdit() {
    this.isEditing = !this.isEditing;
    if (!this.isEditing) {
      // Reset form when canceling edit
      this.initializeEditForm();
    }
  }

  saveProject() {
    if (!this.project) return;

    this.loading = true;
    this.error = null;

    this.projectService.updateProject(this.project.id, this.editForm).subscribe({
      next: () => {
        // Reload project data
        this.loadProject(this.project!.id);
        this.isEditing = false;
        console.log('Project updated successfully');
      },
      error: (error) => {
        this.error = error.message;
        this.loading = false;
        console.error('Error updating project:', error);
        
        // Fallback: update local data
        if (this.project) {
          Object.assign(this.project, this.editForm, { updatedAt: new Date() });
          this.isEditing = false;
          this.loading = false;
        }
      }
    });
  }

  deleteProject() {
    if (!this.project) return;

    if (confirm(`Are you sure you want to delete project "${this.project.name}"?`)) {
      this.projectService.deleteProject(this.project.id).subscribe({
        next: () => {
          console.log('Project deleted successfully');
          this.router.navigate(['/admin/project-management']);
        },
        error: (error) => {
          this.error = error.message;
          console.error('Error deleting project:', error);
        }
      });
    }
  }

  goBack() {
    this.router.navigate(['/admin/project-management']);
  }

  viewSiteDetails(site: ProjectSite) {
    // Navigate to site details or open modal
    console.log('View site details:', site);
  }

  addNewSite() {
    // Navigate to add site component or open modal
    console.log('Add new site for project:', this.project?.id);
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

  formatCurrency(amount: number | undefined): string {
    if (!amount) return 'N/A';
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'PKR'
    }).format(amount);
  }

  formatDate(date: Date | undefined): string {
    if (!date) return 'N/A';
    return new Intl.DateTimeFormat('en-US', {
      year: 'numeric',
      month: 'long',
      day: 'numeric'
    }).format(date);
  }

  formatDateForInput(date: Date | undefined): string {
    if (!date) return '';
    return date.toISOString().split('T')[0];
  }

  onStartDateChange(event: Event): void {
    const target = event.target as HTMLInputElement;
    this.editForm.startDate = target.value ? new Date(target.value) : undefined;
  }

  onEndDateChange(event: Event): void {
    const target = event.target as HTMLInputElement;
    this.editForm.endDate = target.value ? new Date(target.value) : undefined;
  }
}
