import { Component, OnInit, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MaintenanceService } from '../maintenance/services/maintenance.service';
import {
  MaintenanceJob,
  MaintenanceType,
  MaintenanceStatus
} from '../maintenance/models/maintenance.models';

// Report Interfaces
interface MaintenanceSummaryData {
  totalJobs: number;
  byType: {
    preventive: number;
    corrective: number;
    predictive: number;
    emergency: number;
  };
  byStatus: {
    scheduled: number;
    inProgress: number;
    completed: number;
    overdue: number;
  };
  avgCompletionTime: number;
  onTimeCompletion: number;
}

interface PartsUsageData {
  partName: string;
  quantity: number;
  cost: number;
  machines: string[];
  trend: 'up' | 'down' | 'stable';
}

interface MachinePerformanceData {
  machineId: string;
  machineName: string;
  uptime: number;
  downtime: number;
  mtbf: number; // Mean Time Between Failures
  mttr: number; // Mean Time To Repair
  efficiency: number;
}

interface CostAnalysisData {
  totalCost: number;
  laborCost: number;
  partsCost: number;
  byType: {
    preventive: number;
    corrective: number;
    emergency: number;
  };
  costPerMachine: number;
  trend: number; // percentage change
}

@Component({
  selector: 'app-reports',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './reports.component.html',
  styleUrl: './reports.component.scss'
})
export class ReportsComponent implements OnInit {
  // State signals
  isLoading = signal(false);
  error = signal<string | null>(null);
  activeTab = signal<'summary' | 'performance' | 'parts'>('summary');

  // Data signals
  maintenanceJobs = signal<MaintenanceJob[]>([]);

  // Computed reports data
  maintenanceSummary = computed<MaintenanceSummaryData>(() => {
    const jobs = this.maintenanceJobs();
    if (!jobs.length) {
      return {
        totalJobs: 0,
        byType: { preventive: 0, corrective: 0, predictive: 0, emergency: 0 },
        byStatus: { scheduled: 0, inProgress: 0, completed: 0, overdue: 0 },
        avgCompletionTime: 0,
        onTimeCompletion: 0
      };
    }

    const byType = {
      preventive: jobs.filter(j => j.type === MaintenanceType.PREVENTIVE).length,
      corrective: jobs.filter(j => j.type === MaintenanceType.CORRECTIVE).length,
      predictive: jobs.filter(j => j.type === MaintenanceType.PREDICTIVE).length,
      emergency: jobs.filter(j => j.type === MaintenanceType.EMERGENCY).length
    };

    const byStatus = {
      scheduled: jobs.filter(j => j.status === MaintenanceStatus.SCHEDULED).length,
      inProgress: jobs.filter(j => j.status === MaintenanceStatus.IN_PROGRESS).length,
      completed: jobs.filter(j => j.status === MaintenanceStatus.COMPLETED).length,
      overdue: jobs.filter(j => j.status === MaintenanceStatus.OVERDUE).length
    };

    const completedJobs = jobs.filter(j => j.status === MaintenanceStatus.COMPLETED);
    const avgTime = completedJobs.reduce((sum, j) => sum + (j.actualHours || 0), 0) / (completedJobs.length || 1);

    const onTime = completedJobs.filter(j => {
      if (!j.completedDate || !j.scheduledDate) return false;
      return new Date(j.completedDate) <= new Date(j.scheduledDate);
    }).length;

    const onTimeRate = completedJobs.length > 0 ? (onTime / completedJobs.length) * 100 : 0;

    return {
      totalJobs: jobs.length,
      byType,
      byStatus,
      avgCompletionTime: Math.round(avgTime * 10) / 10,
      onTimeCompletion: Math.round(onTimeRate)
    };
  });

