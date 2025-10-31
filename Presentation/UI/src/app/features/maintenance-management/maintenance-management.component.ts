import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { TableModule } from 'primeng/table';
import { DropdownModule } from 'primeng/dropdown';
import { TooltipModule } from 'primeng/tooltip';

// Data model interfaces for maintenance management system
interface MaintenanceRecord {
  id: string;
  machineId: string;
  machineName: string;
  machineModel: string;
  rigNo: string;
  type: 'preventive' | 'corrective' | 'emergency' | 'inspection';
  priority: 'low' | 'medium' | 'high' | 'critical';
  status: 'scheduled' | 'in-progress' | 'completed' | 'overdue' | 'cancelled';
  description: string;
  notes?: string;
  scheduledDate: string;
  scheduledTime: string;
  assignedTechnician: string;
  estimatedDuration: number;
  actualDuration?: number;
  cost?: number;
  actualCost?: number;
  completionNotes?: string;
  createdDate: string;
  completedDate?: string;
  createdBy: string;
}

interface Machine {
  id: string;
  name: string;
  model: string;
  rigNo: string;
  status: string;
}

interface MaintenanceStatistics {
  scheduled: number;
  inProgress: number;
  overdue: number;
  completed: number;
}

interface MaintenanceFormData {
  machineId: string;
  type: string;
  priority: string;
  description: string;
  notes: string;
  scheduledDate: string;
  scheduledTime: string;
  assignedTechnician: string;
  estimatedDuration: number;
  cost: number;
}

interface CompletionData {
  actualDuration: number;
  actualCost: number;
  completionNotes: string;
}

interface ConfirmModalData {
  title: string;
  message: string;
  confirmButtonText: string;
  confirmButtonClass: string;
  onConfirm: () => void;
}

interface OperatorIssue {
  id: number;
  ticketId: string;
  machineId: number;
  machineName: string;
  machineModel: string;
  operatorId: number;
  operatorName: string;
  affectedPart: string;
  problemCategory: string;
  description: string;
  severityLevel: 'Low' | 'Medium' | 'High' | 'Critical';
  status: 'Reported' | 'Acknowledged' | 'InProgress' | 'Resolved' | 'Closed';
  reportedAt: string;
  acknowledgedAt?: string;
  symptoms?: string[];
  errorCodes?: string;
}

/**
 * Maintenance Management Component
 * 
 * Comprehensive maintenance scheduling and tracking system that provides:
 * - Complete maintenance record lifecycle management (schedule, track, complete)
 * - Advanced filtering and search capabilities across multiple criteria
 * - Real-time statistics dashboard for maintenance operations overview
 * - Multi-modal interface for scheduling, editing, and completing maintenance tasks
 * - Technician assignment and workload management
 * - Cost tracking and duration estimation vs actual reporting
 * - Pagination and sorting for large maintenance record datasets
 */
@Component({
  selector: 'app-maintenance-management',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ButtonModule,
    TableModule,
    DropdownModule,
    TooltipModule
  ],
  templateUrl: './maintenance-management.component.html',
  styleUrls: ['./maintenance-management.component.scss']
})
export class MaintenanceManagementComponent implements OnInit {
  // Core data collections for maintenance operations
  maintenanceRecords: MaintenanceRecord[] = [];
  filteredMaintenanceRecords: MaintenanceRecord[] = [];
  paginatedMaintenanceRecords: MaintenanceRecord[] = [];
  availableMachines: Machine[] = [];
  availableTechnicians: string[] = [];
  statistics: MaintenanceStatistics = {
    scheduled: 0,
    inProgress: 0,
    overdue: 0,
    completed: 0
  };

  // Application state management
  isLoading = false;
  isSaving = false;
  isExporting = false;
  errorMessage = '';
  lastUpdated = new Date();

  // Advanced filtering and search capabilities
  searchTerm = '';
  selectedStatus = '';
  selectedType = '';
  selectedPriority = '';
  // Filter expansion state
  isFilterExpanded: boolean = true;

  // Data sorting configuration
  sortField = 'scheduledDate';
  sortDirection: 'asc' | 'desc' = 'asc';

  // Pagination controls for large datasets
  currentPage = 1;
  pageSize = 25;
  totalPages = 0;

