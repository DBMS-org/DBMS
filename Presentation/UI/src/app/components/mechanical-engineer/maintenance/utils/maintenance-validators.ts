import { AbstractControl, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';

export class MaintenanceValidators {
  // Form validators
  static readonly scheduledDate = [Validators.required];
  static readonly estimatedHours = [
    Validators.required, 
    Validators.min(0.5), 
    Validators.max(24)
  ];
  static readonly assignedTo = [
    Validators.required, 
    Validators.minLength(1)
  ];
  static readonly observations = [Validators.maxLength(1000)];
  static readonly reason = [
    Validators.required,
    Validators.minLength(10),
    Validators.maxLength(500)
  ];

  // File upload validation
  static readonly fileUpload = {
    maxSize: 10 * 1024 * 1024, // 10MB
    allowedTypes: [
      'image/jpeg', 
      'image/png', 
      'image/gif',
      'application/pdf', 
      'text/plain',
      'application/msword',
      'application/vnd.openxmlformats-officedocument.wordprocessingml.document'
    ]
  };

  // Custom validators
  static futureDateValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (!control.value) {
        return null;
      }
      
      const selectedDate = new Date(control.value);
      const today = new Date();
      today.setHours(0, 0, 0, 0);
      
      if (selectedDate < today) {
        return { pastDate: { value: control.value } };
      }
      
      return null;
    };
  }

  static businessHoursValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (!control.value) {
        return null;
      }
      
      const selectedDate = new Date(control.value);
      const hour = selectedDate.getHours();
      const dayOfWeek = selectedDate.getDay();
      
      // Check if it's weekend (0 = Sunday, 6 = Saturday)
      if (dayOfWeek === 0 || dayOfWeek === 6) {
        return { weekendDate: { value: control.value } };
      }
      
      // Check if it's business hours (8 AM to 6 PM)
      if (hour < 8 || hour >= 18) {
        return { outsideBusinessHours: { value: control.value } };
      }
      
      return null;
    };
  }

  static fileTypeValidator(allowedTypes: string[]): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (!control.value) {
        return null;
      }
      
      const file = control.value as File;
      if (!allowedTypes.includes(file.type)) {
        return { 
          invalidFileType: { 
            actualType: file.type, 
            allowedTypes: allowedTypes 
          } 
        };
      }
      
      return null;
    };
  }

  static fileSizeValidator(maxSizeBytes: number): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (!control.value) {
        return null;
      }
      
      const file = control.value as File;
      if (file.size > maxSizeBytes) {
        return { 
          fileTooLarge: { 
            actualSize: file.size, 
            maxSize: maxSizeBytes 
          } 
        };
      }
      
      return null;
    };
  }

  static serviceIntervalValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (!control.value) {
        return null;
      }
      
      const group = control as any;
      const intervalHours = group.get('intervalHours')?.value;
      const intervalMonths = group.get('intervalMonths')?.value;
      
      if (!intervalHours && !intervalMonths) {
        return { noInterval: true };
      }
      
      if (intervalHours && intervalHours < 1) {
        return { invalidHours: { value: intervalHours } };
      }
      
      if (intervalMonths && intervalMonths < 1) {
        return { invalidMonths: { value: intervalMonths } };
      }
      
      return null;
    };
  }

  // Validation helper methods
  static validateMaintenanceJob(job: any): string[] {
    const errors: string[] = [];
    
    if (!job.machineId) {
      errors.push('Machine ID is required');
    }
    
    if (!job.scheduledDate) {
      errors.push('Scheduled date is required');
    } else {
      const scheduledDate = new Date(job.scheduledDate);
      const today = new Date();
      today.setHours(0, 0, 0, 0);
      
      if (scheduledDate < today) {
        errors.push('Scheduled date cannot be in the past');
      }
    }
    
    if (!job.type) {
      errors.push('Maintenance type is required');
    }
    
    if (!job.assignedTo || job.assignedTo.length === 0) {
      errors.push('At least one technician must be assigned');
    }
    
    if (!job.estimatedHours || job.estimatedHours < 0.5 || job.estimatedHours > 24) {
      errors.push('Estimated hours must be between 0.5 and 24');
    }
    
    if (!job.reason || job.reason.length < 10) {
      errors.push('Reason must be at least 10 characters long');
    }
    
    return errors;
  }

  static validateFileUpload(file: File): string[] {
    const errors: string[] = [];
    
    if (!file) {
      errors.push('File is required');
      return errors;
    }
    
    if (file.size > MaintenanceValidators.fileUpload.maxSize) {
      const maxSizeMB = MaintenanceValidators.fileUpload.maxSize / (1024 * 1024);
      errors.push(`File size must be less than ${maxSizeMB}MB`);
    }
    
    if (!MaintenanceValidators.fileUpload.allowedTypes.includes(file.type)) {
      errors.push('File type not supported');
    }
    
    return errors;
  }

  static getErrorMessage(error: ValidationErrors): string {
    if (error['required']) {
      return 'This field is required';
    }
    
    if (error['min']) {
      return `Value must be at least ${error['min'].min}`;
    }
    
    if (error['max']) {
      return `Value must be at most ${error['max'].max}`;
    }
    
    if (error['minlength']) {
      return `Must be at least ${error['minlength'].requiredLength} characters`;
    }
    
    if (error['maxlength']) {
      return `Must be at most ${error['maxlength'].requiredLength} characters`;
    }
    
    if (error['pastDate']) {
      return 'Date cannot be in the past';
    }
    
    if (error['weekendDate']) {
      return 'Maintenance cannot be scheduled on weekends';
    }
    
    if (error['outsideBusinessHours']) {
      return 'Maintenance must be scheduled during business hours (8 AM - 6 PM)';
    }
    
    if (error['invalidFileType']) {
      return 'File type not supported';
    }
    
    if (error['fileTooLarge']) {
      const maxSizeMB = error['fileTooLarge'].maxSize / (1024 * 1024);
      return `File size must be less than ${maxSizeMB}MB`;
    }
    
    if (error['noInterval']) {
      return 'Either hours or months interval must be specified';
    }
    
    if (error['invalidHours']) {
      return 'Hours interval must be at least 1';
    }
    
    if (error['invalidMonths']) {
      return 'Months interval must be at least 1';
    }
    
    return 'Invalid input';
  }
}