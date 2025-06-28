import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Project } from '../../../../core/models/project.model';
import { ProjectService } from '../../../../core/services/project.service';
import { SiteService, CreateSiteRequest } from '../../../../core/services/site.service';

interface SiteFormData {
  name: string;
  location: string;
  description: string;
  templateType: string;
  numberOfHoles: number | null;
}

@Component({
  selector: 'app-add-site',
  imports: [CommonModule, FormsModule],
  templateUrl: './add-site.component.html',
  styleUrl: './add-site.component.scss'
})
export class AddSiteComponent implements OnInit {
  project: Project | null = null;
  projectId: number = 0;
  isSubmitting = false;
  error: string | null = null;
  successMessage: string | null = null;
  loading = false;

  siteData: SiteFormData = {
    name: '',
    location: '',
    description: '',
    templateType: '',
    numberOfHoles: null,
  };

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private projectService: ProjectService,
    private siteService: SiteService
  ) {}

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.projectId = +params['id'];
      if (this.projectId) {
        this.loadProject();
      }
    });
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

  selectTemplate(templateType: string) {
    this.siteData.templateType = templateType;
  }

  onSubmit(form: NgForm) {
    if (form.invalid) {
      // Mark all fields as touched to show validation errors
      Object.keys(form.controls).forEach(key => {
        form.controls[key].markAsTouched();
      });
      return;
    }

    this.isSubmitting = true;
    this.error = null;
    this.successMessage = null;

    // Create the site request object
    const newSiteRequest: CreateSiteRequest = {
      projectId: this.projectId,
      name: this.siteData.name,
      location: this.siteData.location,
      status: 'Active', // Default status
      description: this.siteData.description || ''
    };

    // Create the site via the API
    this.siteService.createSite(newSiteRequest).subscribe({
      next: (createdSite) => {
        this.isSubmitting = false;
        this.successMessage = `Site "${createdSite.name}" has been created successfully! Redirecting to drilling pattern creator...`;
        
        // Reset form
        this.resetFormData();
        form.resetForm();
        
        // Navigate to site-specific drilling pattern creator after a brief delay
        setTimeout(() => {
          this.router.navigate(['/blasting-engineer/project-management', this.projectId, 'sites', createdSite.id, 'pattern-creator']);
        }, 2000);
      },
      error: (error) => {
        this.isSubmitting = false;
        this.error = 'Failed to create site. Please try again.';
        console.error('Error creating site:', error);
      }
    });
  }

  private resetFormData() {
    this.siteData = {
      name: '',
      location: '',
      description: '',
      templateType: '',
      numberOfHoles: null,
    };
  }

  goBack() {
    this.router.navigate(['/blasting-engineer/project-management', this.projectId, 'sites']);
  }
}