  // Modal dialog states for various operations
  showScheduleModal = false;
  showDetailsModal = false;
  showCompleteModal = false;
  showConfirmModal = false;
  showIssueReportsModal = false;
  showIssueDetailsModal = false;

  // Form data objects for maintenance operations
  maintenanceFormData: MaintenanceFormData = this.getEmptyFormData();
  completionData: CompletionData = {
    actualDuration: 0,
    actualCost: 0,
    completionNotes: ''
  };
  confirmModalData: ConfirmModalData = {
    title: '',
    message: '',
    confirmButtonText: '',
    confirmButtonClass: '',
    onConfirm: () => {}
  };

  // Currently selected maintenance records for operations
  selectedMaintenance: MaintenanceRecord | null = null;
  editingMaintenance: MaintenanceRecord | null = null;

  // Issue Reports data
  operatorIssues: OperatorIssue[] = [];
  filteredIssues: OperatorIssue[] = [];
  selectedIssue: OperatorIssue | null = null;
  issueSearchTerm = '';
  issueStatusFilter = '';
  issueSeverityFilter = '';
  issueStats = {
    total: 0,
    reported: 0,
    critical: 0,
    resolved: 0
  };

  ngOnInit(): void {
    this.loadMaintenanceData();
    this.loadAvailableMachines();
    this.loadAvailableTechnicians();
  }

  // Data loading methods for initializing component state
  /**
   * Loads all maintenance records from the backend
   * Applies current filters and calculates statistics after loading
   */
  loadMaintenanceData(): void {
    this.isLoading = true;
    this.errorMessage = '';

    // Simulate API call with mock data for development
    setTimeout(() => {
      try {
        this.maintenanceRecords = this.generateMockMaintenanceData();
        this.applyFilters();
        this.calculateStatistics();
        this.lastUpdated = new Date();
      } catch (error) {
        this.errorMessage = 'Failed to load maintenance data. Please try again.';
        console.error('Error loading maintenance data:', error);
      } finally {
        this.isLoading = false;
      }
    }, 1000);
  }

  /**
   * Loads available machines for maintenance scheduling
   * Provides dropdown options in the maintenance form
   */
  loadAvailableMachines(): void {
    // Simulate loading machines from backend service
    this.availableMachines = [
      { id: '1', name: 'Excavator CAT-320', model: 'CAT-320D', rigNo: 'RIG-001', status: 'available' },
      { id: '2', name: 'Bulldozer CAT-D6', model: 'CAT-D6T', rigNo: 'RIG-002', status: 'assigned' },
      { id: '3', name: 'Crane Liebherr-LTM', model: 'LTM-1100', rigNo: 'RIG-003', status: 'available' },
      { id: '4', name: 'Loader CAT-950', model: 'CAT-950M', rigNo: 'RIG-004', status: 'maintenance' },
      { id: '5', name: 'Grader CAT-140', model: 'CAT-140M', rigNo: 'RIG-005', status: 'available' }
    ];
  }

  /**
   * Loads available technicians for maintenance assignment
   * Provides dropdown options for technician selection
   */
  loadAvailableTechnicians(): void {
    this.availableTechnicians = [
      'John Smith',
      'Mike Johnson',
      'Sarah Wilson',
      'David Brown',
      'Lisa Davis',
      'Robert Miller',
      'Jennifer Garcia',
      'Michael Rodriguez'
    ];
  }

