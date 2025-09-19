import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

interface BlastingEngineerRequest {
  requestId: string;
  engineer: {
    name: string;
    title: string;
  };
  explosiveType: string;
  quantity: {
    amount: number;
    unit: string;
  };
  requestDate: string;
  requiredDate: string;
  status: 'PENDING' | 'APPROVED' | 'REJECTED' | 'IN_PROGRESS' | 'COMPLETED';
  purpose: string;
}

@Component({
  selector: 'app-blasting-engineer-requests',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './blasting-engineer-requests.component.html',
  styleUrl: './blasting-engineer-requests.component.scss'
})
export class BlastingEngineerRequestsComponent implements OnInit {
  requests: BlastingEngineerRequest[] = [];
  filteredRequests: BlastingEngineerRequest[] = [];
  searchTerm: string = '';
  statusFilter: string = 'ALL';
  isLoading: boolean = false;

  ngOnInit(): void {
    this.loadRequests();
  }

  loadRequests(): void {
    this.isLoading = true;
    // Simulate API call with sample data
    setTimeout(() => {
      this.requests = [
        {
          requestId: '001',
          engineer: {
            name: 'John Smith',
            title: 'Blasting Engineer'
          },
          explosiveType: 'ANFO',
          quantity: {
            amount: 500,
            unit: 'kg'
          },
          requestDate: 'Jan 15, 2024',
          requiredDate: 'Jan 20, 2024',
          status: 'PENDING',
          purpose: 'Bench blasting operation - Section A mining area'
        },
        {
          requestId: '002',
          engineer: {
            name: 'Sarah Johnson',
            title: 'Senior Blasting Engineer'
          },
          explosiveType: 'Emulsion',
          quantity: {
            amount: 750,
            unit: 'kg'
          },
          requestDate: 'Jan 16, 2024',
          requiredDate: 'Jan 22, 2024',
          status: 'APPROVED',
          purpose: 'Underground tunnel blasting - Phase 2'
        },
        {
          requestId: '003',
          engineer: {
            name: 'Mike Wilson',
            title: 'Blasting Engineer'
          },
          explosiveType: 'ANFO',
          quantity: {
            amount: 300,
            unit: 'kg'
          },
          requestDate: 'Jan 17, 2024',
          requiredDate: 'Jan 25, 2024',
          status: 'IN_PROGRESS',
          purpose: 'Rock fragmentation for quarry operations'
        }
      ];
      this.filteredRequests = [...this.requests];
      this.isLoading = false;
    }, 1000);
  }

  onSearch(): void {
    this.applyFilters();
  }

  onStatusFilterChange(): void {
    this.applyFilters();
  }

  private applyFilters(): void {
    this.filteredRequests = this.requests.filter(request => {
      const matchesSearch = !this.searchTerm || 
        request.requestId.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        request.engineer.name.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        request.explosiveType.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        request.purpose.toLowerCase().includes(this.searchTerm.toLowerCase());
      
      const matchesStatus = this.statusFilter === 'ALL' || request.status === this.statusFilter;
      
      return matchesSearch && matchesStatus;
    });
  }

  getStatusClass(status: string): string {
    switch (status) {
      case 'PENDING': return 'status-pending';
      case 'APPROVED': return 'status-approved';
      case 'REJECTED': return 'status-rejected';
      case 'IN_PROGRESS': return 'status-in-progress';
      case 'COMPLETED': return 'status-completed';
      default: return '';
    }
  }

  onApprove(request: BlastingEngineerRequest): void {
    request.status = 'APPROVED';
    console.log('Approved request:', request.requestId);
  }

  onReject(request: BlastingEngineerRequest): void {
    request.status = 'REJECTED';
    console.log('Rejected request:', request.requestId);
  }

  onViewDetails(request: BlastingEngineerRequest): void {
    console.log('View details for request:', request.requestId);
  }

  clearFilters(): void {
    this.searchTerm = '';
    this.statusFilter = 'ALL';
    this.filteredRequests = [...this.requests];
  }

  getPendingCount(): number {
    return this.filteredRequests.filter(request => request.status === 'PENDING').length;
  }

  getApprovedCount(): number {
    return this.filteredRequests.filter(request => request.status === 'APPROVED').length;
  }
}