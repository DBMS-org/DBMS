import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ProjectService } from '../../../core/services/project.service';
import { Project, ProjectSite } from '../../../core/models/project.model';

// ==========================================
// COMPREHENSIVE ENTERPRISE REPORT INTERFACES
// ==========================================

interface ProjectReport {
  project: Project;
  sites: SiteReportInfo[];
  machineAssignments: MachineAssignmentInfo[];
  machineRequests: MachineRequestInfo[];
  personnel: PersonnelInfo[];
  materialUsage: MaterialUsageInfo[];
  explosiveRequests: ExplosiveRequestInfo[];
  inventoryTransfers: InventoryTransferInfo[];
  maintenanceJobs: MaintenanceJobInfo[];
  problemReports: ProblemReportInfo[];
  usageLogs: UsageLogInfo[];
  drillingOperations: DrillingOperationsInfo;
  blastSequences: BlastSequenceInfo[];
  financials: FinancialSummary;
  performance: PerformanceMetrics;
  statistics: ProjectStatistics;
}

// Site Information with Drilling Data
interface SiteReportInfo extends ProjectSite {
  totalDrillHoles: number;
  completedHoles: number;
  pendingHoles: number;
  totalExplosivesCalculated: {
    anfo: number;
    emulsion: number;
  };
  hasBlastSequence: boolean;
  isSimulationCompleted: boolean;
}

// Machine Assignment Information
interface MachineAssignmentInfo {
  machineId: number;
  machineName: string;
  machineType: string;
  serialNumber: string;
  operatorName: string;
  assignedDate: Date;
  expectedReturnDate?: Date;
  status: string;
  location: string;
  utilizationHours?: number;
}

// Machine Assignment Requests
interface MachineRequestInfo {
  requestId: number;
  machineType: string;
  quantityRequested: number;
  quantityFulfilled: number;
  requestedBy: string;
  requestDate: Date;
  status: string;
  urgency: string;
  approvedBy?: string;
  approvalDate?: Date;
}

// Personnel Information
interface PersonnelInfo {
  userId: number;
  name: string;
  role: string;
  email: string;
  assignedDate?: Date;
  tasksCompleted?: number;
  currentStatus: string;
}

// Material Usage (Explosives & Accessories)
interface MaterialUsageInfo {
  materialType: string;
  category: 'Explosive' | 'Accessory' | 'Fuel' | 'Parts';
  quantity: number;
  unit: string;
  site: string;
  costEstimate?: number;
  usageDate?: Date;
}

// Explosive Requests from Blasting Engineers
interface ExplosiveRequestInfo {
  requestId: number;
  explosiveType: string;
  quantityRequested: number;
  quantityApproved: number;
  unit: string;
  requestedBy: string;
  requestDate: Date;
  requiredDate: Date;
  status: string;
  approvedBy?: string;
  dispatchDate?: Date;
  purpose: string;
}

// Inventory Transfers (Store to Store)
interface InventoryTransferInfo {
  transferId: number;
  batchId: string;
  explosiveType: string;
  quantity: number;
  unit: string;
  fromStore: string;
  toStore: string;
  requestDate: Date;
  dispatchDate?: Date;
  completedDate?: Date;
  status: string;
  truckNumber?: string;
}

// Maintenance Jobs
interface MaintenanceJobInfo {
  jobId: number;
  machineId: number;
  machineName: string;
  type: string; // PREVENTIVE, CORRECTIVE, EMERGENCY
  scheduledDate: Date;
  completedDate?: Date;
  status: string;
  assignedTo: string;
  estimatedHours: number;
  actualHours?: number;
  cost?: number;
  partsReplaced?: string[];
}

// Problem Reports from Operators
interface ProblemReportInfo {
  reportId: number;
  machineId: number;
  machineName: string;
  operatorName: string;
  affectedPart: string;
  problemCategory: string;
  severity: string;
  status: string;
  reportedAt: Date;
  resolvedAt?: Date;
  resolutionTime?: number; // hours
}

// Machine Usage Logs
interface UsageLogInfo {
  logId: number;
  machineId: number;
  machineName: string;
  operatorName: string;
  date: Date;
  engineHours: number;
  idleHours: number;
  workingHours: number;
  fuelConsumed: number;
  hasDowntime: boolean;
  downtimeHours?: number;
}