  /**
   * Generates mock maintenance data for development and testing
   * Creates realistic maintenance records with varied statuses and types
   */
  generateMockMaintenanceData(): MaintenanceRecord[] {
    const records: MaintenanceRecord[] = [];
    const types: MaintenanceRecord['type'][] = ['preventive', 'corrective', 'emergency', 'inspection'];
    const priorities: MaintenanceRecord['priority'][] = ['low', 'medium', 'high', 'critical'];
    const statuses: MaintenanceRecord['status'][] = ['scheduled', 'in-progress', 'completed', 'overdue'];

    for (let i = 1; i <= 50; i++) {
      const machine = this.availableMachines[Math.floor(Math.random() * this.availableMachines.length)];
      const technician = this.availableTechnicians[Math.floor(Math.random() * this.availableTechnicians.length)];
      const type = types[Math.floor(Math.random() * types.length)];
      const priority = priorities[Math.floor(Math.random() * priorities.length)];
      const status = statuses[Math.floor(Math.random() * statuses.length)];
      
      const scheduledDate = new Date();
      scheduledDate.setDate(scheduledDate.getDate() + Math.floor(Math.random() * 60) - 30);
      
      const record: MaintenanceRecord = {
        id: `MAINT-${i.toString().padStart(3, '0')}`,
        machineId: machine.id,
        machineName: machine.name,
        machineModel: machine.model,
        rigNo: machine.rigNo,
        type,
        priority,
        status,
        description: this.getMaintenanceDescription(type),
        notes: Math.random() > 0.5 ? 'Additional maintenance notes and special instructions.' : '',
        scheduledDate: scheduledDate.toISOString().split('T')[0],
        scheduledTime: `${Math.floor(Math.random() * 12) + 8}:${Math.random() > 0.5 ? '00' : '30'}`,
        assignedTechnician: technician,
        estimatedDuration: Math.floor(Math.random() * 8) + 1,
        cost: Math.floor(Math.random() * 5000) + 500,
        createdDate: new Date(Date.now() - Math.random() * 30 * 24 * 60 * 60 * 1000).toISOString(),
        createdBy: 'Machine Manager'
      };

      // Add completion data for completed records
      if (status === 'completed') {
        record.actualDuration = record.estimatedDuration + (Math.random() * 2 - 1);
        record.actualCost = record.cost! + (Math.random() * 1000 - 500);
        record.completionNotes = 'Maintenance completed successfully. All systems checked and operational.';
        record.completedDate = new Date(new Date(record.scheduledDate).getTime() + 24 * 60 * 60 * 1000).toISOString();
      }

      records.push(record);
    }

    return records.sort((a, b) => new Date(b.createdDate).getTime() - new Date(a.createdDate).getTime());
  }

  getMaintenanceDescription(type: MaintenanceRecord['type']): string {
    const descriptions = {
      preventive: 'Routine preventive maintenance including oil change, filter replacement, and system inspection.',
      corrective: 'Corrective maintenance to address identified issues and restore optimal performance.',
      emergency: 'Emergency repair required due to critical system failure or safety concern.',
      inspection: 'Comprehensive inspection of all systems and components for safety and compliance.'
    };
    return descriptions[type];
  }

  // Statistics calculation
  calculateStatistics(): void {
    this.statistics = {
      scheduled: this.maintenanceRecords.filter(r => r.status === 'scheduled').length,
      inProgress: this.maintenanceRecords.filter(r => r.status === 'in-progress').length,
      overdue: this.maintenanceRecords.filter(r => r.status === 'overdue').length,
      completed: this.maintenanceRecords.filter(r => r.status === 'completed').length
    };
  }

  // Filtering and search methods
  applyFilters(): void {
    let filtered = [...this.maintenanceRecords];

    // Apply search filter
    if (this.searchTerm) {
      const term = this.searchTerm.toLowerCase();
      filtered = filtered.filter(record =>
        record.machineName.toLowerCase().includes(term) ||
        record.machineModel.toLowerCase().includes(term) ||
        record.rigNo.toLowerCase().includes(term) ||
        record.type.toLowerCase().includes(term) ||
        record.assignedTechnician.toLowerCase().includes(term) ||
        record.description.toLowerCase().includes(term)
      );
    }

    // Apply status filter
    if (this.selectedStatus) {
      filtered = filtered.filter(record => record.status === this.selectedStatus);
    }

    // Apply type filter
    if (this.selectedType) {
      filtered = filtered.filter(record => record.type === this.selectedType);
    }

    // Apply priority filter
    if (this.selectedPriority) {
      filtered = filtered.filter(record => record.priority === this.selectedPriority);
    }

    this.filteredMaintenanceRecords = filtered;
    this.updatePagination();
  }

  // Filter change handlers
  onStatusFilterChange(): void {
    this.currentPage = 1;
    this.applyFilters();
  }

  onTypeFilterChange(): void {
    this.currentPage = 1;
    this.applyFilters();
  }

  onPriorityFilterChange(): void {
    this.currentPage = 1;
    this.applyFilters();
  }

  clearSearch(): void {
    this.searchTerm = '';
    this.applyFilters();
  }

  clearStatusFilter(): void {
    this.selectedStatus = '';
    this.applyFilters();
  }

