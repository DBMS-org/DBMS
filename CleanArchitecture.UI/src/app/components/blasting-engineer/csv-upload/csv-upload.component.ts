// csv-upload.component.ts
import { Component, EventEmitter, Output, Injectable } from '@angular/core';
import { HttpClient, HttpEventType } from '@angular/common/http';
import { Router } from '@angular/router';
import { finalize } from 'rxjs/operators';
import { CommonModule } from '@angular/common';

interface DrillHole {
  serialNumber: number;
  id: string;
  name?: string;
  easting: number;
  northing: number;
  elevation: number;
  length: number;
  depth: number;
  azimuth: number;
  dip: number;
  actualDepth: number;
  stemming: number;
  createdAt?: string;
  updatedAt?: string;
}

// Service to share drill data between components
@Injectable({
  providedIn: 'root'
})
export class DrillDataService {
  private drillData: DrillHole[] = [];
  
  setDrillData(data: DrillHole[]): void {
    this.drillData = data;
  }
  
  getDrillData(): DrillHole[] {
    return this.drillData;
  }
  
  clearDrillData(): void {
    this.drillData = [];
  }
}

@Component({
  selector: 'app-csv-upload',
  templateUrl: './csv-upload.component.html',
  styleUrls: ['./csv-upload.component.scss'],
  standalone: true,
  imports: [CommonModule]
})
export class CsvUploadComponent {
  @Output() dataLoaded = new EventEmitter<DrillHole[]>();
  
  selectedFile: File | null = null;
  uploadProgress: number = 0;
  isUploading: boolean = false;
  uploadError: string | null = null;

  constructor(private http: HttpClient, private router: Router, private drillDataService: DrillDataService) {}

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      const file = input.files[0];
      
      // Validate file type
      if (file.type === 'text/csv' || file.name.toLowerCase().endsWith('.csv')) {
        this.selectedFile = file;
        this.uploadError = null;
      } else {
        this.uploadError = 'Please select a valid CSV file';
        this.selectedFile = null;
      }
    }
  }

  uploadFile(): void {
    if (!this.selectedFile) {
      this.uploadError = 'Please select a CSV file first';
      return;
    }

    this.isUploading = true;
    this.uploadProgress = 0;
    this.uploadError = null;

    const formData = new FormData();
    formData.append('file', this.selectedFile);

    // Point to the backend API server on port 5019
    const apiUrl = 'http://localhost:5019/api/DrillPlan/upload-csv';
    console.log('Uploading to:', apiUrl);

    this.http.post<DrillHole[]>(apiUrl, formData, {
      reportProgress: true,
      observe: 'events'
    }).pipe(
      finalize(() => {
        this.isUploading = false;
      })
    ).subscribe({
      next: (event) => {
        if (event.type === HttpEventType.UploadProgress && event.total) {
          this.uploadProgress = Math.round(100 * event.loaded / event.total);
        } else if (event.type === HttpEventType.Response) {
          if (event.body) {
            console.log('Upload successful:', event.body);
            console.log('Data structure check:', event.body[0]);
            console.log('CsvUploadComponent - About to emit dataLoaded event');
            console.log('Event body length:', event.body.length);
            
            this.dataLoaded.emit(event.body);
            console.log('CsvUploadComponent - dataLoaded event emitted successfully');

            // Store the data in the service and navigate
            this.drillDataService.setDrillData(event.body);
            console.log('DrillDataService - Data stored, navigating to visualization');

            // Navigate to the drill visualization page
            this.router.navigate(['/blasting-engineer/drill-visualization']);
          }
        }
      },
      error: (error) => {
        this.uploadError = error.error?.message || 'Failed to upload CSV file. Please try again.';
      }
    });
  }

  resetForm(): void {
    this.selectedFile = null;
    this.uploadProgress = 0;
    this.isUploading = false;
    this.uploadError = null;
  }
}