// Drilling Operations Summary
interface DrillingOperationsInfo {
  totalHoles: number;
  completedHoles: number;
  pendingHoles: number;
  totalDepth: number;
  averageDepth: number;
  totalExplosivesRequired: {
    anfo: number;
    emulsion: number;
    detonatingCord: number;
    blastingCaps: number;
  };
}

// Blast Sequences
interface BlastSequenceInfo {
  sequenceId: number;
  siteName: string;
  name: string;
  totalHoles: number;
  totalConnections: number;
  totalDelayTime: number;
  isActive: boolean;
  isSimulated: boolean;
  createdBy: string;
  createdAt: Date;
}

// Financial Summary
interface FinancialSummary {
  explosivesCost: number;
  maintenanceCost: number;
  fuelCost: number;
  partsCost: number;
  laborCost: number;
  totalProjectCost: number;
  budgetStatus: 'Under Budget' | 'On Budget' | 'Over Budget';
  budgetUtilization: number; // percentage
}

// Performance Metrics
interface PerformanceMetrics {
  projectProgress: number; // percentage
  scheduleStatus: 'On Time' | 'Delayed' | 'Ahead';
  daysAhead?: number;
  daysBehind?: number;
  machineUtilization: number; // percentage
  personnelUtilization: number; // percentage
  safetyIncidents: number;
  qualityScore: number; // 0-100
  maintenanceCompliance: number; // percentage
  operationalEfficiency: number; // percentage
}

// Statistics
interface ProjectStatistics {
  totalSites: number;
  activeSites: number;
  completedSites: number;
  totalMachines: number;
  activeMachines: number;
  machinesUnderMaintenance: number;
  totalPersonnel: number;
  activePersonnel: number;
  totalMaterialsUsed: number;
  explosiveRequestsCount: number;
  maintenanceJobsCount: number;
  openProblemReports: number;
  totalDrillingHoles: number;
  completedDrillingHoles: number;
}

@Component({
  selector: 'app-reports',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './reports.component.html',
  styleUrl: './reports.component.scss'
})
export class ReportsComponent implements OnInit {
  projects: Project[] = [];
  projectReports: ProjectReport[] = [];
  filteredReports: ProjectReport[] = [];

  // Filters
  searchQuery: string = '';
  statusFilter: string = '';
  regionFilter: string = '';

  // UI States
  loading = false;
  error: string | null = null;
  selectedReport: ProjectReport | null = null;
  showDetailModal = false;

  // Expanded sections for accordion behavior
  expandedProjectId: number | null = null;

  // Filter options
  statusOptions = ['', 'Active', 'Inactive', 'Completed', 'On Hold', 'Cancelled'];
  regionOptions = ['', 'Muscat', 'Dhofar', 'Musandam', 'Al Buraimi', 'Al Dakhiliyah', 'Al Dhahirah', 'Al Wusta', 'Al Batinah North', 'Al Batinah South', 'Ash Sharqiyah North', 'Ash Sharqiyah South'];

  constructor(private projectService: ProjectService) {}

  ngOnInit() {
    this.loadReports();
  }

  loadReports() {
    this.loading = true;
    this.error = null;

    this.projectService.getProjectsForCurrentUser().subscribe({
      next: (projects) => {
        this.projects = projects;
        this.generateReports(projects);
        this.loading = false;
      },
      error: (error) => {
        this.error = error.message;
        this.loading = false;
        console.error('Error loading projects:', error);
        // Fallback to mock data
        this.loadMockReports();
      }
    });
  }

  private generateReports(projects: Project[]) {
    this.projectReports = projects.map(project => {
      const report: ProjectReport = {
        project: project,
        sites: this.getMockSites(project.id),
        machineAssignments: this.getMockMachineAssignments(project.id),
        machineRequests: this.getMockMachineRequests(project.id),
        personnel: this.getMockPersonnel(project.id),
        materialUsage: this.getMockMaterialUsage(project.id),
        explosiveRequests: this.getMockExplosiveRequests(project.id),
        inventoryTransfers: this.getMockInventoryTransfers(project.id),
        maintenanceJobs: this.getMockMaintenanceJobs(project.id),
        problemReports: this.getMockProblemReports(project.id),
        usageLogs: this.getMockUsageLogs(project.id),
        drillingOperations: this.getMockDrillingOperations(project.id),
        blastSequences: this.getMockBlastSequences(project.id),
        financials: this.calculateFinancials(project.id),
        performance: this.calculatePerformance(project.id, project),
        statistics: this.calculateStatistics(project.id)
      };
      return report;
    });

    this.filteredReports = [...this.projectReports];
  }

