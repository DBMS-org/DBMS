import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, ActivatedRoute } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { StockRequest } from '../../../../core/models/stock-request.model';

@Component({
  selector: 'app-dispatch-info',
  standalone: true,
  imports: [CommonModule, MatIconModule, MatButtonModule],
  templateUrl: './dispatch-info.component.html',
  styleUrls: ['./dispatch-info.component.scss']
})
export class DispatchInfoComponent implements OnInit {
  @Input() request: StockRequest | null = null;
  @Output() close = new EventEmitter<void>();
  
  // For standalone page navigation
  requestId: string | null = null;
  isStandalonePage = false;

  constructor(
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    // Check if this is being used as a standalone page
    this.requestId = this.route.snapshot.paramMap.get('id');
    this.isStandalonePage = !!this.requestId;
    
    if (this.isStandalonePage && this.requestId) {
      // Load request data based on ID
      this.loadRequestData(this.requestId);
    }
  }

  private loadRequestData(id: string) {
    // Mock data for now - in real app, this would come from a service
    const mockRequests: StockRequest[] = [
      {
        id: '1',
        requesterId: 'user1',
        requesterName: 'Ahmed Al-Rashid',
        requesterStoreId: 'store1',
        requesterStoreName: 'Muscat Field Storage',
        requestedItems: [
          {
            explosiveType: 'ANFO' as any,
            requestedQuantity: 0.5,
            unit: 'tons',
            purpose: 'Mining operations - Phase 2',
            specifications: 'Standard grade ANFO for surface mining'
          }
        ],
        requestDate: new Date('2024-01-15'),
        requiredDate: new Date('2024-01-25'),
        status: 'APPROVED' as any,
        dispatched: true,
        dispatchedDate: new Date('2024-01-17'),
        justification: 'Urgent requirement for upcoming mining phase',
        notes: 'Please ensure delivery before 25th Jan',
        approvalDate: new Date('2024-01-16'),
        createdAt: new Date('2024-01-15'),
        updatedAt: new Date('2024-01-16')
      }
    ];
    
    this.request = mockRequests.find(r => r.id === id) || null;
  }

  onReceive(): void {
    console.log('Receive button clicked for request:', this.request?.id);
    // Placeholder for receive functionality
  }

  onClose(): void {
    if (this.isStandalonePage) {
      this.router.navigate(['/store-manager/request-history']);
    } else {
      this.close.emit();
    }
  }

  formatDate(date: Date | undefined): string {
    if (!date) return 'N/A';
    return new Date(date).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric'
    });
  }

  getDispatchStatus(): string {
    return this.request?.dispatched ? 'Dispatched' : 'Not Dispatched';
  }

  getDispatchStatusClass(): string {
    return this.request?.dispatched ? 'status-dispatched' : 'status-not-dispatched';
  }
}