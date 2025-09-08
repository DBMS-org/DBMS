import { Component, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Observable, forkJoin } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { UnifiedDrillDataService, PatternData } from '../../../core/services/unified-drill-data.service';
import { SiteBlastingService } from '../../../core/services/site-blasting.service';
import { DrillLocation, PatternSettings } from '../../../core/models/drilling.model';
import { environment } from '../../../../environments/environment';
import { ExplosiveCalculationsService, ExplosiveCalculationResultDto } from '../../../core/services/explosive-calculations.service';
import { DuplicateCalculationDialogComponent, DuplicateCalculationDialogData, DuplicateCalculationDialogResult } from './duplicate-calculation-dialog/duplicate-calculation-dialog.component';

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
  imports: [CommonModule, ReactiveFormsModule, DuplicateCalculationDialogComponent],
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
  showInputForm = false;
  
  // Form
  explosiveForm: FormGroup;
  
  // Route context
  projectId: number | null = null;
  siteId: number | null = null;
  
  // Backend data
  drillData: DrillLocation[] = [];
  patternSettings: PatternSettings | null = null;
  savedCalculations: ExplosiveCalculationResultDto[] = [];
  currentCalculationId: number | null = null;
  
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private http: HttpClient,
    private fb: FormBuilder,
    private unifiedDrillDataService: UnifiedDrillDataService,
    private siteBlastingService: SiteBlastingService,
    private explosiveCalculationsService: ExplosiveCalculationsService,
    private dialog: MatDialog,
    private snackBar: MatSnackBar
  ) {
    this.explosiveForm = this.createExplosiveForm();
  }
  
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
      
      // Load data from backend when route parameters are available
      if (this.projectId && this.siteId) {
        this.loadBackendData();
        this.loadSavedCalculations();
      }
    });
    
    // Initialize component
    if (this.calculationInputs && !this.calculationResults) {
      this.performCalculations();
    }
  }
  
  /**
   * Load drilling data and pattern settings from backend
   */
  loadBackendData(): void {
    if (!this.projectId || !this.siteId) {
      console.error('Project ID and Site ID are required for data loading');
      return;
    }

    this.isLoading = true;
    this.hasError = false;

    console.log('🔄 Loading backend data for explosive calculations...', {
      projectId: this.projectId,
      siteId: this.siteId
    });

    // Load pattern data from UnifiedDrillDataService
    this.unifiedDrillDataService.loadPatternData(this.projectId, this.siteId)
      .pipe(
        catchError(error => {
          console.error('❌ Error loading pattern data:', error);
          this.hasError = true;
          this.errorMessage = 'Failed to load drilling pattern data from backend';
          this.isLoading = false;
          return [];
        })
      )
      .subscribe((patternData: PatternData | null) => {
        if (patternData) {
          this.drillData = patternData.drillLocations;
          this.patternSettings = patternData.settings;
          
          console.log('✅ Backend data loaded successfully:', {
            drillLocations: this.drillData.length,
            settings: this.patternSettings
          });
          
          // Convert backend data to calculation inputs and perform calculations
          this.convertBackendDataToInputs();
          this.performCalculations();
        } else {
          console.log('ℹ️ No pattern data found in backend');
          this.hasError = true;
          this.errorMessage = 'No drilling pattern data found for this project and site';
        }
        
        this.isLoading = false;
      });
  }

  /**
   * Convert backend drilling data to calculation inputs
   */
  convertBackendDataToInputs(): void {
    if (!this.drillData.length || !this.patternSettings) {
      console.warn('⚠️ Insufficient backend data for calculations');
      return;
    }

    // Extract depths from drill locations
    const depths = this.drillData.map(location => location.depth || 0);
    
    // Use pattern settings for basic parameters
    const settings = this.patternSettings;
    
    // Create calculation inputs with zero fallback values to identify missing backend data
    // These should ideally come from a separate explosive materials service
    this.calculationInputs = {
      numberOfHoles: this.drillData.length, // Number of holes
      numberOfColumns: Math.ceil(Math.sqrt(this.drillData.length)), // Estimated number of columns
      holeDiameter: settings.diameter || 0, // Hole diameter from settings (0 if missing from backend)
      stemming: settings.stemming || 0, // Stemming from settings (0 if missing from backend)
      spacing: settings.spacing || 0, // Spacing from settings (0 if missing from backend)
      burden: settings.burden || 0, // Burden from settings (0 if missing from backend)
      emulsionDensity: 0, // Emulsion density - not from backend, set to 0
      anfoeDensity: 0, // ANFO density - not from backend, set to 0
      emulsionPerHole: 0, // Emulsion per hole - not from backend, set to 0
      depths: depths
    };

    console.log('🔄 Converted backend data to calculation inputs:', this.calculationInputs);
  }

  /**
   * Perform explosive calculations based on input data
   */
  performCalculations(): void {
    if (!this.calculationInputs) {
      this.hasError = true;
      this.errorMessage = 'No calculation inputs provided';
      return;
    }
    
    try {
      this.isLoading = true;
      this.hasError = false;
      
      const inputs = this.calculationInputs;
      
      // Validate inputs
      if (inputs.numberOfHoles <= 0 || inputs.holeDiameter <= 0) {
        throw new Error('Please enter valid values for number of holes and hole diameter');
      }
      
      if (inputs.depths.length !== inputs.numberOfHoles) {
        throw new Error('Number of depth inputs must match the number of holes');
      }
      
      // Calculate derived quantities
      
      // 1. Total Depth
      const totalDepth = inputs.depths.reduce((sum, depth) => sum + (depth > 0 ? depth : 0), 0);
      
      // 2. Average Depth
      const numberOfFilledHoles = inputs.depths.filter(depth => depth > 0).length;
      const averageDepth = numberOfFilledHoles > 0 ? totalDepth / numberOfFilledHoles : 0;
      
      // 3. Emulsion per Meter - Convert g/cm³ to kg/m³ by multiplying by 1000
      const emulsionPerMeter = (22/7000) * Math.pow(inputs.holeDiameter/2, 2) * (inputs.emulsionDensity * 1000);
      
      // 4. ANFO per Meter - Convert g/cm³ to kg/m³ by multiplying by 1000
      const anfoPerMeter = (22/7000) * Math.pow(inputs.holeDiameter/2, 2) * (inputs.anfoeDensity * 1000);
      
      // 5. Emulsion Covering Space - using emulsion per hole
      const emulsionCoveringSpace = emulsionPerMeter > 0 ? inputs.emulsionPerHole / emulsionPerMeter : 0;
      
      // 6. Remaining Space
      const remainingSpace = averageDepth - (inputs.stemming + emulsionCoveringSpace);
      
      // 7. ANFO Covering Space
      const anfoCoveringSpace = anfoPerMeter * Math.max(0, remainingSpace);
      
      // 8. Total ANFO
      const totalAnfo = anfoCoveringSpace * inputs.numberOfHoles;
      
      // 9. Total Emulsion - New formula: Emulsion Covering space * number of holes
      const totalEmulsion = emulsionCoveringSpace * inputs.numberOfHoles;
      
      // 10. Total Volume
      const totalVolume = averageDepth * inputs.numberOfHoles * inputs.spacing * inputs.burden;
      
      // Set calculation results
      this.calculationResults = {
        totalDepth, averageDepth, numberOfFilledHoles, emulsionPerMeter, anfoPerMeter, 
        emulsionCoveringSpace, remainingSpace, anfoCoveringSpace, totalAnfo, totalEmulsion, totalVolume
      };
      
    } catch (error: any) {
      this.hasError = true;
      this.errorMessage = error.message || 'Error in calculation';
    } finally {
      this.isLoading = false;
    }
  }
  
  /**
   * Format number for display
   */
  formatNumber(value: number, decimals: number = 2): string {
    return value.toFixed(decimals);
  }
  
  /**
   * Get unit for specific calculation result
   */
  getUnit(key: string): string {
    const units: { [key: string]: string } = {
      'totalDepth': 'm',
      'averageDepth': 'm',
      'numberOfFilledHoles': '',
      'emulsionPerMeter': 'kg/m',
      'anfoPerMeter': 'kg/m',
      'emulsionCoveringSpace': 'm',
      'remainingSpace': 'm',
      'anfoCoveringSpace': 'kg',
      'totalAnfo': 'kg',
      'totalEmulsion': 'kg',
      'totalVolume': 'm³'
    };
    return units[key] || '';
  }
  
  /**
   * Get display name for calculation result
   */
  getDisplayName(key: string): string {
    const names: { [key: string]: string } = {
      'totalDepth': 'Total Depth',
      'averageDepth': 'Average Depth',
      'numberOfFilledHoles': 'Number of Filled Holes',
      'emulsionPerMeter': 'Emulsion per Meter',
      'anfoPerMeter': 'ANFO per Meter',
      'emulsionCoveringSpace': 'Emulsion Covering Space',
      'remainingSpace': 'Remaining Space',
      'anfoCoveringSpace': 'ANFO Covering Space',
      'totalAnfo': 'Total ANFO',
      'totalEmulsion': 'Total Emulsion',
      'totalVolume': 'Total Volume'
    };
    return names[key] || key;
  }
  
  /**
   * Get detailed results for table display
   */
  getDetailedResults(): any[] {
    if (!this.calculationResults) {
      return [];
    }
    
    const descriptions: { [key: string]: string } = {
      'totalDepth': 'Sum of all hole depths',
      'averageDepth': 'Average depth of filled holes',
      'numberOfFilledHoles': 'Count of holes with depth > 0',
      'emulsionPerMeter': 'Emulsion weight per meter of hole',
      'anfoPerMeter': 'ANFO weight per meter of hole',
      'emulsionCoveringSpace': 'Space covered by emulsion in each hole',
      'remainingSpace': 'Space remaining after stemming and emulsion',
      'anfoCoveringSpace': 'Total ANFO weight for covering remaining space',
      'totalAnfo': 'Total ANFO required for all holes',
      'totalEmulsion': 'Total emulsion calculated as covering space times number of holes',
      'totalVolume': 'Total volume of blast area'
    };
    
    const highlightKeys = ['totalDepth', 'averageDepth', 'totalAnfo', 'totalEmulsion', 'totalVolume'];
    
    return Object.keys(this.calculationResults).map(key => ({
      name: this.getDisplayName(key),
      value: this.formatNumber((this.calculationResults as any)[key], key === 'numberOfFilledHoles' ? 0 : 2),
      unit: this.getUnit(key),
      description: descriptions[key] || '',
      isHighlight: highlightKeys.includes(key)
    }));
  }

  /**
   * Get appropriate icon for parameter name
   */
  getParameterIcon(parameterName: string): string {
    const iconMap: { [key: string]: string } = {
      'Total Depth': 'height',
      'Average Depth': 'straighten',
      'Number of Filled Holes': 'radio_button_checked',
      'Total Volume': 'view_in_ar',
      'Emulsion per Meter': 'opacity',
      'ANFO per Meter': 'science',
      'Emulsion Covering Space': 'space_bar',
      'Remaining Space': 'unfold_more',
      'ANFO Covering Space': 'layers',
      'Total ANFO': 'local_fire_department',
      'Total Emulsion 65%': 'water_drop',
      'Total Emulsion 50%': 'water_drop',
      'Total Emulsion': 'opacity'
    };
    
    return iconMap[parameterName] || 'info';
  }

  /**
   * Save calculation results to backend using the new service
   */
  saveCalculationResults(): void {
    if (!this.calculationResults || !this.calculationInputs || !this.projectId || !this.siteId) {
      console.warn('Cannot save: Missing calculation results or project/site context');
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';

    const calculationData = {
      projectId: this.projectId,
      siteId: this.siteId,
      inputs: this.calculationInputs,
      results: this.calculationResults,
      notes: `Calculation based on ${this.getDataSourceInfo()}`
    };

    this.explosiveCalculationsService.saveCalculationResults(
       this.calculationInputs,
       this.calculationResults,
       this.projectId,
       this.siteId
     ).subscribe({
         next: (response) => {
           console.log('Calculation results saved successfully:', response);
           this.currentCalculationId = response.id;
           this.loadSavedCalculations(); // Refresh the saved calculations list
           this.isLoading = false;
           // Show success message
           this.errorMessage = '';
         },
         error: (error) => {
           console.error('Error saving calculation results:', error);
           
           // Check if the error is due to existing calculation
           if (error.status === 409 && error.error?.error === 'EXISTING_CALCULATION_FOUND') {
             this.isLoading = false;
             this.showExistingCalculationConfirmation();
           } else {
             this.errorMessage = 'Failed to save calculation results. Please try again.';
             this.isLoading = false;
           }
         }
       });
  }

  /**
   * Show confirmation dialog for existing calculations
   */
  showExistingCalculationConfirmation(): void {
    // Get existing calculation info for better user experience
    const existingCalculation = this.savedCalculations.find(calc => 
      calc.projectId === this.projectId && calc.siteId === this.siteId
    );

    const dialogData: DuplicateCalculationDialogData = {
      projectId: this.projectId!,
      siteId: this.siteId!,
      existingCalculationId: existingCalculation?.id?.toString(),
      existingCalculationDate: existingCalculation?.createdAt
    };

    const dialogRef = this.dialog.open(DuplicateCalculationDialogComponent, {
      width: '600px',
      maxWidth: '90vw',
      data: dialogData,
      disableClose: true
    });

    dialogRef.afterClosed().subscribe((result: DuplicateCalculationDialogResult) => {
      if (result) {
        switch (result.action) {
          case 'replace':
            this.saveCalculationResultsWithConfirmation();
            break;
          case 'view_existing':
            this.loadExistingCalculation(existingCalculation);
            break;
          case 'cancel':
          default:
            // Do nothing, user cancelled
            break;
        }
      }
    });
   }

  /**
   * Loads an existing calculation into the form for viewing/editing
   */
  private loadExistingCalculation(calculation: any): void {
    if (calculation) {
      // Load the calculation using the existing load functionality
      this.loadSavedCalculation(calculation.id);
      
      // Show a success message
      this.snackBar.open(
        `Loaded existing calculation: ${calculation.id}`,
        'Close',
        { duration: 3000 }
      );
    }
  }

  /**
   * Saves calculation results with confirmation (overwrite existing)
   */
  saveCalculationResultsWithConfirmation(): void {
    if (!this.calculationResults || !this.calculationInputs || !this.projectId || !this.siteId) {
      console.warn('Cannot save: Missing calculation results or project/site context');
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';

    this.explosiveCalculationsService.saveCalculationResultsWithConfirmation(
       this.calculationInputs,
       this.calculationResults,
       this.projectId,
       this.siteId
     ).subscribe({
         next: (response) => {
           console.log('Calculation results saved with confirmation:', response);
           this.currentCalculationId = response.id;
           this.loadSavedCalculations(); // Refresh the saved calculations list
           this.isLoading = false;
           // Show success message
           this.errorMessage = '';
           alert('Calculation results saved successfully! Previous calculation has been overwritten.');
         },
         error: (error) => {
           console.error('Error saving calculation results with confirmation:', error);
           this.errorMessage = 'Failed to save calculation results. Please try again.';
           this.isLoading = false;
         }
       });
  }

  /**
   * Load all saved calculations for the current project and site
   */
  loadSavedCalculations(): void {
    if (!this.projectId || !this.siteId) return;

    this.explosiveCalculationsService.getByProjectAndSite(this.projectId, this.siteId)
      .subscribe({
        next: (calculations) => {
          // Ensure calculations is an array before sorting
          if (Array.isArray(calculations)) {
            this.savedCalculations = calculations.sort((a, b) => 
              new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime()
            );
          } else {
            console.warn('API returned non-array response:', calculations);
            this.savedCalculations = [];
          }
          console.log('Loaded saved calculations:', this.savedCalculations.length);
        },
        error: (error) => {
          console.error('Error loading saved calculations:', error);
          this.savedCalculations = [];
        }
      });
  }

  /**
    * Load a specific saved calculation
    */
   loadSavedCalculation(calculationId: number): void {
     this.explosiveCalculationsService.getById(calculationId)
       .subscribe({
         next: (calculation) => {
           // Map DTO properties to component inputs and results
           this.calculationInputs = {
             numberOfHoles: calculation.numberOfFilledHoles, // Use numberOfFilledHoles as numberOfHoles
             numberOfColumns: 0, // Not available in DTO, set default
             holeDiameter: 0, // Not available in DTO, set default
             stemming: 0, // Not available in DTO, set default
             spacing: 0, // Not available in DTO, set default
             burden: 0, // Not available in DTO, set default
             emulsionDensity: calculation.emulsionDensity,
             anfoeDensity: calculation.anfoDensity,
             emulsionPerHole: calculation.emulsionPerHole,
             depths: [] // Not available in DTO, set empty array
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
           this.updateFormFromInputs();
           console.log('Loaded calculation:', calculation);
         },
         error: (error) => {
           console.error('Error loading calculation:', error);
           this.errorMessage = 'Failed to load the selected calculation.';
         }
       });
   }

   /**
    * Update form values from calculation inputs
    */
   private updateFormFromInputs(): void {
     if (this.calculationInputs && this.explosiveForm) {
       this.explosiveForm.patchValue({
         numberOfHoles: this.calculationInputs.numberOfHoles,
         numberOfColumns: this.calculationInputs.numberOfColumns,
         holeDiameter: this.calculationInputs.holeDiameter,
         stemming: this.calculationInputs.stemming,
         spacing: this.calculationInputs.spacing,
         burden: this.calculationInputs.burden,
         emulsionDensity: this.calculationInputs.emulsionDensity,
         anfoeDensity: this.calculationInputs.anfoeDensity,
         emulsionPerHole: this.calculationInputs.emulsionPerHole
       });
     }
   }

  /**
   * Delete a saved calculation
   */
  deleteSavedCalculation(calculationId: number): void {
    if (confirm('Are you sure you want to delete this calculation?')) {
      this.explosiveCalculationsService.delete(calculationId)
        .subscribe({
          next: () => {
            console.log('Calculation deleted successfully');
            this.loadSavedCalculations(); // Refresh the list
            if (this.currentCalculationId === calculationId) {
              this.currentCalculationId = null;
            }
          },
          error: (error) => {
            console.error('Error deleting calculation:', error);
            this.errorMessage = 'Failed to delete the calculation.';
          }
        });
    }
  }

  /**
   * Get data source information for display
   */
  getDataSourceInfo(): string {
    if (this.drillData.length > 0) {
      return `Backend drilling pattern data (${this.drillData.length} holes)`;
    }
    return 'Manual input data';
  }

  /**
   * Check if data was loaded from backend
   */
  isBackendDataLoaded(): boolean {
    return this.drillData.length > 0 && this.patternSettings !== null;
  }

  /**
   * Create the explosive calculation form with validation
   */
  createExplosiveForm(): FormGroup {
    return this.fb.group({
      // Basic hole parameters
      numberOfHoles: [0, [Validators.required, Validators.min(1), Validators.max(1000)]],
      numberOfColumns: [0, [Validators.required, Validators.min(1), Validators.max(100)]],
      holeDiameter: [0, [Validators.required, Validators.min(0.05), Validators.max(1.0)]],
      stemming: [0, [Validators.required, Validators.min(0.5), Validators.max(10.0)]],
      spacing: [0, [Validators.required, Validators.min(1.0), Validators.max(20.0)]],
      burden: [0, [Validators.required, Validators.min(1.0), Validators.max(15.0)]],
      
      // Explosive parameters
      emulsionDensity: [0, [Validators.required, Validators.min(0.1), Validators.max(10.0)]],
      anfoeDensity: [0, [Validators.required, Validators.min(0.1), Validators.max(10.0)]],
      emulsionPerHole: [0, [Validators.required, Validators.min(0), Validators.max(200)]],
      
      // Hole depths (as comma-separated values)
      holeDepths: ['', [Validators.required]]
    });
  }

  /**
   * Toggle input form visibility
   */
  toggleInputForm(): void {
    this.showInputForm = !this.showInputForm;
    if (this.showInputForm && this.calculationInputs) {
      this.populateFormFromInputs();
    }
  }

  /**
   * Populate form with existing calculation inputs
   */
  populateFormFromInputs(): void {
    if (!this.calculationInputs) return;
    
    const inputs = this.calculationInputs;
    this.explosiveForm.patchValue({
      numberOfHoles: inputs.numberOfHoles,
      numberOfColumns: inputs.numberOfColumns,
      holeDiameter: inputs.holeDiameter,
      stemming: inputs.stemming,
      spacing: inputs.spacing,
      burden: inputs.burden,
      emulsionDensity: inputs.emulsionDensity,
      anfoeDensity: inputs.anfoeDensity,
      emulsionPerHole: inputs.emulsionPerHole,
      holeDepths: inputs.depths.join(',')
    });
  }

  /**
   * Handle form submission
   */
  onFormSubmit(): void {
    if (this.explosiveForm.valid) {
      const formValues = this.explosiveForm.value;
      
      // Parse hole depths from comma-separated string
      const depths = formValues.holeDepths
        .split(',')
        .map((depth: string) => parseFloat(depth.trim()))
        .filter((depth: number) => !isNaN(depth));
      
      // Create calculation inputs from form data
      this.calculationInputs = {
        numberOfHoles: formValues.numberOfHoles,
        numberOfColumns: formValues.numberOfColumns,
        holeDiameter: formValues.holeDiameter,
        stemming: formValues.stemming,
        spacing: formValues.spacing,
        burden: formValues.burden,
        emulsionDensity: formValues.emulsionDensity,
        anfoeDensity: formValues.anfoeDensity,
        emulsionPerHole: formValues.emulsionPerHole,
        depths: depths
      };
      
      // Perform calculations
      this.performCalculations();
      
      // Hide form after successful submission
      this.showInputForm = false;
    } else {
      // Mark all fields as touched to show validation errors
      Object.keys(this.explosiveForm.controls).forEach(key => {
        this.explosiveForm.get(key)?.markAsTouched();
      });
    }
  }

  /**
   * Get form control for template access
   */
  getFormControl(controlName: string) {
    return this.explosiveForm.get(controlName);
  }

  /**
   * Check if form control has error
   */
  hasFormError(controlName: string, errorType: string): boolean {
    const control = this.explosiveForm.get(controlName);
    return !!(control && control.hasError(errorType) && (control.dirty || control.touched));
  }

  /**
   * Refresh data from backend
   */
  refreshBackendData(): void {
    if (this.projectId && this.siteId) {
      this.loadBackendData();
    }
  }

  /**
   * Navigate back to Pattern Creator
   */
  navigateToPatternCreator(): void {
    if (this.projectId && this.siteId) {
      this.router.navigate([`/blasting-engineer/project-management/${this.projectId}/sites/${this.siteId}/pattern-creator`]);
    }
  }

  /**
   * Navigate to Sequence Designer
   */
  navigateToSequenceDesigner(): void {
    if (this.projectId && this.siteId) {
      this.router.navigate([`/blasting-engineer/project-management/${this.projectId}/sites/${this.siteId}/sequence-designer`]);
    }
  }
}
