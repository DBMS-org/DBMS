import { Component, OnInit, signal, inject, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AuthService } from '../../../../core/services/auth.service';

interface MaintenanceStats {
  totalMachines: number;
  operational: number;
  underMaintenance: number;
  breakdown: number;
  pendingTasks: number;
  scheduledToday: number;
}

interface OperationalStatus {
  running: number;
  idle: number;
  underMaintenance: number;
  breakdown: number;
  available: number;
}

interface MaintenanceAlert {
  id: string;
  machine: string;
  message: string;
  priority: 'HIGH' | 'MEDIUM' | 'LOW';
  timestamp: string;
  icon: string;
}

interface Activity {
  title: string;
  description: string;
  timestamp: string;
  icon: string;
}

interface Machine {
  id: string;
  name: string;
  model: string;
  rigNo: string;
  status: 'Available' | 'In Use' | 'Scheduled for Maintenance' | 'Under Maintenance' | 'Breakdown';
  location: string;
  lastMaintenance?: string;
  nextMaintenance?: string;
  assignedProject?: string;
}

interface MaintenanceScheduleRequest {
  machineId: string;
  maintenanceType: 'Preventive' | 'Corrective' | 'Emergency' | 'Inspection';
  scheduledDate: string;
  notes?: string;
  estimatedDuration: number;
}

interface MaintenanceLogRequest {
  machineId: string;
  maintenanceType: 'Preventive' | 'Corrective' | 'Emergency' | 'Inspection';
  completedDate: string;
  notes: string;
  actualDuration: number;
  partsUsed?: string[];
  cost?: number;
}

interface Accessory {
  id: string;
  name: string;
  stock: number;
  lowStockThreshold: number;
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

  currentUser: any = null;
  isLoading = signal(true);
  
  // Accessories inventory (mock) with low-stock thresholds
  accessories = signal<Accessory[]>([
    { id: 'acc-1', name: 'Oil Filter', stock: 10, lowStockThreshold: 3 },
    { id: 'acc-2', name: 'Air Filter', stock: 8, lowStockThreshold: 2 },
    { id: 'acc-3', name: 'Hydraulic Hose', stock: 5, lowStockThreshold: 2 },
    { id: 'acc-4', name: 'Brake Pads', stock: 12, lowStockThreshold: 4 },
    { id: 'acc-5', name: 'Engine Oil', stock: 20, lowStockThreshold: 6 }
  ]);

  // Per-machine maintenance form data (type, date, notes)
  maintenanceForms = signal<Record<string, { type?: MaintenanceLogRequest['maintenanceType']; date?: string; notes?: string }>>({});

  // Per-machine accessory usage entries
  selectedAccessoryUsage = signal<Record<string, { partName: string; quantity: number }[]>>({});
  
  // Machine data
  machines = signal<Machine[]>([
    {
      id: '1',
      name: 'Excavator EX-001',
      model: 'CAT 320D',
      rigNo: 'EX-001',
      status: 'Available',
      location: 'Site A',
      lastMaintenance: '2024-01-15',
      nextMaintenance: '2024-02-15'
    },
    {
      id: '2',
      name: 'Drill Rig DR-205',
      model: 'Atlas Copco ROC D7',
      rigNo: 'DR-205',
      status: 'In Use',
      location: 'Site B',
      assignedProject: 'Project Alpha',
      lastMaintenance: '2024-01-10',
      nextMaintenance: '2024-02-10'
    },
    {
      id: '3',
      name: 'Loader LD-103',
      model: 'CAT 966M',
      rigNo: 'LD-103',
      status: 'Scheduled for Maintenance',
      location: 'Site C',
      lastMaintenance: '2024-01-05',
      nextMaintenance: '2024-02-05'
    },
    {
      id: '4',
      name: 'Excavator EX-005',
      model: 'Komatsu PC200',
      rigNo: 'EX-005',
      status: 'Under Maintenance',
      location: 'Maintenance Bay 1',
      lastMaintenance: '2024-01-20'
    }
  ]);

  // Computed values
  availableMachines = computed(() => 
    this.machines().filter(m => m.status === 'Available')
  );
  
  scheduledForMaintenance = computed(() => 
    this.machines().filter(m => m.status === 'Scheduled for Maintenance')
  );
  
  underMaintenance = computed(() => 
    this.machines().filter(m => m.status === 'Under Maintenance')
  );

  maintenanceStats: MaintenanceStats = {
    totalMachines: 42,
    operational: 28,
    underMaintenance: 8,
    breakdown: 3,
    pendingTasks: 15,
    scheduledToday: 6
  };

  operationalStatus: OperationalStatus = {
    running: 18,
    idle: 10,
    underMaintenance: 8,
    breakdown: 3,
    available: 3
  };

