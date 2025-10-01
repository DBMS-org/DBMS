import { Injectable } from '@angular/core';
import { Observable, of, delay } from 'rxjs';
import {
  MaintenanceJob,
  MaintenanceAlert,
  MaintenanceStats,
  JobFilters,
  MaintenanceStatus,
  MaintenanceType,
  AlertType,
  Priority,
  FileAttachment,
  ServiceComplianceData,
  MTBFMetrics,
  PartsUsageData,
  UsageMetrics,
  NotificationPreferences
} from '../models/maintenance.models';

@Injectable({
  providedIn: 'root'
})
export class MaintenanceMockService {
  private mockJobs: MaintenanceJob[] = this.generateMockJobs();

  private generateMockJobs(): MaintenanceJob[] {
    const machines = [
      { name: 'Excavator CAT 320', serial: 'CAT320-2023-001', id: 'M001' },
      { name: 'Bulldozer Komatsu D65', serial: 'KOMD65-2022-045', id: 'M002' },
      { name: 'Drill Rig Atlas Copco', serial: 'ATLAS-2021-123', id: 'M003' },
      { name: 'Loader Volvo L120', serial: 'VOLVO-L120-2023-078', id: 'M004' },
      { name: 'Crane Liebherr LTM', serial: 'LIEB-LTM-2022-156', id: 'M005' },
      { name: 'Excavator Hitachi ZX350', serial: 'HITACHI-ZX350-2023-092', id: 'M006' },
      { name: 'Dump Truck Caterpillar 777', serial: 'CAT777-2022-234', id: 'M007' },
      { name: 'Grader John Deere 872G', serial: 'JD872G-2023-156', id: 'M008' },
      { name: 'Compactor Bomag BW213', serial: 'BOMAG-BW213-2021-089', id: 'M009' },
      { name: 'Backhoe JCB 3CX', serial: 'JCB3CX-2022-345', id: 'M010' },
      { name: 'Forklift Toyota 8FG25', serial: 'TOYOTA-8FG25-2023-567', id: 'M011' },
      { name: 'Crusher Metso LT200HP', serial: 'METSO-LT200HP-2021-678', id: 'M012' },
      { name: 'Screener Powerscreen Warrior', serial: 'PS-WARRIOR-2022-789', id: 'M013' },
      { name: 'Generator Caterpillar C18', serial: 'CAT-C18-2023-890', id: 'M014' },
      { name: 'Air Compressor Atlas Copco GA75', serial: 'ATLAS-GA75-2022-901', id: 'M015' },
      { name: 'Welder Miller Dynasty 400', serial: 'MILLER-D400-2023-234', id: 'M016' },
      { name: 'Concrete Pump Putzmeister BSA', serial: 'PUTZ-BSA-2022-567', id: 'M017' },
      { name: 'Tower Crane Potain MDT', serial: 'POTAIN-MDT-2021-890', id: 'M018' },
      { name: 'Mobile Crane Grove GMK', serial: 'GROVE-GMK-2023-123', id: 'M019' },
      { name: 'Pile Driver ICE 44', serial: 'ICE-44-2022-456', id: 'M020' }
    ];

    const projects = [
      'Mining Project Alpha',
      'Construction Site Beta',
      'Quarry Operations',
      'Highway Construction Phase 2',
      'Urban Development Project',
      'Industrial Complex Build',
      'Infrastructure Upgrade',
      'Residential Development',
      'Bridge Construction Project',
      'Tunnel Excavation Site',
      'Airport Expansion',
      'Port Development',
      'Railway Extension',
      'Power Plant Construction',
      'Oil Refinery Upgrade'
    ];

    const technicians = [
      'John Smith',
      'Sarah Wilson',
      'Mike Johnson',
      'David Brown',
      'Lisa Chen',
      'Robert Davis',
      'Emily Rodriguez',
      'James Wilson',
      'Maria Garcia',
      'Thomas Anderson',
      'Jennifer Taylor',
      'Christopher Lee',
      'Amanda White',
      'Michael Thompson',
      'Jessica Martinez',
      'Daniel Clark',
      'Ashley Lewis',
      'Matthew Hall',
      'Nicole Young',
      'Kevin King'
    ];

    const maintenanceReasons = {
      [MaintenanceType.PREVENTIVE]: [
        'Regular preventive maintenance - oil change, filter replacement',
        'Monthly inspection and lubrication',
        'Quarterly comprehensive maintenance',
        'Annual safety inspection',
        'Scheduled belt and hose replacement',
        'Routine hydraulic system check',
        'Engine tune-up and diagnostics',
        'Transmission service and fluid change',
        'Brake system inspection and adjustment',
        'Cooling system maintenance',
        'Electrical system preventive maintenance',
        'Fuel system cleaning and filter replacement',
        'Tire rotation and pressure check',
        'Battery maintenance and terminal cleaning',
        'Exhaust system inspection',
        'Steering system lubrication',
        'Undercarriage inspection and maintenance',
        'Safety system testing and calibration',
        'Operator cabin maintenance',
        'Tool and accessory inspection'
      ],
      [MaintenanceType.CORRECTIVE]: [
        'Hydraulic system leak detected',
        'Engine overheating issue',
        'Transmission slipping problem',
        'Electrical system malfunction',
        'Steering mechanism adjustment needed',
        'Fuel system contamination cleanup',
        'Exhaust system repair',
        'Air conditioning system repair',
        'Hydraulic pump replacement',
        'Radiator repair and coolant flush',
        'Brake system malfunction',
        'Starter motor failure',
        'Alternator replacement',
        'Battery terminal corrosion',
        'Tire puncture repair',
        'Track tension adjustment',
        'Boom cylinder seal replacement',
        'Swing motor repair',
        'Travel motor bearing replacement',
        'Control valve malfunction'
      ],
      [MaintenanceType.PREDICTIVE]: [
        'Predictive maintenance based on vibration analysis',
        'Oil analysis indicates bearing wear',
        'Temperature monitoring shows cooling issues',
        'Pressure readings indicate hydraulic problems',
        'Noise analysis suggests gear wear',
        'Performance data indicates engine issues',
        'Sensor data shows electrical anomalies',
        'Fluid analysis reveals contamination',
        'Thermal imaging detected hot spots',
        'Ultrasonic testing found structural issues',
        'Vibration analysis shows imbalance',
        'Infrared thermography reveals overheating',
        'Motor current analysis indicates winding issues',
        'Lubrication analysis shows wear particles',
        'Performance trending indicates efficiency loss',
        'Condition monitoring alerts',
        'Predictive analytics warning',
        'Equipment health assessment',
        'Reliability analysis results',
        'Failure mode analysis'
      ],
      [MaintenanceType.EMERGENCY]: [
        'Emergency repair - brake system malfunction',
        'Critical engine failure - immediate attention required',
        'Hydraulic system complete failure',
        'Electrical fire damage repair',
        'Structural damage from accident',
        'Safety system failure - urgent repair',
        'Complete transmission failure',
        'Steering system emergency repair',
        'Fuel leak emergency containment',
        'Critical safety violation repair',
        'Boom collapse emergency',
        'Undercarriage failure',
        'Electrical short circuit',
        'Hydraulic hose burst',
        'Engine seizure',
        'Transmission lock-up',
        'Steering loss',
        'Brake failure',
        'Fire suppression system activation',
        'Emergency shutdown required'
      ]
    };

    const parts = [
      'Oil Filter', 'Air Filter', 'Fuel Filter', 'Hydraulic Hose', 'Brake Pads', 
      'Brake Fluid', 'Engine Oil', 'Transmission Fluid', 'Coolant', 'Grease',
      'Spark Plugs', 'Belts', 'Gaskets', 'Seals', 'Bearings', 'Valves',
      'Pistons', 'Cylinders', 'Pumps', 'Motors', 'Sensors', 'Wiring Harness',
      'Radiator', 'Thermostat', 'Water Pump', 'Alternator', 'Starter Motor',
      'Battery', 'Tires', 'Tracks', 'Chains', 'Sprockets', 'Hydraulic Cylinders',
      'Control Valves', 'Pressure Switches', 'Temperature Sensors', 'Flow Meters',
      'Relief Valves', 'Check Valves', 'Directional Valves', 'Proportional Valves',
      'Servo Valves', 'Accumulators', 'Filters', 'Heat Exchangers', 'Cooling Fans',
      'Exhaust Mufflers', 'Catalytic Converters', 'DPF Filters', 'AdBlue Systems',
      'Turbochargers', 'Intercoolers', 'Fuel Injectors', 'Glow Plugs', 'Camshafts',
      'Crankshafts', 'Connecting Rods', 'Cylinder Heads', 'Engine Blocks'
    ];

    const fileAttachments: FileAttachment[] = [
      {
        id: '1',
        fileName: 'maintenance_report_2024.pdf',
        fileType: 'pdf',
        fileSize: 2048576,
        uploadedAt: new Date(),
        url: '/assets/documents/maintenance_report_2024.pdf'
      },
      {
        id: '2',
        fileName: 'before_repair_photo.jpg',
        fileType: 'jpg',
        fileSize: 1048576,
        uploadedAt: new Date(),
        url: '/assets/images/before_repair_photo.jpg'
      },
      {
        id: '3',
        fileName: 'after_repair_photo.jpg',
        fileType: 'jpg',
        fileSize: 1536000,
        uploadedAt: new Date(),
        url: '/assets/images/after_repair_photo.jpg'
      },
      {
        id: '4',
        fileName: 'parts_invoice.xlsx',
        fileType: 'xlsx',
        fileSize: 512000,
        uploadedAt: new Date(),
        url: '/assets/documents/parts_invoice.xlsx'
      },
      {
        id: '5',
        fileName: 'safety_checklist.pdf',
        fileType: 'pdf',
        fileSize: 307200,
        uploadedAt: new Date(),
        url: '/assets/documents/safety_checklist.pdf'
      },
      {
        id: '6',
        fileName: 'diagnostic_report.pdf',
        fileType: 'pdf',
        fileSize: 819200,
        uploadedAt: new Date(),
        url: '/assets/documents/diagnostic_report.pdf'
      },
      {
        id: '7',
        fileName: 'work_order.pdf',
        fileType: 'pdf',
        fileSize: 409600,
        uploadedAt: new Date(),
        url: '/assets/documents/work_order.pdf'
      },
      {
        id: '8',
        fileName: 'quality_inspection.pdf',
        fileType: 'pdf',
        fileSize: 614400,
        uploadedAt: new Date(),
        url: '/assets/documents/quality_inspection.pdf'
      }
    ];

    const jobs: MaintenanceJob[] = [];
    const today = new Date();
    
    // Generate jobs for the past 30 days and next 60 days
    for (let dayOffset = -30; dayOffset <= 60; dayOffset++) {
      const date = new Date(today);
      date.setDate(today.getDate() + dayOffset);
      
      // Generate 1-4 jobs per day (more on weekdays)
      const isWeekend = date.getDay() === 0 || date.getDay() === 6;
      const jobsPerDay = isWeekend ? Math.floor(Math.random() * 2) + 1 : Math.floor(Math.random() * 3) + 2;
      
      for (let jobIndex = 0; jobIndex < jobsPerDay; jobIndex++) {
        const machine = machines[Math.floor(Math.random() * machines.length)];
        const project = projects[Math.floor(Math.random() * projects.length)];
        const type = Object.values(MaintenanceType)[Math.floor(Math.random() * Object.values(MaintenanceType).length)];
        
        // Set time for the job (8 AM to 6 PM)
        const jobDate = new Date(date);
        jobDate.setHours(8 + Math.floor(Math.random() * 10), Math.floor(Math.random() * 60), 0, 0);
        
        // Determine status based on date
        let status: MaintenanceStatus;
        if (dayOffset < -7) {
          // Past jobs - mostly completed, some overdue
          status = Math.random() < 0.85 ? MaintenanceStatus.COMPLETED : MaintenanceStatus.OVERDUE;
        } else if (dayOffset < -2) {
          // Recent past - mix of completed, in progress, and overdue
          const rand = Math.random();
          if (rand < 0.6) status = MaintenanceStatus.COMPLETED;
          else if (rand < 0.8) status = MaintenanceStatus.IN_PROGRESS;
          else status = MaintenanceStatus.OVERDUE;
        } else if (dayOffset <= 0) {
          // Today and yesterday - mostly in progress or scheduled
          status = Math.random() < 0.6 ? MaintenanceStatus.IN_PROGRESS : MaintenanceStatus.SCHEDULED;
        } else {
          // Future jobs - scheduled, with some cancelled
          status = Math.random() < 0.9 ? MaintenanceStatus.SCHEDULED : MaintenanceStatus.CANCELLED;
        }
        
        // Assign technicians (1-3 per job)
        const numTechnicians = Math.floor(Math.random() * 3) + 1;
        const assignedTechnicians = [];
        const availableTechnicians = [...technicians];
        for (let i = 0; i < numTechnicians; i++) {
          const techIndex = Math.floor(Math.random() * availableTechnicians.length);
          assignedTechnicians.push(availableTechnicians.splice(techIndex, 1)[0]);
        }
        
        const reason = maintenanceReasons[type][Math.floor(Math.random() * maintenanceReasons[type].length)];
        const estimatedHours = Math.floor(Math.random() * 16) + 2; // 2-18 hours
        const actualHours = status === MaintenanceStatus.COMPLETED ? 
          Math.floor(estimatedHours * (0.8 + Math.random() * 0.4)) : undefined;
        
        // Generate parts replaced for completed jobs
        const partsReplaced = status === MaintenanceStatus.COMPLETED ? 
          parts.slice(0, Math.floor(Math.random() * 5) + 1) : [];
        
        const observations = this.generateObservations(status, type);
        
        // Generate attachments for completed and in-progress jobs
        const attachments: FileAttachment[] = [];
        if (status === MaintenanceStatus.COMPLETED || status === MaintenanceStatus.IN_PROGRESS) {
          const numAttachments = Math.floor(Math.random() * 4) + 1;
          const availableAttachments = [...fileAttachments];
          for (let i = 0; i < numAttachments && availableAttachments.length > 0; i++) {
            const attachmentIndex = Math.floor(Math.random() * availableAttachments.length);
            const attachment = availableAttachments.splice(attachmentIndex, 1)[0];
            // Create a copy with unique ID and random upload date
            attachments.push({
              ...attachment,
              id: `${attachment.id}-${jobs.length + 1}-${i}`,
              uploadedAt: new Date(jobDate.getTime() + Math.random() * 24 * 60 * 60 * 1000)
            });
          }
        }
        
        const job: MaintenanceJob = {
          id: `${jobs.length + 1}`,
          machineId: machine.id,
          machineName: machine.name,
          serialNumber: machine.serial,
          project: project,
          scheduledDate: jobDate,
          type: type,
          status: status,
          assignedTo: assignedTechnicians,
          estimatedHours: estimatedHours,
          actualHours: actualHours,
          reason: reason,
          observations: observations,
          partsReplaced: partsReplaced,
          attachments: attachments,
          createdAt: new Date(jobDate.getTime() - Math.random() * 7 * 24 * 60 * 60 * 1000), // Created 0-7 days before scheduled
          updatedAt: new Date()
        };
        
        jobs.push(job);
      }
    }
    
    return jobs.sort((a, b) => a.scheduledDate.getTime() - b.scheduledDate.getTime());
  }

