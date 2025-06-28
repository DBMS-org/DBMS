import { Component, EventEmitter, Input, OnInit, Output, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Subscription, forkJoin } from 'rxjs';
import { ProjectService } from '../../../../../core/services/project.service';
import { SiteService } from '../../../../../core/services/site.service';

interface UserAssignment {
  id: number;
  name: string;
  email: string;
  role: string;
  region: string;
  status: string;
}

interface ProjectDetails {
  id: number;
  name: string;
  description?: string;
  region: string;
  status: string;
  startDate?: Date;
  endDate?: Date;
  sites: any[];
  assignedUserId?: number;
  assignedUsers?: number[];
  progress?: number;
  createdAt?: Date;
  updatedAt?: Date;
  isPatternApproved?: boolean;
  isSimulationConfirmed?: boolean;
}

@Component({
  selector: 'app-user-projects',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './user-projects.component.html',
  styleUrls: ['./user-projects.component.scss']
})
export class UserProjectsComponent implements OnInit, OnDestroy {
  @Input() selectedUser: UserAssignment | null = null;
  @Output() close = new EventEmitter<void>();

  userProjects: ProjectDetails[] = [];
  allProjects: ProjectDetails[] = [];
  isLoading = false;
  error: string | null = null;
  
  // Filter options
  statusFilter = '';
  searchTerm = '';
  
  private subscriptions: Subscription[] = [];

  constructor(
    private projectService: ProjectService,
    private siteService: SiteService
  ) {}

  ngOnInit(): void {
    if (this.selectedUser) {
      this.loadUserProjects();
    }
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }

  private loadUserProjects(): void {
    if (!this.selectedUser) return;

    this.isLoading = true;
    this.error = null;

    const sub = forkJoin({
      projects: this.projectService.getProjects(),
      sites: this.siteService.getAllSites()
    }).subscribe({
      next: ({ projects, sites }) => {
        this.allProjects = projects.map(project => ({
          ...project,
          sites: sites.filter(site => site.projectId === project.id),
          progress: this.calculateProjectProgress(project, sites.filter(site => site.projectId === project.id))
        }));
        
        this.filterUserProjects();
        this.isLoading = false;
      },
      error: (error) => {
        this.error = 'Failed to load projects: ' + error.message;
        this.isLoading = false;
        console.error('Error loading projects:', error);
      }
    });

    this.subscriptions.push(sub);
  }

  private filterUserProjects(): void {
    if (!this.selectedUser) return;

    // Filter projects based on user's region - all projects in the same region as the user
    this.userProjects = this.allProjects.filter(project => {
      return project.region === this.selectedUser!.region;
    });

    this.applyFilters();
  }

  private calculateProjectProgress(project: any, sites: any[]): number {
    if (sites.length === 0) return 0;
    
    const completedSites = sites.filter(site => site.status === 'Completed').length;
    return Math.round((completedSites / sites.length) * 100);
  }

  applyFilters(): void {
    let filtered = [...this.userProjects];

    // Apply status filter
    if (this.statusFilter) {
      filtered = filtered.filter(project => project.status === this.statusFilter);
    }

    // Apply search filter
    if (this.searchTerm.trim()) {
      const term = this.searchTerm.toLowerCase().trim();
      filtered = filtered.filter(project =>
        project.name.toLowerCase().includes(term) ||
        (project.description && project.description.toLowerCase().includes(term)) ||
        project.region.toLowerCase().includes(term)
      );
    }

    this.userProjects = filtered;
  }

  onStatusFilterChange(): void {
    this.applyFilters();
  }

  onSearchChange(): void {
    this.applyFilters();
  }

  clearFilters(): void {
    this.statusFilter = '';
    this.searchTerm = '';
    this.filterUserProjects();
  }

  refreshData(): void {
    this.loadUserProjects();
  }

  closeModal(): void {
    this.close.emit();
  }

  getStatusClass(status: string): string {
    const statusClasses: { [key: string]: string } = {
      'Active': 'status-active',
      'Planning': 'status-planning',
      'In Progress': 'status-in-progress',
      'Completed': 'status-completed',
      'On Hold': 'status-on-hold',
      'Cancelled': 'status-cancelled'
    };
    return statusClasses[status] || 'status-default';
  }

  getProgressClass(progress: number): string {
    if (progress >= 80) return 'progress-high';
    if (progress >= 50) return 'progress-medium';
    if (progress >= 20) return 'progress-low';
    return 'progress-none';
  }

  formatDate(dateString: string): string {
    if (!dateString) return 'N/A';
    const date = new Date(dateString);
    return date.toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric'
    });
  }

  getUserInitials(name: string): string {
    const names = name.split(' ');
    if (names.length >= 2) {
      return (names[0][0] + names[1][0]).toUpperCase();
    }
    return names[0].substring(0, 2).toUpperCase();
  }

  trackByProjectId(index: number, project: ProjectDetails): number {
    return project.id;
  }
}