  maintenanceAlerts: MaintenanceAlert[] = [
    {
      id: '1',
      machine: 'Excavator EX-001',
      message: 'Service threshold reached - 8 hours remaining',
      priority: 'HIGH',
      timestamp: '2 hours ago',
      icon: 'error'
    },
    {
      id: '2',
      machine: 'Drill Rig DR-205',
      message: 'Scheduled maintenance due in 24 hours',
      priority: 'MEDIUM',
      timestamp: '4 hours ago',
      icon: 'schedule'
    },
    {
      id: '3',
      machine: 'Loader LD-103',
      message: 'Oil change required - overdue by 2 days',
      priority: 'HIGH',
      timestamp: '6 hours ago',
      icon: 'warning'
    }
  ];

  recentActivities: Activity[] = [
    {
      title: 'Scheduled Maintenance',
      description: 'Scheduled preventive maintenance for Excavator EX-005',
      timestamp: '30 minutes ago',
      icon: 'schedule'
    },
    {
      title: 'Maintenance Alert Resolved',
      description: 'Resolved hydraulic system alert for Drill Rig DR-102',
      timestamp: '2 hours ago',
      icon: 'check_circle'
    },
    {
      title: 'Machine Status Update',
      description: 'Updated operational status for 5 machines',
      timestamp: '4 hours ago',
      icon: 'update'
    },
    {
      title: 'Service Completed',
      description: 'Completed routine service for Loader LD-201',
      timestamp: '1 day ago',
      icon: 'build'
    },
    {
      title: 'Inventory Check',
      description: 'Performed inventory check on spare parts',
      timestamp: '2 days ago',
      icon: 'inventory'
    }
  ];

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit() {
    this.loadDashboardData();
  }

  loadDashboardData() {
    this.isLoading.set(true);
    this.currentUser = this.authService.getCurrentUser();
    
    // Simulate loading time
    setTimeout(() => {
      this.isLoading.set(false);
    }, 1000);
  }

  // Form helpers
  setMaintenanceForm(machineId: string, field: 'type' | 'date' | 'notes', value: string) {
    this.maintenanceForms.update(forms => ({
      ...forms,
      [machineId]: { ...(forms[machineId] || {}), [field]: value }
    }));
  }

  addAccessoryUsage(machineId: string, partName: string, quantity: number) {
    if (!partName || !quantity || quantity <= 0 || !Number.isFinite(quantity)) {
      this.snackBar.open('Please select a part and valid quantity', 'Close', { duration: 4000, panelClass: ['error-snackbar'] });
      return;
    }

    const partExists = this.accessories().find(a => a.name === partName);
    if (!partExists) {
      this.snackBar.open(`Unknown accessory: ${partName}`, 'Close', { duration: 4000, panelClass: ['error-snackbar'] });
      return;
    }

    this.selectedAccessoryUsage.update(map => {
      const list = map[machineId] ? [...map[machineId]] : [];
      list.push({ partName, quantity });
      return { ...map, [machineId]: list };
    });
  }

  removeAccessoryUsage(machineId: string, index: number) {
    this.selectedAccessoryUsage.update(map => {
      const list = map[machineId] ? [...map[machineId]] : [];
      if (index >= 0 && index < list.length) list.splice(index, 1);
      return { ...map, [machineId]: list };
    });
  }

  refreshDashboard() {
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
    return this.currentUser?.region || '';
  }

  getLastLoginInfo(): string {
    return 'Last login: Today at 9:30 AM';
  }

  // Navigation methods
  navigateToMaintenanceManagement() {
    this.router.navigate(['/mechanical-engineer/maintenance']);
  }

  // Irrelevant routes removed: schedule, inventory, alerts

  logout() {
    this.authService.logout();
    this.router.navigate(['/auth/login']);
  }

  // UC-36: Schedule Maintenance for Machine
  async scheduleMaintenance(machine: Machine) {
    // Check if machine is currently in use and cannot be pulled out
    if (machine.status === 'In Use' && machine.assignedProject) {
      this.snackBar.open(
        'Machine currently in use – cannot schedule maintenance',
        'Close',
        { duration: 5000, panelClass: ['error-snackbar'] }
      );
      return;
    }

    try {
      // In a real implementation, this would open a dialog for maintenance details
      const maintenanceRequest: MaintenanceScheduleRequest = {
        machineId: machine.id,
        maintenanceType: 'Preventive',
        scheduledDate: new Date().toISOString().split('T')[0],
        notes: 'Scheduled maintenance',
        estimatedDuration: 4
      };

      // Update machine status
      this.machines.update(machines => 
        machines.map(m => 
          m.id === machine.id 
            ? { ...m, status: 'Scheduled for Maintenance' as const }
            : m
        )
      );

      this.snackBar.open(
        `Maintenance scheduled for ${machine.name}`,
        'Close',
        { duration: 3000, panelClass: ['success-snackbar'] }
      );

      // Update stats
      this.updateMaintenanceStats();

    } catch (error) {
      this.snackBar.open(
        'Failed to schedule maintenance',
        'Close',
        { duration: 5000, panelClass: ['error-snackbar'] }
      );
    }
  }

