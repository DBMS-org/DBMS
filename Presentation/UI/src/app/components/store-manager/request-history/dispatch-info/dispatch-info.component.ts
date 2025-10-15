import { Component, Input, Output, EventEmitter, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, ActivatedRoute } from '@angular/router';
import { Subject, takeUntil } from 'rxjs';
import { InventoryTransferRequest } from '../../../../core/models/inventory-transfer.model';
import { InventoryTransferService } from '../../../../core/services/inventory-transfer.service';
import { MessageService } from 'primeng/api';
import { PanelModule } from 'primeng/panel';
import { ButtonModule } from 'primeng/button';
import { TagModule } from 'primeng/tag';
import { TableModule } from 'primeng/table';
import { MessageModule } from 'primeng/message';
import { ToastModule } from 'primeng/toast';

@Component({
  selector: 'app-dispatch-info',
  standalone: true,
  imports: [
    CommonModule,
    PanelModule,
    ButtonModule,
    TagModule,
    TableModule,
    MessageModule,
    ToastModule
  ],
  providers: [MessageService],
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
    private messageService: MessageService
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
          this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Error loading request details' });
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
          this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Delivery confirmed successfully!' });
          this.isLoading = false;
          // Reload the request data
          if (this.request) {
            this.loadRequestData(this.request.id);
          }
        },
        error: (error) => {
          console.error('Error confirming delivery:', error);
          this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Error confirming delivery' });
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