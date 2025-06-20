import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CsvUploadComponent } from './csv-upload/csv-upload.component';
import { DrillVisualizationComponent } from './drill-visualization/drill-visualization.component';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';

interface DrillHole {
  serialNumber?: number;
  id: string;
  name: string;
  easting: number;
  northing: number;
  elevation: number;
  depth: number;
  azimuth: number;
  dip: number;
  createdAt?: string;
  updatedAt?: string;
}

@Component({
  selector: 'app-blasting-engineer',
  templateUrl: './blasting-engineer.component.html',
  styleUrls: ['./blasting-engineer.component.scss'],
  standalone: true,
  imports: [CommonModule, CsvUploadComponent, DrillVisualizationComponent]
})
export class BlastingEngineerComponent implements OnInit {
  drillHoles: DrillHole[] = [];

  constructor(private http: HttpClient) {}

  ngOnInit() {
    this.loadDrillHoles();
  }

  private loadDrillHoles() {
    this.http.get<DrillHole[]>(`${environment.apiUrl}/api/DrillPlan`).subscribe(
      (data) => {
        console.log('Loaded drill holes:', data);
        this.drillHoles = data;
      },
      (error) => {
        console.error('Error loading drill holes:', error);
        // Initialize with empty array if no data available
        this.drillHoles = [];
      }
    );
  }

  onDrillHolesLoaded(drillHoles: DrillHole[]) {
    console.log('BlastingEngineerComponent - onDrillHolesLoaded called');
    console.log('Received drill holes:', drillHoles);
    console.log('Drill holes length:', drillHoles?.length);
    console.log('Previous drillHoles:', this.drillHoles);
    
    this.drillHoles = drillHoles;
    
    console.log('Updated drillHoles:', this.drillHoles);
    console.log('Updated drillHoles length:', this.drillHoles?.length);
  }
}