  // UC-37: Log Maintenance Activity
  async logMaintenanceActivity(machine: Machine) {
    if (machine.status !== 'Scheduled for Maintenance' && machine.status !== 'Under Maintenance') {
      this.snackBar.open(
        'Machine must be scheduled for maintenance to log activity',
        'Close',
        { duration: 5000, panelClass: ['error-snackbar'] }
      );
      return;
    }

    try {
      // Collect form data for this machine
      const form = this.maintenanceForms()[machine.id] || {};
      const usage = this.selectedAccessoryUsage()[machine.id] || [];

      const maintenanceLog: MaintenanceLogRequest = {
        machineId: machine.id,
        maintenanceType: (form.type || 'Preventive'),
        completedDate: (form.date || new Date().toISOString().split('T')[0]),
        notes: (form.notes || '').trim(),
        actualDuration: 3.5,
        partsUsed: usage.map(u => u.partName),
        cost: 250
      };

      // Validation: mandatory fields
      if (!maintenanceLog.maintenanceType || !maintenanceLog.completedDate || !maintenanceLog.notes) {
        this.snackBar.open(
          'All fields are mandatory',
          'Close',
          { duration: 5000, panelClass: ['error-snackbar'] }
        );
        return;
      }

      // Validation: accessory stock levels
      for (const item of usage) {
        const acc = this.accessories().find(a => a.name === item.partName);
        if (!acc) {
          this.snackBar.open(`Unknown accessory: ${item.partName}`, 'Close', { duration: 5000, panelClass: ['error-snackbar'] });
          return;
        }
        if (item.quantity > acc.stock) {
          this.snackBar.open(`Insufficient stock for part: ${item.partName}`, 'Close', { duration: 5000, panelClass: ['error-snackbar'] });
          return;
        }
      }

      // Deduct accessory stock and record usage
      if (usage.length > 0) {
        this.accessories.update(list => list.map(acc => {
          const used = usage.find(u => u.partName === acc.name);
          if (used) {
            const newStock = acc.stock - used.quantity;
            const updated = { ...acc, stock: newStock };
            // Generate low-stock alert if threshold crossed
            if (newStock <= acc.lowStockThreshold) {
              this.maintenanceAlerts.unshift({
                id: `${Date.now()}-${acc.id}`,
                machine: machine.name,
                message: `Low stock: ${acc.name} at ${newStock} (threshold ${acc.lowStockThreshold})`,
                priority: 'HIGH',
                timestamp: 'Just now',
                icon: 'inventory_2'
              });
            }
            return updated;
          }
          return acc;
        }));
      }

      // Update machine status back to Available
      this.machines.update(machines => 
        machines.map(m => 
          m.id === machine.id 
            ? { 
                ...m, 
                status: 'Available' as const,
                lastMaintenance: maintenanceLog.completedDate
              }
            : m
        )
      );

      this.snackBar.open(
        `Maintenance activity logged for ${machine.name}`,
        'Close',
        { duration: 3000, panelClass: ['success-snackbar'] }
      );

      // Notify Machine Manager (simulated in-app notification)
      this.recentActivities.unshift({
        title: 'Maintenance Logged',
        description: `Maintenance completed for ${machine.name} by ${this.currentUser?.name || 'Mechanical Engineer'}`,
        timestamp: 'Just now',
        icon: 'assignment_turned_in'
      });

      // Record accessory usage entries
      for (const item of usage) {
        this.recentActivities.unshift({
          title: 'Accessory Used',
          description: `${item.quantity} × ${item.partName} used on ${machine.name}`,
          timestamp: 'Just now',
          icon: 'inventory_2'
        });
      }

      // Update stats
      this.updateMaintenanceStats();

      // Clear form and accessory usage for this machine
      this.maintenanceForms.update(forms => ({ ...forms, [machine.id]: {} }));
      this.selectedAccessoryUsage.update(map => ({ ...map, [machine.id]: [] }));

    } catch (error) {
      this.snackBar.open(
        'Failed to log maintenance activity',
        'Close',
        { duration: 5000, panelClass: ['error-snackbar'] }
      );
    }
  }

  private updateMaintenanceStats() {
    const machines = this.machines();
    this.maintenanceStats = {
      totalMachines: machines.length,
      operational: machines.filter(m => m.status === 'Available' || m.status === 'In Use').length,
      underMaintenance: machines.filter(m => m.status === 'Under Maintenance').length,
      breakdown: machines.filter(m => m.status === 'Breakdown').length,
      pendingTasks: machines.filter(m => m.status === 'Scheduled for Maintenance').length,
      scheduledToday: machines.filter(m => m.status === 'Scheduled for Maintenance').length
    };
  }

  // Navigation methods for maintenance management
  navigateToMaintenanceJobs() {
    this.router.navigate(['/mechanical-engineer/maintenance/jobs']);
  }

  navigateToMaintenanceAnalytics() {
    this.router.navigate(['/mechanical-engineer/maintenance/analytics']);
  }
}
