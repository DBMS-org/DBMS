import { Component, EventEmitter, Output } from '@angular/core';
import { HttpClient, HttpEventType } from '@angular/common/http';
import { finalize } from 'rxjs/operators';
import { CommonModule } from '@angular/common';

interface DrillHole {
  id: string;
  east: number;
  north: number;
  elevation: number;
  depth: number;
  azimuth: number;
  dip: number;
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
  isDragover: boolean = false;

  // Updated to use the correct port 5166
  private apiBaseUrl = 'https://localhost:5166';

  constructor(private http: HttpClient) {}

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      const file = input.files[0];
      this.validateAndSetFile(file);
    }
  }

  onDragOver(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
    this.isDragover = true;
  }

  onDragLeave(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
    this.isDragover = false;
  }

  onDrop(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
    this.isDragover = false;

    if (event.dataTransfer?.files && event.dataTransfer.files.length > 0) {
      const file = event.dataTransfer.files[0];
      this.validateAndSetFile(file);
    }
  }

  private validateAndSetFile(file: File): void {
    if (file.type === 'text/csv' || file.name.toLowerCase().endsWith('.csv')) {
      this.selectedFile = file;
      this.uploadError = null;
    } else {
      this.uploadError = 'Please select a valid CSV file';
      this.selectedFile = null;
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

    this.http.post<DrillHole[]>(`${this.apiBaseUrl}/api/DrillPlan/upload-csv`, formData, {
      reportProgress: true,
      observe: 'events',
      headers: {
        'Accept': 'application/json'
      }
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
            this.dataLoaded.emit(event.body);
          }
        }
      },
      error: (error) => {
        console.error('Upload error:', error);
        if (error.status === 0) {
          this.uploadError = 'Unable to connect to the server. Please check if the server is running.';
        } else if (error.status === 404) {
          this.uploadError = 'API endpoint not found. Please check the API URL configuration.';
        } else if (error.error instanceof ErrorEvent) {
          this.uploadError = `Network error: ${error.error.message}`;
        } else {
          this.uploadError = error.error?.message || 'Failed to upload CSV file. Please try again.';
        }
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
