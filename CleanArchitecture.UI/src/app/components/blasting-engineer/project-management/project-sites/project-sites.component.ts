import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Project } from '../../../../core/models/project.model';
import { ProjectService } from '../../../../core/services/project.service';
import { SiteService, ProjectSite } from '../../../../core/services/site.service';

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
    private projectService: ProjectService,
    private siteService: SiteService
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
        this.error = 'Failed to load project information.';
        this.loading = false;
      }
    });
  }

  loadProjectSites() {
    if (!this.projectId) return;
    
    this.loading = true;
    this.error = null;

    this.siteService.getProjectSites(this.projectId).subscribe({
      next: (sites) => {
        this.sites = sites.map(site => ({
          ...site,
          createdAt: new Date(site.createdAt),
          updatedAt: new Date(site.updatedAt)
        }));
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading project sites:', error);
        this.error = 'Failed to load project sites.';
        this.loading = false;
      }
    });
  }

  goBack() {
    this.router.navigate(['/blasting-engineer/project-management']);
  }

  addNewSite() {
    this.router.navigate(['/blasting-engineer/project-management', this.projectId, 'sites', 'new']);
  }

  addSite() {
    this.addNewSite();
  }

  uploadSurvey(site: ProjectSite) {
    // Navigate to CSV upload page for survey data
    this.router.navigate(['/blasting-engineer/csv-upload'], {
      queryParams: { 
        siteId: site.id, 
        siteName: site.name,
        projectId: this.projectId 
      }
    });
  }

  createPattern(site: ProjectSite) {
    // Navigate to site dashboard
    this.router.navigate(['/blasting-engineer/project-management', this.projectId, 'sites', site.id, 'dashboard']);
  }

  viewDrillVisualization(site: ProjectSite) {
    // Navigate to drill visualization with proper route parameters
    this.router.navigate(['/blasting-engineer/project-management', this.projectId, 'sites', site.id, 'drill-visualization']);
  }

  deleteSite(site: ProjectSite) {
    if (confirm(`Are you sure you want to delete the site "${site.name}"?`)) {
      this.siteService.deleteSite(site.id).subscribe({
        next: () => {
          // Remove the deleted site from the local array
          this.sites = this.sites.filter(s => s.id !== site.id);
        },
        error: (error) => {
          console.error('Error deleting site:', error);
          this.error = 'Failed to delete site. Please try again.';
        }
      });
    }
  }

  formatDate(date: Date): string {
    return new Intl.DateTimeFormat('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric'
    }).format(date);
  }
}
