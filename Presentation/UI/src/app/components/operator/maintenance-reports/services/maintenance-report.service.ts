import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, firstValueFrom, catchError, throwError, of } from 'rxjs';
import { environment } from '../../../../../environments/environment';
import { 
  ProblemReport, 
  OperatorMachine, 
  ProblemReportSummary,
  CreateProblemReportRequest,
  UpdateReportStatusRequest,
  ReportStatus,
  ReportFilters
} from '../models/maintenance-report.models';

@Injectable({
  providedIn: 'root'
})
export class MaintenanceReportService {
  private http = inject(HttpClient);
  private readonly baseUrl = environment.apiUrl;
  private readonly apiUrl = `${this.baseUrl}/api/maintenance-reports`;
  private readonly machinesApiUrl = `${this.baseUrl}/api/machines`;

  /**
   * Create a new maintenance problem report
   * @param reportData - The problem report data to submit
   * @returns Promise<ProblemReport> - The created problem report with generated ID and ticket number
   */
  async submitProblemReport(reportData: CreateProblemReportRequest): Promise<ProblemReport> {
    try {
      const response = await firstValueFrom(
        this.http.post<ProblemReport>(`${this.apiUrl}/submit`, reportData)
          .pipe(
            catchError(error => {
              console.error('Failed to submit problem report:', error);
              return throwError(() => this.handleSubmissionError(error));
            })
          )
      );
      return response;
    } catch (error) {
      throw error;
    }
  }

  /**
   * Get all maintenance reports submitted by an operator
   * @param operatorId - The ID of the operator
   * @returns Observable<ProblemReport[]> - List of operator's problem reports
   */
  getOperatorReports(operatorId: number | string): Observable<ProblemReport[]> {
    return this.http.get<ProblemReport[]>(`${this.apiUrl}/operator/${operatorId}`)
      .pipe(
        catchError(error => {
          console.error('Failed to fetch operator reports:', error);
          
          // Return empty array for development when no reports exist
          if (error.status === 404) {
            console.log('Creating default empty reports array since none were found');
            return of([]);
          }
          
          return throwError(() => new Error('Failed to load your maintenance reports. Please try again.'));
        })
      );
  }

  /**
   * Get filtered problem reports for an operator
   * @param operatorId - The ID of the operator
   * @param filters - Filter criteria for reports
   * @returns Observable<ProblemReport[]> - Filtered list of problem reports
   */
  getFilteredOperatorReports(operatorId: number | string, filters: ReportFilters): Observable<ProblemReport[]> {
    let params = new HttpParams();
    
    if (filters.status?.length) {
      filters.status.forEach(status => {
        params = params.append('status', status);
      });
    }
    
    if (filters.severity?.length) {
      filters.severity.forEach(severity => {
        params = params.append('severity', severity);
      });
    }
    
    if (filters.dateRange) {
      params = params.append('startDate', filters.dateRange.start.toISOString());
      params = params.append('endDate', filters.dateRange.end.toISOString());
    }
    
    if (filters.searchTerm) {
      params = params.append('search', filters.searchTerm);
    }

    return this.http.get<ProblemReport[]>(`${this.apiUrl}/operator/${operatorId}/filtered`, { params })
      .pipe(
        catchError(error => {
          console.error('Failed to fetch filtered reports:', error);
          return throwError(() => new Error('Failed to load filtered reports. Please try again.'));
        })
      );
  }

  /**
   * Get the machine assigned to an operator
   * @param operatorId - The ID of the operator
   * @returns Observable<OperatorMachine> - The operator's assigned machine details
   */
  getOperatorMachine(operatorId: number | string): Observable<OperatorMachine> {
    // First try the dedicated maintenance reports endpoint
    return this.http.get<OperatorMachine>(`${this.apiUrl}/operator/${operatorId}/machine`)
      .pipe(
        catchError(error => {
          console.error('Failed to fetch operator machine from maintenance API:', error);
          
          // Fall back to the machines API
          return this.http.get<any>(`${this.machinesApiUrl}/operator/${operatorId}`)
            .pipe(
              catchError(fallbackError => {
                console.error('Failed to fetch operator machine from machines API:', fallbackError);
                
                // Return a default empty machine object for development/testing purposes
                if (fallbackError.status === 404) {
                  console.log('Creating default machine object since none was found');
                  return of({
                    id: '0',
                    name: 'Demo Machine',
                    model: 'Demo Model',
                    serialNumber: 'DEMO-12345',
                    currentLocation: 'Development Site',
                    status: 'AVAILABLE',
                    assignedOperatorId: operatorId.toString()
                  });
                }
                
                return throwError(() => new Error('Failed to load machine information. Please contact support.'));
              })
            );
        })
      );
  }

  /**
   * Get summary statistics for an operator's reports
   * @param operatorId - The ID of the operator
   * @returns Observable<ProblemReportSummary> - Summary statistics
   */
  getReportSummary(operatorId: number | string): Observable<ProblemReportSummary> {
    return this.http.get<ProblemReportSummary>(`${this.apiUrl}/operator/${operatorId}/summary`)
      .pipe(
        catchError(error => {
          console.error('Failed to fetch report summary:', error);
          return throwError(() => new Error('Failed to load report summary.'));
        })
      );
  }