  // Computed: Machine Performance Data
  machinePerformanceData = computed<MachinePerformanceData[]>(() => {
    const jobs = this.maintenanceJobs();
    const machineMap = new Map<number, MachinePerformanceData>();

    jobs.forEach(job => {
      if (!machineMap.has(job.machineId)) {
        machineMap.set(job.machineId, {
          machineId: job.machineId.toString(),
          machineName: job.machineName,
          uptime: 0,
          downtime: 0,
          mtbf: 0,
          mttr: 0,
          efficiency: 0
        });
      }

      const machine = machineMap.get(job.machineId)!;

      // Calculate downtime from completed jobs
      if (job.status === MaintenanceStatus.COMPLETED && job.actualHours) {
        machine.downtime += job.actualHours;
      }
    });

    // Mock calculations for uptime, MTBF, MTTR, efficiency
    machineMap.forEach((machine) => {
      const totalHours = 720; // 30 days * 24 hours
      machine.uptime = totalHours - machine.downtime;
      machine.efficiency = machine.uptime > 0 ? Math.round((machine.uptime / totalHours) * 100) : 0;

      const machineJobs = jobs.filter(j => j.machineId.toString() === machine.machineId);
      const failures = machineJobs.filter(j =>
        j.type === MaintenanceType.CORRECTIVE || j.type === MaintenanceType.EMERGENCY
      ).length;

      machine.mtbf = failures > 0 ? Math.round(machine.uptime / failures) : machine.uptime;
      machine.mttr = failures > 0 ? Math.round(machine.downtime / failures) : 0;
    });

    return Array.from(machineMap.values());
  });

  // Computed: Parts Usage Data
  partsUsageData = computed<PartsUsageData[]>(() => {
    const jobs = this.maintenanceJobs();
    const partsMap = new Map<string, PartsUsageData>();

    jobs.forEach(job => {
      if (job.partsReplaced && job.partsReplaced.length > 0) {
        job.partsReplaced.forEach(part => {
          if (!partsMap.has(part)) {
            partsMap.set(part, {
              partName: part,
              quantity: 0,
              cost: 0,
              machines: [],
              trend: 'stable' as 'up' | 'down' | 'stable'
            });
          }

          const partData = partsMap.get(part)!;
          partData.quantity += 1;
          partData.cost += 150; // Mock cost per part
          if (!partData.machines.includes(job.machineName)) {
            partData.machines.push(job.machineName);
          }
        });
      }
    });

    return Array.from(partsMap.values()).slice(0, 10); // Top 10 parts
  });

  // Computed: Cost Analysis Data
  costAnalysisData = computed<CostAnalysisData>(() => {
    const jobs = this.maintenanceJobs();

    const byType = {
      preventive: 0,
      corrective: 0,
      emergency: 0
    };

    let totalCost = 0;
    let laborCost = 0;
    let partsCost = 0;

    jobs.forEach(job => {
      const jobCost = (job.actualHours || job.estimatedHours) * 75; // $75/hour labor
      totalCost += jobCost;
      laborCost += jobCost;

      if (job.partsReplaced) {
        const partsTotal = job.partsReplaced.length * 150; // $150 per part
        partsCost += partsTotal;
        totalCost += partsTotal;
      }

      if (job.type === MaintenanceType.PREVENTIVE) {
        byType.preventive += jobCost;
      } else if (job.type === MaintenanceType.CORRECTIVE) {
        byType.corrective += jobCost;
      } else if (job.type === MaintenanceType.EMERGENCY) {
        byType.emergency += jobCost;
      }
    });

    const uniqueMachines = new Set(jobs.map(j => j.machineId)).size;
    const costPerMachine = uniqueMachines > 0 ? totalCost / uniqueMachines : 0;

    return {
      totalCost,
      laborCost,
      partsCost,
      byType,
      costPerMachine: Math.round(costPerMachine),
      trend: 5.2 // Mock trend percentage
    };
  });

  constructor(private maintenanceService: MaintenanceService) {}

  ngOnInit() {
    this.loadMaintenanceData();
  }

  async loadMaintenanceData() {
    this.isLoading.set(true);
    this.error.set(null);

    try {
      // Load maintenance jobs
      this.maintenanceService.getMaintenanceJobs().subscribe({
        next: (jobs: MaintenanceJob[]) => {
          this.maintenanceJobs.set(jobs);
          this.isLoading.set(false);
        },
        error: (err: any) => {
          console.error('Error loading maintenance data:', err);
          this.error.set('Failed to load maintenance data. Using sample data.');
          this.loadMockData();
          this.isLoading.set(false);
        }
      });
    } catch (err: any) {
      console.error('Error:', err);
      this.error.set(err.message);
      this.loadMockData();
      this.isLoading.set(false);
    }
  }