  clearTypeFilter(): void {
    this.selectedType = '';
    this.applyFilters();
  }

  clearPriorityFilter(): void {
    this.selectedPriority = '';
    this.applyFilters();
  }

  clearAllFilters(): void {
    this.searchTerm = '';
    this.selectedStatus = '';
    this.selectedType = '';
    this.selectedPriority = '';
    this.currentPage = 1;
    this.applyFilters();
  }

  toggleFilterExpansion(): void {
    this.isFilterExpanded = !this.isFilterExpanded;
  }

  hasActiveFilters(): boolean {
    return !!(this.selectedStatus || this.selectedType || this.selectedPriority);
  }

  // Sorting methods
  sortBy(field: string): void {
    if (this.sortField === field) {
      this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
    } else {
      this.sortField = field;
      this.sortDirection = 'asc';
    }

    this.filteredMaintenanceRecords.sort((a, b) => {
      let aValue = (a as any)[field];
      let bValue = (b as any)[field];

      if (field === 'scheduledDate') {
        aValue = new Date(aValue).getTime();
        bValue = new Date(bValue).getTime();
      }

      if (aValue < bValue) {
        return this.sortDirection === 'asc' ? -1 : 1;
      }
      if (aValue > bValue) {
        return this.sortDirection === 'asc' ? 1 : -1;
      }
      return 0;
    });

    this.updatePagination();
  }

  // Pagination methods
  updatePagination(): void {
    this.totalPages = Math.ceil(this.filteredMaintenanceRecords.length / this.pageSize);
    if (this.currentPage > this.totalPages) {
      this.currentPage = 1;
    }
    this.updatePaginatedRecords();
  }

