import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError, forkJoin, of } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';
import { environment } from '../../../environments/environment';

export interface DrillHole {
  serialNumber?: number;
  id?: string;
  name?: string;
  easting: number;
  northing: number;
  elevation: number;
  length: number;
  depth: number;
  azimuth?: number | null;
  dip?: number | null;
  actualDepth: number;
  stemming: number;
  projectId?: number;
  siteId?: number;
  createdAt?: string;
  updatedAt?: string;
  has3DData?: boolean;
  requiresFallbackTo2D?: boolean;
}

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

  getAllDrillHoles(): Observable<DrillHole[]> {
    return this.http.get<DrillHole[]>(this.apiUrl).pipe(
   catchError(this.handleError)
    );}

  getDrillHole(id: string): Observable<DrillHole> {
    return this.http.get<DrillHole>(`${this.apiUrl}/${id}`).pipe(
      catchError(this.handleError)
    );
  }

  createDrillHole(drillHole: DrillHole): Observable<DrillHole> {
    const cleanedHole = this.validateAndCleanDrillHole(drillHole);

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

    if (request.projectId <= 0 || request.siteId <= 0) {
      return throwError(() => new Error(`Invalid project/site context: ProjectId=${request.projectId}, SiteId=${request.siteId}`));
    }

    const projectId = cleanedHole.projectId;
    const siteId = cleanedHole.siteId;

    let endpoint = this.apiUrl;
    if (projectId && siteId && projectId > 0 && siteId > 0) {
      endpoint = `${this.apiUrl}/projects/${projectId}/sites/${siteId}`;
    }

    return this.http.post<DrillHole>(endpoint, request).pipe(
      catchError(this.handleError)
    );
  }

  saveMultipleDrillHoles(drillHoles: DrillHole[], projectId?: number, siteId?: number): Observable<DrillHole[]> {
    if (!projectId || !siteId || projectId <= 0 || siteId <= 0) {
      return throwError(() => new Error(`Project ID (${projectId}) and Site ID (${siteId}) must be greater than 0`));
    }

    const holesWithContext = drillHoles.map(hole => ({
      ...hole,
      projectId: projectId,
      siteId: siteId
    }));

    return this.deleteDrillHolesBySite(projectId, siteId).pipe(
      switchMap(() => {
        const cleanedHoles = holesWithContext.map(hole => this.validateAndCleanDrillHole(hole));

        if (cleanedHoles.length === 0) {
          return of([]);
        }

        const requests = cleanedHoles.map(hole => this.createDrillHole(hole));
        return forkJoin(requests);
      }),
      catchError(this.handleError)
    );
  }

  getDrillHolesBySite(projectId: number, siteId: number): Observable<DrillHole[]> {
    return this.getAllDrillHoles().pipe(
      map(holes => holes.filter(hole => 
        hole.projectId === projectId && hole.siteId === siteId
      )),
      catchError(this.handleError)
    );
  }

  deleteDrillHolesBySite(projectId: number, siteId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/site/${projectId}/${siteId}`).pipe(
      catchError(this.handleError)
    );
  }

  createMultipleDrillHoles(drillHoles: DrillHole[]): Observable<DrillHole[]> {
    return this.saveMultipleDrillHoles(drillHoles);
  }

  updateDrillHole(id: string, drillHole: DrillHole): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, drillHole).pipe(
      catchError(this.handleError)
    );
  }

  deleteDrillHole(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`).pipe(
      catchError(this.handleError)
    );
  }

  deleteAllDrillHoles(): Observable<void> {
    return this.getAllDrillHoles().pipe(
      switchMap(holes => {
        if (holes.length === 0) {
          return of(undefined);
        }

        const deleteRequests = holes.map(hole =>
          this.deleteDrillHole(hole.id!).pipe(
            catchError(() => of(undefined))
          )
        );

        return forkJoin(deleteRequests);
      }),
      map(() => undefined),
      catchError(this.handleError)
    );
  }

  uploadCsvFile(file: File): Observable<DrillHole[]> {
    const formData = new FormData();
    formData.append('file', file);

    return this.http.post<DrillHole[]>(`${this.apiUrl}/upload-csv`, formData).pipe(
      catchError(this.handleError)
    );
  }

  verifyDatabaseEmpty(): Observable<boolean> {
    return this.getAllDrillHoles().pipe(
      map(holes => holes.length === 0),
      catchError(() => of(false))
    );
  }

  private validateAndCleanDrillHole(hole: DrillHole): DrillHole {
    const cleanId = hole.id && hole.id.trim() ? hole.id.trim() : this.generateId();

    const safeNumber = (value: any, defaultVal: number = 0): number => {
      if (value === null || value === undefined || value === '') return defaultVal;
      const num = typeof value === 'string' ? parseFloat(value) : Number(value);
      return isNaN(num) ? defaultVal : num;
    };

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
      length: Math.max(0.1, safeNumber(hole.length, 1)),
      depth: Math.max(0.1, safeNumber(hole.depth, 1)),
      azimuth: Math.max(0, Math.min(360, safeNumber(hole.azimuth, 0))),
      dip: Math.max(-90, Math.min(90, safeNumber(hole.dip, 0))),
      actualDepth: Math.max(0.1, safeNumber(hole.actualDepth, hole.depth || 1)),
      stemming: Math.max(0, safeNumber(hole.stemming, 0)),
      projectId: safeInt(hole.projectId, 0),
      siteId: safeInt(hole.siteId, 0),
      createdAt: new Date().toISOString(),
      updatedAt: new Date().toISOString()
    };

    return cleaned;
  }

  private generateId(): string {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
      const r = Math.random() * 16 | 0;
      const v = c == 'x' ? r : (r & 0x3 | 0x8);
      return v.toString(16);
    });
  }

  private handleError(error: HttpErrorResponse): Observable<never> {
    let errorMessage = 'An unknown error occurred';

    if (error.error instanceof ErrorEvent) {
      errorMessage = `Error: ${error.error.message}`;
    } else {
      switch (error.status) {
        case 400:
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