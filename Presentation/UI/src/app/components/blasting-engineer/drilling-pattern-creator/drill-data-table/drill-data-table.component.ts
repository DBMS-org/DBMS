import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { DrillPoint } from '../models/drill-point.model';
import { PatternSettings } from '../models/drill-point.model';
import { ExplosiveCalculationsService, ExplosiveCalculationResultDto } from '../../../../core/services/explosive-calculations.service';

export interface DrillDataRow {
  holeId: string;
  depth: number;
  stemming: number;
  spacing: number;
  burden: number;
  subDrill: number;
  volume: number;
  anfo: number;
  emulsion: number;
}

@Component({
  selector: 'app-drill-data-table',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule
  ],
  templateUrl: './drill-data-table.component.html',
  styleUrls: ['./drill-data-table.component.scss']
})
export class DrillDataTableComponent implements OnInit {
  @Input() drillPoints: DrillPoint[] = [];
  @Input() projectId!: number;
  @Input() siteId!: number;
  @Input() settings: PatternSettings = {
    spacing: 3,
    burden: 2.5,
    depth: 10,
    diameter: 89,
    stemming: 3,
    subDrill: 0.5
  };
  
  // Density inputs for calculations
  emulsionDensity: number = 1.2; // g/cm³
  anfoeDensity: number = 0.8; // g/cm³
  emulsionPerHole: number = 50; // kg
  holeDiameter: number = 89; // mm
  @Output() close = new EventEmitter<void>();
  @Output() save = new EventEmitter<DrillDataRow[]>();
  dataSource: DrillDataRow[] = [];

  // Totals for each numeric column
  totals = {
    depth: 0,
    stemming: 0,
    spacing: 0,
    burden: 0,
    subDrill: 0,
    volume: 0,
    anfo: 0,
    emulsion: 0
  };

  constructor(private explosiveCalculationsService: ExplosiveCalculationsService) {}

  ngOnInit(): void {
    console.log('DrillDataTableComponent ngOnInit called');
    console.log('drillPoints:', this.drillPoints);
    console.log('settings:', this.settings);
    
    // Initialize hole diameter from pattern settings (convert from meters to mm)
    if (this.settings && this.settings.diameter) {
      this.holeDiameter = this.settings.diameter * 1000; // Convert meters to mm
      console.log('Initialized hole diameter from pattern settings:', {
        settingsDiameterMeters: this.settings.diameter,
        holeDiameterMm: this.holeDiameter
      });
    }
    
    // Load explosive parameters from database if projectId and siteId are available
    if (this.projectId && this.siteId) {
      this.loadExplosiveParametersFromDatabase();
    } else {
      this.generateTableData();
    }
    
    console.log('dataSource after generation:', this.dataSource);
  }

  /**
   * Load explosive parameters from database based on project and site IDs
   */
  private loadExplosiveParametersFromDatabase(): void {
    console.log('Loading explosive parameters from database for project:', this.projectId, 'site:', this.siteId);
    
    this.explosiveCalculationsService.getLatestByProjectAndSite(this.projectId, this.siteId).subscribe({
      next: (latestCalculation: ExplosiveCalculationResultDto | null) => {
        if (latestCalculation) {
          console.log('Found latest explosive calculation:', latestCalculation);
          
          // Update explosive parameters with database values
          this.emulsionDensity = latestCalculation.emulsionDensity;
          this.anfoeDensity = latestCalculation.anfoDensity;
          this.emulsionPerHole = latestCalculation.emulsionPerHole;
          
          // Update hole diameter from settings if available, otherwise keep default
          // Note: holeDiameter might come from pattern settings rather than explosive calculations
          
          console.log('Updated explosive parameters from database:', {
            emulsionDensity: this.emulsionDensity,
            anfoeDensity: this.anfoeDensity,
            emulsionPerHole: this.emulsionPerHole,
            holeDiameter: this.holeDiameter
          });
        } else {
          console.log('No previous explosive calculations found, using default values');
        }
        
        // Generate table data after loading parameters
        this.generateTableData();
      },
      error: (error) => {
        console.error('Error loading explosive parameters from database:', error);
        // Fall back to default values and generate table data
        this.generateTableData();
      }
    });
  }

