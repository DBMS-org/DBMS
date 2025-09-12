import { Component, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { DrillPoint } from '../models/drill-point.model';
import { PatternSettings } from '../models/drill-point.model';

export interface DrillDataRow {
  holeId: string;
  depth: number;
  stemming: number;
  spacing: number;
  burden: number;
  subDrill: number;
  volume: number;
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
  @Input() settings: PatternSettings = {
    spacing: 3,
    burden: 2.5,
    depth: 10,
    diameter: 89,
    stemming: 3,
    subDrill: 0.5
  };
  dataSource: DrillDataRow[] = [];

  constructor() {}

  ngOnInit(): void {
    console.log('DrillDataTableComponent ngOnInit called');
    console.log('drillPoints:', this.drillPoints);
    console.log('settings:', this.settings);
    this.generateTableData();
    console.log('dataSource after generation:', this.dataSource);
  }

  generateTableData(): void {
    this.dataSource = this.drillPoints.map(point => {
      const volume = this.calculateVolume(point.spacing, point.burden, point.depth);
      return {
        holeId: point.id,
        depth: point.depth,
        stemming: this.settings.stemming,
        spacing: point.spacing,
        burden: point.burden,
        subDrill: this.settings.subDrill,
        volume: volume
      };
    });
  }

  calculateVolume(spacing: number, burden: number, depth: number): number {
    // Volume = spacing × burden × depth
    return Number((spacing * burden * depth).toFixed(2));
  }

  onCellEdit(element: DrillDataRow, field: keyof DrillDataRow, value: any): void {
    const numericValue = Number(value);
    if (!isNaN(numericValue) && numericValue >= 0) {
      (element as any)[field] = numericValue;
      
      // Recalculate volume if spacing, burden, or depth changed
      if (field === 'spacing' || field === 'burden' || field === 'depth') {
        element.volume = this.calculateVolume(element.spacing, element.burden, element.depth);
      }
    }
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


}