  private generateObservations(status: MaintenanceStatus, type: MaintenanceType): string {
    const observations = {
      [MaintenanceStatus.SCHEDULED]: [
        'Awaiting parts delivery',
        'Machine ready for maintenance',
        'Scheduled during downtime',
        'Coordinating with operations team',
        'Pre-maintenance inspection completed',
        'Safety briefing scheduled',
        'Tools and equipment prepared',
        'Work permit obtained',
        'Site access arranged',
        'Weather conditions favorable'
      ],
      [MaintenanceStatus.IN_PROGRESS]: [
        'Work in progress, 50% complete',
        'Waiting for specialized tools',
        'Parts replacement underway',
        'Diagnostic tests running',
        'Technician on-site working',
        'Expected completion by end of shift',
        'Additional parts required',
        'Quality checks in progress',
        'Safety protocols being followed',
        'Team coordination ongoing'
      ],
      [MaintenanceStatus.COMPLETED]: [
        'All systems functioning properly',
        'Maintenance completed successfully',
        'Machine tested and operational',
        'Quality check passed',
        'Ready for operation',
        'Performance improved after maintenance',
        'Safety systems verified',
        'Documentation completed',
        'Operator training provided',
        'Warranty work completed'
      ],
      [MaintenanceStatus.OVERDUE]: [
        'Delayed due to parts shortage',
        'Technician unavailable - rescheduling needed',
        'Weather conditions preventing work',
        'Machine still in operation - urgent scheduling required',
        'Waiting for specialized equipment',
        'Site access issues',
        'Safety concerns need resolution',
        'Budget approval pending',
        'Contractor scheduling conflict',
        'Emergency repairs taking priority'
      ],
      [MaintenanceStatus.CANCELLED]: [
        'Machine sold - maintenance no longer needed',
        'Replaced with newer model',
        'Project cancelled',
        'Maintenance rescheduled to different date',
        'Machine out of service permanently',
        'Budget constraints',
        'Operational changes',
        'Equipment relocation',
        'Contract termination',
        'Safety decommissioning'
      ]
    };
    
    const statusObservations = observations[status];
    return statusObservations[Math.floor(Math.random() * statusObservations.length)];
  }

