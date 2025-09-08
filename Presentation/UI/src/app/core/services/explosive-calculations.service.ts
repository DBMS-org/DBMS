import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { AuthService } from './auth.service';

// Interfaces for explosive calculation data
export interface ExplosiveCalculationInputs {
  numberOfHoles: number;
  numberOfColumns: number;
  holeDiameter: number;
  stemming: number;
  spacing: number;
  burden: number;
  emulsionDensity: number;
  anfoeDensity: number;
  emulsionPerHole: number;
  depths: number[];
}

export interface ExplosiveCalculationResults {
  totalDepth: number;
  averageDepth: number;
  numberOfFilledHoles: number;
  emulsionPerMeter: number;
  anfoPerMeter: number;
  emulsionCoveringSpace: number;
  remainingSpace: number;
  anfoCoveringSpace: number;
  totalAnfo: number;
  totalEmulsion: number;
  totalVolume: number;
}

export interface ExplosiveCalculationResultDto {
  id: number;
  calculationId: string;
  patternSettingsId?: number;
  
  // Explosive material properties
  emulsionDensity: number;
  anfoDensity: number;
  emulsionPerHole: number;
  
  // Calculated results
  totalDepth: number;
  averageDepth: number;
  numberOfFilledHoles: number;
  emulsionPerMeter: number;
  anfoPerMeter: number;
  emulsionCoveringSpace: number;
  remainingSpace: number;
  anfoCoveringSpace: number;
  totalAnfo: number;
  totalEmulsion: number;
  totalVolume: number;
  
  // Context
  projectId: number;
  siteId: number;
  owningUserId: number;
  
  // Audit fields
  createdAt: string;
  updatedAt: string;
  createdBy?: string;
  updatedBy?: string;
}

export interface CreateExplosiveCalculationResultRequest {
  calculationId: string;
  projectId: number;
  siteId: number;
  patternSettingsId?: number;
  // Explosive material properties
  emulsionDensity: number;
  anfoDensity: number;
  emulsionPerHole: number;
  // Calculated results
  totalDepth: number;
  averageDepth: number;
  numberOfFilledHoles: number;
  emulsionPerMeter: number;
  anfoPerMeter: number;
  emulsionCoveringSpace: number;
  remainingSpace: number;
  anfoCoveringSpace: number;
  totalAnfo: number;
  totalEmulsion: number;
  totalVolume: number;
  owningUserId: number;
}

export interface UpdateExplosiveCalculationResultRequest {
  calculationId?: string;
  patternSettingsId?: number;
  // Explosive material properties
  emulsionDensity?: number;
  anfoDensity?: number;
  emulsionPerHole?: number;
  // Calculated results
  totalDepth?: number;
  averageDepth?: number;
  numberOfFilledHoles?: number;
  emulsionPerMeter?: number;
  anfoPerMeter?: number;
  emulsionCoveringSpace?: number;
  remainingSpace?: number;
  anfoCoveringSpace?: number;
  totalAnfo?: number;
  totalEmulsion?: number;
  totalVolume?: number;
}

@Injectable({
  providedIn: 'root'
})
export class ExplosiveCalculationsService {
  private readonly apiUrl = `${environment.apiUrl}/api/explosive-calculation-results`;

  constructor(private http: HttpClient, private authService: AuthService) {}

  /**
   * Get all explosive calculation results for a specific project and site
   */
  getByProjectAndSite(projectId: number, siteId: number): Observable<ExplosiveCalculationResultDto[]> {
    return this.http.get<ExplosiveCalculationResultDto[]>(`${this.apiUrl}/project/${projectId}/site/${siteId}`)
      .pipe(
        catchError(this.handleError)
      );
  }

  /**
   * Get explosive calculation result by ID
   */
  getById(id: number): Observable<ExplosiveCalculationResultDto> {
    return this.http.get<ExplosiveCalculationResultDto>(`${this.apiUrl}/${id}`)
      .pipe(
        catchError(this.handleError)
      );
  }

  /**
   * Get explosive calculation result by calculation ID
   */
  getByCalculationId(calculationId: string): Observable<ExplosiveCalculationResultDto> {
    return this.http.get<ExplosiveCalculationResultDto>(`${this.apiUrl}/calculation/${calculationId}`)
      .pipe(
        catchError(this.handleError)
      );
  }

  /**
   * Check if existing calculations exist for a site
   */
  checkExistingCalculations(projectId: number, siteId: number): Observable<{ hasExisting: boolean; count: number; calculations: ExplosiveCalculationResultDto[] | null }> {
    return this.http.get<{ hasExisting: boolean; count: number; calculations: ExplosiveCalculationResultDto[] | null }>(`${this.apiUrl}/project/${projectId}/site/${siteId}/check-existing`)
      .pipe(
        catchError(this.handleError)
      );
  }

  /**
   * Create a new explosive calculation result
   */
  create(request: CreateExplosiveCalculationResultRequest): Observable<ExplosiveCalculationResultDto> {
    return this.http.post<ExplosiveCalculationResultDto>(this.apiUrl, request)
      .pipe(
        catchError(this.handleError)
      );
  }

  /**
   * Create a new explosive calculation result with confirmation (overwrites existing)
   */
  createWithConfirmation(request: CreateExplosiveCalculationResultRequest): Observable<ExplosiveCalculationResultDto> {
    return this.http.post<ExplosiveCalculationResultDto>(`${this.apiUrl}/create-with-confirmation`, request)
      .pipe(
        catchError(this.handleError)
      );
  }

