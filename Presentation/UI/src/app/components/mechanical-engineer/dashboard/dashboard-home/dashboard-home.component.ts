import { Component, OnInit, signal, inject, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AuthService } from '../../../../core/services/auth.service';

interface MechanicalEngineerStats {
  totalMachines: number;
  operational: number;
  underMaintenance: number;
  breakdown: number;
  pendingTasks: number;
  scheduledToday: number;
  completedThisWeek: number;
  averageUptime: number;
}

interface EquipmentPerformance {
  machineId: string;
  machineName: string;
  uptime: number;
  efficiency: number;
  lastService: string;
  nextService: string;
  status: 'Excellent' | 'Good' | 'Fair' | 'Poor';
}

interface MaintenanceAlert {
  id: string;
  machineId: string;
  machineName: string;
  message: string;
  priority: 'CRITICAL' | 'HIGH' | 'MEDIUM' | 'LOW';
  timestamp: string;
  type: 'OVERDUE' | 'DUE_SOON' | 'BREAKDOWN' | 'INSPECTION';
  icon: string;
}

interface MaintenanceActivity {
  id: string;
  title: string;
  description: string;
  timestamp: string;
  machineId: string;
  type: 'COMPLETED' | 'SCHEDULED' | 'CANCELLED';
  icon: string;
}

interface Machine {
  id: string;
  name: string;
  type: string;
  model: string;
  serialNumber: string;
  status: 'OPERATIONAL' | 'IN_USE' | 'MAINTENANCE_SCHEDULED' | 'UNDER_MAINTENANCE' | 'BREAKDOWN' | 'OFFLINE';
  location: string;
  assignedProject?: string;
  lastMaintenance?: string;
  nextMaintenance?: string;
  nextMaintenanceType?: string;
  nextMaintenanceDate?: string;
  priority?: 'HIGH' | 'MEDIUM' | 'LOW';
  hoursOperated: number;
  fuelConsumption: number;
  efficiency: number;
}

interface SparePart {
  id: string;
  name: string;
  partNumber: string;
  currentStock: number;
  minimumStock: number;
  minStock: number;
  maxStock: number;
  unitCost: number;
  supplier: string;
  category: 'ENGINE' | 'HYDRAULIC' | 'ELECTRICAL' | 'CONSUMABLE' | 'SAFETY';
}

@Component({
  selector: 'app-dashboard-home',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard-home.component.html',
  styleUrl: './dashboard-home.component.scss'
})
export class DashboardHomeComponent implements OnInit {
  private dialog = inject(MatDialog);
  private snackBar = inject(MatSnackBar);
  private authService = inject(AuthService);
  private router = inject(Router);

  // Component state
  currentUser: any = null;
  isLoading = signal(true);
  error = signal<string | null>(null);

  // Dashboard data signals
  stats = signal<MechanicalEngineerStats>({
    totalMachines: 0,
    operational: 0,
    underMaintenance: 0,
    breakdown: 0,
    pendingTasks: 0,
    scheduledToday: 0,
    completedThisWeek: 0,
    averageUptime: 0
  });

  equipmentPerformance = signal<EquipmentPerformance[]>([]);
  maintenanceAlerts = signal<MaintenanceAlert[]>([]);
  recentActivities = signal<MaintenanceActivity[]>([]);
  machines = signal<Machine[]>([]);
  spareParts = signal<SparePart[]>([]);

  // Computed properties
  criticalAlerts = computed(() => 
    this.maintenanceAlerts().filter(alert => alert.priority === 'CRITICAL')
  );

  lowStockParts = computed(() => 
    this.spareParts().filter(part => part.currentStock <= part.minimumStock)
  );

  machinesNeedingMaintenance = computed(() => 
    this.machines().filter(machine => 
      machine.status === 'MAINTENANCE_SCHEDULED' || machine.status === 'UNDER_MAINTENANCE'
    )
  );

  operationalMachines = computed(() => 
    this.machines().filter(machine => 
      machine.status === 'OPERATIONAL' || machine.status === 'IN_USE'
    )
  );

  // Computed properties for the new data structure
  mechanicalStats = computed(() => ({
    totalEquipment: this.machines().length,
    operationalEquipment: this.operationalMachines().length,
    underMaintenance: this.machinesNeedingMaintenance().length,
    efficiencyRate: this.calculateAverageEfficiency(),
    pendingMaintenance: this.machinesNeedingMaintenance().length
  }));

  ngOnInit(): void {
    this.loadDashboardData();
  }

