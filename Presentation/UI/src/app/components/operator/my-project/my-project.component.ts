import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router, ActivatedRoute } from '@angular/router';
import { ProjectService } from '../../../core/services/project.service';
import { AuthService } from '../../../core/services/auth.service';
import { Project, ProjectSite } from '../../../core/models/project.model';
import { SiteService } from '../../../core/services/site.service';

@Component({
  selector: 'app-operator-my-project',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './my-project.component.html',
  styleUrl: './my-project.component.scss'
})
export class MyProjectComponent implements OnInit {
  isLoading = true;
  error: string | null = null;
  project: Project | null = null;

  sites: ProjectSite[] = [];
  sitesLoading = false;
  sitesError: string | null = null;
  showSites = false;

  constructor(
    private projectService: ProjectService, 
    private authService: AuthService,
    private router: Router,
    private route: ActivatedRoute,
    private siteService: SiteService
  ) {}

  ngOnInit(): void {
    const currentUser = this.authService.getCurrentUser();
    if (!currentUser) {
      this.error = 'User not found';
      this.isLoading = false;
      return;
    }

    this.route.queryParams.subscribe(params => {
      if (params['showSites'] === 'true' || params['showSites'] === true) {
        this.showSites = true;
        setTimeout(() => this.scrollToSitesSection(), 0);
      }
    });

    this.projectService.getProjectByOperator(currentUser.id).subscribe({
      next: (proj: Project | null) => {
        this.project = proj;
        this.isLoading = false;
        if (proj) {
          this.loadSites(proj.id);
        }
      },
      error: (err: any) => {
        this.error = err.message;
        this.isLoading = false;
      }
    });
  }

  private loadSites(projectId: number): void {
    this.sitesLoading = true;
    this.sitesError = null;
    this.siteService.getProjectSites(projectId).subscribe({
      next: (sites) => {
        this.sites = sites;
        this.sitesLoading = false;
        if (this.showSites) {
          setTimeout(() => this.scrollToSitesSection(), 0);
        }
      },
      error: (err) => {
        this.sitesError = 'Failed to load project sites';
        this.sitesLoading = false;
      }
    });
  }

  formatDate(date?: Date): string {
    return date ? new Date(date).toLocaleDateString() : '-';
  }

  calculateDuration(): string {
    if (!this.project?.startDate || !this.project?.endDate) {
      return '-';
    }
    const start = new Date(this.project.startDate);
    const end = new Date(this.project.endDate);
    const diffTime = Math.abs(end.getTime() - start.getTime());
    const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));
    
    if (diffDays < 30) {
      return `${diffDays} days`;
    } else if (diffDays < 365) {
      const months = Math.floor(diffDays / 30);
      const remainingDays = diffDays % 30;
      return remainingDays > 0 ? `${months} months, ${remainingDays} days` : `${months} months`;
    } else {
      const years = Math.floor(diffDays / 365);
      const remainingDays = diffDays % 365;
      const months = Math.floor(remainingDays / 30);
      return `${years} year${years > 1 ? 's' : ''}${months > 0 ? `, ${months} months` : ''}`;
    }
  }

  calculateOverallProgress(): number {
    if (!this.sites || this.sites.length === 0) {
      return 0;
    }
    const completedSites = this.sites.filter(site => site.isOperatorCompleted).length;
    return Math.round((completedSites / this.sites.length) * 100);
  }

  getCompletedSitesCount(): number {
    return this.sites ? this.sites.filter(site => site.isOperatorCompleted).length : 0;
  }

  navigateToSites(): void {
    this.showSites = true;
    setTimeout(() => this.scrollToSitesSection(), 0);
  }

  toggleSites(): void {
    this.showSites = !this.showSites;
    if (this.showSites) {
      setTimeout(() => this.scrollToSitesSection(), 0);
    }
  }

  private scrollToSitesSection(): void {
    const el = document.getElementById('sites-section');
    if (el) el.scrollIntoView({ behavior: 'smooth', block: 'start' });
  }

  viewPattern(site: ProjectSite): void {
    this.router.navigate(['/operator/my-project/sites', site.id, 'pattern-view']);
  }

  viewSiteDetails(site: ProjectSite): void {
    this.router.navigate(['/operator/my-project/sites', site.id, 'details']);
  }
}