  generateTableData(): void {
    this.dataSource = this.drillPoints.map(point => {
      const subDrill = point.subDrill || this.settings.subDrill;
      const volume = this.calculateVolume(point.spacing, point.burden, point.depth, subDrill);
      const stemming = point.stemming || this.settings.stemming;
      
      // Calculate ANFO and Emulsion using the provided formulas
      const { anfo, emulsion } = this.calculateExplosives(point.depth, stemming);
      
      return {
        holeId: point.id,
        depth: point.depth,
        stemming: stemming,
        spacing: point.spacing,
        burden: point.burden,
        subDrill: subDrill,
        volume: volume,
        anfo: anfo,
        emulsion: emulsion
      };
    }).sort((a, b) => {
      // Sort by holeId in ascending order
      // Handle both numeric and string hole IDs
      const aId = a.holeId;
      const bId = b.holeId;
      
      // If both are numeric, compare as numbers
      const aNum = parseFloat(aId);
      const bNum = parseFloat(bId);
      
      if (!isNaN(aNum) && !isNaN(bNum)) {
        return aNum - bNum;
      }
      
      // Otherwise, compare as strings
      return aId.localeCompare(bId, undefined, { numeric: true, sensitivity: 'base' });
    });
    
    // Calculate totals after generating data
    this.calculateTotals();
  }

  calculateVolume(spacing: number, burden: number, depth: number, subDrill: number = 0): number {
    // Volume = spacing × burden × (depth - subDrill)
    const effectiveDepth = Math.max(0, depth - subDrill); // Ensure non-negative depth
    return Number((spacing * burden * effectiveDepth).toFixed(2));
  }

  calculateExplosives(depth: number, stemming: number): { anfo: number, emulsion: number } {
    // Use the exact formulas provided by the user
    // Emulsion per meter = 22/7000(diameter/2)^2 * Density
    const emulsionPerMeter = (22/7000) * Math.pow(this.holeDiameter/2, 2) * this.emulsionDensity;
    
    // ANFO per meter = 22/7000(diameter/2)^2 * Density
    const anfoPerMeter = (22/7000) * Math.pow(this.holeDiameter/2, 2) * this.anfoeDensity;
    
    // Emulsion Covering space = Emulsion per Hole / Emulsion per meter
    const emulsionCoveringSpace = emulsionPerMeter > 0 ? this.emulsionPerHole / emulsionPerMeter : 0;
    
    // Remaining Space = Depth - (stemming + Emulsion Covering space)
    const remainingSpace = depth - (stemming + emulsionCoveringSpace);
    
    // Anfo = ANFO PER METER * Remaining space
    const anfo = anfoPerMeter * Math.max(0, remainingSpace);
    
    // Emulsion = Emulsion Covering space
    const emulsion = emulsionCoveringSpace;
    
    return {
      anfo: Number(anfo.toFixed(2)),
      emulsion: Number(emulsion.toFixed(2))
    };
  }

  calculateTotals(): void {
    this.totals = {
      depth: Number(this.dataSource.reduce((sum, row) => sum + row.depth, 0).toFixed(2)),
      stemming: Number(this.dataSource.reduce((sum, row) => sum + row.stemming, 0).toFixed(2)),
      spacing: Number(this.dataSource.reduce((sum, row) => sum + row.spacing, 0).toFixed(2)),
      burden: Number(this.dataSource.reduce((sum, row) => sum + row.burden, 0).toFixed(2)),
      subDrill: Number(this.dataSource.reduce((sum, row) => sum + row.subDrill, 0).toFixed(2)),
      volume: Number(this.dataSource.reduce((sum, row) => sum + row.volume, 0).toFixed(2)),
      anfo: Number(this.dataSource.reduce((sum, row) => sum + row.anfo, 0).toFixed(2)),
      emulsion: Number(this.dataSource.reduce((sum, row) => sum + row.emulsion, 0).toFixed(2))
    };
  }

