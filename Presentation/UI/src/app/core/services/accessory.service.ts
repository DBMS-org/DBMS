import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

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
  private apiUrl = `${environment.apiUrl}/accessories`;

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

    return this.http.get<Accessory[]>(this.apiUrl, { params });
  }

  // Get single accessory by ID
  getAccessory(id: number): Observable<Accessory> {
    return this.http.get<Accessory>(`${this.apiUrl}/${id}`);
  }

  // Get stock adjustment history
  getStockHistory(id: number): Observable<StockAdjustmentHistory[]> {
    return this.http.get<StockAdjustmentHistory[]>(`${this.apiUrl}/${id}/stock-history`);
  }

  // Get statistics
  getStatistics(): Observable<AccessoryStatistics> {
    return this.http.get<AccessoryStatistics>(`${this.apiUrl}/statistics`);
  }

  // Create new accessory
  createAccessory(request: CreateAccessoryRequest): Observable<Accessory> {
    return this.http.post<Accessory>(this.apiUrl, request);
  }

  // Update existing accessory
  updateAccessory(id: number, request: UpdateAccessoryRequest): Observable<Accessory> {
    return this.http.put<Accessory>(`${this.apiUrl}/${id}`, request);
  }

  // Adjust stock levels
  adjustStock(id: number, request: StockAdjustmentRequest): Observable<Accessory> {
    return this.http.post<Accessory>(`${this.apiUrl}/${id}/adjust-stock`, request);
  }

  // Delete accessory
  deleteAccessory(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
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