  private loadMockData() {
    const mockJobs: MaintenanceJob[] = [
      {
        id: 1,
        machineId: 101,
        machineName: 'Excavator EX-001',
        machineModel: 'CAT 320D',
        scheduledDate: new Date('2024-10-01'),
        completedDate: new Date('2024-10-02'),
        type: MaintenanceType.PREVENTIVE,
        status: MaintenanceStatus.COMPLETED,
        estimatedHours: 8,
        actualHours: 7.5,
        reason: 'Regular scheduled maintenance'
      },
      {
        id: 2,
        machineId: 102,
        machineName: 'Drill Rig DR-002',
        machineModel: 'Atlas Copco ROC',
        scheduledDate: new Date('2024-10-15'),
        type: MaintenanceType.CORRECTIVE,
        status: MaintenanceStatus.IN_PROGRESS,
        estimatedHours: 12,
        reason: 'Hydraulic system malfunction'
      },
      {
        id: 3,
        machineId: 103,
        machineName: 'Loader LD-003',
        machineModel: 'Volvo L120',
        scheduledDate: new Date('2024-09-25'),
        type: MaintenanceType.PREVENTIVE,
        status: MaintenanceStatus.OVERDUE,
        estimatedHours: 6,
        reason: 'Scheduled service overdue'
      },
      {
        id: 4,
        machineId: 104,
        machineName: 'Crusher CR-004',
        machineModel: 'Metso HP300',
        scheduledDate: new Date('2024-10-20'),
        type: MaintenanceType.PREVENTIVE,
        status: MaintenanceStatus.SCHEDULED,
        estimatedHours: 10,
        reason: 'Quarterly maintenance'
      },
      {
        id: 5,
        machineId: 101,
        machineName: 'Excavator EX-001',
        machineModel: 'CAT 320D',
        scheduledDate: new Date('2024-09-15'),
        completedDate: new Date('2024-09-15'),
        type: MaintenanceType.EMERGENCY,
        status: MaintenanceStatus.COMPLETED,
        estimatedHours: 4,
        actualHours: 6,
        reason: 'Engine failure - emergency repair'
      }
    ];

    this.maintenanceJobs.set(mockJobs);
  }

  exportAllReports() {
    console.log('Exporting all maintenance reports...');
    alert('Export functionality will be implemented in the next phase.\n\nThis will generate a comprehensive PDF/Excel report with:\n- Maintenance Summary\n- Machine Performance Metrics\n- Parts Usage Analysis\n- Cost Breakdown\n- Compliance Data\n- Downtime Analysis');
  }

  printAllReports() {
    console.log('Printing all maintenance reports...');
    window.print();
  }

  refreshReports() {
    this.loadMaintenanceData();
  }

  switchTab(tab: 'summary' | 'performance' | 'parts') {
    this.activeTab.set(tab);
  }

  // Helper methods
  formatCurrency(amount: number): string {
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'USD'
    }).format(amount);
  }

  formatNumber(num: number): string {
    return new Intl.NumberFormat('en-US').format(num);
  }

  getTypeColor(type: MaintenanceType): string {
    switch (type) {
      case MaintenanceType.PREVENTIVE:
        return '#10b981';
      case MaintenanceType.CORRECTIVE:
        return '#f59e0b';
      case MaintenanceType.PREDICTIVE:
        return '#3b82f6';
      case MaintenanceType.EMERGENCY:
        return '#ef4444';
      default:
        return '#6b7280';
    }
  }

  getStatusColor(status: MaintenanceStatus): string {
    switch (status) {
      case MaintenanceStatus.COMPLETED:
        return '#10b981';
      case MaintenanceStatus.IN_PROGRESS:
        return '#3b82f6';
      case MaintenanceStatus.SCHEDULED:
        return '#8b5cf6';
      case MaintenanceStatus.OVERDUE:
        return '#ef4444';
      default:
        return '#6b7280';
    }
  }

  formatDate(date: Date): string {
    return new Date(date).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric'
    });
  }

  // Enum getters for template
  get MaintenanceType() {
    return MaintenanceType;
  }

  get MaintenanceStatus() {
    return MaintenanceStatus;
  }
}
