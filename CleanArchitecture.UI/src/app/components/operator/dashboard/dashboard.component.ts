import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { ProjectService } from '../../../core/services/project.service';
import { Project } from '../../../core/models/project.model';

@Component({
  selector: 'app-operator-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class OperatorDashboardComponent implements OnInit {
  currentUserName = '';
  assignedProject: Project | null = null;
  isLoading = true;

  constructor(private authService: AuthService, private projectService: ProjectService) {}

  ngOnInit(): void {
    const user = this.authService.getCurrentUser();
    this.currentUserName = user?.name || 'Operator';

    if (user) {
      this.projectService.getProjectByOperator(user.id).subscribe({
        next: proj => {
          this.assignedProject = proj;
          this.isLoading = false;
        },
        error: err => {
          console.error('Failed to load project', err);
          this.isLoading = false;
        }
      });
    } else {
      this.isLoading = false;
    }
  }

  formatDate(date?: Date): string {
    return date ? new Date(date).toLocaleDateString() : '-';
  }
} 