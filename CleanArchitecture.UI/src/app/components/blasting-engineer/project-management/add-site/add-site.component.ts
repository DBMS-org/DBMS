import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Project, ProjectSite } from '../../../../core/models/project.model';
import { ProjectService } from '../../../../core/services/project.service';

interface SiteFormData {
  name: string;
  location: string;
  description: string;
  coordinates: {
    latitude: number | null;
    longitude: number | null;
  };
}

@Component({
  selector: 'app-add-site',
  imports: [CommonModule, FormsModule],
  templateUrl: './add-site.component.html',
  styleUrl: './add-site.component.scss'
})
export class AddSiteComponent implements OnInit {
  project: Project | null = null;
  projectId: string = '';
  isSubmitting = false;
  error: string | null = null;
  successMessage: string | null = null;

  siteData: SiteFormData = {
    name: '',
    location: '',
    description: '',
    coordinates: {
      latitude: null,
      longitude: null
    }
  };

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private projectService: ProjectService
  ) {}

  ngOnInit() {
    this.projectId = this.route.snapshot.paramMap.get('id') || '';
    if (this.projectId) {
      this.loadProject();
    }
  }

  loadProject() {
    this.projectService.getProject(this.projectId).subscribe({
      next: (project) => {
        this.project = project;
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
        id: '001',
        name: 'Project Alpha',
        region: 'Muscat',
        project: 'Muttrah Construction',
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
        id: '002',
        name: 'Project Beta',
        region: 'Dhofar',
        project: 'Salalah Infrastructure',
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
        id: '003',
        name: 'Project Gamma',
        region: 'Al Batinah North',
        project: 'Sohar Industrial Zone',
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

    // Create the site object
    const newSite: Omit<ProjectSite, 'id' | 'createdAt' | 'updatedAt'> = {
      projectId: this.projectId,
      name: this.siteData.name,
      location: this.siteData.location,
      description: this.siteData.description || undefined,
      coordinates: {
        latitude: this.siteData.coordinates.latitude!,
        longitude: this.siteData.coordinates.longitude!
      }
    };

    // Simulate API call
    setTimeout(() => {
      try {
        // In a real app, this would be a service call:
        // this.projectService.addProjectSite(newSite).subscribe({...})
        
        this.isSubmitting = false;
        this.successMessage = 'Site added successfully!';
        
        // Reset form
        form.resetForm();
        this.resetFormData();

        // Navigate back to sites list after a delay
        setTimeout(() => {
          this.goBack();
        }, 2000);

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
      location: '',
      description: '',
      coordinates: {
        latitude: null,
        longitude: null
      }
    };
  }

  goBack() {
    this.router.navigate(['/blasting-engineer/project-management', this.projectId, 'sites']);
  }
}