  // Dashboard and Stats
  getMaintenanceStats(): Observable<MaintenanceStats> {
    const jobs = this.mockJobs;
    const stats: MaintenanceStats = {
      totalMachines: 20,
      scheduledJobs: jobs.filter(j => j.status === MaintenanceStatus.SCHEDULED).length,
      inProgressJobs: jobs.filter(j => j.status === MaintenanceStatus.IN_PROGRESS).length,
      completedJobs: jobs.filter(j => j.status === MaintenanceStatus.COMPLETED).length,
      overdueJobs: jobs.filter(j => j.status === MaintenanceStatus.OVERDUE).length,
      serviceDueAlerts: jobs.filter(j => {
        const daysDiff = Math.ceil((j.scheduledDate.getTime() - new Date().getTime()) / (1000 * 60 * 60 * 24));
        return daysDiff <= 3 && daysDiff >= 0 && j.status === MaintenanceStatus.SCHEDULED;
      }).length
    };
    return of(stats).pipe(delay(300));
  }

  getServiceDueAlerts(): Observable<MaintenanceAlert[]> {
    const alerts = this.mockJobs
      .filter(job => {
        const daysDiff = Math.ceil((job.scheduledDate.getTime() - new Date().getTime()) / (1000 * 60 * 60 * 24));
        return daysDiff <= 3 && daysDiff >= 0 && job.status === MaintenanceStatus.SCHEDULED;
      })
      .map(job => {
        const daysDiff = Math.ceil((job.scheduledDate.getTime() - new Date().getTime()) / (1000 * 60 * 60 * 24));
        return {
          id: `alert-${job.id}`,
          machineId: job.machineId,
          machineName: job.machineName,
          alertType: AlertType.SERVICE_DUE,
          message: `${job.type} maintenance due in ${daysDiff} day(s)`,
          dueDate: job.scheduledDate,
          priority: daysDiff <= 1 ? Priority.HIGH : daysDiff <= 2 ? Priority.MEDIUM : Priority.LOW,
          daysUntilDue: daysDiff
        };
      });
    
    return of(alerts).pipe(delay(200));
  }

