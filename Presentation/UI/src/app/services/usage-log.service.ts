import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import {
  CreateUsageLogRequest,
  UsageLogDto,
  UsageStatisticsDto
} from '../components/operator/my-machines/models/usage-log.models';

/**
 * Usage Log Service - Connects to Backend API
 * API Base: /api/usage-logs
 */
@Injectable({
  providedIn: 'root'
})
export class UsageLogService {
  private readonly apiUrl = `${environment.apiUrl}/api/usage-logs`;

  constructor(private http: HttpClient) {}

  /**
   * Create usage log (Operator)
   * POST /api/usage-logs
   *
   * Triggered by: Operator submitting usage log form
   * Backend behavior: Creates log, increments machine service hours via domain event
   */
  createUsageLog(request: CreateUsageLogRequest): Observable<UsageLogDto> {
    return this.http.post<UsageLogDto>(this.apiUrl, request);
  }

  /**
   * Get usage log by ID
   * GET /api/usage-logs/{id}
   */
  getUsageLogById(id: number): Observable<UsageLogDto> {
    return this.http.get<UsageLogDto>(`${this.apiUrl}/${id}`);
  }

  /**
   * Get usage logs for a machine
   * GET /api/usage-logs/machine/{machineId}
   *
   * Used by: Operator (My Machines), Mechanical Engineer (Job Details), Machine Manager (Reports)
   */
  getUsageLogsByMachine(
    machineId: number,
    startDate?: Date,
    endDate?: Date
  ): Observable<UsageLogDto[]> {
    let params = new HttpParams();
    if (startDate) {
      params = params.set('startDate', startDate.toISOString());
    }
    if (endDate) {
      params = params.set('endDate', endDate.toISOString());
    }

    return this.http.get<UsageLogDto[]>(
      `${this.apiUrl}/machine/${machineId}`,
      { params }
    );
  }

  /**
   * Get latest usage log for machine (for pre-filling form)
   * GET /api/usage-logs/machine/{machineId}/latest
   *
   * Used by: Operator when opening usage log form to pre-fill engine hour start
   */
  getLatestUsageLog(machineId: number): Observable<UsageLogDto> {
    return this.http.get<UsageLogDto>(`${this.apiUrl}/machine/${machineId}/latest`);
  }

  /**
   * Get usage statistics for a machine
   * GET /api/usage-logs/machine/{machineId}/statistics
   *
   * Used by: Operator (My Machines dashboard), Machine Manager (Reports)
   * Returns: Aggregated stats for specified time period
   */
  getUsageStatistics(machineId: number, days: number = 30): Observable<UsageStatisticsDto> {
    return this.http.get<UsageStatisticsDto>(
      `${this.apiUrl}/machine/${machineId}/statistics`,
      { params: new HttpParams().set('days', days.toString()) }
    );
  }

  /**
   * Get usage logs by operator
   * GET /api/usage-logs/operator/{operatorId}
   *
   * Used by: Machine Manager to view all logs for an operator
   */
  getUsageLogsByOperator(
    operatorId: number,
    startDate?: Date,
    endDate?: Date
  ): Observable<UsageLogDto[]> {
    let params = new HttpParams();
    if (startDate) {
      params = params.set('startDate', startDate.toISOString());
    }
    if (endDate) {
      params = params.set('endDate', endDate.toISOString());
    }

    return this.http.get<UsageLogDto[]>(
      `${this.apiUrl}/operator/${operatorId}`,
      { params }
    );
  }

  /**
   * Approve usage log (Machine Manager)
   * PATCH /api/usage-logs/{id}/approve
   */
  approveUsageLog(id: number): Observable<{ message: string }> {
    return this.http.patch<{ message: string }>(`${this.apiUrl}/${id}/approve`, {});
  }

  /**
   * Reject usage log (Machine Manager)
   * PATCH /api/usage-logs/{id}/reject
   */
  rejectUsageLog(id: number): Observable<{ message: string }> {
    return this.http.patch<{ message: string }>(`${this.apiUrl}/${id}/reject`, {});
  }
}
