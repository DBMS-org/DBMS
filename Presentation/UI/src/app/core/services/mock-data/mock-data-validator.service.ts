import { Injectable } from '@angular/core';
import { Observable, forkJoin, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { MockDataService } from './mock-data.service';
import { MockDataMapperService } from './mock-data-mapper.service';
import { StockRequest } from '../../models/stock-request.model';
import { ExplosiveRequest } from '../../../components/explosive-manager/requests/models/explosive-request.model';

export interface ValidationResult {
  isValid: boolean;
  errors: string[];
  warnings: string[];
  summary: {
    totalStockRequests: number;
    totalExplosiveRequests: number;
    matchingIds: number;
    consistentData: number;
  };
}

export interface DataConsistencyReport {
  requestId: string;
  stockRequestExists: boolean;
  explosiveRequestExists: boolean;
  dataMatches: boolean;
  inconsistencies: string[];
}

/**
 * Mock Data Validator Service
 * Validates consistency and accuracy across all mock data implementations
 */
@Injectable({
  providedIn: 'root'
})
export class MockDataValidatorService {

  constructor(
    private mockDataService: MockDataService,
    private mapperService: MockDataMapperService
  ) {}

  /**
   * Validate all mock data for consistency
   */
  validateAllMockData(): Observable<ValidationResult> {
    return forkJoin({
      stockRequests: this.mockDataService.getStockRequestMockData(),
      explosiveRequests: this.mockDataService.getExplosiveRequestMockData()
    }).pipe(
      map(({ stockRequests, explosiveRequests }) => {
        const errors: string[] = [];
        const warnings: string[] = [];
        let consistentData = 0;

        // Create maps for quick lookup
        const stockRequestMap = new Map(stockRequests.map(req => [req.id, req]));
        const explosiveRequestMap = new Map(explosiveRequests.map(req => [req.id, req]));

        // Get all unique IDs
        const allIds = new Set([...stockRequestMap.keys(), ...explosiveRequestMap.keys()]);
        const matchingIds = [...allIds].filter(id => 
          stockRequestMap.has(id) && explosiveRequestMap.has(id)
        ).length;

        // Validate each request
        allIds.forEach(id => {
          const stockRequest = stockRequestMap.get(id);
          const explosiveRequest = explosiveRequestMap.get(id);

          if (stockRequest && explosiveRequest) {
            // Both exist - validate consistency
            const isConsistent = this.mapperService.validateDataConsistency(stockRequest, explosiveRequest);
            if (isConsistent) {
              consistentData++;
            } else {
              errors.push(`Data inconsistency found for request ID: ${id}`);
            }

            // Validate specific fields
            this.validateRequestFields(stockRequest, explosiveRequest, errors, warnings);
          } else if (stockRequest && !explosiveRequest) {
            warnings.push(`Stock request ${id} exists but corresponding explosive request is missing`);
          } else if (!stockRequest && explosiveRequest) {
            warnings.push(`Explosive request ${id} exists but corresponding stock request is missing`);
          }
        });

        // Validate data integrity
        this.validateDataIntegrity(stockRequests, explosiveRequests, errors, warnings);

        return {
          isValid: errors.length === 0,
          errors,
          warnings,
          summary: {
            totalStockRequests: stockRequests.length,
            totalExplosiveRequests: explosiveRequests.length,
            matchingIds,
            consistentData
          }
        };
      })
    );
  }

  /**
   * Generate detailed consistency report
   */
  generateConsistencyReport(): Observable<DataConsistencyReport[]> {
    return forkJoin({
      stockRequests: this.mockDataService.getStockRequestMockData(),
      explosiveRequests: this.mockDataService.getExplosiveRequestMockData()
    }).pipe(
      map(({ stockRequests, explosiveRequests }) => {
        const stockRequestMap = new Map(stockRequests.map(req => [req.id, req]));
        const explosiveRequestMap = new Map(explosiveRequests.map(req => [req.id, req]));
        const allIds = new Set([...stockRequestMap.keys(), ...explosiveRequestMap.keys()]);

        return [...allIds].map(id => {
          const stockRequest = stockRequestMap.get(id);
          const explosiveRequest = explosiveRequestMap.get(id);
          const inconsistencies: string[] = [];

          let dataMatches = false;
          if (stockRequest && explosiveRequest) {
            dataMatches = this.mapperService.validateDataConsistency(stockRequest, explosiveRequest);
            if (!dataMatches) {
              inconsistencies.push(...this.getDetailedInconsistencies(stockRequest, explosiveRequest));
            }
          }

          return {
            requestId: id,
            stockRequestExists: !!stockRequest,
            explosiveRequestExists: !!explosiveRequest,
            dataMatches,
            inconsistencies
          };
        });
      })
    );
  }

  /**
   * Validate specific request fields
   */
  private validateRequestFields(
    stockRequest: StockRequest,
    explosiveRequest: ExplosiveRequest,
    errors: string[],
    warnings: string[]
  ): void {
    // Validate dates
    if (stockRequest.requestDate.getTime() !== explosiveRequest.requestDate.getTime()) {
      errors.push(`Request date mismatch for ID ${stockRequest.id}`);
    }

    if (stockRequest.requiredDate.getTime() !== explosiveRequest.requiredDate.getTime()) {
      errors.push(`Required date mismatch for ID ${stockRequest.id}`);
    }

    // Validate requester information
    if (stockRequest.requesterName !== explosiveRequest.requesterName) {
      errors.push(`Requester name mismatch for ID ${stockRequest.id}`);
    }

    // Validate requested items consistency
    if (stockRequest.requestedItems.length === 0 && !explosiveRequest.requestedItems) {
      warnings.push(`No requested items found for ID ${stockRequest.id}`);
    }

    // Validate status mapping
    const mappedStatus = this.mapperService.mapStockRequestStatusToExplosiveRequestStatus(stockRequest.status);
    if (mappedStatus !== explosiveRequest.status) {
      warnings.push(`Status mapping inconsistency for ID ${stockRequest.id}: ${stockRequest.status} -> ${explosiveRequest.status}`);
    }
  }

  /**
   * Validate overall data integrity
   */
  private validateDataIntegrity(
    stockRequests: StockRequest[],
    explosiveRequests: ExplosiveRequest[],
    errors: string[],
    warnings: string[]
  ): void {
    // Check for duplicate IDs within each dataset
    const stockIds = stockRequests.map(req => req.id);
    const explosiveIds = explosiveRequests.map(req => req.id);

    const duplicateStockIds = stockIds.filter((id, index) => stockIds.indexOf(id) !== index);
    const duplicateExplosiveIds = explosiveIds.filter((id, index) => explosiveIds.indexOf(id) !== index);

    if (duplicateStockIds.length > 0) {
      errors.push(`Duplicate stock request IDs found: ${duplicateStockIds.join(', ')}`);
    }

    if (duplicateExplosiveIds.length > 0) {
      errors.push(`Duplicate explosive request IDs found: ${duplicateExplosiveIds.join(', ')}`);
    }

    // Validate user consistency
    const stockUsers = new Set(stockRequests.map(req => req.requesterName));
    const explosiveUsers = new Set(explosiveRequests.map(req => req.requesterName));

    stockUsers.forEach(user => {
      if (!explosiveUsers.has(user)) {
        warnings.push(`User ${user} exists in stock requests but not in explosive requests`);
      }
    });

    explosiveUsers.forEach(user => {
      if (!stockUsers.has(user)) {
        warnings.push(`User ${user} exists in explosive requests but not in stock requests`);
      }
    });
  }

  /**
   * Get detailed inconsistencies between two requests
   */
  private getDetailedInconsistencies(
    stockRequest: StockRequest,
    explosiveRequest: ExplosiveRequest
  ): string[] {
    const inconsistencies: string[] = [];

    if (stockRequest.requesterId !== explosiveRequest.requesterId) {
      inconsistencies.push(`Requester ID: ${stockRequest.requesterId} vs ${explosiveRequest.requesterId}`);
    }

    if (stockRequest.requesterName !== explosiveRequest.requesterName) {
      inconsistencies.push(`Requester name: ${stockRequest.requesterName} vs ${explosiveRequest.requesterName}`);
    }

    if (stockRequest.requestDate.getTime() !== explosiveRequest.requestDate.getTime()) {
      inconsistencies.push(`Request date: ${stockRequest.requestDate.toISOString()} vs ${explosiveRequest.requestDate.toISOString()}`);
    }

    if (stockRequest.requiredDate.getTime() !== explosiveRequest.requiredDate.getTime()) {
      inconsistencies.push(`Required date: ${stockRequest.requiredDate.toISOString()} vs ${explosiveRequest.requiredDate.toISOString()}`);
    }

    return inconsistencies;
  }

  /**
   * Validate mock data service methods
   */
  validateMockDataService(): Observable<boolean> {
    return forkJoin({
      stockRequests: this.mockDataService.getStockRequestMockData(),
      explosiveRequests: this.mockDataService.getExplosiveRequestMockData(),
      users: of(this.mockDataService.getMockUsers()),
      items: of(this.mockDataService.getMockRequestItems())
    }).pipe(
      map(({ stockRequests, explosiveRequests, users, items }) => {
        // Validate that all methods return data
        return stockRequests.length > 0 &&
               explosiveRequests.length > 0 &&
               users.storeManagers.length > 0 &&
               users.explosiveManagers.length > 0 &&
               items.length > 0;
      })
    );
  }
}