  // ==========================================
  // MOCK DATA GENERATORS (TO BE REPLACED WITH API CALLS)
  // ==========================================

  private getMockSites(projectId: number): SiteReportInfo[] {
    return [
      {
        id: projectId * 10 + 1,
        projectId: projectId,
        name: `Site A - Construction Zone`,
        location: 'Primary Location, Muscat',
        coordinates: '23.5859,58.4059',
        description: 'Main construction site with active drilling operations',
        status: 'Active',
        isPatternApproved: true,
        isSimulationConfirmed: true,
        isOperatorCompleted: false,
        isCompleted: false,
        createdAt: new Date('2024-01-15'),
        updatedAt: new Date(),
        totalDrillHoles: 150,
        completedHoles: 120,
        pendingHoles: 30,
        totalExplosivesCalculated: {
          anfo: 750,
          emulsion: 250
        },
        hasBlastSequence: true,
        isSimulationCompleted: true
      },
      {
        id: projectId * 10 + 2,
        projectId: projectId,
        name: `Site B - Storage Area`,
        location: 'Secondary Location',
        coordinates: '23.6145,58.5627',
        description: 'Material storage and secondary drilling site',
        status: 'Active',
        isPatternApproved: true,
        isSimulationConfirmed: false,
        isOperatorCompleted: false,
        isCompleted: false,
        createdAt: new Date('2024-02-01'),
        updatedAt: new Date(),
        totalDrillHoles: 80,
        completedHoles: 45,
        pendingHoles: 35,
        totalExplosivesCalculated: {
          anfo: 400,
          emulsion: 150
        },
        hasBlastSequence: false,
        isSimulationCompleted: false
      }
    ];
  }

  private getMockMachineAssignments(projectId: number): MachineAssignmentInfo[] {
    return [
      {
        machineId: projectId * 100 + 1,
        machineName: `Drill Rig DR-${projectId}01`,
        machineType: 'Drill Rig',
        serialNumber: `DR-SN-${projectId}001`,
        operatorName: 'John Smith',
        assignedDate: new Date('2024-01-20'),
        expectedReturnDate: new Date('2024-06-30'),
        status: 'Active',
        location: 'Site A - Construction Zone',
        utilizationHours: 850
      },
      {
        machineId: projectId * 100 + 2,
        machineName: `Excavator EX-${projectId}02`,
        machineType: 'Excavator',
        serialNumber: `EX-SN-${projectId}002`,
        operatorName: 'Sarah Johnson',
        assignedDate: new Date('2024-01-22'),
        expectedReturnDate: new Date('2024-06-30'),
        status: 'Active',
        location: 'Site B - Storage Area',
        utilizationHours: 720
      },
      {
        machineId: projectId * 100 + 3,
        machineName: `Loader LD-${projectId}03`,
        machineType: 'Loader',
        serialNumber: `LD-SN-${projectId}003`,
        operatorName: 'Ahmed Al-Balushi',
        assignedDate: new Date('2024-01-25'),
        expectedReturnDate: new Date('2024-06-30'),
        status: 'Under Maintenance',
        location: 'Maintenance Facility',
        utilizationHours: 600
      }
    ];
  }

  private getMockMachineRequests(projectId: number): MachineRequestInfo[] {
    return [
      {
        requestId: projectId * 1000 + 1,
        machineType: 'Drill Rig',
        quantityRequested: 3,
        quantityFulfilled: 3,
        requestedBy: 'Project Manager',
        requestDate: new Date('2024-01-10'),
        status: 'Completed',
        urgency: 'High',
        approvedBy: 'Machine Manager',
        approvalDate: new Date('2024-01-12')
      },
      {
        requestId: projectId * 1000 + 2,
        machineType: 'Compressor',
        quantityRequested: 2,
        quantityFulfilled: 1,
        requestedBy: 'Site Supervisor',
        requestDate: new Date('2024-02-15'),
        status: 'Partially Fulfilled',
        urgency: 'Medium',
        approvedBy: 'Machine Manager',
        approvalDate: new Date('2024-02-16')
      }
    ];
  }