  getOverdueAlerts(): Observable<MaintenanceAlert[]> {
    const alerts = this.mockJobs
      .filter(job => job.status === MaintenanceStatus.OVERDUE)
      .map(job => {
        const daysPastDue = Math.ceil((new Date().getTime() - job.scheduledDate.getTime()) / (1000 * 60 * 60 * 24));
        return {
          id: `overdue-${job.id}`,
          machineId: job.machineId,
          machineName: job.machineName,
          alertType: AlertType.OVERDUE,
          message: `${job.type} maintenance overdue by ${daysPastDue} day(s)`,
          dueDate: job.scheduledDate,
          priority: Priority.HIGH,
          daysPastDue: daysPastDue
        };
      });
    
    return of(alerts).pipe(delay(200));
  }

  // Maintenance Jobs
  getMaintenanceJobs(filters?: JobFilters): Observable<MaintenanceJob[]> {
    let filteredJobs = [...this.mockJobs];
    
    if (filters) {
      if (filters.status && filters.status.length > 0) {
        filteredJobs = filteredJobs.filter(job => filters.status!.includes(job.status));
      }
      
      if (filters.dateRange) {
        filteredJobs = filteredJobs.filter(job => 
          job.scheduledDate >= filters.dateRange!.start && 
          job.scheduledDate <= filters.dateRange!.end
        );
      }
      
      if (filters.searchTerm) {
        const searchTerm = filters.searchTerm.toLowerCase();
        filteredJobs = filteredJobs.filter(job => 
          job.machineName.toLowerCase().includes(searchTerm) ||
          job.project.toLowerCase().includes(searchTerm) ||
          job.reason.toLowerCase().includes(searchTerm)
        );
      }
    }
    
    return of(filteredJobs).pipe(delay(400));
  }