  onCellEdit(element: DrillDataRow, field: keyof DrillDataRow, value: any): void {
    const numericValue = Number(value);
    if (!isNaN(numericValue) && numericValue >= 0) {
      (element as any)[field] = numericValue;
      
      // Recalculate volume if spacing, burden, depth, or subDrill changes
      if (field === 'spacing' || field === 'burden' || field === 'depth' || field === 'subDrill') {
        element.volume = this.calculateVolume(element.spacing, element.burden, element.depth, element.subDrill);
      }
      
      // Recalculate ANFO and Emulsion if depth or stemming changes
      if (field === 'depth' || field === 'stemming') {
        const { anfo, emulsion } = this.calculateExplosives(element.depth, element.stemming);
        element.anfo = anfo;
        element.emulsion = emulsion;
      }
      
      // Recalculate totals after any cell edit
      this.calculateTotals();
    }
  }
  
  onDensityInputChange(): void {
    // Recalculate ANFO and Emulsion for all rows when density inputs change
    this.dataSource.forEach(element => {
      const { anfo, emulsion } = this.calculateExplosives(element.depth, element.stemming);
      element.anfo = anfo;
      element.emulsion = emulsion;
    });
    
    // Recalculate totals after density changes
    this.calculateTotals();
  }

  onHoleDiameterChange(): void {
    console.log('Hole diameter changed to:', this.holeDiameter);
    
    // Recalculate ANFO and Emulsion for all rows when hole diameter changes
    this.dataSource.forEach(element => {
      const { anfo, emulsion } = this.calculateExplosives(element.depth, element.stemming);
      element.anfo = anfo;
      element.emulsion = emulsion;
    });
    
    // Recalculate totals after diameter changes
    this.calculateTotals();
  }

  onKeyDown(event: KeyboardEvent): void {
    if (event.key === 'ArrowUp' || event.key === 'ArrowDown' || event.key === 'ArrowLeft' || event.key === 'ArrowRight') {
      // Prevent default behavior to stop increment/decrement on number inputs
      event.preventDefault();
      
      const target = event.target as HTMLInputElement;
      const currentCell = target.closest('td');
      const currentRow = currentCell?.closest('tr');
      
      if (!currentCell || !currentRow) return;
      
      let nextCell: HTMLElement | null = null;
      
      if (event.key === 'ArrowUp') {
        const prevRow = currentRow.previousElementSibling as HTMLTableRowElement;
        if (prevRow) {
          const cellIndex = Array.from(currentRow.children).indexOf(currentCell);
          nextCell = prevRow.children[cellIndex]?.querySelector('input') as HTMLElement;
        }
      } else if (event.key === 'ArrowDown') {
        const nextRow = currentRow.nextElementSibling as HTMLTableRowElement;
        if (nextRow) {
          const cellIndex = Array.from(currentRow.children).indexOf(currentCell);
          nextCell = nextRow.children[cellIndex]?.querySelector('input') as HTMLElement;
        }
      } else if (event.key === 'ArrowLeft') {
        const prevCell = currentCell.previousElementSibling as HTMLTableCellElement;
        if (prevCell) {
          nextCell = prevCell.querySelector('input') as HTMLElement;
        }
      } else if (event.key === 'ArrowRight') {
        const nextCellElement = currentCell.nextElementSibling as HTMLTableCellElement;
        if (nextCellElement) {
          nextCell = nextCellElement.querySelector('input') as HTMLElement;
        }
      }
      
      if (nextCell) {
        nextCell.focus();
        (nextCell as HTMLInputElement).select();
      }
    }
  }

  onCancel(): void {
    this.close.emit();
  }

  onSave(): void {
    // Emit the data source with all calculated explosive properties
    const drillDataWithExplosives = this.dataSource.map(row => ({
      ...row,
      // Ensure explosive calculation properties are included
      volume: row.volume || 0,
      anfo: row.anfo || 0,
      emulsion: row.emulsion || 0
    }));
    
    console.log('Saving drill data with explosive properties:', drillDataWithExplosives);
    this.save.emit(drillDataWithExplosives);
  }

}