  private getMockPersonnel(projectId: number): PersonnelInfo[] {
    return [
      {
        userId: projectId * 1000 + 1,
        name: 'John Smith',
        role: 'Operator',
        email: 'john.smith@example.com',
        assignedDate: new Date('2024-01-15'),
        tasksCompleted: 45,
        currentStatus: 'Active'
      },
      {
        userId: projectId * 1000 + 2,
        name: 'Sarah Johnson',
        role: 'Operator',
        email: 'sarah.johnson@example.com',
        assignedDate: new Date('2024-01-16'),
        tasksCompleted: 38,
        currentStatus: 'Active'
      },
      {
        userId: projectId * 1000 + 3,
        name: 'Ahmed Al-Balushi',
        role: 'Operator',
        email: 'ahmed.balushi@example.com',
        assignedDate: new Date('2024-01-18'),
        tasksCompleted: 32,
        currentStatus: 'On Leave'
      },
      {
        userId: projectId * 1000 + 4,
        name: 'Mohammed Al-Rashdi',
        role: 'Blasting Engineer',
        email: 'mohammed.rashdi@example.com',
        assignedDate: new Date('2024-01-15'),
        tasksCompleted: 12,
        currentStatus: 'Active'
      },
      {
        userId: projectId * 1000 + 5,
        name: 'Fatima Al-Hinai',
        role: 'Mechanical Engineer',
        email: 'fatima.hinai@example.com',
        assignedDate: new Date('2024-01-20'),
        tasksCompleted: 28,
        currentStatus: 'Active'
      }
    ];
  }

  private getMockMaterialUsage(projectId: number): MaterialUsageInfo[] {
    return [
      {
        materialType: 'ANFO',
        category: 'Explosive',
        quantity: 1150,
        unit: 'kg',
        site: 'Site A - Construction Zone',
        costEstimate: 2300,
        usageDate: new Date('2024-03-15')
      },
      {
        materialType: 'Emulsion',
        category: 'Explosive',
        quantity: 400,
        unit: 'kg',
        site: 'Site A - Construction Zone',
        costEstimate: 1600,
        usageDate: new Date('2024-03-16')
      },
      {
        materialType: 'Detonating Cord',
        category: 'Explosive',
        quantity: 2500,
        unit: 'm',
        site: 'All Sites',
        costEstimate: 1250,
        usageDate: new Date('2024-03-10')
      },
      {
        materialType: 'Blasting Caps',
        category: 'Explosive',
        quantity: 450,
        unit: 'pieces',
        site: 'All Sites',
        costEstimate: 900,
        usageDate: new Date('2024-03-10')
      },
      {
        materialType: 'Diesel Fuel',
        category: 'Fuel',
        quantity: 5000,
        unit: 'liters',
        site: 'All Sites',
        costEstimate: 3500,
        usageDate: new Date('2024-03-01')
      },
      {
        materialType: 'Drill Bits',
        category: 'Parts',
        quantity: 24,
        unit: 'pieces',
        site: 'Maintenance',
        costEstimate: 4800,
        usageDate: new Date('2024-02-20')
      }
    ];
  }