  private async loadDashboardData(): Promise<void> {
    try {
      console.log('Starting dashboard data load...');
      this.isLoading.set(true);
      this.error.set(null);
      this.currentUser = this.authService.getCurrentUser();
      console.log('Current user:', this.currentUser);

      // Load all dashboard data
      console.log('Loading dashboard data components...');
      await Promise.all([
        this.loadMachineData(),
        this.loadMaintenanceAlerts(),
        this.loadRecentActivities(),
        this.loadSparePartsInventory(),
        this.loadEquipmentPerformance()
      ]);
      console.log('All data loaded successfully');

      this.updateStats();
      console.log('Stats updated');
    } catch (error) {
      console.error('Error loading dashboard data:', error);
      this.error.set('Failed to load dashboard data');
    } finally {
      console.log('Setting loading to false');
      this.isLoading.set(false);
    }
  }

  private async loadMachineData(): Promise<void> {
    return new Promise((resolve) => {
      // Mock data for machines
      const mockMachines: Machine[] = [
        {
          id: 'DR-102',
          name: 'Drill Rig Atlas Copco',
          type: 'Drill Rig',
          model: 'Atlas Copco ROC L8',
          serialNumber: 'AC-ROC-L8-102',
          status: 'OPERATIONAL',
          location: 'Site A - Block 3',
          assignedProject: 'Project Alpha',
          lastMaintenance: '2024-01-15',
          nextMaintenance: '2024-02-15',
          nextMaintenanceType: 'Scheduled Service',
          nextMaintenanceDate: '2024-02-15',
          priority: 'MEDIUM',
          hoursOperated: 1250,
          fuelConsumption: 18.5,
          efficiency: 92
        },
        {
          id: 'LD-201',
          name: 'Loader Caterpillar 980M',
          type: 'Loader',
          model: 'CAT 980M',
          serialNumber: 'CAT980M201',
          status: 'IN_USE',
          location: 'Site B - Loading Zone',
          assignedProject: 'Project Beta',
          lastMaintenance: '2024-01-10',
          nextMaintenance: '2024-02-10',
          nextMaintenanceType: 'Oil Change',
          nextMaintenanceDate: '2024-02-10',
          priority: 'LOW',
          hoursOperated: 980,
          fuelConsumption: 22.3,
          efficiency: 88
        },
        {
          id: 'LD-103',
          name: 'Loader CAT 966M',
          type: 'Loader',
          model: 'CAT 966M',
          serialNumber: 'CAT966M103',
          status: 'MAINTENANCE_SCHEDULED',
          location: 'Maintenance Bay 2',
          lastMaintenance: '2024-01-05',
          nextMaintenance: '2024-02-05',
          nextMaintenanceType: 'Hydraulic System Check',
          nextMaintenanceDate: '2024-02-05',
          priority: 'HIGH',
          hoursOperated: 1450,
          fuelConsumption: 25.1,
          efficiency: 85
        },
        {
          id: 'EX-005',
          name: 'Excavator Komatsu PC200',
          type: 'Excavator',
          model: 'PC200',
          serialNumber: 'PC200-005',
          status: 'UNDER_MAINTENANCE',
          location: 'Maintenance Bay 1',
          lastMaintenance: '2024-01-20',
          nextMaintenanceType: 'Engine Overhaul',
          nextMaintenanceDate: '2024-02-25',
          priority: 'HIGH',
          hoursOperated: 2100,
          fuelConsumption: 28.7,
          efficiency: 78
        }
      ];
      this.machines.set(mockMachines);
      resolve();
    });
  }

  private async loadMaintenanceAlerts(): Promise<void> {
    return new Promise((resolve) => {
      const mockAlerts: MaintenanceAlert[] = [
        {
          id: '1',
          machineId: 'DR-102',
          machineName: 'Drill Rig Atlas Copco',
          message: 'Hydraulic system pressure low - requires immediate attention',
          priority: 'CRITICAL',
          timestamp: '2 hours ago',
          type: 'BREAKDOWN',
          icon: 'error'
        },
        {
          id: '2',
          machineId: 'LD-201',
          machineName: 'Loader Caterpillar 980M',
          message: 'Scheduled maintenance due in 2 days',
          priority: 'HIGH',
          timestamp: '4 hours ago',
          type: 'DUE_SOON',
          icon: 'schedule'
        },
        {
          id: '3',
          machineId: 'LD-103',
          machineName: 'Loader CAT 966M',
          message: 'Oil change required - overdue by 2 days',
          priority: 'HIGH',
          timestamp: '6 hours ago',
          type: 'OVERDUE',
          icon: 'warning'
        }
      ];
      this.maintenanceAlerts.set(mockAlerts);
      resolve();
    });
  }

