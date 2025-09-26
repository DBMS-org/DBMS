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
  status: 'pending' | 'completed';
  x: number;
  y: number;
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

  // View States
  activeTab: 'overview' | 'drill-plan' = 'overview';

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
      { id: 'DH-002', depth: 11.8, burden: 3.5, spacing: 4.0, stemming: 2.8, chargeAmount: 24.2, status: 'pending', x: 104, y: 150 },
      { id: 'DH-003', depth: 13.2, burden: 3.5, spacing: 4.0, stemming: 3.2, chargeAmount: 26.8, status: 'pending', x: 108, y: 150 },
      { id: 'DH-004', depth: 12.0, burden: 3.5, spacing: 4.0, stemming: 3.0, chargeAmount: 25.0, status: 'pending', x: 112, y: 150 }
    ];
  }

  setActiveTab(tab: 'overview' | 'drill-plan'): void {
    this.activeTab = tab;
  }

  markDrillHoleCompleted(holeId: string): void {
    const hole = this.drillHoles.find(h => h.id === holeId);
    if (hole && hole.status === 'pending') {
      hole.status = 'completed';
    }
  }

  goBack(): void {
    this.router.navigate(['/operator/my-project']);
  }
}