  private getMockExplosiveRequests(projectId: number): ExplosiveRequestInfo[] {
    return [
      {
        requestId: projectId * 2000 + 1,
        explosiveType: 'ANFO',
        quantityRequested: 1200,
        quantityApproved: 1200,
        unit: 'kg',
        requestedBy: 'Mohammed Al-Rashdi',
        requestDate: new Date('2024-03-01'),
        requiredDate: new Date('2024-03-15'),
        status: 'Dispatched',
        approvedBy: 'Explosive Manager',
        dispatchDate: new Date('2024-03-10'),
        purpose: 'Site A drilling operations'
      },
      {
        requestId: projectId * 2000 + 2,
        explosiveType: 'Emulsion',
        quantityRequested: 500,
        quantityApproved: 400,
        unit: 'kg',
        requestedBy: 'Mohammed Al-Rashdi',
        requestDate: new Date('2024-03-05'),
        requiredDate: new Date('2024-03-20'),
        status: 'Approved',
        approvedBy: 'Explosive Manager',
        purpose: 'Site B drilling operations'
      },
      {
        requestId: projectId * 2000 + 3,
        explosiveType: 'Detonating Cord',
        quantityRequested: 3000,
        quantityApproved: 2500,
        unit: 'm',
        requestedBy: 'Mohammed Al-Rashdi',
        requestDate: new Date('2024-02-28'),
        requiredDate: new Date('2024-03-10'),
        status: 'Completed',
        approvedBy: 'Explosive Manager',
        dispatchDate: new Date('2024-03-08'),
        purpose: 'Blast sequence connections'
      }
    ];
  }

  private getMockInventoryTransfers(projectId: number): InventoryTransferInfo[] {
    return [
      {
        transferId: projectId * 3000 + 1,
        batchId: `ANFO-BATCH-${projectId}-001`,
        explosiveType: 'ANFO',
        quantity: 1200,
        unit: 'kg',
        fromStore: 'Central Warehouse',
        toStore: 'Regional Store - Muscat',
        requestDate: new Date('2024-03-01'),
        dispatchDate: new Date('2024-03-08'),
        completedDate: new Date('2024-03-09'),
        status: 'Completed',
        truckNumber: 'TRK-001'
      },
      {
        transferId: projectId * 3000 + 2,
        batchId: `EMUL-BATCH-${projectId}-002`,
        explosiveType: 'Emulsion',
        quantity: 400,
        unit: 'kg',
        fromStore: 'Central Warehouse',
        toStore: 'Regional Store - Muscat',
        requestDate: new Date('2024-03-05'),
        dispatchDate: new Date('2024-03-12'),
        status: 'In Progress',
        truckNumber: 'TRK-002'
      }
    ];
  }

  private getMockMaintenanceJobs(projectId: number): MaintenanceJobInfo[] {
    return [
      {
        jobId: projectId * 4000 + 1,
        machineId: projectId * 100 + 1,
        machineName: `Drill Rig DR-${projectId}01`,
        type: 'PREVENTIVE',
        scheduledDate: new Date('2024-03-01'),
        completedDate: new Date('2024-03-02'),
        status: 'Completed',
        assignedTo: 'Fatima Al-Hinai',
        estimatedHours: 8,
        actualHours: 7.5,
        cost: 1500,
        partsReplaced: ['Oil Filter', 'Air Filter', 'Hydraulic Fluid']
      },
      {
        jobId: projectId * 4000 + 2,
        machineId: projectId * 100 + 3,
        machineName: `Loader LD-${projectId}03`,
        type: 'CORRECTIVE',
        scheduledDate: new Date('2024-03-15'),
        status: 'In Progress',
        assignedTo: 'Maintenance Team B',
        estimatedHours: 16,
        cost: 3500,
        partsReplaced: ['Transmission Component']
      },
      {
        jobId: projectId * 4000 + 3,
        machineId: projectId * 100 + 2,
        machineName: `Excavator EX-${projectId}02`,
        type: 'PREVENTIVE',
        scheduledDate: new Date('2024-03-25'),
        status: 'Scheduled',
        assignedTo: 'Maintenance Team A',
        estimatedHours: 6
      }
    ];
  }

  private getMockProblemReports(projectId: number): ProblemReportInfo[] {
    return [
      {
        reportId: projectId * 5000 + 1,
        machineId: projectId * 100 + 3,
        machineName: `Loader LD-${projectId}03`,
        operatorName: 'Ahmed Al-Balushi',
        affectedPart: 'TRANSMISSION',
        problemCategory: 'MECHANICAL_BREAKDOWN',
        severity: 'HIGH',
        status: 'In Progress',
        reportedAt: new Date('2024-03-14'),
        resolutionTime: 24
      },
      {
        reportId: projectId * 5000 + 2,
        machineId: projectId * 100 + 1,
        machineName: `Drill Rig DR-${projectId}01`,
        operatorName: 'John Smith',
        affectedPart: 'HYDRAULIC_SYSTEM',
        problemCategory: 'HYDRAULIC_PROBLEMS',
        severity: 'MEDIUM',
        status: 'Resolved',
        reportedAt: new Date('2024-02-28'),
        resolvedAt: new Date('2024-03-01'),
        resolutionTime: 48
      }
    ];
  }