  private async loadRecentActivities(): Promise<void> {
    return new Promise((resolve) => {
      const mockActivities: MaintenanceActivity[] = [
        {
          id: '1',
          title: 'Scheduled Maintenance',
          description: 'Scheduled preventive maintenance for Excavator EX-005',
          timestamp: '30 minutes ago',
          machineId: 'EX-005',
          type: 'SCHEDULED',
          icon: 'schedule'
        },
        {
          id: '2',
          title: 'Maintenance Alert Resolved',
          description: 'Resolved hydraulic system alert for Drill Rig DR-102',
          timestamp: '2 hours ago',
          machineId: 'DR-102',
          type: 'COMPLETED',
          icon: 'check_circle'
        },
        {
          id: '3',
          title: 'Service Completed',
          description: 'Completed routine service for Loader LD-201',
          timestamp: '1 day ago',
          machineId: 'LD-201',
          type: 'COMPLETED',
          icon: 'build'
        }
      ];
      this.recentActivities.set(mockActivities);
      resolve();
    });
  }

  private async loadSparePartsInventory(): Promise<void> {
    return new Promise((resolve) => {
      const mockSpareParts: SparePart[] = [
        {
          id: 'SP-001',
          name: 'Hydraulic Filter',
          partNumber: 'HF-2024-001',
          currentStock: 5,
          minimumStock: 10,
          minStock: 10,
          maxStock: 50,
          unitCost: 125.50,
          supplier: 'Caterpillar Inc.',
          category: 'HYDRAULIC'
        },
        {
          id: 'SP-002',
          name: 'Engine Oil Filter',
          partNumber: 'EOF-2024-002',
          currentStock: 8,
          minimumStock: 15,
          minStock: 15,
          maxStock: 60,
          unitCost: 45.75,
          supplier: 'Atlas Copco',
          category: 'ENGINE'
        },
        {
          id: 'SP-003',
          name: 'Brake Pads',
          partNumber: 'BP-2024-003',
          currentStock: 3,
          minimumStock: 8,
          minStock: 8,
          maxStock: 40,
          unitCost: 89.25,
          supplier: 'Komatsu Parts',
          category: 'SAFETY'
        }
      ];
      this.spareParts.set(mockSpareParts);
      resolve();
    });
  }

  private async loadEquipmentPerformance(): Promise<void> {
    return new Promise((resolve) => {
      const mockPerformance: EquipmentPerformance[] = [
        {
          machineId: 'DR-102',
          machineName: 'Drill Rig Atlas Copco',
          uptime: 92.5,
          efficiency: 92,
          lastService: '2024-01-15',
          nextService: '2024-02-15',
          status: 'Excellent'
        },
        {
          machineId: 'LD-201',
          machineName: 'Loader Caterpillar 980M',
          uptime: 88.2,
          efficiency: 88,
          lastService: '2024-01-10',
          nextService: '2024-02-10',
          status: 'Good'
        },
        {
          machineId: 'LD-103',
          machineName: 'Loader CAT 966M',
          uptime: 85.1,
          efficiency: 85,
          lastService: '2024-01-05',
          nextService: '2024-02-05',
          status: 'Good'
        },
        {
          machineId: 'EX-005',
          machineName: 'Excavator Komatsu PC200',
          uptime: 78.3,
          efficiency: 78,
          lastService: '2024-01-20',
          nextService: '2024-02-20',
          status: 'Fair'
        }
      ];
      this.equipmentPerformance.set(mockPerformance);
      resolve();
    });
  }

  private updateStats(): void {
    const machines = this.machines();
    const alerts = this.maintenanceAlerts();
    const activities = this.recentActivities();

    this.stats.set({
      totalMachines: machines.length,
      operational: machines.filter(m => m.status === 'OPERATIONAL' || m.status === 'IN_USE').length,
      underMaintenance: machines.filter(m => m.status === 'UNDER_MAINTENANCE').length,
      breakdown: machines.filter(m => m.status === 'BREAKDOWN').length,
      pendingTasks: machines.filter(m => m.status === 'MAINTENANCE_SCHEDULED').length,
      scheduledToday: machines.filter(m => m.status === 'MAINTENANCE_SCHEDULED').length,
      completedThisWeek: activities.filter(a => a.type === 'COMPLETED').length,
      averageUptime: Math.round(machines.reduce((acc, m) => acc + m.efficiency, 0) / machines.length)
    });
  }

