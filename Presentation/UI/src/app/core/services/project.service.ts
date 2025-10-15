import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { Project, CreateProjectRequest, UpdateProjectRequest, ProjectSite } from '../models/project.model';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class ProjectService {
  private readonly apiUrl = `${environment.apiUrl}/api/projects`;

  constructor(
    private http: HttpClient,
    private authService: AuthService
  ) {}

  // Get all projects (admin only)
  getProjects(): Observable<Project[]> {
    return this.http.get<Project[]>(this.apiUrl).pipe(
      map(projects => projects.map(project => ({
        ...project,
        startDate: project.startDate ? new Date(project.startDate) : undefined,
        endDate: project.endDate ? new Date(project.endDate) : undefined,
        createdAt: new Date(project.createdAt),
        updatedAt: new Date(project.updatedAt)
      }))),
      catchError(this.handleError)
    );
  }

  // Get projects filtered by current user role and region
  getProjectsForCurrentUser(): Observable<Project[]> {
    return this.getProjects().pipe(
      map(projects => {
        const currentUser = this.authService.getCurrentUser();
        
        // Admin and Machine Manager can see all projects
        if (this.authService.isAdmin() || this.authService.isMachineManager()) {
          return projects;
        }
        
        // Blasting engineers can only see projects in their region
        if (this.authService.isBlastingEngineer() && currentUser?.region) {
          return projects.filter(project => 
            project.region.toLowerCase() === currentUser.region.toLowerCase()
          );
        }
        
        // Default: return empty array if user doesn't have proper role/region
        return [];
      })
    );
  }

  // Get all projects for administrative purposes (machine inventory, etc.)
  getAllProjects(): Observable<Project[]> {
    return this.getProjects();
  }

  // Get project by ID
  getProject(id: number): Observable<Project> {
    return this.http.get<Project>(`${this.apiUrl}/${id}`).pipe(
      map(project => ({
        ...project,
        startDate: project.startDate ? new Date(project.startDate) : undefined,
        endDate: project.endDate ? new Date(project.endDate) : undefined,
        createdAt: new Date(project.createdAt),
        updatedAt: new Date(project.updatedAt)
      })),
      catchError(this.handleError)
    );
  }

  // Create new project
  createProject(projectRequest: CreateProjectRequest): Observable<Project> {
    console.log('Sending project creation request:', projectRequest);
    return this.http.post<Project>(this.apiUrl, projectRequest).pipe(
      map(project => ({
        ...project,
        startDate: project.startDate ? new Date(project.startDate) : undefined,
        endDate: project.endDate ? new Date(project.endDate) : undefined,
        createdAt: new Date(project.createdAt),
        updatedAt: new Date(project.updatedAt)
      })),
      catchError(this.handleError)
    );
  }

  // Update existing project
  updateProject(id: number, projectRequest: UpdateProjectRequest): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, projectRequest).pipe(
      catchError(this.handleError)
    );
  }

  // Delete project
  deleteProject(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`).pipe(
      catchError(this.handleError)
    );
  }

  // Get project sites
  getProjectSites(projectId: number): Observable<ProjectSite[]> {
    return this.http.get<ProjectSite[]>(`${this.apiUrl}/${projectId}/sites`).pipe(
      map(sites => sites.map(site => ({
        ...site,
        createdAt: new Date(site.createdAt),
        updatedAt: new Date(site.updatedAt)
      }))),
      catchError(this.handleError)
    );
  }

  // Complete a project site
  completeSite(siteId: number): Observable<any> {
    return this.http.post<any>(`${environment.apiUrl}/api/projectsites/${siteId}/complete`, {}).pipe(
      catchError(this.handleError)
    );
  }

  // Check if operator is already assigned to a project
  getProjectByOperator(operatorId: number): Observable<Project | null> {
    return this.http.get<Project | null>(`${this.apiUrl}/by-operator/${operatorId}`).pipe(
      map(project => {
        if (project) {
          return {
            ...project,
            startDate: project.startDate ? new Date(project.startDate) : undefined,
            endDate: project.endDate ? new Date(project.endDate) : undefined,
            createdAt: new Date(project.createdAt),
            updatedAt: new Date(project.updatedAt)
          } as Project;
        }
        return null;
      }),
      catchError((error: HttpErrorResponse) => {
        // For 404 errors, we just return null as it means no project is assigned
        if (error.status === 404) {
          console.log('No project assigned to this operator');
          return of(null);
        }
        // For other errors, use the general error handler
        return this.handleError(error);
      })
    );
  }

  // Error handling
  private handleError(error: HttpErrorResponse): Observable<never> {
    let errorMessage = 'An unknown error occurred!';
    
    if (error.error instanceof ErrorEvent) {
      // Client-side error
      errorMessage = `Client Error: ${error.error.message}`;
    } else {
      // Server-side error
      console.error('Full error response:', error);
      
      if (error.status === 404) {
        errorMessage = 'Project not found';
      } else if (error.status === 400) {
        // Try to get specific validation errors from the response
        if (typeof error.error === 'string') {
          errorMessage = `Validation Error: ${error.error}`;
        } else if (error.error && error.error.errors) {
          // Handle model validation errors
          const validationErrors = Object.values(error.error.errors).flat();
          errorMessage = `Validation Errors: ${validationErrors.join(', ')}`;
        } else if (error.error && error.error.message) {
          errorMessage = `Validation Error: ${error.error.message}`;
        } else {
          errorMessage = 'Invalid request data - please check all required fields';
        }
      } else if (error.status === 500) {
        errorMessage = 'Server error occurred';
      } else {
        errorMessage = `Server Error Code: ${error.status}\nMessage: ${error.message}`;
      }
    }
    
    console.error('ProjectService Error:', errorMessage);
    return throwError(() => error);
  }
} 