  private getMockUsageLogs(projectId: number): UsageLogInfo[] {
    const logs: UsageLogInfo[] = [];
    const machines = this.getMockMachineAssignments(projectId);

    // Generate last 7 days of logs for each machine
    for (let i = 0; i < 7; i++) {
      const date = new Date();
      date.setDate(date.getDate() - i);

      machines.forEach((machine, idx) => {
        logs.push({
          logId: projectId * 6000 + (i * 10) + idx,
          machineId: machine.machineId,
          machineName: machine.machineName,
          operatorName: machine.operatorName,
          date: date,
          engineHours: 8 + Math.random() * 4,
          idleHours: 1 + Math.random() * 2,
          workingHours: 6 + Math.random() * 3,
          fuelConsumed: 50 + Math.random() * 30,
          hasDowntime: Math.random() > 0.8,
          downtimeHours: Math.random() > 0.8 ? Math.random() * 2 : undefined
        });
      });
    }

    return logs;
  }

  private getMockDrillingOperations(projectId: number): DrillingOperationsInfo {
    const sites = this.getMockSites(projectId);
    const totalHoles = sites.reduce((sum, site) => sum + site.totalDrillHoles, 0);
    const completedHoles = sites.reduce((sum, site) => sum + site.completedHoles, 0);
    const totalAnfo = sites.reduce((sum, site) => sum + site.totalExplosivesCalculated.anfo, 0);
    const totalEmulsion = sites.reduce((sum, site) => sum + site.totalExplosivesCalculated.emulsion, 0);

    return {
      totalHoles: totalHoles,
      completedHoles: completedHoles,
      pendingHoles: totalHoles - completedHoles,
      totalDepth: totalHoles * 12.5, // Average 12.5m per hole
      averageDepth: 12.5,
      totalExplosivesRequired: {
        anfo: totalAnfo,
        emulsion: totalEmulsion,
        detonatingCord: totalHoles * 15, // 15m per hole
        blastingCaps: totalHoles * 2 // 2 caps per hole
      }
    };
  }

  private getMockBlastSequences(projectId: number): BlastSequenceInfo[] {
    return [
      {
        sequenceId: projectId * 7000 + 1,
        siteName: 'Site A - Construction Zone',
        name: 'Primary Blast Pattern A1',
        totalHoles: 150,
        totalConnections: 149,
        totalDelayTime: 4500, // milliseconds
        isActive: true,
        isSimulated: true,
        createdBy: 'Mohammed Al-Rashdi',
        createdAt: new Date('2024-02-15')
      },
      {
        sequenceId: projectId * 7000 + 2,
        siteName: 'Site A - Construction Zone',
        name: 'Secondary Blast Pattern A2',
        totalHoles: 80,
        totalConnections: 79,
        totalDelayTime: 2400,
        isActive: false,
        isSimulated: true,
        createdBy: 'Mohammed Al-Rashdi',
        createdAt: new Date('2024-02-20')
      }
    ];
  }

  private calculateFinancials(projectId: number): FinancialSummary {
    const materials = this.getMockMaterialUsage(projectId);
    const maintenance = this.getMockMaintenanceJobs(projectId);

    const explosivesCost = materials
      .filter(m => m.category === 'Explosive')
      .reduce((sum, m) => sum + (m.costEstimate || 0), 0);

    const maintenanceCost = maintenance
      .reduce((sum, m) => sum + (m.cost || 0), 0);

    const fuelCost = materials
      .filter(m => m.category === 'Fuel')
      .reduce((sum, m) => sum + (m.costEstimate || 0), 0);

    const partsCost = materials
      .filter(m => m.category === 'Parts')
      .reduce((sum, m) => sum + (m.costEstimate || 0), 0);

    const laborCost = 150000; // Mock labor cost
    const totalProjectCost = explosivesCost + maintenanceCost + fuelCost + partsCost + laborCost;
    const budget = 200000; // Mock budget
    const utilization = (totalProjectCost / budget) * 100;

    return {
      explosivesCost,
      maintenanceCost,
      fuelCost,
      partsCost,
      laborCost,
      totalProjectCost,
      budgetStatus: utilization < 90 ? 'Under Budget' : utilization < 100 ? 'On Budget' : 'Over Budget',
      budgetUtilization: utilization
    };
  }

