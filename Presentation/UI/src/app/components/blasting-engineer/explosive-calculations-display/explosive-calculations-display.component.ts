import { Component, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { catchError } from 'rxjs/operators';
import { ExplosiveCalculationsService, ExplosiveCalculationResultDto } from '../../../core/services/explosive-calculations.service';

// Interfaces for explosive calculations
export interface ExplosiveCalculationInputs {
  numberOfHoles: number; // Number of holes
  numberOfColumns: number; // Number of columns
  holeDiameter: number;  // Hole diameter
  stemming: number;  // Stemming
  spacing: number; // Spacing
  burden: number;  // Burden
  emulsionDensity: number; // Emulsion density
  anfoeDensity: number; // ANFO density
  emulsionPerHole: number; // Emulsion per hole
  depths: number[]; // Array of hole depths
}

export interface ExplosiveCalculationResults {
  totalDepth: number;   // Total Depth
  averageDepth: number;   // Average Depth
  numberOfFilledHoles: number;   // Number of filled holes
  emulsionPerMeter: number;   // Emulsion per Meter
  anfoPerMeter: number;   // ANFO per Meter
  emulsionCoveringSpace: number;   // Emulsion Covering Space
  remainingSpace: number;   // Remaining Space
  anfoCoveringSpace: number;   // ANFO Covering Space
  totalAnfo: number;   // Total ANFO
  totalEmulsion: number;   // Total Emulsion
  totalVolume: number;   // Total Volume
}

@Component({
  selector: 'app-explosive-calculations-display',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './explosive-calculations-display.component.html',
  styleUrl: './explosive-calculations-display.component.scss'
})
export class ExplosiveCalculationsDisplayComponent implements OnInit {
  @Input() calculationInputs: ExplosiveCalculationInputs | null = null;
  @Input() calculationResults: ExplosiveCalculationResults | null = null;
  
  // Component state
  isLoading = false;
  hasError = false;
  errorMessage = '';
  
  // Route context
  projectId: number | null = null;
  siteId: number | null = null;
  
  // Backend data
  savedCalculations: ExplosiveCalculationResultDto[] = [];
  currentCalculationId: number | null = null;
  
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private explosiveCalculationsService: ExplosiveCalculationsService,
    private snackBar: MatSnackBar
  ) {}
  
  ngOnInit(): void {
    // Extract project and site IDs from route parameters
    this.route.params.subscribe(params => {
      if (params['projectId']) {
        this.projectId = +params['projectId'];
        console.log('Explosive calculations - Project ID:', this.projectId);
      }
      if (params['siteId']) {
        this.siteId = +params['siteId'];
        console.log('Explosive calculations - Site ID:', this.siteId);
      }
      
      // Load saved calculations from database
      if (this.projectId && this.siteId) {
        this.loadSavedCalculations();
        this.loadLatestCalculation();
      }
    });
  }
  
  /**
   * Load the latest calculation automatically
   */
  loadLatestCalculation(): void {
    if (!this.projectId || !this.siteId) return;
    
    this.explosiveCalculationsService.getLatestByProjectAndSite(this.projectId, this.siteId)
      .subscribe({
        next: (calculation) => {
          if (calculation) {
            this.loadCalculationData(calculation);
          }
        },
        error: (error) => {
          console.error('Error loading latest calculation:', error);
        }
      });
  }
  
  /**
   * Load calculation data into the component
   */
  private loadCalculationData(calculation: ExplosiveCalculationResultDto): void {
    // Map database calculation to component interfaces
    this.calculationInputs = {
      numberOfHoles: calculation.numberOfFilledHoles,
      numberOfColumns: 0, // Not stored in database
      holeDiameter: 0, // Not available in ExplosiveCalculationResultDto
      stemming: 0, // Not available in ExplosiveCalculationResultDto
      spacing: 0, // Not available in ExplosiveCalculationResultDto
      burden: 0, // Not available in ExplosiveCalculationResultDto
      emulsionDensity: calculation.emulsionDensity,
      anfoeDensity: calculation.anfoDensity,
      emulsionPerHole: calculation.emulsionPerHole,
      depths: [] // Not stored as array in database
    };
    
    this.calculationResults = {
      totalDepth: calculation.totalDepth,
      averageDepth: calculation.averageDepth,
      numberOfFilledHoles: calculation.numberOfFilledHoles,
      emulsionPerMeter: calculation.emulsionPerMeter,
      anfoPerMeter: calculation.anfoPerMeter,
      emulsionCoveringSpace: calculation.emulsionCoveringSpace,
      remainingSpace: calculation.remainingSpace,
      anfoCoveringSpace: calculation.anfoCoveringSpace,
      totalAnfo: calculation.totalAnfo,
      totalEmulsion: calculation.totalEmulsion,
      totalVolume: calculation.totalVolume
    };
    
    this.currentCalculationId = calculation.id;
    console.log('Loaded calculation:', calculation);
  }
  
  formatNumber(value: number, decimals: number = 2): string {
    return value?.toFixed(decimals) || '0.00';
  }
  
  getUnit(key: string): string {
    const units: { [key: string]: string } = {
      'totalDepth': 'm',
      'averageDepth': 'm',
      'numberOfFilledHoles': 'holes',
      'emulsionPerMeter': 'tons/m',
      'anfoPerMeter': 'tons/m',
      'emulsionCoveringSpace': 'm',
      'remainingSpace': 'm',
      'anfoCoveringSpace': 'tons',
      'totalAnfo': 'tons',
      'totalEmulsion': 'tons',
      'totalVolume': 'm³'
    };
    return units[key] || '';
  }
  
  getDisplayName(key: string): string {
    const displayNames: { [key: string]: string } = {
      'totalDepth': 'Total Depth',
      'averageDepth': 'Average Depth',
      'numberOfFilledHoles': 'Filled Holes',
      'emulsionPerMeter': 'Emulsion per Meter',
      'anfoPerMeter': 'ANFO per Meter',
      'emulsionCoveringSpace': 'Emulsion Coverage',
      'remainingSpace': 'Remaining Space',
      'anfoCoveringSpace': 'ANFO Coverage',
      'totalAnfo': 'Total ANFO',
      'totalEmulsion': 'Total Emulsion',
      'totalVolume': 'Total Volume'
    };
    return displayNames[key] || key;
  }
  
  getDetailedResults(): any[] {
    if (!this.calculationResults) {
      return [];
    }
    
    return Object.entries(this.calculationResults).map(([key, value]) => ({
      parameter: this.getDisplayName(key),
      value: this.formatNumber(value as number),
      unit: this.getUnit(key),
      icon: this.getParameterIcon(this.getDisplayName(key))
    }));
  }

  getParameterIcon(parameterName: string): string {
    const iconMap: { [key: string]: string } = {
      'Total Depth': 'height',
      'Average Depth': 'straighten',
      'Filled Holes': 'construction',
      'Emulsion per Meter': 'science',
      'ANFO per Meter': 'local_fire_department',
      'Emulsion Coverage': 'layers',
      'Remaining Space': 'space_bar',
      'ANFO Coverage': 'whatshot',
      'Total ANFO': 'local_fire_department',
      'Total Emulsion': 'science',
      'Total Volume': 'view_in_ar'
    };
    return iconMap[parameterName] || 'info';
  }

  loadSavedCalculations(): void {
    if (!this.projectId || !this.siteId) {
      console.error('Project ID and Site ID are required for loading saved calculations');
      return;
    }

    console.log('Loading saved calculations for project:', this.projectId, 'site:', this.siteId);
    
    this.explosiveCalculationsService.getByProjectAndSite(this.projectId, this.siteId).subscribe({
      next: (calculations) => {
        this.savedCalculations = calculations;
        console.log('Loaded saved calculations:', calculations);
      },
      error: (error) => {
        console.error('Error loading saved calculations:', error);
        this.errorMessage = 'Failed to load saved calculations.';
      }
    });
  }

  loadSavedCalculation(calculationId: number): void {
    console.log('Loading calculation with ID:', calculationId);
    
    this.explosiveCalculationsService.getById(calculationId).subscribe({
      next: (calculation) => {
        if (calculation) {
          this.loadCalculationData(calculation);
        }
      },
      error: (error) => {
        console.error('Error loading calculation:', error);
        this.errorMessage = 'Failed to load the selected calculation.';
      }
    });
  }

  deleteSavedCalculation(calculationId: number): void {
    if (confirm('Are you sure you want to delete this calculation?')) {
      this.explosiveCalculationsService.delete(calculationId).subscribe({
        next: () => {
          this.snackBar.open('Calculation deleted successfully', 'Close', { duration: 3000 });
          this.loadSavedCalculations(); // Refresh the list
          
          // If the deleted calculation was currently loaded, clear the display
          if (this.currentCalculationId === calculationId) {
            this.calculationInputs = null;
            this.calculationResults = null;
            this.currentCalculationId = null;
          }
        },
        error: (error) => {
          console.error('Error deleting calculation:', error);
          this.snackBar.open('Failed to delete calculation', 'Close', { duration: 3000 });
        }
      });
    }
  }

  getDataSourceInfo(): string {
    return `Displaying saved calculations from database`;
  }

  isBackendDataLoaded(): boolean {
    return this.projectId !== null && this.siteId !== null;
  }

  navigateToPatternCreator(): void {
    if (this.projectId && this.siteId) {
      this.router.navigate(['/blasting-engineer/project-management', this.projectId, 'sites', this.siteId, 'pattern-creator']);
    }
  }

  navigateToSequenceDesigner(): void {
    if (this.projectId && this.siteId) {
      this.router.navigate(['/blasting-engineer/project-management', this.projectId, 'sites', this.siteId, 'sequence-designer']);
    }
  }
}
