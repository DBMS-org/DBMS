import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface ReportMetadata {
  id: string;
  name: string;
  description: string;
  icon: string;
  category: string;
  lastGenerated?: Date;
  isAvailable: boolean;
}

export interface ReportFilter {
  startDate?: Date;
  endDate?: Date;
  regionId?: string;
  projectId?: string;
  metrics?: string[];
}

export interface ApiResponse<T> {
  success: boolean;
  data: T;
  message?: string;
  statusCode?: number;
  timestamp?: Date;
  version?: string;
}

@Injectable({
  providedIn: 'root'
})
export class ReportService {
  private apiUrl = `${environment.apiUrl}/api/reports`;

  constructor(private http: HttpClient) {}

  private getHeaders(): HttpHeaders {
    const token = localStorage.getItem('token');
    return new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });
  }

  getAvailableReports(): Observable<ApiResponse<ReportMetadata[]>> {
    return this.http.get<ApiResponse<ReportMetadata[]>>(
      `${this.apiUrl}/available`,
      { headers: this.getHeaders() }
    );
  }

  // REMOVED IN PHASE 1: Fleet Management Report - Insufficient data
  // getFleetManagementReport(filter?: ReportFilter): Observable<ApiResponse<any>> {
  //   return this.http.post<ApiResponse<any>>(
  //     `${this.apiUrl}/fleet-management`,
  //     filter || {},
  //     { headers: this.getHeaders() }
  //   );
  // }

  getMaintenancePerformanceReport(filter?: ReportFilter): Observable<ApiResponse<any>> {
    return this.http.post<ApiResponse<any>>(
      `${this.apiUrl}/maintenance-performance`,
      filter || {},
      { headers: this.getHeaders() }
    );
  }

  // REMOVED IN PHASE 1: Inventory Status Report - StoreInventories table empty
  // getInventoryStatusReport(filter?: ReportFilter): Observable<ApiResponse<any>> {
  //   return this.http.post<ApiResponse<any>>(
  //     `${this.apiUrl}/inventory-status`,
  //     filter || {},
  //     { headers: this.getHeaders() }
  //   );
  // }

  // REMOVED IN PHASE 1: Operational Efficiency Report - Insufficient data
  // getOperationalEfficiencyReport(filter?: ReportFilter): Observable<ApiResponse<any>> {
  //   return this.http.post<ApiResponse<any>>(
  //     `${this.apiUrl}/operational-efficiency`,
  //     filter || {},
  //     { headers: this.getHeaders() }
  //   );
  // }

  getRegionalPerformanceReport(filter?: ReportFilter): Observable<ApiResponse<any>> {
    return this.http.post<ApiResponse<any>>(
      `${this.apiUrl}/regional-performance`,
      filter || {},
      { headers: this.getHeaders() }
    );
  }

  // PHASE 3: Drilling Operations Report - Core business data (164 drill holes)
  getDrillingOperationsReport(filter?: ReportFilter): Observable<ApiResponse<any>> {
    return this.http.post<ApiResponse<any>>(
      `${this.apiUrl}/drilling-operations`,
      filter || {},
      { headers: this.getHeaders() }
    );
  }

  // PHASE 4: Explosive Workflow Report - Approval & Transfer tracking (12 requests)
  getExplosiveWorkflowReport(filter?: ReportFilter): Observable<ApiResponse<any>> {
    return this.http.post<ApiResponse<any>>(
      `${this.apiUrl}/explosive-workflow`,
      filter || {},
      { headers: this.getHeaders() }
    );
  }

  // PHASE 5: User & Access Management Report - Admin oversight (8 users, 7 roles)
  getUserAccessReport(filter?: ReportFilter): Observable<ApiResponse<any>> {
    return this.http.post<ApiResponse<any>>(
      `${this.apiUrl}/user-access`,
      filter || {},
      { headers: this.getHeaders() }
    );
  }

  getFinancialOverviewReport(filter?: ReportFilter): Observable<ApiResponse<any>> {
    return this.http.post<ApiResponse<any>>(
      `${this.apiUrl}/financial-overview`,
      filter || {},
      { headers: this.getHeaders() }
    );
  }

  generateCustomReport(request: any): Observable<ApiResponse<any>> {
    return this.http.post<ApiResponse<any>>(
      `${this.apiUrl}/custom`,
      request,
      { headers: this.getHeaders() }
    );
  }

  exportReportToPdf(reportType: string, filter?: ReportFilter): Observable<Blob> {
    return this.http.post(
      `${this.apiUrl}/export/pdf`,
      {
        reportType,
        format: 'PDF',
        ...filter
      },
      {
        headers: this.getHeaders(),
        responseType: 'blob'
      }
    );
  }

  exportReportToExcel(reportType: string, filter?: ReportFilter): Observable<Blob> {
    return this.http.post(
      `${this.apiUrl}/export/excel`,
      {
        reportType,
        format: 'Excel',
        ...filter
      },
      {
        headers: this.getHeaders(),
        responseType: 'blob'
      }
    );
  }
}