  getMaintenanceJob(jobId: string): Observable<MaintenanceJob> {
    const job = this.mockJobs.find(j => j.id === jobId);
    if (job) {
      return of(job).pipe(delay(200));
    }
    throw new Error('Maintenance job not found');
  }

  createMaintenanceJob(job: Partial<MaintenanceJob>): Observable<MaintenanceJob> {
    const newJob: MaintenanceJob = {
      id: (this.mockJobs.length + 1).toString(),
      machineId: job.machineId || '',
      machineName: job.machineName || '',
      serialNumber: job.serialNumber || '',
      project: job.project || '',
      scheduledDate: job.scheduledDate || new Date(),
      type: job.type || MaintenanceType.PREVENTIVE,
      status: job.status || MaintenanceStatus.SCHEDULED,
      assignedTo: job.assignedTo || [],
      estimatedHours: job.estimatedHours || 0,
      reason: job.reason || '',
      observations: job.observations,
      partsReplaced: job.partsReplaced,
      attachments: job.attachments || [],
      createdAt: new Date(),
      updatedAt: new Date()
    };
    
    this.mockJobs.push(newJob);
    return of(newJob).pipe(delay(500));
  }

  updateMaintenanceJob(jobId: string, job: Partial<MaintenanceJob>): Observable<MaintenanceJob> {
    const index = this.mockJobs.findIndex(j => j.id === jobId);
    if (index !== -1) {
      this.mockJobs[index] = { ...this.mockJobs[index], ...job, updatedAt: new Date() };
      return of(this.mockJobs[index]).pipe(delay(400));
    }
    throw new Error('Maintenance job not found');
  }

