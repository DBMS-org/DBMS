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
  showDeleteModal = false;
  siteToDelete: ProjectSite | null = null;

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

  openDeleteModal(site: ProjectSite) {
    this.siteToDelete = site;
    this.showDeleteModal = true;
  }

  closeDeleteModal() {
    this.showDeleteModal = false;
    this.siteToDelete = null;
  }

  confirmDelete() {
    if (this.siteToDelete) {
      this.siteService.deleteSite(this.siteToDelete.id).subscribe({
        next: () => {
          this.sites = this.sites.filter(s => s.id !== this.siteToDelete!.id);
          this.closeDeleteModal();
        },
        error: (error) => {
          console.error('Error deleting site:', error);
          this.error = 'Failed to delete site. Please try again.';
          this.closeDeleteModal();
        }
      });
    }
  }

  deleteSite(site: ProjectSite) {
    this.openDeleteModal(site);
  }

  formatDate(date: Date): string {
    return new Intl.DateTimeFormat('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric'
    }).format(date);
  }

  // Completion statistics methods
  getCompletedSitesCount(): number {
    return this.sites.filter(site => site.isOperatorCompleted).length;
  }

  getPendingSitesCount(): number {
    return this.sites.filter(site => !site.isOperatorCompleted).length;
  }

  getCompletionPercentage(): number {
    if (this.sites.length === 0) return 0;
    return Math.round((this.getCompletedSitesCount() / this.sites.length) * 100);
  }

  // Site completion functionality
  canCompleteSite(site: ProjectSite): boolean {
    // A site can be completed if:
    // 1. Pattern is approved (isPatternApproved = true)
    // 2. Simulation is confirmed (isSimulationConfirmed = true)
    // 3. Explosive approval has been requested (isExplosiveApprovalRequested = true)
    // 4. Site is not already completed by operator
    return site.isPatternApproved && 
           site.isSimulationConfirmed && 
           site.isExplosiveApprovalRequested && 
           !site.isOperatorCompleted;
  }

  getCompleteButtonTooltip(site: ProjectSite): string {
    if (site.isOperatorCompleted) {
      return 'Site is already completed by operator';
    }

    const missingRequirements: string[] = [];
    
    if (!site.isPatternApproved) {
      missingRequirements.push('Pattern approval');
    }
    
    if (!site.isSimulationConfirmed) {
      missingRequirements.push('Simulation confirmation');
    }
    
    if (!site.isExplosiveApprovalRequested) {
      missingRequirements.push('Explosive approval request');
    }

    if (missingRequirements.length > 0) {
      return `Missing requirements: ${missingRequirements.join(', ')}`;
    }

    return 'Mark site as complete';
  }

  completeSite(site: ProjectSite) {
    if (!this.canCompleteSite(site)) {
      return;
    }

    // Show confirmation dialog before completing
    const confirmMessage = `Are you sure you want to mark "${site.name}" as complete?\n\nThis action will:\n- Mark the site as completed by the operator\n- Update the completion status\n- This action cannot be undone`;
    
    if (confirm(confirmMessage)) {
      this.loading = true;
      this.error = null;

      this.siteService.completeSite(site.id).subscribe({
        next: () => {
          // Update the local site data
          const siteIndex = this.sites.findIndex(s => s.id === site.id);
          if (siteIndex !== -1) {
            this.sites[siteIndex] = {
              ...this.sites[siteIndex],
              isOperatorCompleted: true,
              updatedAt: new Date()
            };
          }
          this.loading = false;
          // Show success message (you can replace this with a toast notification)
          alert('Site marked as complete successfully!');
        },
        error: (error) => {
          console.error('Error completing site:', error);
          this.error = 'Failed to complete site. Please try again.';
          this.loading = false;
          // Show error message (you can replace this with a toast notification)
          alert('Failed to complete site. Please try again.');
        }
      });
    }
  }
}
