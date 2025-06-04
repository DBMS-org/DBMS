import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { Project, CreateProjectRequest, UpdateProjectRequest, ProjectSite } from '../models/project.model';

@Injectable({
  providedIn: 'root'
})
export class ProjectService {
  private readonly apiUrl = `${environment.apiUrl}/api/projects`;

  constructor(private http: HttpClient) {}

  // Get all projects
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
    return throwError(() => new Error(errorMessage));
  }
} 