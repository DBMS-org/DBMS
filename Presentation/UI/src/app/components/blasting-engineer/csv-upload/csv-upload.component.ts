// csv-upload.component.ts
import { Component, EventEmitter, Output, Injectable, OnInit } from '@angular/core';
import { HttpClient, HttpEventType } from '@angular/common/http';
import { Router, ActivatedRoute } from '@angular/router';
import { finalize } from 'rxjs/operators';
import { CommonModule } from '@angular/common';
import { UnifiedDrillDataService } from '../../../core/services/unified-drill-data.service';
import { DrillLocation, DrillModelConverter } from '../../../core/models/drilling.model';

interface DrillHole {
  serialNumber?: number;
  id: string;
  name?: string;
  easting: number;
  northing: number;
  elevation: number;
  length: number;
  depth: number;
  azimuth?: number | null; // Made optional for 2D fallback
  dip?: number | null;     // Made optional for 2D fallback
  actualDepth: number;
  stemming: number;
  createdAt?: string;
  updatedAt?: string;
  
  // Helper properties for 2D/3D detection
  has3DData?: boolean;
  requiresFallbackTo2D?: boolean;
}

// Service to share drill data between components
// DrillDataService has been deprecated and replaced by UnifiedDrillDataService
// This service is kept temporarily for backward compatibility but will be removed

@Component({
  selector: 'app-csv-upload',
  templateUrl: './csv-upload.component.html',
  styleUrls: ['./csv-upload.component.scss'],
  standalone: true,
  imports: [CommonModule]
})
export class CsvUploadComponent implements OnInit {
  @Output() dataLoaded = new EventEmitter<DrillHole[]>();
  
  selectedFile: File | null = null;
  uploadProgress: number = 0;
  isUploading: boolean = false;
  uploadError: string | null = null;

  // Site-specific properties
  siteId: string | null = null;
  siteName: string | null = null;
  projectId: string | null = null;

  // Selector state
  selectorVisible: boolean = false;
  has3DData: boolean = false;

  constructor(private http: HttpClient, private router: Router, private unifiedDrillDataService: UnifiedDrillDataService, private activatedRoute: ActivatedRoute) {}

