import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { ProjectSite } from '../../../core/models/project.model';
import { SiteService } from '../../../core/services/site.service';
import { UnifiedDrillDataService } from '../../../core/services/unified-drill-data.service';
import { DrillLocation } from '../../../core/models/drilling.model';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { TagModule } from 'primeng/tag';

@Component({
  selector: 'app-site-details',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule, TableModule, ButtonModule, TagModule],
  templateUrl: './site-details.component.html',
  styleUrl: './site-details.component.scss'
})
export class SiteDetailsComponent implements OnInit {
  siteId: number = 0;
  projectId: number = 0;
  site: ProjectSite | null = null;
  loading = true;
  error: string | null = null;

  // Drill Points Data
  drillPoints: DrillLocation[] = [];
  loadingDrillPoints = false;

  // View States
  activeTab: 'overview' | 'drill-points' = 'overview';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private siteService: SiteService,
    private drillDataService: UnifiedDrillDataService
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.siteId = +params['siteId'];
      this.loadSiteDetails();
    });
  }

  private loadSiteDetails(): void {
    this.loading = true;
    this.siteService.getSite(this.siteId).subscribe({
      next: (site) => {
        this.site = site;
        this.projectId = site.projectId;
        this.loading = false;
        // Load drill points after we have projectId
        this.loadDrillPoints();
      },
      error: (err) => {
        this.error = 'Failed to load site details';
        this.loading = false;
      }
    });
  }

  private loadDrillPoints(): void {
    if (!this.projectId || !this.siteId) {
      console.error('Project ID or Site ID not available');
      return;
    }

    this.loadingDrillPoints = true;
    this.drillDataService.getDrillPoints(this.projectId, this.siteId).subscribe({
      next: (drillPoints) => {
        // Sort drill points numerically by extracting the number from the ID
        this.drillPoints = drillPoints.sort((a, b) => {
          const numA = this.extractNumberFromId(a.id);
          const numB = this.extractNumberFromId(b.id);
          return numA - numB;
        });
        this.loadingDrillPoints = false;
        console.log('Drill points loaded:', drillPoints.length);
      },
      error: (error) => {
        console.error('Error loading drill points:', error);
        this.loadingDrillPoints = false;
      }
    });
  }

  private extractNumberFromId(id: string): number {
    // Extract numeric part from IDs like "DH1", "DH10", "DH2"
    const match = id.match(/\d+/);
    return match ? parseInt(match[0], 10) : 0;
  }

  setActiveTab(tab: 'overview' | 'drill-points'): void {
    this.activeTab = tab;
  }

  goBack(): void {
    this.router.navigate(['/operator/my-project']);
  }

  getStatus(pointId: string): 'pending' | 'completed' {
    const point = this.drillPoints.find(p => p.id === pointId);
    return point?.isCompleted ? 'completed' : 'pending';
  }

  markAsCompleted(pointId: string): void {
    if (!this.projectId || !this.siteId) {
      console.error('Project ID or Site ID not available');
      return;
    }

    this.siteService.markDrillPointAsCompleted(pointId, this.projectId, this.siteId).subscribe({
      next: (response) => {
        if (response.success) {
          // Update the local drill point status
          const point = this.drillPoints.find(p => p.id === pointId);
          if (point) {
            point.isCompleted = true;
            point.completedAt = new Date();
          }
          console.log(`Drill point ${pointId} marked as completed`);
        }
      },
      error: (error) => {
        console.error('Error marking drill point as completed:', error);
        this.error = 'Failed to mark drill point as completed';
      }
    });
  }

  isCompleted(pointId: string): boolean {
    const point = this.drillPoints.find(p => p.id === pointId);
    return point?.isCompleted || false;
  }
}