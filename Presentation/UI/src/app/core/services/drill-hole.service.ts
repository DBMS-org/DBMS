import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError, forkJoin, of } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';
import { environment } from '../../../environments/environment';

export interface DrillHole {
  serialNumber?: number;
  id?: string; // Optional when creating; server will generate
  name?: string;
  easting: number;
  northing: number;
  elevation: number;
  length: number;
  depth: number;
  azimuth?: number | null; // Made optional for 2D fallback
  dip?: number | null;     // Made optional for 2D fallback
  actualDepth: number;
  stemming: number;
  projectId?: number;
  siteId?: number;
  createdAt?: string;
  updatedAt?: string;
  
  // Helper properties for 2D/3D detection
  has3DData?: boolean;
  requiresFallbackTo2D?: boolean;
}

// DTO interface for backend communication
export interface CreateDrillHoleRequest {
  name: string;
  easting: number;
  northing: number;
  elevation: number;
  length: number;
  depth: number;
  azimuth?: number | null;
  dip?: number | null;
  actualDepth: number;
  stemming: number;
  projectId: number;
  siteId: number;
}

@Injectable({
  providedIn: 'root'
})
export class DrillHoleService {
  private readonly apiUrl = `${environment.apiUrl}/api/DrillPlan`;

  constructor(private http: HttpClient) {}

  // Get all drill holes
  getAllDrillHoles(): Observable<DrillHole[]> {
    return this.http.get<DrillHole[]>(this.apiUrl).pipe(
   catchError(this.handleError)
    );}

  // Get drill hole by ID
  getDrillHole(id: string): Observable<DrillHole> {
    return this.http.get<DrillHole>(`${this.apiUrl}/${id}`).pipe(
      catchError(this.handleError)
    );
  }

  // Create a new drill hole
  createDrillHole(drillHole: DrillHole): Observable<DrillHole> {
    console.log('🔧 createDrillHole called with:', { 
      name: drillHole.name, 
      projectId: drillHole.projectId, 
      siteId: drillHole.siteId 
    });
    
    // Validate and clean single drill hole
    const cleanedHole = this.validateAndCleanDrillHole(drillHole);
    
    console.log('🧹 After cleaning:', { 
      name: cleanedHole.name, 
      projectId: cleanedHole.projectId, 
      siteId: cleanedHole.siteId 
    });
    
    // Convert to CreateDrillHoleRequest format
    const request: CreateDrillHoleRequest = {
      name: cleanedHole.name || '',
      easting: cleanedHole.easting,
      northing: cleanedHole.northing,
      elevation: cleanedHole.elevation,
      length: cleanedHole.length,
      depth: cleanedHole.depth,
      azimuth: cleanedHole.azimuth,
      dip: cleanedHole.dip,
      actualDepth: cleanedHole.actualDepth,
      stemming: cleanedHole.stemming,
      projectId: cleanedHole.projectId || 0,
      siteId: cleanedHole.siteId || 0
    };
    
    // Ensure ProjectId and SiteId are valid for validation
    if (request.projectId <= 0 || request.siteId <= 0) {
      console.error('❌ Invalid ProjectId or SiteId in request:', { projectId: request.projectId, siteId: request.siteId });
      return throwError(() => new Error(`Invalid project/site context: ProjectId=${request.projectId}, SiteId=${request.siteId}`));
    }
    
    console.log('📤 Request being sent:', { 
      name: request.name, 
      projectId: request.projectId, 
      siteId: request.siteId 
    });
    
    // Use the appropriate endpoint based on whether we have project/site context
    const projectId = cleanedHole.projectId;
    const siteId = cleanedHole.siteId;
    
    let endpoint = this.apiUrl;
    if (projectId && siteId && projectId > 0 && siteId > 0) {
      // Use the new endpoint that properly handles project/site context
      endpoint = `${this.apiUrl}/projects/${projectId}/sites/${siteId}`;
      console.log('✅ Using project/site specific endpoint:', endpoint);
    } else {
      console.log('❌ Using generic endpoint (no project/site context):', endpoint);
      console.log('❌ ProjectId:', projectId, 'SiteId:', siteId);
    }
    
    return this.http.post<DrillHole>(endpoint, request).pipe(
      catchError((error) => {
        console.error('❌ Error creating drill hole:', error);
        console.error('❌ Drill hole data:', request);
        console.error('❌ Endpoint used:', endpoint);
        return this.handleError(error);
      })
    );
  }

