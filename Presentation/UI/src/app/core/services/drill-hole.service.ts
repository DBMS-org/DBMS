import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError, forkJoin, of } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';
import { environment } from '../../../environments/environment';

export interface DrillHole {
  serialNumber?: number;
  id: string;
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
    );
  }

  // Get drill hole by ID
  getDrillHole(id: string): Observable<DrillHole> {
    return this.http.get<DrillHole>(`${this.apiUrl}/${id}`).pipe(
      catchError(this.handleError)
    );
  }

  // Create a new drill hole
  createDrillHole(drillHole: DrillHole): Observable<DrillHole> {
    // Validate and clean single drill hole
    const cleanedHole = this.validateAndCleanDrillHole(drillHole);
    
    return this.http.post<DrillHole>(this.apiUrl, cleanedHole).pipe(
      catchError((error) => {
        console.error('Error creating drill hole:', error);
        console.error('Drill hole data:', cleanedHole);
        return this.handleError(error);
      })
    );
  }

  // Create or update multiple drill holes (replaces existing data)
  saveMultipleDrillHoles(drillHoles: DrillHole[], projectId?: number, siteId?: number): Observable<DrillHole[]> {
    console.log('Saving drill holes - clearing existing data first...', { projectId, siteId });
    
    // Add project/site context to all drill holes
    const holesWithContext = drillHoles.map(hole => ({
      ...hole,
      projectId: projectId || hole.projectId || 0,
      siteId: siteId || hole.siteId || 0
    }));
    
    // First delete all existing drill holes, then create new ones
    return this.deleteAllDrillHoles().pipe(
      switchMap(() => {
        console.log('Existing data cleared, now creating new drill holes...');
        
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
    return this.getDrillHolesBySite(projectId, siteId).pipe(
      switchMap(holes => {
        if (holes.length === 0) {
          return of(undefined);
        }
        
        const deleteRequests = holes.map(hole => this.deleteDrillHole(hole.id));
        return forkJoin(deleteRequests);
      }),
      map(() => undefined),
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
          this.deleteDrillHole(hole.id).pipe(
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
      id: cleanId,
      name: hole.name && hole.name.trim() ? hole.name.trim() : cleanId,
      easting: safeNumber(hole.easting, 0),
      northing: safeNumber(hole.northing, 0),
      elevation: safeNumber(hole.elevation, 0),
      length: Math.max(0, safeNumber(hole.length, 0)),
      depth: Math.max(0, safeNumber(hole.depth, 0)),
      azimuth: Math.max(0, Math.min(360, safeNumber(hole.azimuth, 0))),
      dip: Math.max(-90, Math.min(90, safeNumber(hole.dip, 0))),
      actualDepth: Math.max(0, safeNumber(hole.actualDepth, 0)),
      stemming: Math.max(0, safeNumber(hole.stemming, 0)),
      projectId: hole.projectId || 0,
      siteId: hole.siteId || 0,
      createdAt: new Date().toISOString(),
      updatedAt: new Date().toISOString()
    };

    console.log('Cleaned hole data:', cleaned);
    return cleaned;
  }

  private generateId(): string {
    return 'hole_' + Date.now() + '_' + Math.random().toString(36).substr(2, 9);
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