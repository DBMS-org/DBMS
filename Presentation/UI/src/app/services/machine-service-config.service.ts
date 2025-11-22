import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

/**
 * Service Due Alert DTO - Matches backend
 */
export interface ServiceDueAlertDto {
  machineId: number;
  machineName: string;
  serviceType: string; // "Engine" or "Drifter"
  hoursRemaining: number;
  isOverdue: boolean;
  urgencyLevel: string; // "GREEN", "YELLOW", "ORANGE", "RED"
}

/**
 * Machine Service Status DTO - Matches backend
 */
export interface MachineServiceStatusDto {
  machineId: number;
  machineName: string;
  engineServiceInterval: number;
  currentEngineServiceHours: number;
  engineHoursRemaining: number;
  lastEngineServiceDate?: Date;
  isEngineServiceDue: boolean;
  drifterServiceInterval?: number;
  currentDrifterServiceHours?: number;
  drifterHoursRemaining?: number;
  lastDrifterServiceDate?: Date;
  isDrifterServiceDue: boolean;
}

/**
 * Update Machine Service Config Request - Matches backend
 */
export interface UpdateMachineServiceConfigRequest {
  engineServiceInterval: number;
  currentEngineServiceHours: number;
  drifterServiceInterval?: number;
  currentDrifterServiceHours?: number;
}

/**
 * Machine Service Configuration Service
 * API Base: /api/machines
 *
 * Handles:
 * - Service due alerts
 * - Machine service status
 * - Service configuration updates
 */
@Injectable({
  providedIn: 'root'
})
export class MachineServiceConfigService {
  private readonly apiUrl = `${environment.apiUrl}/machines`;

  constructor(private http: HttpClient) {}

  /**
   * Get machine service status
   * GET /api/machines/{id}/service-status
   *
   * Used by: Machine Manager (Machine Details page)
   * Returns: Current service hours, intervals, and hours remaining
   */
  getMachineServiceStatus(machineId: number): Observable<MachineServiceStatusDto> {
    return this.http.get<MachineServiceStatusDto>(`${this.apiUrl}/${machineId}/service-status`);
  }

  /**
   * Get all service due alerts
   * GET /api/machines/service-alerts
   *
   * Used by: Machine Manager (Dashboard)
   * Returns: All machines with service due or approaching (< 100 hours remaining)
   */
  getServiceDueAlerts(): Observable<ServiceDueAlertDto[]> {
    return this.http.get<ServiceDueAlertDto[]>(`${this.apiUrl}/service-alerts`);
  }

  /**
   * Get service alerts by region
   * GET /api/machines/service-alerts/region/{regionId}
   *
   * Used by: Regional managers to see alerts for their region
   */
  getServiceDueAlertsByRegion(regionId: number): Observable<ServiceDueAlertDto[]> {
    return this.http.get<ServiceDueAlertDto[]>(`${this.apiUrl}/service-alerts/region/${regionId}`);
  }

  /**
   * Update machine service configuration (Machine Manager)
   * PATCH /api/machines/{id}/service-config
   *
   * Used by: Machine Manager when:
   * - Setting up new machine
   * - Adjusting service intervals
   * - Manually correcting service hours
   */
  updateServiceConfig(
    machineId: number,
    request: UpdateMachineServiceConfigRequest
  ): Observable<void> {
    return this.http.patch<void>(`${this.apiUrl}/${machineId}/service-config`, request);
  }

  /**
   * Helper: Calculate urgency CSS class for alert
   */
  getAlertSeverityClass(urgencyLevel: string): string {
    switch (urgencyLevel) {
      case 'RED':
        return 'alert-critical';  // Overdue or < 20 hours
      case 'ORANGE':
        return 'alert-urgent';    // < 50 hours
      case 'YELLOW':
        return 'alert-warning';   // < 100 hours
      case 'GREEN':
        return 'alert-info';      // > 100 hours (good)
      default:
        return '';
    }
  }

  /**
   * Helper: Calculate progress percentage
   */
  calculateServiceProgress(currentHours: number, intervalHours: number): number {
    if (!intervalHours || intervalHours <= 0) return 0;
    return Math.min((currentHours / intervalHours) * 100, 100);
  }

  /**
   * Helper: Get urgency class based on hours remaining
   */
  getServiceUrgencyClass(hoursRemaining: number): string {
    if (hoursRemaining < 0) return 'urgency-overdue';
    if (hoursRemaining < 20) return 'urgency-critical';
    if (hoursRemaining < 50) return 'urgency-warning';
    if (hoursRemaining < 100) return 'urgency-caution';
    return 'urgency-good';
  }
}