  updateJobStatus(jobId: string, status: MaintenanceStatus): Observable<void> {
    const job = this.mockJobs.find(j => j.id === jobId);
    if (job) {
      job.status = status;
      job.updatedAt = new Date();
      return of(void 0).pipe(delay(300));
    }
    throw new Error('Maintenance job not found');
  }

  deleteMaintenanceJob(jobId: string): Observable<void> {
    const index = this.mockJobs.findIndex(j => j.id === jobId);
    if (index !== -1) {
      this.mockJobs.splice(index, 1);
      return of(void 0).pipe(delay(300));
    }
    throw new Error('Maintenance job not found');
  }

  // Calendar and Timeline
  getMaintenanceJobsByDateRange(startDate: Date, endDate: Date): Observable<MaintenanceJob[]> {
    const filteredJobs = this.mockJobs.filter(job => 
      job.scheduledDate >= startDate && job.scheduledDate <= endDate
    );
    return of(filteredJobs).pipe(delay(300));
  }

  // Search and Filtering
  searchMaintenanceJobs(searchTerm: string): Observable<MaintenanceJob[]> {
    const filteredJobs = this.mockJobs.filter(job => 
      job.machineName.toLowerCase().includes(searchTerm.toLowerCase()) ||
      job.project.toLowerCase().includes(searchTerm.toLowerCase()) ||
      job.reason.toLowerCase().includes(searchTerm.toLowerCase())
    );
    return of(filteredJobs).pipe(delay(300));
  }

  // Analytics Methods
  getServiceComplianceData(): Observable<ServiceComplianceData> {
    // Calculate service compliance based on completed vs overdue jobs in the last 30 days
    const thirtyDaysAgo = new Date();
    thirtyDaysAgo.setDate(thirtyDaysAgo.getDate() - 30);
    
    const recentJobs = this.mockJobs.filter(job => 
      job.scheduledDate >= thirtyDaysAgo && 
      (job.status === MaintenanceStatus.COMPLETED || job.status === MaintenanceStatus.OVERDUE)
    );
    
    const onTimeJobs = recentJobs.filter(job => job.status === MaintenanceStatus.COMPLETED).length;
    const overdueJobs = recentJobs.filter(job => job.status === MaintenanceStatus.OVERDUE).length;
    const total = onTimeJobs + overdueJobs;
    
    const complianceData: ServiceComplianceData = {
      onTime: onTimeJobs,
      overdue: overdueJobs,
      percentage: total > 0 ? Math.round((onTimeJobs / total) * 100) : 0
    };
    
    return of(complianceData).pipe(delay(400));
  }