  private calculatePerformance(projectId: number, project: Project): PerformanceMetrics {
    const drilling = this.getMockDrillingOperations(projectId);
    const machines = this.getMockMachineAssignments(projectId);
    const personnel = this.getMockPersonnel(projectId);
    const maintenance = this.getMockMaintenanceJobs(projectId);
    const problemReports = this.getMockProblemReports(projectId);

    const progress = (drilling.completedHoles / drilling.totalHoles) * 100;

    // Calculate schedule status
    const today = new Date();
    const start = project.startDate || today;
    const end = project.endDate || today;
    const totalDays = Math.ceil((end.getTime() - start.getTime()) / (1000 * 60 * 60 * 24));
    const elapsedDays = Math.ceil((today.getTime() - start.getTime()) / (1000 * 60 * 60 * 24));
    const expectedProgress = (elapsedDays / totalDays) * 100;
    const scheduleVariance = progress - expectedProgress;

    // Machine utilization
    const activeMachines = machines.filter(m => m.status === 'Active').length;
    const machineUtilization = (activeMachines / machines.length) * 100;

    // Personnel utilization
    const activePersonnel = personnel.filter(p => p.currentStatus === 'Active').length;
    const personnelUtilization = (activePersonnel / personnel.length) * 100;

    // Maintenance compliance
    const completedMaintenance = maintenance.filter(m => m.status === 'Completed').length;
    const maintenanceCompliance = maintenance.length > 0 ? (completedMaintenance / maintenance.length) * 100 : 100;

    return {
      projectProgress: Math.round(progress),
      scheduleStatus: scheduleVariance > 5 ? 'Ahead' : scheduleVariance < -5 ? 'Delayed' : 'On Time',
      daysAhead: scheduleVariance > 5 ? Math.round(scheduleVariance / 100 * totalDays) : undefined,
      daysBehind: scheduleVariance < -5 ? Math.round(Math.abs(scheduleVariance) / 100 * totalDays) : undefined,
      machineUtilization: Math.round(machineUtilization),
      personnelUtilization: Math.round(personnelUtilization),
      safetyIncidents: problemReports.filter(p => p.severity === 'HIGH').length,
      qualityScore: 85, // Mock quality score
      maintenanceCompliance: Math.round(maintenanceCompliance),
      operationalEfficiency: Math.round((machineUtilization + personnelUtilization) / 2)
    };
  }

  private calculateStatistics(projectId: number): ProjectStatistics {
    const sites = this.getMockSites(projectId);
    const machines = this.getMockMachineAssignments(projectId);
    const personnel = this.getMockPersonnel(projectId);
    const materials = this.getMockMaterialUsage(projectId);
    const explosiveRequests = this.getMockExplosiveRequests(projectId);
    const maintenance = this.getMockMaintenanceJobs(projectId);
    const problemReports = this.getMockProblemReports(projectId);
    const drilling = this.getMockDrillingOperations(projectId);

    return {
      totalSites: sites.length,
      activeSites: sites.filter(s => s.status === 'Active').length,
      completedSites: sites.filter(s => s.isCompleted).length,
      totalMachines: machines.length,
      activeMachines: machines.filter(m => m.status === 'Active').length,
      machinesUnderMaintenance: machines.filter(m => m.status === 'Under Maintenance').length,
      totalPersonnel: personnel.length,
      activePersonnel: personnel.filter(p => p.currentStatus === 'Active').length,
      totalMaterialsUsed: materials.length,
      explosiveRequestsCount: explosiveRequests.length,
      maintenanceJobsCount: maintenance.length,
      openProblemReports: problemReports.filter(p => p.status !== 'Resolved').length,
      totalDrillingHoles: drilling.totalHoles,
      completedDrillingHoles: drilling.completedHoles
    };
  }