  /**
   * Get a specific problem report by ID
   * @param reportId - The ID of the problem report
   * @returns Observable<ProblemReport> - The problem report details
   */
  getReportById(reportId: string): Observable<ProblemReport> {
    return this.http.get<ProblemReport>(`${this.apiUrl}/${reportId}`)
      .pipe(
        catchError(error => {
          console.error('Failed to fetch report details:', error);
          return throwError(() => new Error('Failed to load report details. Please try again.'));
        })
      );
  }

  /**
   * Update the status of a problem report (typically used by mechanical engineers)
   * @param reportId - The ID of the problem report
   * @param statusUpdate - The status update data
   * @returns Observable<void>
   */
  updateReportStatus(reportId: string, statusUpdate: UpdateReportStatusRequest): Observable<void> {
    return this.http.patch<void>(`${this.apiUrl}/${reportId}/status`, statusUpdate)
      .pipe(
        catchError(error => {
          console.error('Failed to update report status:', error);
          return throwError(() => new Error('Failed to update report status. Please try again.'));
        })
      );
  }

  /**
   * Check if there are any pending offline reports to sync
   * @returns boolean - True if there are pending reports
   */
  hasPendingOfflineReports(): boolean {
    const pendingReports = localStorage.getItem('pending_maintenance_reports');
    return pendingReports ? JSON.parse(pendingReports).length > 0 : false;
  }

  /**
   * Save a report draft locally for offline scenarios
   * @param draft - The draft report data
   */
  saveDraftReport(draft: Partial<CreateProblemReportRequest>): void {
    try {
      localStorage.setItem('maintenance_report_draft', JSON.stringify({
        ...draft,
        savedAt: new Date().toISOString()
      }));
    } catch (error) {
      console.error('Failed to save draft report:', error);
    }
  }

  /**
   * Get saved draft report from local storage
   * @returns Partial<CreateProblemReportRequest> | null - The draft report or null if none exists
   */
  getDraftReport(): Partial<CreateProblemReportRequest> | null {
    try {
      const draft = localStorage.getItem('maintenance_report_draft');
      return draft ? JSON.parse(draft) : null;
    } catch (error) {
      console.error('Failed to retrieve draft report:', error);
      return null;
    }
  }

  /**
   * Clear the saved draft report
   */
  clearDraftReport(): void {
    try {
      localStorage.removeItem('maintenance_report_draft');
    } catch (error) {
      console.error('Failed to clear draft report:', error);
    }
  }

  /**
   * Save a report for offline submission when network is restored
   * @param reportData - The report data to save for later submission
   */
  saveOfflineReport(reportData: CreateProblemReportRequest): void {
    try {
      const existingReports = localStorage.getItem('pending_maintenance_reports');
      const pendingReports = existingReports ? JSON.parse(existingReports) : [];
      
      pendingReports.push({
        ...reportData,
        savedAt: new Date().toISOString(),
        id: `offline_${Date.now()}_${Math.random().toString(36).substr(2, 9)}`
      });
      
      localStorage.setItem('pending_maintenance_reports', JSON.stringify(pendingReports));
    } catch (error) {
      console.error('Failed to save offline report:', error);
    }
  }

  /**
   * Get all pending offline reports
   * @returns Array of pending reports
   */
  getPendingOfflineReports(): (CreateProblemReportRequest & { id: string; savedAt: string })[] {
    try {
      const pendingReports = localStorage.getItem('pending_maintenance_reports');
      return pendingReports ? JSON.parse(pendingReports) : [];
    } catch (error) {
      console.error('Failed to retrieve pending reports:', error);
      return [];
    }
  }

  /**
   * Sync all pending offline reports when network is restored
   * @returns Promise<number> - Number of successfully synced reports
   */
  async syncOfflineReports(): Promise<number> {
    const pendingReports = this.getPendingOfflineReports();
    if (pendingReports.length === 0) return 0;

    let syncedCount = 0;
    const failedReports = [];

    for (const report of pendingReports) {
      try {
        const { id, savedAt, ...reportData } = report;
        await this.submitProblemReport(reportData);
        syncedCount++;
      } catch (error) {
        console.error('Failed to sync offline report:', error);
        failedReports.push(report);
      }
    }

    // Update localStorage with only the failed reports
    try {
      localStorage.setItem('pending_maintenance_reports', JSON.stringify(failedReports));
    } catch (error) {
      console.error('Failed to update pending reports:', error);
    }

    return syncedCount;
  }

  /**
   * Handle submission errors and provide user-friendly messages
   * @param error - The HTTP error response
   * @returns Error with user-friendly message
   */
  private handleSubmissionError(error: any): Error {
    let errorMessage = 'Failed to submit problem report. Please try again.';
    
    switch (error.status) {
      case 400:
        errorMessage = 'Invalid report data. Please check all required fields and try again.';
        break;
      case 401:
        errorMessage = 'You are not authorized to submit reports. Please log in again.';
        break;
      case 404:
        errorMessage = 'Machine not found. Please contact support.';
        break;
      case 422:
        errorMessage = 'Report validation failed. Please check all fields and try again.';
        break;
      case 500:
        errorMessage = 'Server error occurred. Please try again later or contact support.';
        break;
      case 0:
        errorMessage = 'Network error. Please check your connection and try again.';
        break;
    }
    
    return new Error(errorMessage);
  }
}