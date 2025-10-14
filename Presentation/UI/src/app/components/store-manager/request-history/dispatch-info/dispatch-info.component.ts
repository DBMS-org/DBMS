import { Component, Input, Output, EventEmitter, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, ActivatedRoute } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { Subject, takeUntil } from 'rxjs';
import { InventoryTransferRequest } from '../../../../core/models/inventory-transfer.model';
import { InventoryTransferService } from '../../../../core/services/inventory-transfer.service';

@Component({
  selector: 'app-dispatch-info',
  standalone: true,
  imports: [CommonModule, MatIconModule, MatButtonModule, MatSnackBarModule],
  templateUrl: './dispatch-info.component.html',
  styleUrls: ['./dispatch-info.component.scss']
})
export class DispatchInfoComponent implements OnInit, OnDestroy {
  private destroy$ = new Subject<void>();

  @Input() request: InventoryTransferRequest | null = null;
  @Output() close = new EventEmitter<void>();

  // For standalone page navigation
  requestId: number | null = null;
  isStandalonePage = false;
  isLoading = false;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private transferService: InventoryTransferService,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit() {
    // Check if this is being used as a standalone page
    const idParam = this.route.snapshot.paramMap.get('id');
    this.requestId = idParam ? parseInt(idParam) : null;
    this.isStandalonePage = !!this.requestId;

    if (this.isStandalonePage && this.requestId) {
      // Load request data based on ID
      this.loadRequestData(this.requestId);
    }
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private loadRequestData(id: number): void {
    this.isLoading = true;
    this.transferService.getTransferRequestById(id)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (request) => {
          this.request = request;
          this.isLoading = false;
        },
        error: (error) => {
          console.error('Error loading request:', error);
          this.snackBar.open('Error loading request details', 'Close', { duration: 3000 });
          this.isLoading = false;
          this.router.navigate(['/store-manager/request-history']);
        }
      });
  }

  onReceive(): void {
    if (!this.request) return;

    this.isLoading = true;
    this.transferService.confirmDelivery(this.request.id)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: () => {
          this.snackBar.open('Delivery confirmed successfully!', 'Close', { duration: 3000 });
          this.isLoading = false;
          // Reload the request data
          if (this.request) {
            this.loadRequestData(this.request.id);
          }
        },
        error: (error) => {
          console.error('Error confirming delivery:', error);
          this.snackBar.open('Error confirming delivery', 'Close', { duration: 3000 });
          this.isLoading = false;
        }
      });
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
    return this.request?.dispatchDate ? 'Dispatched' : 'Not Dispatched';
  }

  getDispatchStatusClass(): string {
    return this.request?.dispatchDate ? 'status-dispatched' : 'status-not-dispatched';
  }

  canConfirmDelivery(): boolean {
    return !!this.request?.dispatchDate && !this.request?.deliveryConfirmedDate;
  }
}