  private loadMockReports() {
    // Fallback mock data
    const mockProjects: Project[] = [
      {
        id: 1,
        name: 'Project Alpha - Muttrah Construction',
        region: 'Muscat',
        status: 'Active',
        description: 'Large-scale construction project involving multiple drilling sites and blast operations',
        startDate: new Date('2024-01-15'),
        endDate: new Date('2024-12-31'),
        assignedUserId: 1,
        createdAt: new Date('2024-01-01'),
        updatedAt: new Date()
      },
      {
        id: 2,
        name: 'Project Beta - Salalah Infrastructure',
        region: 'Dhofar',
        status: 'Active',
        description: 'Infrastructure development project with extensive rock excavation',
        startDate: new Date('2024-02-01'),
        endDate: new Date('2025-01-31'),
        assignedUserId: 2,
        createdAt: new Date('2024-01-15'),
        updatedAt: new Date()
      }
    ];

    this.projects = mockProjects;
    this.generateReports(mockProjects);
  }

  onSearch() {
    this.applyFilters();
  }

  onFilter() {
    this.applyFilters();
  }

  private applyFilters() {
    let filtered = [...this.projectReports];

    // Apply search filter
    if (this.searchQuery.trim()) {
      const query = this.searchQuery.toLowerCase().trim();
      filtered = filtered.filter(report =>
        report.project.name?.toLowerCase().includes(query) ||
        report.project.region.toLowerCase().includes(query) ||
        report.project.id.toString().includes(query)
      );
    }

    // Apply status filter
    if (this.statusFilter) {
      filtered = filtered.filter(report => report.project.status === this.statusFilter);
    }

    // Apply region filter
    if (this.regionFilter) {
      filtered = filtered.filter(report => report.project.region === this.regionFilter);
    }

    this.filteredReports = filtered;
  }

  toggleProjectExpansion(projectId: number) {
    this.expandedProjectId = this.expandedProjectId === projectId ? null : projectId;
  }

  isProjectExpanded(projectId: number): boolean {
    return this.expandedProjectId === projectId;
  }

  viewDetailedReport(report: ProjectReport) {
    this.selectedReport = report;
    this.showDetailModal = true;
  }

  closeDetailModal() {
    this.showDetailModal = false;
    this.selectedReport = null;
  }

  exportReport(report: ProjectReport) {
    // Generate CSV or PDF export
    console.log('Exporting comprehensive report for:', report.project.name);
    // TODO: Implement actual export functionality with all enterprise data
    alert(`Export functionality will be implemented in the backend integration phase.\n\nReport: ${report.project.name}\n\nThis will include:\n- All drilling operations data\n- Complete machine assignments and usage logs\n- Maintenance history and costs\n- Explosive requests and transfers\n- Financial summary\n- Performance metrics`);
  }

  printReport(report: ProjectReport) {
    // Open print dialog
    console.log('Printing comprehensive report for:', report.project.name);
    window.print();
  }

  getStatusClass(status: string): string {
    switch (status.toLowerCase()) {
      case 'active':
        return 'status-active';
      case 'inactive':
        return 'status-inactive';
      case 'completed':
        return 'status-completed';
      case 'on hold':
        return 'status-hold';
      case 'cancelled':
        return 'status-cancelled';
      default:
        return 'status-default';
    }
  }

  getSeverityClass(severity: string): string {
    switch (severity.toLowerCase()) {
      case 'critical':
      case 'high':
        return 'severity-high';
      case 'medium':
        return 'severity-medium';
      case 'low':
        return 'severity-low';
      default:
        return 'severity-default';
    }
  }

  getScheduleStatusClass(status: string): string {
    switch (status) {
      case 'Ahead':
        return 'schedule-ahead';
      case 'On Time':
        return 'schedule-ontime';
      case 'Delayed':
        return 'schedule-delayed';
      default:
        return '';
    }
  }

  formatDate(date: Date | undefined): string {
    if (!date) return 'N/A';
    return new Intl.DateTimeFormat('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric'
    }).format(date);
  }

  formatCurrency(amount: number): string {
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'OMR'
    }).format(amount);
  }

  formatNumber(num: number): string {
    return new Intl.NumberFormat('en-US').format(num);
  }
}
