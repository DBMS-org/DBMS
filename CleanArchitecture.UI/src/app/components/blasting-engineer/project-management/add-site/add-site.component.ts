import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Project, ProjectSite } from '../../../../core/models/project.model';
import { ProjectService } from '../../../../core/services/project.service';

interface SiteFormData {
  name: string;
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
    description: '',
    templateType: '',
    numberOfHoles: null,
  };

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private projectService: ProjectService
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
        // Fallback to mock data
        this.loadMockProject();
      }
    });
  }

  private loadMockProject() {
    // Mock project data as fallback
    const mockProjects = [
      {
        id: 1,
        name: 'Project Alpha',
        region: 'Muscat',
        projectType: 'Muttrah Construction',
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
        id: 2,
        name: 'Project Beta',
        region: 'Dhofar',
        projectType: 'Salalah Infrastructure',
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
        id: 3,
        name: 'Project Gamma',
        region: 'Al Batinah North',
        projectType: 'Sohar Industrial Zone',
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

    this.project = mockProjects.find(p => p.id === this.projectId) || mockProjects[0];
    this.loading = false;
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

    // Create the site object with template information
    const newSite = {
      projectId: this.projectId,
      name: this.siteData.name,
      description: this.siteData.description || undefined,
      templateType: this.siteData.templateType,
      drillingConfig: {
        numberOfHoles: this.siteData.numberOfHoles
      }
    };

    // Simulate API call
    setTimeout(() => {
      try {
        // In a real app, this would be a service call:
        // this.projectService.addProjectSite(newSite).subscribe({...})
        
        this.isSubmitting = false;
        
        // Navigate to drilling pattern creator instead of showing success message
        this.router.navigate(['/blasting-engineer/drilling-pattern']);

      } catch (error) {
        this.isSubmitting = false;
        this.error = 'Failed to add site. Please try again.';
        console.error('Error adding site:', error);
      }
    }, 1500);
  }

  private resetFormData() {
    this.siteData = {
      name: '',
      description: '',
      templateType: '',
      numberOfHoles: null,
    };
  }

  goBack() {
    this.router.navigate(['/blasting-engineer/project-management', this.projectId, 'sites']);
  }
}
