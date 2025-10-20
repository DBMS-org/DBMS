import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CsvUploadComponent } from './csv-upload/csv-upload.component';
import { DrillVisualizationComponent } from './drill-visualization/drill-visualization.component';

// Main component for the blasting engineer module
@Component({
  selector: 'app-blasting-engineer',
  templateUrl: './blasting-engineer.component.html',
  styleUrls: ['./blasting-engineer.component.scss'],
  standalone: true,
  imports: [CommonModule, CsvUploadComponent, DrillVisualizationComponent]
})
export class BlastingEngineerComponent {
}
