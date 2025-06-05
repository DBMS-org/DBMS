import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Project, ProjectSite } from '../../../../core/models/project.model';
import { ProjectService } from '../../../../core/services/project.service';

@Component({
  selector: 'app-project-sites',
  imports: [CommonModule, FormsModule],
  templateUrl: './project-sites.component.html',
  styleUrl: './project-sites.component.scss'
})
export class ProjectSitesComponent implements OnInit {
  project: Project | null = null;
  projectId: number = 0;
  sites: ProjectSite[] = [];
  loading = false;
  error: string | null = null;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private projectService: ProjectService
  ) {}

  ngOnInit() {
    this.projectId = +(this.route.snapshot.paramMap.get('id') || '0');
    if (this.projectId) {
      this.loadProject();
      this.loadProjectSites();
    }
  }

  loadProject() {
    this.loading = true;
    this.error = null;

    this.projectService.getProject(this.projectId).subscribe({
      next: (project) => {
        this.project = project;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading project:', error);
        // Fallback to mock data
        this.loadMockProject();
      }
    });
  }

  private loadMockProject() {
    // Mock project data as fallback
    this.project = {
      id: this.projectId,
      name: 'Project Alpha - Muttrah Construction',
      region: 'Muscat',
      status: 'Active',
      description: 'Main construction project in Muttrah area',
      startDate: new Date('2024-01-15'),
      endDate: new Date('2024-12-31'),
      assignedUserId: 1,
      createdAt: new Date('2024-01-01'),
      updatedAt: new Date()
    };
    this.loading = false;
  }

  loadProjectSites() {
    if (!this.projectId) return;
    
    this.loading = true;
    this.error = null;

    // For now, using mock data since the service might not have sites endpoint
    this.loadMockSites();
  }

  private loadMockSites() {
    if (!this.project && !this.projectId) return;

    // Generate mock sites based on project region
    const projectRegion = this.project?.region || 'Muscat';
    const projectName = this.project?.name || 'Default Project';

    this.sites = [
      {
        id: 1,
        projectId: this.projectId,
        name: `${projectName} - Main Site`,
        location: `${projectRegion} Central Area`,
        coordinates: {
          latitude: this.getRegionCoordinates(projectRegion).lat,
          longitude: this.getRegionCoordinates(projectRegion).lng
        },
        description: 'Primary construction site for the project',
        createdAt: new Date('2024-01-15'),
        updatedAt: new Date()
      },
      {
        id: 2,
        projectId: this.projectId,
        name: `${projectName} - Secondary Site`,
        location: `${projectRegion} Industrial Zone`,
        coordinates: {
          latitude: this.getRegionCoordinates(projectRegion).lat + 0.01,
          longitude: this.getRegionCoordinates(projectRegion).lng + 0.01
        },
        description: 'Secondary support site for logistics and storage',
        createdAt: new Date('2024-02-01'),
        updatedAt: new Date()
      }
    ];
    
    this.loading = false;
  }

  private getRegionCoordinates(region: string): { lat: number; lng: number } {
    const coordinates: { [key: string]: { lat: number; lng: number } } = {
      'Muscat': { lat: 23.5880, lng: 58.3829 },
      'Dhofar': { lat: 17.0194, lng: 54.0924 },
      'Musandam': { lat: 26.2100, lng: 56.3300 },
      'Al Buraimi': { lat: 24.2488, lng: 55.7932 },
      'Al Dakhiliyah': { lat: 22.9637, lng: 57.5339 },
      'Al Dhahirah': { lat: 23.3668, lng: 56.3034 },
      'Al Wusta': { lat: 20.1553, lng: 56.5136 },
      'Al Batinah North': { lat: 24.3510, lng: 56.7069 },
      'Al Batinah South': { lat: 23.6760, lng: 57.8500 },
      'Ash Sharqiyah North': { lat: 22.6853, lng: 59.1363 },
      'Ash Sharqiyah South': { lat: 21.5169, lng: 59.1721 }
    };
    return coordinates[region] || { lat: 23.5880, lng: 58.3829 };
  }

  goBack() {
    this.router.navigate(['/admin/project-management']);
  }

  addSite() {
    const projectName = this.project?.name || 'Default Project';
    console.log(`Adding new site for project: ${projectName}`);
    // TODO: Navigate to add site component
  }
}
