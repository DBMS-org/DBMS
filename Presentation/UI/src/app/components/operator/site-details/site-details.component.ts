import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { ProjectSite } from '../../../core/models/project.model';
import { SiteService } from '../../../core/services/site.service';

// Drill Plan interfaces
export interface DrillHole {
  id: string;
  depth: number;
  burden: number;
  spacing: number;
  stemming: number;
  chargeAmount: number;
  status: 'pending' | 'drilled' | 'completed';
  x: number;
  y: number;
}

export interface BlastSequence {
  holeId: string;
  sequence: number;
  delay: number;
  firingOrder: number;
  chargeType: string;
  timing: string;
}

export interface SafetyChecklistItem {
  id: string;
  description: string;
  isCompleted: boolean;
  isRequired: boolean;
}

@Component({
  selector: 'app-site-details',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './site-details.component.html',
  styleUrl: './site-details.component.scss'
})
export class SiteDetailsComponent implements OnInit {
  siteId: number = 0;
  site: ProjectSite | null = null;
  loading = true;
  error: string | null = null;

  // Drill Plan Data
  drillHoles: DrillHole[] = [];
  blastSequence: BlastSequence[] = [];
  
  // Safety Checklist
  safetyChecklist: SafetyChecklistItem[] = [
    { id: '1', description: 'Verify all personnel are at safe distance (minimum 500m)', isCompleted: false, isRequired: true },
    { id: '2', description: 'Check weather conditions (wind speed < 15 km/h)', isCompleted: false, isRequired: true },
    { id: '3', description: 'Confirm blast area is clear of equipment and vehicles', isCompleted: false, isRequired: true },
    { id: '4', description: 'Verify explosive charges are properly connected', isCompleted: false, isRequired: true },
    { id: '5', description: 'Test detonation system and backup systems', isCompleted: false, isRequired: true },
    { id: '6', description: 'Confirm emergency evacuation routes are clear', isCompleted: false, isRequired: true },
    { id: '7', description: 'Radio communication with all team members established', isCompleted: false, isRequired: true },
    { id: '8', description: 'Environmental impact assessment completed', isCompleted: false, isRequired: false }
  ];

  // Task Management
  drillingTasks: any[] = [];
  blastingTasks: any[] = [];

  // View States
  activeTab: 'overview' | 'drill-plan' | 'blast-sequence' | 'safety' | 'tasks' = 'overview';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private siteService: SiteService
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.siteId = +params['siteId'];
      this.loadSiteDetails();
      this.loadDrillPlan();
      this.loadBlastSequence();
      this.loadTasks();
    });
  }

  private loadSiteDetails(): void {
    this.loading = true;
    this.siteService.getSite(this.siteId).subscribe({
      next: (site) => {
        this.site = site;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load site details';
        this.loading = false;
      }
    });
  }

  private loadDrillPlan(): void {
    // Mock data - replace with actual service call
    this.drillHoles = [
      { id: 'DH-001', depth: 12.5, burden: 3.5, spacing: 4.0, stemming: 3.0, chargeAmount: 25.5, status: 'completed', x: 100, y: 150 },
      { id: 'DH-002', depth: 11.8, burden: 3.5, spacing: 4.0, stemming: 2.8, chargeAmount: 24.2, status: 'drilled', x: 104, y: 150 },
      { id: 'DH-003', depth: 13.2, burden: 3.5, spacing: 4.0, stemming: 3.2, chargeAmount: 26.8, status: 'pending', x: 108, y: 150 },
      { id: 'DH-004', depth: 12.0, burden: 3.5, spacing: 4.0, stemming: 3.0, chargeAmount: 25.0, status: 'pending', x: 112, y: 150 }
    ];
  }

  private loadBlastSequence(): void {
    // Mock data - replace with actual service call
    this.blastSequence = [
      { holeId: 'DH-001', sequence: 1, delay: 0, firingOrder: 1, chargeType: 'ANFO', timing: '00:00:00' },
      { holeId: 'DH-002', sequence: 2, delay: 25, firingOrder: 2, chargeType: 'ANFO', timing: '00:00:25' },
      { holeId: 'DH-003', sequence: 3, delay: 50, firingOrder: 3, chargeType: 'Emulsion', timing: '00:00:50' },
      { holeId: 'DH-004', sequence: 4, delay: 75, firingOrder: 4, chargeType: 'ANFO', timing: '00:01:15' }
    ];
  }

  private loadTasks(): void {
    // Mock data - replace with actual service calls
    this.drillingTasks = [
      { id: 1, description: 'Drill hole DH-001', status: 'completed', assignedDate: new Date('2024-01-15') },
      { id: 2, description: 'Drill hole DH-002', status: 'completed', assignedDate: new Date('2024-01-16') },
      { id: 3, description: 'Drill hole DH-003', status: 'in-progress', assignedDate: new Date('2024-01-17') },
      { id: 4, description: 'Drill hole DH-004', status: 'pending', assignedDate: new Date('2024-01-18') }
    ];

    this.blastingTasks = [
      { id: 1, description: 'Load explosives in DH-001', status: 'pending', assignedDate: new Date('2024-01-20') },
      { id: 2, description: 'Connect detonation sequence', status: 'pending', assignedDate: new Date('2024-01-21') },
      { id: 3, description: 'Execute blast sequence', status: 'pending', assignedDate: new Date('2024-01-22') }
    ];
  }

  setActiveTab(tab: 'overview' | 'drill-plan' | 'blast-sequence' | 'safety' | 'tasks'): void {
    this.activeTab = tab;
  }

  markDrillHoleCompleted(holeId: string): void {
    const hole = this.drillHoles.find(h => h.id === holeId);
    if (hole && hole.status !== 'completed') {
      hole.status = hole.status === 'pending' ? 'drilled' : 'completed';
    }
  }

  markTaskCompleted(taskId: number, taskType: 'drilling' | 'blasting'): void {
    const tasks = taskType === 'drilling' ? this.drillingTasks : this.blastingTasks;
    const task = tasks.find(t => t.id === taskId);
    if (task) {
      task.status = task.status === 'completed' ? 'pending' : 'completed';
    }
  }

  toggleSafetyItem(itemId: string): void {
    const item = this.safetyChecklist.find(i => i.id === itemId);
    if (item) {
      item.isCompleted = !item.isCompleted;
    }
  }

  canExecuteBlast(): boolean {
    const requiredItems = this.safetyChecklist.filter(item => item.isRequired);
    return requiredItems.every(item => item.isCompleted);
  }

  executeBlast(): void {
    if (this.canExecuteBlast()) {
      // Implement blast execution logic
      alert('Blast sequence initiated successfully!');
    } else {
      alert('Please complete all required safety checklist items before executing blast.');
    }
  }

  goBack(): void {
    this.router.navigate(['/operator/my-project']);
  }
}