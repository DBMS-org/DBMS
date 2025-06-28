import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { Project } from '../../../core/models/project.model';
import { ProjectSite, SiteService } from '../../../core/services/site.service';
import { ProjectService } from '../../../core/services/project.service';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-operator-project-sites',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './project-sites.component.html',
  styleUrl: './project-sites.component.scss'
})
export class OperatorProjectSitesComponent implements OnInit {
  project: Project | null = null;
  projectId: number = 0;
  sites: ProjectSite[] = [];
  loading = false;
  error: string | null = null;

  constructor(
    private router: Router,
    private projectService: ProjectService,
    private siteService: SiteService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    const user = this.authService.getCurrentUser();
    if (!user) {
      this.error = 'User not found';
      return;
    }

    this.projectService.getProjectByOperator(user.id).subscribe({
      next: project => {
        if (!project) {
          this.error = 'No project assigned';
          return;
        }
        this.project = project;
        this.projectId = project.id;
        this.loadProjectSites();
      },
      error: err => {
        this.error = err.message;
      }
    });
  }

  loadProjectSites(): void {
    if (!this.projectId) return;
    this.loading = true;
    this.siteService.getProjectSites(this.projectId).subscribe({
      next: sites => {
        this.sites = sites.map(s => ({
          ...s,
          createdAt: new Date(s.createdAt),
          updatedAt: new Date(s.updatedAt)
        }));
        this.loading = false;
      },
      error: err => {
        console.error('Error loading sites:', err);
        this.error = 'Failed to load project sites';
        this.loading = false;
      }
    });
  }

  viewPattern(site: ProjectSite): void {
    this.router.navigate(['/operator/my-project/sites', site.id, 'pattern-view']);
  }

  formatDate(date: Date): string {
    return new Intl.DateTimeFormat('en-US', { year: 'numeric', month: 'short', day: 'numeric' }).format(date);
  }

  goBack(): void {
    this.router.navigate(['/operator/dashboard']);
  }
} 