  refreshDashboard(): void {
    this.loadDashboardData();
  }

  getUserWelcomeMessage(): string {
    if (!this.currentUser) return 'Welcome, Mechanical Engineer';
    
    const hour = new Date().getHours();
    let greeting = 'Good morning';
    
    if (hour >= 12 && hour < 17) {
      greeting = 'Good afternoon';
    } else if (hour >= 17) {
      greeting = 'Good evening';
    }
    
    return `${greeting}, ${this.currentUser.name}!`;
  }

  getInitials(): string {
    if (!this.currentUser?.name) return 'ME';
    
    const names = this.currentUser.name.split(' ');
    if (names.length >= 2) {
      return (names[0][0] + names[1][0]).toUpperCase();
    }
    return names[0].substring(0, 2).toUpperCase();
  }

  getUserLocationInfo(): string {
    return this.currentUser?.region || 'Mining Site';
  }

  getLastLoginInfo(): string {
    return 'Last login: Today at 9:30 AM';
  }

  // Navigation methods
  navigateToMaintenanceManagement(): void {
    this.router.navigate(['/mechanical-engineer/maintenance']);
  }

  navigateToEquipmentMonitoring(): void {
    this.router.navigate(['/mechanical-engineer/equipment']);
  }

  navigateToSparePartsInventory(): void {
    this.router.navigate(['/mechanical-engineer/spare-parts']);
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/auth/login']);
  }

  // Maintenance management methods
  scheduleMaintenance(machine: Machine): void {
    // Check if machine is currently in use and cannot be pulled out
    if (machine.status === 'IN_USE' && machine.assignedProject) {
      this.snackBar.open(
        'Machine currently in use – cannot schedule maintenance',
        'Close',
        { duration: 5000, panelClass: ['error-snackbar'] }
      );
      return;
    }

    // Update machine status
    this.machines.update(machines => 
      machines.map(m => 
        m.id === machine.id 
          ? { ...m, status: 'MAINTENANCE_SCHEDULED' as const }
          : m
      )
    );

    this.snackBar.open(
      `Maintenance scheduled for ${machine.name}`,
      'Close',
      { duration: 3000, panelClass: ['success-snackbar'] }
    );

    // Update stats
    this.updateStats();
  }

  viewMachineDetails(machine: Machine): void {
    this.router.navigate(['/mechanical-engineer/machines', machine.id]);
  }

  viewAlertDetails(alert: MaintenanceAlert): void {
    this.router.navigate(['/mechanical-engineer/alerts', alert.id]);
  }

  // Navigation methods for maintenance management
  navigateToMaintenanceJobs(): void {
    this.router.navigate(['/mechanical-engineer/maintenance/jobs']);
  }

  navigateToMaintenanceAnalytics(): void {
    this.router.navigate(['/mechanical-engineer/maintenance/analytics']);
  }

  private calculateAverageEfficiency(): number {
    const machines = this.machines();
    if (machines.length === 0) return 0;
    const totalEfficiency = machines.reduce((sum, machine) => sum + machine.efficiency, 0);
    return Math.round(totalEfficiency / machines.length);
  }

  // Helper methods for template
  getEfficiencyClass(efficiency: number): string {
    if (efficiency >= 90) return 'efficiency-excellent';
    if (efficiency >= 80) return 'efficiency-good';
    if (efficiency >= 70) return 'efficiency-fair';
    return 'efficiency-poor';
  }

  getStockLevelClass(part: SparePart): string {
    const ratio = part.currentStock / part.minimumStock;
    if (ratio <= 0.5) return 'stock-critical';
    if (ratio <= 1) return 'stock-low';
    return 'stock-normal';
  }

  getStockStatusClass(part: SparePart): string {
    const ratio = part.currentStock / part.minimumStock;
    if (ratio <= 0.5) return 'status-critical';
    if (ratio <= 1) return 'status-low';
    return 'status-normal';
  }

  getStockStatus(part: SparePart): string {
    const ratio = part.currentStock / part.minimumStock;
    if (ratio <= 0.5) return 'Critical';
    if (ratio <= 1) return 'Low Stock';
    return 'Normal';
  }

  // Navigation methods
  navigateToMaintenance(): void {
    this.router.navigate(['/mechanical-engineer/maintenance']);
  }

  navigateToEquipment(): void {
    this.router.navigate(['/mechanical-engineer/equipment']);
  }

  navigateToAnalytics(): void {
    this.router.navigate(['/mechanical-engineer/analytics']);
  }
}