  updatePaginatedRecords(): void {
    const startIndex = (this.currentPage - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    this.paginatedMaintenanceRecords = this.filteredMaintenanceRecords.slice(startIndex, endIndex);
  }

  previousPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.updatePaginatedRecords();
    }
  }

  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.updatePaginatedRecords();
    }
  }

  goToPage(page: number): void {
    this.currentPage = page;
    this.updatePaginatedRecords();
  }

  onPageSizeChange(): void {
    this.currentPage = 1;
    this.updatePagination();
  }

  getPageNumbers(): number[] {
    const pages: number[] = [];
    const maxVisible = 5;
    let start = Math.max(1, this.currentPage - Math.floor(maxVisible / 2));
    let end = Math.min(this.totalPages, start + maxVisible - 1);

    if (end - start + 1 < maxVisible) {
      start = Math.max(1, end - maxVisible + 1);
    }

    for (let i = start; i <= end; i++) {
      pages.push(i);
    }

    return pages;
  }

  getStartIndex(): number {
    return (this.currentPage - 1) * this.pageSize;
  }

  getEndIndex(): number {
    return Math.min(this.getStartIndex() + this.pageSize, this.filteredMaintenanceRecords.length);
  }

  // Modal methods
  openScheduleModal(): void {
    this.editingMaintenance = null;
    this.maintenanceFormData = this.getEmptyFormData();
    this.showScheduleModal = true;
  }

  closeScheduleModal(): void {
    this.showScheduleModal = false;
    this.editingMaintenance = null;
    this.maintenanceFormData = this.getEmptyFormData();
  }

  openDetailsModal(record: MaintenanceRecord): void {
    this.selectedMaintenance = record;
    this.showDetailsModal = true;
  }

  closeDetailsModal(): void {
    this.showDetailsModal = false;
    this.selectedMaintenance = null;
  }

  openCompleteModal(record: MaintenanceRecord): void {
    this.selectedMaintenance = record;
    this.completionData = {
      actualDuration: record.estimatedDuration,
      actualCost: record.cost || 0,
      completionNotes: ''
    };
    this.showCompleteModal = true;
  }

  closeCompleteModal(): void {
    this.showCompleteModal = false;
    this.selectedMaintenance = null;
    this.completionData = {
      actualDuration: 0,
      actualCost: 0,
      completionNotes: ''
    };
  }

  openConfirmModal(data: ConfirmModalData): void {
    this.confirmModalData = data;
    this.showConfirmModal = true;
  }

  closeConfirmModal(): void {
    this.showConfirmModal = false;
  }

  // CRUD operations
  viewMaintenanceDetails(record: MaintenanceRecord): void {
    this.openDetailsModal(record);
  }

  editMaintenance(record: MaintenanceRecord): void {
    this.editingMaintenance = record;
    this.maintenanceFormData = {
      machineId: record.machineId,
      type: record.type,
      priority: record.priority,
      description: record.description,
      notes: record.notes || '',
      scheduledDate: record.scheduledDate,
      scheduledTime: record.scheduledTime,
      assignedTechnician: record.assignedTechnician,
      estimatedDuration: record.estimatedDuration,
      cost: record.cost || 0
    };
    this.closeDetailsModal();
    this.showScheduleModal = true;
  }

  startMaintenance(record: MaintenanceRecord): void {
    this.openConfirmModal({
      title: 'Start Maintenance',
      message: `Are you sure you want to start maintenance for ${record.machineName}?`,
      confirmButtonText: 'Start Maintenance',
      confirmButtonClass: 'btn-success',
      onConfirm: () => {
        this.confirmStartMaintenance(record);
      }
    });
  }

  completeMaintenance(record: MaintenanceRecord): void {
    this.closeConfirmModal();
    this.openCompleteModal(record);
  }

  cancelMaintenance(record: MaintenanceRecord): void {
    this.openConfirmModal({
      title: 'Cancel Maintenance',
      message: `Are you sure you want to cancel maintenance for ${record.machineName}? This action cannot be undone.`,
      confirmButtonText: 'Cancel Maintenance',
      confirmButtonClass: 'btn-danger',
      onConfirm: () => {
        this.confirmCancelMaintenance(record);
      }
    });
  }

  // Form methods
  getEmptyFormData(): MaintenanceFormData {
    return {
      machineId: '',
      type: '',
      priority: '',
      description: '',
      notes: '',
      scheduledDate: '',
      scheduledTime: '',
      assignedTechnician: '',
      estimatedDuration: 1,
      cost: 0
    };
  }

  isMaintenanceFormValid(): boolean {
    return !!(
      this.maintenanceFormData.machineId &&
      this.maintenanceFormData.type &&
      this.maintenanceFormData.priority &&
      this.maintenanceFormData.description &&
      this.maintenanceFormData.scheduledDate &&
      this.maintenanceFormData.scheduledTime &&
      this.maintenanceFormData.assignedTechnician &&
      this.maintenanceFormData.estimatedDuration > 0
    );
  }

  isCompletionFormValid(): boolean {
    return !!(
      this.completionData.actualDuration > 0 &&
      this.completionData.completionNotes.trim()
    );
  }

  saveMaintenance(): void {
    if (!this.isMaintenanceFormValid()) {
      return;
    }

    this.isSaving = true;

    // Simulate API call
    setTimeout(() => {
      try {
        if (this.editingMaintenance) {
          // Update existing maintenance
          const index = this.maintenanceRecords.findIndex(r => r.id === this.editingMaintenance!.id);
          if (index !== -1) {
            const machine = this.availableMachines.find(m => m.id === this.maintenanceFormData.machineId);
            this.maintenanceRecords[index] = {
              ...this.maintenanceRecords[index],
              machineId: this.maintenanceFormData.machineId,
              machineName: machine?.name || '',
              machineModel: machine?.model || '',
              rigNo: machine?.rigNo || '',
              type: this.maintenanceFormData.type as MaintenanceRecord['type'],
              priority: this.maintenanceFormData.priority as MaintenanceRecord['priority'],
              description: this.maintenanceFormData.description,
              notes: this.maintenanceFormData.notes,
              scheduledDate: this.maintenanceFormData.scheduledDate,
              scheduledTime: this.maintenanceFormData.scheduledTime,
              assignedTechnician: this.maintenanceFormData.assignedTechnician,
              estimatedDuration: this.maintenanceFormData.estimatedDuration,
              cost: this.maintenanceFormData.cost
            };
          }
        } else {
          // Create new maintenance
          const machine = this.availableMachines.find(m => m.id === this.maintenanceFormData.machineId);
          const newRecord: MaintenanceRecord = {
            id: `MAINT-${(this.maintenanceRecords.length + 1).toString().padStart(3, '0')}`,
            machineId: this.maintenanceFormData.machineId,
            machineName: machine?.name || '',
            machineModel: machine?.model || '',
            rigNo: machine?.rigNo || '',
            type: this.maintenanceFormData.type as MaintenanceRecord['type'],
            priority: this.maintenanceFormData.priority as MaintenanceRecord['priority'],
            status: 'scheduled',
            description: this.maintenanceFormData.description,
            notes: this.maintenanceFormData.notes,
            scheduledDate: this.maintenanceFormData.scheduledDate,
            scheduledTime: this.maintenanceFormData.scheduledTime,
            assignedTechnician: this.maintenanceFormData.assignedTechnician,
            estimatedDuration: this.maintenanceFormData.estimatedDuration,
            cost: this.maintenanceFormData.cost,
            createdDate: new Date().toISOString(),
            createdBy: 'Machine Manager'
          };
          this.maintenanceRecords.unshift(newRecord);
        }

        this.applyFilters();
        this.calculateStatistics();
        this.closeScheduleModal();
        this.lastUpdated = new Date();
      } catch (error) {
        this.errorMessage = 'Failed to save maintenance. Please try again.';
        console.error('Error saving maintenance:', error);
      } finally {
        this.isSaving = false;
      }
    }, 1000);
  }

  confirmStartMaintenance(record: MaintenanceRecord): void {
    this.isSaving = true;

    setTimeout(() => {
      const index = this.maintenanceRecords.findIndex(r => r.id === record.id);
      if (index !== -1) {
        this.maintenanceRecords[index].status = 'in-progress';
        this.applyFilters();
        this.calculateStatistics();
        this.lastUpdated = new Date();
      }
      this.isSaving = false;
      this.closeConfirmModal();
    }, 500);
  }

  confirmCompleteMaintenance(): void {
    if (!this.isCompletionFormValid() || !this.selectedMaintenance) {
      return;
    }

    this.isSaving = true;

    setTimeout(() => {
      const index = this.maintenanceRecords.findIndex(r => r.id === this.selectedMaintenance!.id);
      if (index !== -1) {
        this.maintenanceRecords[index] = {
          ...this.maintenanceRecords[index],
          status: 'completed',
          actualDuration: this.completionData.actualDuration,
          actualCost: this.completionData.actualCost,
          completionNotes: this.completionData.completionNotes,
          completedDate: new Date().toISOString()
        };
        this.applyFilters();
        this.calculateStatistics();
        this.lastUpdated = new Date();
      }
      this.isSaving = false;
      this.closeCompleteModal();
    }, 1000);
  }

  confirmCancelMaintenance(record: MaintenanceRecord): void {
    this.isSaving = true;

    setTimeout(() => {
      const index = this.maintenanceRecords.findIndex(r => r.id === record.id);
      if (index !== -1) {
        this.maintenanceRecords[index].status = 'cancelled';
        this.applyFilters();
        this.calculateStatistics();
        this.lastUpdated = new Date();
      }
      this.isSaving = false;
      this.closeConfirmModal();
    }, 500);
  }

  // Utility methods
  refreshData(): void {
    this.loadMaintenanceData();
  }

  openIssueReports(): void {
    // Navigate to issue reports modal/component
    this.showIssueReportsModal = true;
    this.loadOperatorIssues();
  }

  exportMaintenanceData(): void {
    this.isExporting = true;

    // Simulate export process
    setTimeout(() => {
      try {
        const dataToExport = this.filteredMaintenanceRecords.map(record => ({
          'Maintenance ID': record.id,
          'Machine': record.machineName,
          'Model': record.machineModel,
          'Rig No': record.rigNo,
          'Type': record.type,
          'Priority': record.priority,
          'Status': record.status,
          'Scheduled Date': record.scheduledDate,
          'Scheduled Time': record.scheduledTime,
          'Technician': record.assignedTechnician,
          'Estimated Duration': record.estimatedDuration,
          'Actual Duration': record.actualDuration || '',
          'Estimated Cost': record.cost || '',
          'Actual Cost': record.actualCost || '',
          'Description': record.description,
          'Notes': record.notes || '',
          'Completion Notes': record.completionNotes || '',
          'Created Date': record.createdDate,
          'Completed Date': record.completedDate || ''
        }));

        // In a real application, you would use a library like xlsx or csv-writer
        console.log('Exporting maintenance data:', dataToExport);
        
        // Simulate file download
        const blob = new Blob([JSON.stringify(dataToExport, null, 2)], { type: 'application/json' });
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        link.download = `maintenance-data-${new Date().toISOString().split('T')[0]}.json`;
        link.click();
        window.URL.revokeObjectURL(url);
      } catch (error) {
        this.errorMessage = 'Failed to export data. Please try again.';
        console.error('Error exporting data:', error);
      } finally {
        this.isExporting = false;
      }
    }, 2000);
  }

  clearError(): void {
    this.errorMessage = '';
  }

  // Issue Reports Methods
  loadOperatorIssues(): void {
    this.isLoading = true;
    // Simulate API call - in production this would call the backend
    setTimeout(() => {
      this.operatorIssues = this.generateMockIssueData();
      this.applyIssueFilters();
      this.calculateIssueStats();
      this.isLoading = false;
    }, 800);
  }

  applyIssueFilters(): void {
    this.filteredIssues = this.operatorIssues.filter(issue => {
      const matchesSearch = !this.issueSearchTerm ||
        issue.ticketId.toLowerCase().includes(this.issueSearchTerm.toLowerCase()) ||
        issue.machineName.toLowerCase().includes(this.issueSearchTerm.toLowerCase()) ||
        issue.operatorName.toLowerCase().includes(this.issueSearchTerm.toLowerCase()) ||
        issue.description.toLowerCase().includes(this.issueSearchTerm.toLowerCase());

      const matchesStatus = !this.issueStatusFilter || issue.status === this.issueStatusFilter;
      const matchesSeverity = !this.issueSeverityFilter || issue.severityLevel === this.issueSeverityFilter;

      return matchesSearch && matchesStatus && matchesSeverity;
    });
  }

  calculateIssueStats(): void {
    this.issueStats = {
      total: this.operatorIssues.length,
      reported: this.operatorIssues.filter(i => i.status === 'Reported').length,
      critical: this.operatorIssues.filter(i => i.severityLevel === 'Critical').length,
      resolved: this.operatorIssues.filter(i => i.status === 'Resolved' || i.status === 'Closed').length
    };
  }

  onIssueSearchChange(): void {
    this.applyIssueFilters();
  }

  onIssueStatusFilterChange(): void {
    this.applyIssueFilters();
  }

  onIssueSeverityFilterChange(): void {
    this.applyIssueFilters();
  }

  viewIssueDetails(issue: OperatorIssue): void {
    this.selectedIssue = issue;
    this.showIssueDetailsModal = true;
  }

  closeIssueDetailsModal(): void {
    this.showIssueDetailsModal = false;
    this.selectedIssue = null;
  }

  closeIssueReportsModal(): void {
    this.showIssueReportsModal = false;
    this.issueSearchTerm = '';
    this.issueStatusFilter = '';
    this.issueSeverityFilter = '';
  }

  scheduleMaintenanceFromIssue(issue: OperatorIssue): void {
    // Pre-fill the maintenance form with issue details
    const machine = this.availableMachines.find(m => m.id === issue.machineId.toString());
    if (machine) {
      this.maintenanceFormData = {
        machineId: issue.machineId.toString(),
        type: issue.severityLevel === 'Critical' ? 'emergency' : 'corrective',
        priority: issue.severityLevel.toLowerCase(),
        description: `Issue ${issue.ticketId}: ${issue.description}`,
        notes: `Reported by ${issue.operatorName} on ${new Date(issue.reportedAt).toLocaleDateString()}\nAffected Part: ${issue.affectedPart}\nProblem Category: ${issue.problemCategory}`,
        scheduledDate: '',
        scheduledTime: '',
        assignedTechnician: '',
        estimatedDuration: 0,
        cost: 0
      };
    }
    this.closeIssueDetailsModal();
    this.closeIssueReportsModal();
    this.showScheduleModal = true;
  }

  acknowledgeIssue(issue: OperatorIssue): void {
    // In production, this would call the backend API
    issue.status = 'Acknowledged';
    issue.acknowledgedAt = new Date().toISOString();
    this.calculateIssueStats();
    this.applyIssueFilters();
  }

  generateMockIssueData(): OperatorIssue[] {
    const issues: OperatorIssue[] = [
      {
        id: 1,
        ticketId: 'MR-20251030-001',
        machineId: 1,
        machineName: 'Loader CAT-950',
        machineModel: 'CAT-950M',
        operatorId: 101,
        operatorName: 'John Smith',
        affectedPart: 'Hydraulic System',
        problemCategory: 'Hydraulic Problems',
        description: 'Hydraulic fluid leaking from main cylinder, pressure dropping during operation',
        severityLevel: 'Critical',
        status: 'Reported',
        reportedAt: new Date(Date.now() - 2 * 60 * 60 * 1000).toISOString(),
        symptoms: ['Fluid Leaks', 'Loss of Power', 'Unusual Noise']
      },
      {
        id: 2,
        ticketId: 'MR-20251030-002',
        machineId: 2,
        machineName: 'Excavator EX-200',
        machineModel: 'EX-200LC',
        operatorId: 102,
        operatorName: 'Sarah Johnson',
        affectedPart: 'Engine',
        problemCategory: 'Engine Issues',
        description: 'Engine overheating during extended operation, temperature gauge reading high',
        severityLevel: 'High',
        status: 'Acknowledged',
        reportedAt: new Date(Date.now() - 5 * 60 * 60 * 1000).toISOString(),
        acknowledgedAt: new Date(Date.now() - 4 * 60 * 60 * 1000).toISOString(),
        symptoms: ['Overheating', 'Performance Issues']
      },
      {
        id: 3,
        ticketId: 'MR-20251029-015',
        machineId: 3,
        machineName: 'Bulldozer D8T',
        machineModel: 'D8T',
        operatorId: 103,
        operatorName: 'Mike Williams',
        affectedPart: 'Drill Bit',
        problemCategory: 'Drill Bit Issues',
        description: 'Excessive wear on drill bit, reduced penetration rate',
        severityLevel: 'Medium',
        status: 'InProgress',
        reportedAt: new Date(Date.now() - 24 * 60 * 60 * 1000).toISOString(),
        acknowledgedAt: new Date(Date.now() - 23 * 60 * 60 * 1000).toISOString(),
        symptoms: ['Excessive Wear', 'Vibration']
      },
      {
        id: 4,
        ticketId: 'MR-20251029-008',
        machineId: 4,
        machineName: 'Crane Liebherr-LTM',
        machineModel: 'LTM-1100',
        operatorId: 104,
        operatorName: 'Emily Davis',
        affectedPart: 'Electrical System',
        problemCategory: 'Electrical Faults',
        description: 'Intermittent electrical failure, dashboard lights flickering',
        severityLevel: 'High',
        status: 'Reported',
        reportedAt: new Date(Date.now() - 18 * 60 * 60 * 1000).toISOString(),
        symptoms: ['Intermittent Failure', 'Warning Lights On'],
        errorCodes: 'P0562, U0100'
      },
      {
        id: 5,
        ticketId: 'MR-20251028-022',
        machineId: 5,
        machineName: 'Dump Truck DT-450',
        machineModel: 'DT-450X',
        operatorId: 105,
        operatorName: 'Robert Brown',
        affectedPart: 'Mechanical Components',
        problemCategory: 'Mechanical Breakdown',
        description: 'Transmission slipping between gears, difficulty shifting',
        severityLevel: 'Critical',
        status: 'Acknowledged',
        reportedAt: new Date(Date.now() - 48 * 60 * 60 * 1000).toISOString(),
        acknowledgedAt: new Date(Date.now() - 46 * 60 * 60 * 1000).toISOString(),
        symptoms: ['Slipping', 'Difficulty Shifting']
      },
      {
        id: 6,
        ticketId: 'MR-20251027-010',
        machineId: 1,
        machineName: 'Loader CAT-950',
        machineModel: 'CAT-950M',
        operatorId: 101,
        operatorName: 'John Smith',
        affectedPart: 'Drill Rod',
        problemCategory: 'Drill Rod Problems',
        description: 'Drill rod showing signs of metal fatigue, needs inspection',
        severityLevel: 'Low',
        status: 'Resolved',
        reportedAt: new Date(Date.now() - 72 * 60 * 60 * 1000).toISOString(),
        acknowledgedAt: new Date(Date.now() - 70 * 60 * 60 * 1000).toISOString(),
        symptoms: ['Excessive Wear', 'Vibration']
      }
    ];
    return issues;
  }
}