  /**
   * Update an existing explosive calculation result
   */
  update(id: number, request: UpdateExplosiveCalculationResultRequest): Observable<ExplosiveCalculationResultDto> {
    return this.http.put<ExplosiveCalculationResultDto>(`${this.apiUrl}/${id}`, request)
      .pipe(
        catchError(this.handleError)
      );
  }

  /**
   * Delete an explosive calculation result
   */
  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`)
      .pipe(
        catchError(this.handleError)
      );
  }

  /**
   * Delete all explosive calculation results for a project
   */
  deleteByProject(projectId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/project/${projectId}`)
      .pipe(
        catchError(this.handleError)
      );
  }

  /**
   * Delete all explosive calculation results for a site
   */
  deleteBySite(projectId: number, siteId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/project/${projectId}/site/${siteId}`)
      .pipe(
        catchError(this.handleError)
      );
  }

  /**
   * Get count of explosive calculation results for a project and site
   */
  getCount(projectId: number, siteId: number): Observable<{ count: number }> {
    return this.http.get<{ count: number }>(`${this.apiUrl}/project/${projectId}/site/${siteId}/count`)
      .pipe(
        catchError(this.handleError)
      );
  }

  /**
   * Save calculation results with auto-generated calculation ID
   */
  saveCalculationResults(inputs: ExplosiveCalculationInputs, results: ExplosiveCalculationResults, projectId: number, siteId: number, owningUserId?: number): Observable<ExplosiveCalculationResultDto> {
    const currentUser = this.authService.getCurrentUser();
    if (!currentUser && !owningUserId) {
      throw new Error('User not authenticated');
    }

    const request: CreateExplosiveCalculationResultRequest = {
      calculationId: this.generateCalculationId(),
      projectId,
      siteId,
      // Map inputs
      emulsionDensity: inputs.emulsionDensity,
      anfoDensity: inputs.anfoeDensity,
      emulsionPerHole: inputs.emulsionPerHole,
      // Map results
      totalDepth: results.totalDepth,
      averageDepth: results.averageDepth,
      numberOfFilledHoles: results.numberOfFilledHoles,
      emulsionPerMeter: results.emulsionPerMeter,
      anfoPerMeter: results.anfoPerMeter,
      emulsionCoveringSpace: results.emulsionCoveringSpace,
      remainingSpace: results.remainingSpace,
      anfoCoveringSpace: results.anfoCoveringSpace,
      totalAnfo: results.totalAnfo,
      totalEmulsion: results.totalEmulsion,
      totalVolume: results.totalVolume,
      owningUserId: owningUserId || currentUser!.id
    };
    
    return this.create(request);
  }

  /**
   * Save calculation results with confirmation (overwrites existing)
   */
  saveCalculationResultsWithConfirmation(inputs: ExplosiveCalculationInputs, results: ExplosiveCalculationResults, projectId: number, siteId: number, owningUserId?: number): Observable<ExplosiveCalculationResultDto> {
    const currentUser = this.authService.getCurrentUser();
    if (!currentUser && !owningUserId) {
      throw new Error('User not authenticated');
    }

    const request: CreateExplosiveCalculationResultRequest = {
      calculationId: this.generateCalculationId(),
      projectId,
      siteId,
      // Map inputs
      emulsionDensity: inputs.emulsionDensity,
      anfoDensity: inputs.anfoeDensity,
      emulsionPerHole: inputs.emulsionPerHole,
      // Map results
      totalDepth: results.totalDepth,
      averageDepth: results.averageDepth,
      numberOfFilledHoles: results.numberOfFilledHoles,
      emulsionPerMeter: results.emulsionPerMeter,
      anfoPerMeter: results.anfoPerMeter,
      emulsionCoveringSpace: results.emulsionCoveringSpace,
      remainingSpace: results.remainingSpace,
      anfoCoveringSpace: results.anfoCoveringSpace,
      totalAnfo: results.totalAnfo,
      totalEmulsion: results.totalEmulsion,
      totalVolume: results.totalVolume,
      owningUserId: owningUserId || currentUser!.id
    };
    
    return this.createWithConfirmation(request);
  }

  /**
   * Get the latest calculation result for a project and site
   */
  getLatestByProjectAndSite(projectId: number, siteId: number): Observable<ExplosiveCalculationResultDto | null> {
    return this.getByProjectAndSite(projectId, siteId)
      .pipe(
        map(results => {
          if (results && results.length > 0) {
            // Sort by createdAt descending and return the latest
            return results.sort((a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime())[0];
          }
          return null;
        }),
        catchError(error => {
          console.warn('No calculation results found:', error);
          return [null];
        })
      );
  }

  /**
   * Check if calculation results exist for a project and site
   */
  hasCalculationResults(projectId: number, siteId: number): Observable<boolean> {
    return this.getCount(projectId, siteId)
      .pipe(
        map(response => response.count > 0),
        catchError(() => [false])
      );
  }

  /**
   * Generate a unique calculation ID
   */
  private generateCalculationId(): string {
    const timestamp = new Date().getTime();
    const random = Math.floor(Math.random() * 1000);
    return `CALC_${timestamp}_${random}`;
  }

  /**
   * Handle HTTP errors
   */
  private handleError(error: any): Observable<never> {
    console.error('ExplosiveCalculationsService error:', error);
    
    let errorMessage = 'An unknown error occurred';
    
    if (error.error instanceof ErrorEvent) {
      // Client-side error
      errorMessage = `Client Error: ${error.error.message}`;
    } else {
      // Server-side error
      errorMessage = `Server Error: ${error.status} - ${error.message}`;
      if (error.error && typeof error.error === 'string') {
        errorMessage = error.error;
      } else if (error.error && error.error.message) {
        errorMessage = error.error.message;
      }
    }
    
    return throwError(() => new Error(errorMessage));
  }
}