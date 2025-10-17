import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Project, ProjectSite } from '../../../../core/models/project.model';
import { ProjectService } from '../../../../core/services/project.service';

@Component({
  selector: 'app-view-project',
  imports: [CommonModule],
  templateUrl: './view-project.component.html',
  styleUrl: './view-project.component.scss'
})
export class ViewProjectComponent {
  @Input() project: Project | null = null;
  @Input() isVisible: boolean = false;
  @Output() close = new EventEmitter<void>();

  projectSites: ProjectSite[] = [];
  loading = false;

  constructor(private projectService: ProjectService) {}

  ngOnChanges() {
    if (this.project && this.isVisible) {
      this.loadProjectSites();
    }
  }

  loadProjectSites() {
    if (!this.project) return;
    
    this.projectService.getProjectSites(this.project.id).subscribe({
      next: (sites) => {
        this.projectSites = sites;
      },
      error: (error) => {
        console.error('Error loading project sites:', error);
        // Fallback to mock sites
        this.loadMockSites();
      }
    });
  }

  private loadMockSites() {
    this.projectSites = [
      {
        id: 1,
        projectId: this.project!.id,
        name: 'Main Construction Site',
        location: 'Muscat, Oman',
        coordinates: { latitude: 23.5859, longitude: 58.4059 },
        description: 'Primary construction site for the project',
        createdAt: new Date('2024-01-15'),
        updatedAt: new Date()
      },
      {
        id: 2,
        projectId: this.project!.id,
        name: 'Secondary Site',
        location: 'Muttrah, Oman',
        coordinates: { latitude: 23.6145, longitude: 58.5627 },
        description: 'Secondary site for material storage and processing',
        createdAt: new Date('2024-02-01'),
        updatedAt: new Date()
      }
    ];
  }

  closeModal() {
    this.close.emit();
  }

  viewSiteDetails(site: ProjectSite) {
    console.log('View site details:', site);
    // TODO: Navigate to site details or open site modal
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

  formatDate(date: Date | undefined): string {
    if (!date) return 'N/A';
    return new Intl.DateTimeFormat('en-US', {
      year: 'numeric',
      month: 'long',
      day: 'numeric'
    }).format(date);
  }
}
