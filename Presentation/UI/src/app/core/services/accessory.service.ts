import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '../../../environments/environment';

// API Response wrapper interface
interface ApiResponse<T> {
  data: T;
  statusCode: number;
  message: string;
  success: boolean;
  timestamp: string;
  version: string;
}

export interface Accessory {
  id: number;
  name: string;
  category: string;
  partNumber: string;
  description?: string;
  quantity: number;
  unit: string;
  minStockLevel: number;
  supplier: string;
  location?: string;
  status: string;
  isLowStock: boolean;
  isOutOfStock: boolean;
  createdAt: Date;
  updatedAt?: Date;
}

export interface AccessoryStatistics {
  totalAvailable: number;
  lowStock: number;
  outOfStock: number;
  totalItems: number;
}

export interface CreateAccessoryRequest {
  name: string;
  category: string;
  partNumber: string;
  description?: string;
  quantity: number;
  unit: string;
  minStockLevel: number;
  supplier: string;
  location?: string;
}

export interface UpdateAccessoryRequest {
  name: string;
  category: string;
  partNumber: string;
  description?: string;
  unit: string;
  minStockLevel: number;
  supplier: string;
  location?: string;
}

export interface StockAdjustmentRequest {
  type: string; // 'Add', 'Remove', 'Set'
  quantity: number;
  reason: string; // 'Purchase', 'Usage', 'Damaged', 'Lost', 'Returned', 'Correction', 'Other'
  notes?: string;
}

export interface StockAdjustmentHistory {
  id: number;
  adjustmentType: string;
  quantityChanged: number;
  previousQuantity: number;
  newQuantity: number;
  reason: string;
  notes?: string;
  adjustedBy: string;
  adjustedDate: Date;
}

@Injectable({
  providedIn: 'root'
})
export class AccessoryService {
  private apiUrl = `${environment.apiUrl}/api/accessories`;

  constructor(private http: HttpClient) {}

  // Get all accessories with optional filters
  getAccessories(
    search?: string,
    category?: string,
    supplier?: string,
    status?: string
  ): Observable<Accessory[]> {
    let params = new HttpParams();

    if (search) params = params.set('search', search);
    if (category) params = params.set('category', category);
    if (supplier) params = params.set('supplier', supplier);
    if (status) params = params.set('status', status);

    return this.http.get<ApiResponse<Accessory[]>>(this.apiUrl, { params })
      .pipe(map(response => response?.data || []));
  }

  // Get single accessory by ID
  getAccessory(id: number): Observable<Accessory> {
    return this.http.get<ApiResponse<Accessory>>(`${this.apiUrl}/${id}`)
      .pipe(map(response => response.data));
  }

  // Get stock adjustment history
  getStockHistory(id: number): Observable<StockAdjustmentHistory[]> {
    return this.http.get<ApiResponse<StockAdjustmentHistory[]>>(`${this.apiUrl}/${id}/stock-history`)
      .pipe(map(response => response?.data || []));
  }

  // Get statistics
  getStatistics(): Observable<AccessoryStatistics> {
    return this.http.get<ApiResponse<AccessoryStatistics>>(`${this.apiUrl}/statistics`)
      .pipe(map(response => response?.data || { totalAvailable: 0, lowStock: 0, outOfStock: 0, totalItems: 0 }));
  }

  // Create new accessory
  createAccessory(request: CreateAccessoryRequest): Observable<Accessory> {
    return this.http.post<ApiResponse<Accessory>>(this.apiUrl, request)
      .pipe(map(response => response.data));
  }

  // Update existing accessory
  updateAccessory(id: number, request: UpdateAccessoryRequest): Observable<Accessory> {
    return this.http.put<ApiResponse<Accessory>>(`${this.apiUrl}/${id}`, request)
      .pipe(map(response => response.data));
  }

  // Adjust stock levels
  adjustStock(id: number, request: StockAdjustmentRequest): Observable<Accessory> {
    return this.http.post<ApiResponse<Accessory>>(`${this.apiUrl}/${id}/adjust-stock`, request)
      .pipe(map(response => response.data));
  }

  // Delete accessory
  deleteAccessory(id: number): Observable<any> {
    return this.http.delete<ApiResponse<any>>(`${this.apiUrl}/${id}`)
      .pipe(map(response => response.data));
  }

  // Export to CSV
  exportToCsv(
    search?: string,
    category?: string,
    supplier?: string,
    status?: string
  ): Observable<Blob> {
    let params = new HttpParams();

    if (search) params = params.set('search', search);
    if (category) params = params.set('category', category);
    if (supplier) params = params.set('supplier', supplier);
    if (status) params = params.set('status', status);

    return this.http.get(`${this.apiUrl}/export`, {
      params,
      responseType: 'blob'
    });
  }
}