  // Create or update multiple drill holes (replaces existing data)
  saveMultipleDrillHoles(drillHoles: DrillHole[], projectId?: number, siteId?: number): Observable<DrillHole[]> {
    console.log('🚀 saveMultipleDrillHoles called with:', { 
      drillHolesCount: drillHoles.length, 
      projectId, 
      siteId,
      firstHoleOriginal: drillHoles[0] ? { 
        name: drillHoles[0].name, 
        projectId: drillHoles[0].projectId, 
        siteId: drillHoles[0].siteId 
      } : null
    });
    
    // Validate that we have valid project and site IDs
    if (!projectId || !siteId || projectId <= 0 || siteId <= 0) {
      console.error('❌ Cannot save drill holes without valid projectId and siteId', { 
        providedProjectId: projectId, 
        providedSiteId: siteId
      });
      return throwError(() => new Error(`Project ID (${projectId}) and Site ID (${siteId}) must be greater than 0`));
    }
    
    // Force set the correct project/site IDs on all holes
    const holesWithContext = drillHoles.map(hole => ({
      ...hole,
      projectId: projectId,
      siteId: siteId
    }));
    
    console.log('🔄 After adding context:', {
      firstHoleWithContext: holesWithContext[0] ? {
        name: holesWithContext[0].name,
        projectId: holesWithContext[0].projectId,
        siteId: holesWithContext[0].siteId
      } : null
    });
    
    // First delete existing drill holes for this specific project/site, then create new ones
    return this.deleteDrillHolesBySite(projectId, siteId).pipe(
      switchMap(() => {
        console.log('Existing data cleared for project/site, now creating new drill holes...', { projectId, siteId });
        
        // Validate and clean the drill holes before sending
        const cleanedHoles = holesWithContext.map(hole => this.validateAndCleanDrillHole(hole));
        
        // Log the data being sent for debugging
        console.log('Sending drill holes to backend:', cleanedHoles.slice(0, 2)); // Log first 2 for debugging
        
        if (cleanedHoles.length === 0) {
          return of([]);
        }
        
        // Create all drill holes in parallel
        const requests = cleanedHoles.map(hole => this.createDrillHole(hole));
        return forkJoin(requests);
      }),
      catchError(this.handleError)
    );
  }

  // Get drill holes for a specific site
  getDrillHolesBySite(projectId: number, siteId: number): Observable<DrillHole[]> {
    return this.getAllDrillHoles().pipe(
      map(holes => holes.filter(hole => 
        hole.projectId === projectId && hole.siteId === siteId
      )),
      catchError(this.handleError)
    );
  }