  getMTBFMetrics(): Observable<MTBFMetrics[]> {
    // Generate MTBF data for different machine types based on historical failure data
    // MTBF is calculated as total operating hours divided by number of failures
    const machineTypes = [
      { type: 'Excavator', baseOperatingHours: 1200, baseFailures: 5 },
      { type: 'Bulldozer', baseOperatingHours: 1080, baseFailures: 6 },
      { type: 'Drill Rig', baseOperatingHours: 960, baseFailures: 3 },
      { type: 'Loader', baseOperatingHours: 1260, baseFailures: 6 },
      { type: 'Crane', baseOperatingHours: 870, baseFailures: 3 },
      { type: 'Dump Truck', baseOperatingHours: 1320, baseFailures: 8 },
      { type: 'Grader', baseOperatingHours: 825, baseFailures: 3 },
      { type: 'Compactor', baseOperatingHours: 780, baseFailures: 4 },
      { type: 'Backhoe', baseOperatingHours: 900, baseFailures: 4 },
      { type: 'Forklift', baseOperatingHours: 620, baseFailures: 2 }
    ];

    const mtbfMetrics: MTBFMetrics[] = machineTypes.map(machine => {
      // Add some realistic variance to the data
      const operatingHours = machine.baseOperatingHours + Math.floor(Math.random() * 200) - 100;
      const failures = Math.max(1, machine.baseFailures + Math.floor(Math.random() * 3) - 1);
      const mtbfHours = Math.round(operatingHours / failures);

      return {
        machineType: machine.type,
        mtbfHours: mtbfHours,
        periodMonths: 6,
        failureCount: failures
      };
    });

    // Sort by MTBF hours descending (better performing machines first)
    mtbfMetrics.sort((a, b) => b.mtbfHours - a.mtbfHours);

    return of(mtbfMetrics).pipe(delay(500));
  }



  getPartsUsageData(): Observable<PartsUsageData[]> {
    // Mock data showing which parts are used most often and their costs
    const partsData: PartsUsageData[] = [
      {
        partName: 'Oil Filter',
        usageCount: 45,
        totalCost: 2250.00,
        machineTypes: ['Excavator', 'Bulldozer', 'Loader', 'Dump Truck']
      },
      {
        partName: 'Air Filter',
        usageCount: 38,
        totalCost: 1140.00,
        machineTypes: ['Excavator', 'Bulldozer', 'Grader']
      },
      {
        partName: 'Hydraulic Hose',
        usageCount: 32,
        totalCost: 4800.00,
        machineTypes: ['Excavator', 'Crane', 'Backhoe']
      },
      {
        partName: 'Brake Pads',
        usageCount: 28,
        totalCost: 3360.00,
        machineTypes: ['Dump Truck', 'Loader', 'Grader']
      },
      {
        partName: 'Engine Oil',
        usageCount: 52,
        totalCost: 3120.00,
        machineTypes: ['All Types']
      },
      {
        partName: 'Fuel Filter',
        usageCount: 41,
        totalCost: 1230.00,
        machineTypes: ['Excavator', 'Bulldozer', 'Dump Truck', 'Grader']
      },
      {
        partName: 'Hydraulic Cylinders',
        usageCount: 15,
        totalCost: 7500.00,
        machineTypes: ['Excavator', 'Backhoe', 'Crane']
      },
      {
        partName: 'Belts',
        usageCount: 24,
        totalCost: 1200.00,
        machineTypes: ['Compactor', 'Grader', 'Loader']
      },
      {
        partName: 'Bearings',
        usageCount: 19,
        totalCost: 2850.00,
        machineTypes: ['Drill Rig', 'Crane', 'Compactor']
      },
      {
        partName: 'Seals',
        usageCount: 35,
        totalCost: 1750.00,
        machineTypes: ['Excavator', 'Bulldozer', 'Backhoe']
      },
      {
        partName: 'Transmission Fluid',
        usageCount: 22,
        totalCost: 1320.00,
        machineTypes: ['Dump Truck', 'Loader', 'Grader']
      },
      {
        partName: 'Coolant',
        usageCount: 29,
        totalCost: 870.00,
        machineTypes: ['All Types']
      },
      {
        partName: 'Tires',
        usageCount: 16,
        totalCost: 9600.00,
        machineTypes: ['Dump Truck', 'Loader', 'Forklift']
      },
      {
        partName: 'Tracks',
        usageCount: 8,
        totalCost: 12000.00,
        machineTypes: ['Excavator', 'Bulldozer']
      },
      {
        partName: 'Battery',
        usageCount: 12,
        totalCost: 1800.00,
        machineTypes: ['All Types']
      },
      {
        partName: 'Alternator',
        usageCount: 7,
        totalCost: 2100.00,
        machineTypes: ['Dump Truck', 'Crane', 'Grader']
      },
      {
        partName: 'Starter Motor',
        usageCount: 9,
        totalCost: 2700.00,
        machineTypes: ['Excavator', 'Bulldozer', 'Loader']
      },
      {
        partName: 'Water Pump',
        usageCount: 11,
        totalCost: 1650.00,
        machineTypes: ['All Types']
      },
      {
        partName: 'Radiator',
        usageCount: 6,
        totalCost: 1800.00,
        machineTypes: ['Dump Truck', 'Bulldozer', 'Grader']
      },
      {
        partName: 'Turbocharger',
        usageCount: 4,
        totalCost: 6000.00,
        machineTypes: ['Excavator', 'Dump Truck']
      }
    ];

    // Sort by usage count descending
    partsData.sort((a, b) => b.usageCount - a.usageCount);

    return of(partsData).pipe(delay(450));
  }