  ngOnInit(): void {
    // Handle query parameters
    this.activatedRoute.queryParams.subscribe(params => {
      this.siteId = params['siteId'] || null;
      this.siteName = params['siteName'] || null;
      this.projectId = params['projectId'] || null;
    });
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      const file = input.files[0];
      
      // Enhanced file validation with detailed error messages
      const fileName = file.name.toLowerCase();
      const fileType = file.type;
      const fileSize = file.size;
      
      console.log('File selected:', { name: file.name, type: fileType, size: fileSize });
      
      // Check file type
      if (!fileName.endsWith('.csv') && fileType !== 'text/csv') {
        if (fileName.endsWith('.xlsx') || fileName.endsWith('.xls')) {
          this.uploadError = `Excel files (.xlsx/.xls) are not supported. Please convert "${file.name}" to CSV format first. In Excel: File → Save As → CSV (Comma delimited)`;
        } else if (fileName.endsWith('.txt')) {
          this.uploadError = `Text files (.txt) are not supported. Please save "${file.name}" as CSV format with comma separators.`;
      } else {
          this.uploadError = `File type "${fileType || 'unknown'}" is not supported. Please select a CSV file (.csv) only. Current file: "${file.name}"`;
        }
        this.selectedFile = null;
        return;
      }
      
      // Check file size (max 10MB)
      const maxSize = 10 * 1024 * 1024; // 10MB
      if (fileSize > maxSize) {
        this.uploadError = `File is too large (${(fileSize / (1024 * 1024)).toFixed(1)}MB). Maximum allowed size is 10MB. Please reduce the file size or split it into smaller files.`;
        this.selectedFile = null;
        return;
      }
      
      // Check if file is empty
      if (fileSize === 0) {
        this.uploadError = `The selected file "${file.name}" is empty. Please select a file that contains drill hole data.`;
        this.selectedFile = null;
        return;
      }
      
      this.selectedFile = file;
      this.uploadError = null;
      console.log('File validation passed:', file.name);
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

            // Convert drill holes to drill locations
            const drillLocations = event.body.map(hole => 
              DrillModelConverter.drillHoleToDrillLocation(hole)
            );

            // Create a proper pattern context first
            const patternData = {
              drillLocations: drillLocations,
              settings: {
                name: 'CSV Import',
                projectId: Number(this.projectId) || 0,
                siteId: Number(this.siteId) || 0,
                spacing: 3.0,
                burden: 2.5,
                depth: 10.0
              },
              projectId: Number(this.projectId) || 0,
              siteId: Number(this.siteId) || 0
            };

            // Set the complete pattern context to ensure data persistence
            this.unifiedDrillDataService.setCurrentPattern(patternData);
            
            console.log('✅ Pattern data stored in unified service:', {
              drillLocationCount: drillLocations.length,
              projectId: patternData.projectId,
              siteId: patternData.siteId
            });

            // Verify the data was stored correctly
            const retrievedData = this.unifiedDrillDataService.getDrillLocations();
            console.log('🔍 Verification - Retrieved data count:', retrievedData.length);

            // Compute 3D capability
            this.has3DData = event.body.some(hole => (hole as any).azimuth !== null && (hole as any).azimuth !== undefined && (hole as any).dip !== null && (hole as any).dip !== undefined);
            this.selectorVisible = true; // Show selector buttons
            console.log(`Selector visible. 3D capable: ${this.has3DData}`);
          }
        }
      },
      error: (error) => {
        console.error('Upload error details:', error);
        
        // Enhanced error handling with specific messages
        let errorMessage = 'Failed to upload CSV file. ';
        
        if (error.status === 0) {
          // Network error
          errorMessage = `❌ Cannot connect to the server. Please check:
• Is the backend server running on http://localhost:5019?
• Check your internet connection
• Try refreshing the page and uploading again`;
        } else if (error.status === 400) {
          // Bad Request - usually validation or parsing errors
          const serverMessage = error.error?.message || error.error || error.message;
          if (typeof serverMessage === 'string') {
            if (serverMessage.includes('Only CSV files are allowed')) {
              errorMessage = `❌ Invalid file format detected by server. ${serverMessage}
              
💡 Solution: Save your file as CSV format:
• Open file in Excel/Sheets
• File → Save As → CSV (Comma delimited)
• Make sure the file extension is .csv`;
            } else if (serverMessage.includes('No valid drill holes found')) {
              errorMessage = `❌ No valid drill hole data found in your CSV file.

💡 Please check your CSV file contains:
• Header row with column names like: ID, Easting, Northing, Elevation, Depth
• At least one data row with valid numbers
• Columns separated by commas
• No completely empty rows

📋 Supported column names:
• ID: id, hole id, holeid, hole_id
• Easting: easting, east, x, design collar (e)
• Northing: northing, north, y, design collar (n)  
• Elevation: elevation, elev, z, design collar (rl)
• Depth: depth, hole depth, actual depth`;
            } else if (serverMessage.includes('empty')) {
              errorMessage = `❌ The uploaded file is empty or has no data.
              
💡 Please ensure your CSV file contains:
• Header row with column names
• At least one row of drill hole data`;
            } else {
              errorMessage = `❌ Server validation error: ${serverMessage}`;
            }
          } else {
            errorMessage = `❌ Bad request error. Please check your CSV file format and try again.`;
          }
        } else if (error.status === 413) {
          // Payload too large
          errorMessage = `❌ File is too large for the server to process.
          
💡 Try these solutions:
• Reduce the number of rows in your CSV
• Split your data into multiple smaller files
• Remove unnecessary columns`;
        } else if (error.status === 415) {
          // Unsupported media type
          errorMessage = `❌ Unsupported file type.
          
💡 Make sure you're uploading a CSV file (.csv) with proper comma-separated values.`;
        } else if (error.status === 500) {
          // Server error
          const serverMessage = error.error?.message || error.error || 'Internal server error';
          errorMessage = `❌ Server processing error: ${serverMessage}

💡 This might be caused by:
• Invalid data format in your CSV
• Missing required columns
• Corrupted file content
• Server configuration issues

🔧 Try these solutions:
• Check your CSV file for any unusual characters
• Ensure all numeric fields contain valid numbers
• Remove any special formatting from Excel before saving as CSV`;
        } else if (error.status >= 400 && error.status < 500) {
          // Client errors
          errorMessage = `❌ Request error (${error.status}): ${error.error?.message || error.message || 'Unknown client error'}`;
        } else {
          // Other errors
          const serverMessage = error.error?.message || error.error || error.message;
          errorMessage = `❌ Upload failed: ${serverMessage}

💡 Try these solutions:
• Check if your CSV file is properly formatted
• Ensure the file is not corrupted
• Try uploading a smaller test file first
• Check the browser console for more details`;
        }
        
        this.uploadError = errorMessage;
      }
    });
  }

  resetForm(): void {
    this.selectedFile = null;
    this.uploadProgress = 0;
    this.isUploading = false;
    this.uploadError = null;
  }

  goBackToSites(): void {
    if (this.projectId) {
      this.router.navigate(['/blasting-engineer/project-management', this.projectId, 'sites']);
    } else {
      this.router.navigate(['/blasting-engineer/project-management']);
    }
  }

  formatErrorMessage(errorMessage: string): string {
    if (!errorMessage) return '';
    
    // Convert line breaks to HTML breaks and format bullet points
    return errorMessage
      .replace(/\n/g, '<br>')
      .replace(/• /g, '<span class="bullet">•</span> ')
      .replace(/❌/g, '<span class="error-icon">❌</span>')
      .replace(/💡/g, '<span class="tip-icon">💡</span>')
      .replace(/📋/g, '<span class="info-icon">📋</span>')
      .replace(/🔧/g, '<span class="tool-icon">🔧</span>')
      .replace(/Solution:/g, '<strong>Solution:</strong>')
      .replace(/Try these solutions:/g, '<strong>Try these solutions:</strong>')
      .replace(/Supported column names:/g, '<strong>Supported column names:</strong>')
      .replace(/Please check:/g, '<strong>Please check:</strong>')
      .replace(/This might be caused by:/g, '<strong>This might be caused by:</strong>');
  }

  // Navigation methods for 2D / 3D
  navigateTo2D(): void {
    if (!this.selectorVisible) return;

    if (this.projectId && this.siteId) {
      this.router.navigate(['/blasting-engineer/project-management', this.projectId, 'sites', this.siteId, 'pattern-creator']);
    } else {
      this.router.navigate(['/blasting-engineer/drilling-pattern']);
    }
  }

  navigateTo3D(): void {
    if (!this.selectorVisible || !this.has3DData) return;

    if (this.projectId && this.siteId) {
      this.router.navigate(['/blasting-engineer/project-management', this.projectId, 'sites', this.siteId, 'drill-visualization']);
    } else {
      this.router.navigate(['/blasting-engineer/drill-visualization']);
    }
  }
}