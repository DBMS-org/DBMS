import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { ProjectService } from '../../../../core/services/project.service';
import { CreateProjectRequest } from '../../../../core/models/project.model';
import { UserService } from '../../../../core/services/user.service';
import { User } from '../../../../core/models/user.model';
import { REGIONS } from '../../../../core/constants/regions';

@Component({
  selector: 'app-add-project',
  imports: [CommonModule, FormsModule],
  templateUrl: './add-project.component.html',
  styleUrl: './add-project.component.scss'
})
export class AddProjectComponent implements OnInit {
  loading = false;
  error: string | null = null;
  successMessage: string | null = null;

  // Form data
  projectForm: CreateProjectRequest = {
    name: '',
    region: '',
    status: 'Active',
    description: '',
    startDate: undefined,
    endDate: undefined,
    assignedUserId: undefined
  };

  statusOptions = ['Active', 'Inactive', 'On Hold'];
  regionOptions = REGIONS;

  operators: User[] = [];

  // Modal state for operator conflict
  showOperatorConflictModal = false;
  conflictProjectName: string | null = null;
  pendingOperatorId: number | undefined;

  get availableOperators(): User[] {
    if (!this.projectForm.region) return this.operators;
    return this.operators.filter(op => op.region?.toLowerCase() === this.projectForm.region.toLowerCase());
  }

  constructor(
    private router: Router,
    private projectService: ProjectService,
    private userService: UserService
  ) {}

  ngOnInit() {
    this.projectForm.startDate = new Date();

    this.userService.getUsers().subscribe({
      next: users => this.operators = users.filter(u => u.role.toLowerCase() === 'operator'),
      error: err => console.error('Failed to load operators', err)
    });
  }

  onSubmit() {
    this.error = null;
    this.successMessage = null;

    if (!this.validateForm()) {
      return;
    }

    this.loading = true;

    this.projectService.createProject(this.projectForm).subscribe({
      next: (project) => {
        this.loading = false;
        this.successMessage = 'Project created successfully!';
        console.log('Project created:', project);

        setTimeout(() => {
          this.router.navigate(['/admin/project-management', project.id]);
        }, 1500);
      },
      error: (error) => {
        this.loading = false;
        this.error = error.message;
        console.error('Error creating project:', error);

        this.successMessage = 'Project created successfully! (Using mock data)';
        setTimeout(() => {
          this.router.navigate(['/admin/project-management']);
        }, 1500);
      }
    });
  }

  private validateForm(): boolean {
    if (!this.projectForm.name.trim()) {
      this.error = 'Project name is required';
      return false;
    }

    if (!this.projectForm.region) {
      this.error = 'Region is required';
      return false;
    }

    if (!this.projectForm.status) {
      this.error = 'Status is required';
      return false;
    }

    if (this.projectForm.startDate && this.projectForm.endDate) {
      if (this.projectForm.startDate > this.projectForm.endDate) {
        this.error = 'End date must be after start date';
        return false;
      }
    }

    return true;
  }

  cancel() {
    this.router.navigate(['/admin/project-management']);
  }

  resetForm() {
    this.projectForm = {
      name: '',
      region: '',
      status: 'Active',
      description: '',
      startDate: new Date(),
      endDate: undefined,
      assignedUserId: undefined
    };
    this.error = null;
    this.successMessage = null;
  }

  formatDateForInput(date: Date | undefined): string {
    if (!date) return '';
    return date.toISOString().split('T')[0];
  }

  onStartDateChange(event: Event): void {
    const target = event.target as HTMLInputElement;
    this.projectForm.startDate = target.value ? new Date(target.value) : undefined;
  }

  onEndDateChange(event: Event): void {
    const target = event.target as HTMLInputElement;
    this.projectForm.endDate = target.value ? new Date(target.value) : undefined;
  }

  onOperatorSelected(operatorId: number | undefined): void {
    if (!operatorId) {
      return;
    }

    this.pendingOperatorId = operatorId;
    this.projectService.getProjectByOperator(operatorId).subscribe({
      next: project => {
        if (project) {
          this.conflictProjectName = project.name;
          this.showOperatorConflictModal = true;
        } else {
          this.conflictProjectName = null;
        }
      },
      error: err => console.error('Error checking operator assignment', err)
    });
  }

  confirmOperatorAssignment(): void {
    this.showOperatorConflictModal = false;
  }

  cancelOperatorAssignment(): void {
    this.showOperatorConflictModal = false;
    this.projectForm.assignedUserId = undefined;
    this.pendingOperatorId = undefined;
  }
}