  getUsageMetrics(machineId?: string): Observable<UsageMetrics[]> {
    // Generate usage metrics for machines
    const machines = [
      { id: 'M001', name: 'Excavator CAT 320' },
      { id: 'M002', name: 'Bulldozer Komatsu D65' },
      { id: 'M003', name: 'Drill Rig Atlas Copco' },
      { id: 'M004', name: 'Loader Volvo L120' },
      { id: 'M005', name: 'Crane Liebherr LTM' },
      { id: 'M006', name: 'Excavator Hitachi ZX350' },
      { id: 'M007', name: 'Dump Truck Caterpillar 777' },
      { id: 'M008', name: 'Grader John Deere 872G' },
      { id: 'M009', name: 'Compactor Bomag BW213' },
      { id: 'M010', name: 'Backhoe JCB 3CX' },
      { id: 'M011', name: 'Forklift Toyota 8FG25' },
      { id: 'M012', name: 'Crusher Metso LT200HP' },
      { id: 'M013', name: 'Screener Powerscreen Warrior' },
      { id: 'M014', name: 'Generator Caterpillar C18' },
      { id: 'M015', name: 'Air Compressor Atlas Copco GA75' },
      { id: 'M016', name: 'Welder Miller Dynasty 400' },
      { id: 'M017', name: 'Concrete Pump Putzmeister BSA' },
      { id: 'M018', name: 'Tower Crane Potain MDT' },
      { id: 'M019', name: 'Mobile Crane Grove GMK' },
      { id: 'M020', name: 'Pile Driver ICE 44' }
    ];

    let targetMachines = machines;
    if (machineId) {
      targetMachines = machines.filter(m => m.id === machineId);
    }

    const usageMetrics: UsageMetrics[] = targetMachines.map(machine => {
      // Generate realistic usage patterns based on machine type
      const baseEngineHours = 800 + Math.floor(Math.random() * 1200); // 800-2000 hours
      const baseIdleHours = Math.floor(baseEngineHours * (0.15 + Math.random() * 0.25)); // 15-40% idle
      const baseServiceHours = Math.floor(baseEngineHours * (0.02 + Math.random() * 0.03)); // 2-5% service

      return {
        machineId: machine.id,
        engineHours: baseEngineHours,
        idleHours: baseIdleHours,
        serviceHours: baseServiceHours,
        lastUpdated: new Date(Date.now() - Math.floor(Math.random() * 7 * 24 * 60 * 60 * 1000)) // Updated within last week
      };
    });

    return of(usageMetrics).pipe(delay(350));
  }

  // Notification Preferences
  getNotificationPreferences(): Observable<NotificationPreferences> {
    // Return comprehensive default notification preferences
    const preferences: NotificationPreferences = {
      emailNotifications: true,
      inAppNotifications: true,
      alertWindowDays: 30,
      overdueNotifications: true,
      
      // Advanced notification settings
      emailFrequency: 'immediate',
      quietHoursEnabled: false,
      quietHoursStart: '22:00',
      quietHoursEnd: '08:00',
      weekendNotifications: true,

      // Notification type preferences
      serviceDueEmail: true,
      serviceDueInApp: true,
      overdueEmail: true,
      overdueInApp: true,
      jobAssignedEmail: true,
      jobAssignedInApp: true,
      jobCompletedEmail: false,
      jobCompletedInApp: true,
      systemAlertsEmail: true,
      systemAlertsInApp: true,

      // Escalation settings
      escalationEnabled: false,
      escalationDelayHours: 24,
      escalationRecipients: []
    };
    
    return of(preferences).pipe(delay(300));
  }

  updateNotificationPreferences(preferences: NotificationPreferences): Observable<NotificationPreferences> {
    // In a real implementation, this would save to the backend
    // For now, just return the updated preferences
    return of(preferences).pipe(delay(400));
  }
}