  // Delete drill holes for a specific site
  deleteDrillHolesBySite(projectId: number, siteId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/site/${projectId}/${siteId}`).pipe(
      catchError(this.handleError)
    );
  }

  // Legacy method - kept for compatibility
  createMultipleDrillHoles(drillHoles: DrillHole[]): Observable<DrillHole[]> {
    return this.saveMultipleDrillHoles(drillHoles);
  }

  // Update drill hole
  updateDrillHole(id: string, drillHole: DrillHole): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, drillHole).pipe(
      catchError(this.handleError)
    );
  }

  // Delete drill hole
  deleteDrillHole(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`).pipe(
      catchError(this.handleError)
    );
  }

  // Delete all drill holes (by deleting each one individually)
  deleteAllDrillHoles(): Observable<void> {
    console.log('Starting delete all drill holes operation...');
    
    return this.getAllDrillHoles().pipe(
      switchMap(holes => {
        console.log(`Found ${holes.length} drill holes to delete`);
        
        if (holes.length === 0) {
          console.log('No drill holes to delete');
          return of(undefined);
        }
        
        console.log('Deleting holes with IDs:', holes.map(h => h.id));
        const deleteRequests = holes.map(hole => 
          this.deleteDrillHole(hole.id!).pipe(
            catchError(error => {
              console.warn(`Failed to delete hole ${hole.id}:`, error);
              // Continue with other deletions even if one fails
              return of(undefined);
            })
          )
        );
        
        return forkJoin(deleteRequests);
      }),
      map(() => {
        console.log('Delete all operation completed');
        return undefined;
      }),
      catchError(error => {
        console.error('Error in deleteAllDrillHoles:', error);
        return this.handleError(error);
      })
    );
  }

  // Upload CSV file
  uploadCsvFile(file: File): Observable<DrillHole[]> {
    const formData = new FormData();
    formData.append('file', file);
    
    return this.http.post<DrillHole[]>(`${this.apiUrl}/upload-csv`, formData).pipe(
      catchError(this.handleError)
    );
  }

  // Verify database is empty
  verifyDatabaseEmpty(): Observable<boolean> {
    return this.getAllDrillHoles().pipe(
      map(holes => {
        const isEmpty = holes.length === 0;
        console.log(`Database verification: ${isEmpty ? 'empty' : `contains ${holes.length} holes`}`);
        return isEmpty;
      }),
      catchError(error => {
        console.error('Error verifying database state:', error);
        return of(false);
      })
    );
  }

  private validateAndCleanDrillHole(hole: DrillHole): DrillHole {
    console.log('Original hole data:', hole);
    
    // Ensure ID is not empty - generate one if needed
    const cleanId = hole.id && hole.id.trim() ? hole.id.trim() : this.generateId();
    
    // Helper function to safely parse numbers
    const safeNumber = (value: any, defaultVal: number = 0): number => {
      if (value === null || value === undefined || value === '') return defaultVal;
      const num = typeof value === 'string' ? parseFloat(value) : Number(value);
      return isNaN(num) ? defaultVal : num;
    };

    // Helper function to safely parse integers
    const safeInt = (value: any, defaultVal: number = 0): number => {
      if (value === null || value === undefined || value === '') return defaultVal;
      const num = typeof value === 'string' ? parseInt(value, 10) : Number(value);
      return isNaN(num) ? defaultVal : num;
    };
    
    const cleaned: DrillHole = {
      name: hole.name && hole.name.trim() ? hole.name.trim() : cleanId,
      easting: safeNumber(hole.easting, 0),
      northing: safeNumber(hole.northing, 0),
      elevation: safeNumber(hole.elevation, 0),
      length: Math.max(0.1, safeNumber(hole.length, 1)), // Ensure minimum length of 0.1
      depth: Math.max(0.1, safeNumber(hole.depth, 1)), // Ensure minimum depth of 0.1
      azimuth: Math.max(0, Math.min(360, safeNumber(hole.azimuth, 0))),
      dip: Math.max(-90, Math.min(90, safeNumber(hole.dip, 0))),
      actualDepth: Math.max(0.1, safeNumber(hole.actualDepth, hole.depth || 1)), // Use depth as fallback, ensure > 0
      stemming: Math.max(0, safeNumber(hole.stemming, 0)),
      projectId: safeInt(hole.projectId, 0),
      siteId: safeInt(hole.siteId, 0),
      createdAt: new Date().toISOString(),
      updatedAt: new Date().toISOString()
    };

    console.log('Cleaned hole data:', cleaned);
    return cleaned;
  }

  private generateId(): string {
    // Generate a proper GUID/UUID v4
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
      const r = Math.random() * 16 | 0;
      const v = c == 'x' ? r : (r & 0x3 | 0x8);
      return v.toString(16);
    });
  }

  private handleError(error: HttpErrorResponse): Observable<never> {
    let errorMessage = 'An unknown error occurred';
    
    if (error.error instanceof ErrorEvent) {
      // Client-side error
      errorMessage = `Error: ${error.error.message}`;
    } else {
      // Server-side error
      switch (error.status) {
        case 400:
          // Try to extract specific validation error from response
          if (error.error && typeof error.error === 'string') {
            errorMessage = error.error;
          } else if (error.error?.message) {
            errorMessage = error.error.message;
          } else {
            errorMessage = 'Bad Request: The drill hole data is invalid. Please check all required fields.';
          }
          break;
        case 404:
          errorMessage = 'API endpoint not found. Please check if the backend server is running.';
          break;
        case 500:
          errorMessage = 'Internal server error. Please try again later.';
          break;
        default:
          errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
          if (error.error?.message) {
            errorMessage = error.error.message;
          }
      }
    }
    
    console.error('DrillHoleService Error:', errorMessage, error);
    return throwError(() => error);
  }
}