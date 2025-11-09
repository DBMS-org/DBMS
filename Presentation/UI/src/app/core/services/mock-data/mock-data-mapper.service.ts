import { Injectable } from '@angular/core';
import { StockRequest, StockRequestStatus } from '../../models/stock-request.model';
import { ExplosiveRequest, RequestStatus } from '../../../components/explosive-manager/requests/models/explosive-request.model';

/**
 * Mock Data Mapper Service
 * Handles transformation between different data models used by components
 */
@Injectable({
  providedIn: 'root'
})
export class MockDataMapperService {

  constructor() {}

  /**
   * Map StockRequest status to ExplosiveRequest status
   */
  mapStockRequestStatusToExplosiveRequestStatus(stockStatus: StockRequestStatus): RequestStatus {
    const statusMap: { [key in StockRequestStatus]: RequestStatus } = {
      [StockRequestStatus.PENDING]: RequestStatus.PENDING,
      [StockRequestStatus.UNDER_REVIEW]: RequestStatus.PENDING,
      [StockRequestStatus.APPROVED]: RequestStatus.APPROVED,
      [StockRequestStatus.REJECTED]: RequestStatus.REJECTED,
      [StockRequestStatus.PARTIALLY_APPROVED]: RequestStatus.APPROVED,
      [StockRequestStatus.IN_PROGRESS]: RequestStatus.IN_PROGRESS,
      [StockRequestStatus.FULFILLED]: RequestStatus.COMPLETED,
      [StockRequestStatus.CANCELLED]: RequestStatus.CANCELLED
    };

    return statusMap[stockStatus] || RequestStatus.PENDING;
  }

  /**
   * Map ExplosiveRequest status to StockRequest status
   */
  mapExplosiveRequestStatusToStockRequestStatus(explosiveStatus: RequestStatus): StockRequestStatus {
    const statusMap: { [key in RequestStatus]: StockRequestStatus } = {
      [RequestStatus.PENDING]: StockRequestStatus.PENDING,
      [RequestStatus.APPROVED]: StockRequestStatus.APPROVED,
      [RequestStatus.REJECTED]: StockRequestStatus.REJECTED,
      [RequestStatus.IN_PROGRESS]: StockRequestStatus.IN_PROGRESS,
      [RequestStatus.COMPLETED]: StockRequestStatus.FULFILLED,
      [RequestStatus.CANCELLED]: StockRequestStatus.CANCELLED,
      [RequestStatus.DISPATCHED]: StockRequestStatus.IN_PROGRESS
    };

    return statusMap[explosiveStatus] || StockRequestStatus.PENDING;
  }

  /**
   * Convert StockRequest to ExplosiveRequest format
   */
  stockRequestToExplosiveRequest(stockRequest: StockRequest): ExplosiveRequest {
    // Get primary explosive type from first item
    const primaryItem = stockRequest.requestedItems[0];
    
    return {
      id: stockRequest.id,
      requesterId: stockRequest.requesterId,
      requesterName: stockRequest.requesterName,
      requesterRole: 'Store Manager',
      explosiveType: primaryItem?.explosiveType as any,
      quantity: primaryItem?.requestedQuantity,
      unit: primaryItem?.unit,
      requestDate: stockRequest.requestDate,
      requiredDate: stockRequest.requiredDate,
      status: this.mapStockRequestStatusToExplosiveRequestStatus(stockRequest.status),
      approvalDate: stockRequest.approvalDate,
      approvedBy: stockRequest.explosiveManagerName,
      rejectionReason: stockRequest.rejectionReason,
      notes: stockRequest.notes,
      storeLocation: stockRequest.requesterStoreName,
      purpose: primaryItem?.purpose || stockRequest.justification,
      dispatchDate: stockRequest.dispatchedDate,
      requestedItems: stockRequest.requestedItems.map(item => ({
        explosiveType: item.explosiveType as any,
        quantity: item.requestedQuantity,
        unit: item.unit,
        purpose: item.purpose,
        specifications: item.specifications
      }))
    };
  }

  /**
   * Convert ExplosiveRequest to StockRequest format
   */
  explosiveRequestToStockRequest(explosiveRequest: ExplosiveRequest): StockRequest {
    return {
      id: explosiveRequest.id,
      requesterId: explosiveRequest.requesterId,
      requesterName: explosiveRequest.requesterName,
      requesterStoreId: `store_${explosiveRequest.requesterId}`,
      requesterStoreName: explosiveRequest.storeLocation,
      explosiveManagerName: explosiveRequest.approvedBy,
      requestedItems: explosiveRequest.requestedItems?.map(item => ({
        explosiveType: item.explosiveType as any,
        requestedQuantity: item.quantity,
        unit: item.unit,
        purpose: item.purpose,
        specifications: item.specifications
      })) || [],
      requestDate: explosiveRequest.requestDate,
      requiredDate: explosiveRequest.requiredDate,
      status: this.mapExplosiveRequestStatusToStockRequestStatus(explosiveRequest.status),
      dispatched: explosiveRequest.status === RequestStatus.DISPATCHED || explosiveRequest.dispatchDate != null,
      dispatchedDate: explosiveRequest.dispatchDate,
      fulfillmentDate: explosiveRequest.status === RequestStatus.COMPLETED ? explosiveRequest.dispatchDate : undefined,
      justification: explosiveRequest.purpose,
      notes: explosiveRequest.notes,
      approvalDate: explosiveRequest.approvalDate,
      rejectionReason: explosiveRequest.rejectionReason,
      createdAt: explosiveRequest.requestDate,
      updatedAt: explosiveRequest.approvalDate || explosiveRequest.requestDate
    };
  }

  /**
   * Ensure data consistency between formats
   */
  validateDataConsistency(stockRequest: StockRequest, explosiveRequest: ExplosiveRequest): boolean {
    return (
      stockRequest.id === explosiveRequest.id &&
      stockRequest.requesterId === explosiveRequest.requesterId &&
      stockRequest.requesterName === explosiveRequest.requesterName &&
      stockRequest.requestDate.getTime() === explosiveRequest.requestDate.getTime() &&
      stockRequest.requiredDate.getTime() === explosiveRequest.requiredDate.getTime()
    );
  }

  /**
   * Get unified request data that can be used by both components
   */
  getUnifiedRequestData(baseData: any): { stockRequest: StockRequest; explosiveRequest: ExplosiveRequest } {
    let stockRequest: StockRequest;
    let explosiveRequest: ExplosiveRequest;

    if ('requesterStoreId' in baseData) {
      // Base data is StockRequest format
      stockRequest = baseData as StockRequest;
      explosiveRequest = this.stockRequestToExplosiveRequest(stockRequest);
    } else {
      // Base data is ExplosiveRequest format
      explosiveRequest = baseData as ExplosiveRequest;
      stockRequest = this.explosiveRequestToStockRequest(explosiveRequest);
    }

    return { stockRequest, explosiveRequest };
  }
}